using Framework.Caching;
using Newtonsoft.Json;
using Schedule.Common;
using Schedule.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Schedule.Controllers
{
    public class ApiController : BaseController
    {
        private async Task<IEnumerable<Week>> GetWeek()
        {
            var resultData = MemoryCacheManager.Instance.Get<IEnumerable<Week>>("week");
            if(resultData == null)
            {
                using (var service = new ApiService())
                {
                    var result = await service.Post<string>("/XepLich/AFXepLich.aspx/GetTuan");
                    var html = HtmlService.Instance.Load(result.d);
                    var resultHtml = html.DocumentNode.SelectNodes("//option").Select(x =>
                    {
                        DateTime dt = DateTime.ParseExact(x.Attributes["value"].Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        bool cur = false;
                        if (dt <= DateTime.Now && DateTime.Now <= dt.AddDays(7))
                        {
                            cur = true;
                        }
                        return new Week
                        {
                            Number = Convert.ToInt32(x.Attributes["stttuan"].Value),
                            Time = x.Attributes["value"].Value,
                            Text = x.InnerText,
                            Current = cur
                        };
                    });
                    resultData =  resultHtml;
                }
                MemoryCacheManager.Instance.Set("week", resultData, 60 * 24);
            }
            return resultData;
        }
        public async Task<ActionResult> Week()
        {
            return this.JsonExpando(await this.GetWeek());
        }
        private async Task<IEnumerable<Classes>> GetClasses()
        {
            var resultData = MemoryCacheManager.Instance.Get<IEnumerable<Classes>>("classes");
            if(resultData == null)
            {
                using (var api = new ApiService())
                {
                    var result = await api.Post<string>("/AjaxFunction.aspx/GetLopGoc");
                    var document = HtmlService.Instance.Load(result.d);
                    var resultHtml = document.DocumentNode.SelectNodes("//option").Select(x =>
                    {
                        return new Classes
                        {
                            Name = x.InnerText,
                            Current = x.InnerText.Equals("CÐD8I")
                        };
                    });
                    resultData = resultHtml;
                    MemoryCacheManager.Instance.Set("classes", resultData, 60 * 24);
                }
            }
            return resultData;
            
        }
        public async Task<ActionResult> Class()
        {
            return this.JsonExpando(await this.GetClasses());
        }
        private async Task<IEnumerable<ScheduleTime>> GetSchedule(string Class, string Nhom, string TuNgay, string DenNgay)
        {
            using (var api = new ApiService())
            {
                var data = new Dictionary<string, object>
                {
                    ["GiaoVienID"] = -1,
                    ["Phong"] = -1,
                    ["MonHocID"] = "0",
                    ["LopGoc"] = Class,
                    ["TuNgay"] = TuNgay,
                    ["DenNgay"] = DenNgay
                };
                var result = await api.Post<string>("/XepLich/AFXepLich.aspx/GetLichTraCuu", data);
                var schedule = JsonConvert.DeserializeObject<IEnumerable<ScheduleTime>>(result.d);
                var resultModel = schedule;
                if (!string.IsNullOrEmpty(Nhom))
                {
                    resultModel = schedule.Where(x =>
                    {
                        if (x.TenLop.EndsWith(Nhom) || !x.TenLop.Contains("_"))
                        {
                            return true;
                        }
                        return false;
                    });
                }
                return resultModel;
            }
        }
        public ActionResult Group()
        {
            var l = new List<dynamic>();
            l.Add(new
            {
                Name = "Tất cả",
                Value = "",
                Current = false
            });
            l.Add(new
            {
                Name = "Nhóm 2",
                Value = "N2",
                Current = true
            });
            return this.JsonExpando(l);
        }

        public async Task<ActionResult> Schedule(string Class, string Nhom, string TuNgay, string DenNgay)
        {
            try
            {
                var weekAsync = await this.GetWeek();
                if(TuNgay == DenNgay)
                {
                    var next = weekAsync.FirstOrDefault(x => TuNgay.ToDateTime().Date < x.Time.ToDateTime().Date);
                    if(next != null)
                    {
                        DenNgay = next.Time;
                    }
                }
                var scheduleAsync = await this.GetSchedule(Class, Nhom, TuNgay, DenNgay);

                var tn = TuNgay.ToDateTime();
                var dn = DenNgay.ToDateTime();
                var week = weekAsync.Where(x => tn <= x.Time.ToDateTime() && x.Time.ToDateTime() <= dn);
                var result = week.Select(x =>
                {
                    var days = new List<Day>();
                    for (int i = 0; i < 7; i++)
                    {
                        var time = x.Time.ToDateTime().AddDays(i);
                        days.Add(new Day
                        {
                            Time = time.ToString("dd/MM"),
                            Name = time.DayOfWeek.ConvertString(),
                            Current = time == DateTime.Now.Date,
                            Morning = scheduleAsync.Where(w => w.Ngay.ToDateTime().Date == time.Date && w.Ngay.ToDateTime().Date.AddMinutes(w.ThoiGianBatDau).Hour <= 12),
                            Afternoon = scheduleAsync.Where(w => w.Ngay.ToDateTime().Date == time.Date && w.Ngay.ToDateTime().Date.AddMinutes(w.ThoiGianBatDau).Hour > 12)
                        });
                    }
                    return new
                    {
                        name = x.Text,
                        days
                    };
                });
                return this.JsonExpando(result);
            }
            catch (Exception)
            {
                return this.JsonExpando(new List<dynamic>());
            }
        }
    }
}
