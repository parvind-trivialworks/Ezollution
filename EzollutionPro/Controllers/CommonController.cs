using EzollutionPro_BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EzollutionPro.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        [HttpPost]
        public JsonResult ValidateMAWB(string PageName,string MAWBNo)
        {
            var res=MAWBService.Instance.ValidateMAWB(PageName, MAWBNo);
            return Json(res,JsonRequestBehavior.AllowGet);
        }
    }
}