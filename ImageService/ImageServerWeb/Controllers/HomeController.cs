using ImageServerWeb.Models;
using System.Web.Mvc;

namespace ImageServerWeb.Controllers
{
    public class HomeController : Controller
    {

        static HomePageModel mainModel = new HomePageModel();
        static ConfigModel configModel = new ConfigModel();
        static LogModel logModel = new LogModel();
        static ImagesModel imagesModel = new ImagesModel();


        // GET: Home
        public ActionResult Index()
        {
            mainModel.Initialize();
            mainModel.countImages(configModel.OutputDirectory);
            return View(mainModel);
        }

        public ActionResult Config()
        {
            configModel.Initialize();
            
            return View(configModel);

        }

        public ActionResult RemoveHandler(int id)
        {
            configModel.SelectedItem = configModel.Handlers[id];
            return View(configModel);
        }

        public ActionResult Logs()
        {
            logModel.Initialize();
            return View(logModel);
        }

        public ActionResult Images()
        {

            return View(imagesModel);
        }


    }
}