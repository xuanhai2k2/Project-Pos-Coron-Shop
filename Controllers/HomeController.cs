using Microsoft.AspNetCore.Mvc;

namespace devpro_project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
            //return Redirect("/Admin");
        }
    }
}
