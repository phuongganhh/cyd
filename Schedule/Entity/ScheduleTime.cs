using Schedule.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Schedule.Entity
{
    public class ScheduleTime
    {
        public string Phong { get; set; }
        public string Ngay { get; set; }
        public int ThoiGianBatDau { get; set; }
        public int SoTiet { get; set; }
        public string NoiDungDay { get; set; }
        public string TenGV { get; set; }
        public string TenMH { get; set; }
        public string TenLop { get; set; }
        public int IDLich { get; set; }
        public int LoaiLich { get; set; }
        private DateTime DateOf
        {
            get
            {
                return DateTime.ParseExact(this.Ngay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
        }
        public string ThoiGianVaoHoc
        {
            get
            {
                return DateTime.Now.Date.AddMinutes(this.ThoiGianBatDau).ToString("HH:mm");

            }
        }
        public string DayOfWeek
        {
            get
            {
                return this.DateOf.DayOfWeek.ConvertString();
            }
        }
    }
}