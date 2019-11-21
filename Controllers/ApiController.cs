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
        public async Task<ActionResult> Week()
        {
            using(var service = new ApiService())
            {
                var result = await service.Post<string>("/XepLich/AFXepLich.aspx/GetTuan");
                var html = HtmlService.Instance.Load(result.d);
                var resultHtml = html.DocumentNode.SelectNodes("//option").Select(x =>
                {
                    DateTime dt = DateTime.ParseExact(x.Attributes["value"].Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    bool cur = false;
                    if(dt <= DateTime.Now && DateTime.Now <= dt.AddDays(7))
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
                return this.JsonExpando(resultHtml);
            }
        }
        public async Task<ActionResult> Class() {
            using(var api = new ApiService())
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
                return this.JsonExpando(resultHtml);
            }
        }
        public async Task<ActionResult> Schedule(string Class,string TuNgay, string DenNgay) {
            using(var api = new ApiService())
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
                var result = await api.Post<string>("/XepLich/AFXepLich.aspx/GetLichTraCuu",data);
                var schedule = JsonConvert.DeserializeObject<IEnumerable<ScheduleTime>>(result.d);
                return this.JsonExpando(schedule);
            }
        }
    }
}