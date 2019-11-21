using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedule.Common
{
    public class HtmlService
    {
        public HtmlService()
        {

        }
        private static HtmlService _instance { get; set; }
        public static HtmlService Instance
        {
            get
            {
                return _instance ?? (_instance = new HtmlService());
            }
        }
        public HtmlDocument Load(string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            return document;
        }
    }
}