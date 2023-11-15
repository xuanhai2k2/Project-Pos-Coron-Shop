using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using devpro_project.Areas.Admin.Attributes;
namespace devpro_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class HomeController : Controller
    {
        //phải đặt tag sau vào các controller của Area = Admin
        public IActionResult Index()
        {
           //string passwordHash = BCrypt.Net.BCrypt.HashPassword("123");
           // return Content(passwordHash);
           return View();
        }
    }
}
