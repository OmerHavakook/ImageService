using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageServiceWeb1.Models;

namespace ImageServiceWeb1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HomePageModel model = new HomePageModel();
            
            

            return View(model);
        }

        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}