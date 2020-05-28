using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WelcomeScreen.Models;

namespace WelcomeScreen.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        // GET: Error
        //public ActionResult Unathorized([Bind(Include = "Role,UserName")] IntraRole role)
        public ActionResult Unauthorized(string Role, string UserName)
        {
            IntraRole intrarole = new IntraRole
            {
                Role = Role,
                UserName = UserName
            };
            return View(intrarole);
        }

    }
}
