using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Schedule.Common
{
    public class BaseController : Controller
    {
        public ActionResult JsonExpando(object o)
        {
            return new JsonResult
            {
                Data = o,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}