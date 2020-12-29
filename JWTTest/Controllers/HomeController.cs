using JWTTest.Helper;
using System.Web.Mvc;

namespace JWTTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
