using Microsoft.AspNetCore.Mvc;
using devpro_project.Models;
using X.PagedList;
namespace devpro_project.Controllers
{
    public class NewsController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail(int id)
        {
            var record = db.News.Where(item => item.Id == id).FirstOrDefault();
            return View("NewsDetail", record);
        }
    }
}
