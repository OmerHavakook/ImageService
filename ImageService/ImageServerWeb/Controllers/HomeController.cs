using ImageServerWeb.Models;
using System.Web.Mvc;

namespace ImageServerWeb.Controllers
{
    public class HomeController : Controller
    {

        static HomePageModel mainModel = new HomePageModel();
        static ConfigModel configModel = new ConfigModel();

        // GET: Home
        public ActionResult Index()
        {
            mainModel.Initialize();
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
    }
}