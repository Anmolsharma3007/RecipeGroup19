using System.Web.Mvc;
using FluentNHibernate.Mapping;

namespace Group19.Recipes1.Controllers
{
    public class AboutController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}