using ImageServerWeb.Models;
using System.Web.Mvc;

namespace ImageServerWeb.Controllers
{
    public class HomeController : Controller
    {

        static readonly HomePageModel MainModel = new HomePageModel();
        static readonly ConfigModel ConfigModel = new ConfigModel();
        static readonly LogModel LogModel = new LogModel();
        static readonly ImagesModel ImagesModel = new ImagesModel();

        // GET: Home
        public ActionResult Index()
        {

            ConfigModel.Initialize();
            if (MainModel.Initialize())
            {
                while (ConfigModel.OutputDirectory == null)
                {
                }
                MainModel.countImages(ConfigModel.OutputDirectory);
            }
            else
            {
                MainModel.NumOfImages = 0;
            }
            return View(MainModel);
        }
        public ActionResult Config()
        {
            ConfigModel.Initialize();
            return View(ConfigModel);

        }

        public ActionResult RemoveHandler(int id)
        {
            ConfigModel.SelectedItem = ConfigModel.Handlers[id];
            return View(ConfigModel);
        }

        public ActionResult RemoveOk()
        {
            ConfigModel.OnRemove();

            return RedirectToAction("Config","Home");
        }

        public ActionResult Logs()
        {
            LogModel.Initialize();
            return View(LogModel);
        }

        public ActionResult Images()
        {
            ImagesModel.OutputDirectory = ConfigModel.OutputDirectory;
            ImagesModel.initialize();
            return View(ImagesModel);
        }

        public ActionResult RemoveImage(int id)
        {
            ImagesModel.SelectedItem = ImagesModel.Images[id];
            return View(ImagesModel);
        }

        public ActionResult ViewImage(int id)
        {
            ImagesModel.SelectedItem = ImagesModel.Images[id];
            return View(ImagesModel);
        }
    }
}