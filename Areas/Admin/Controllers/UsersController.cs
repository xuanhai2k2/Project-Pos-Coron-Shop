using Microsoft.AspNetCore.Mvc;
using devpro_project.Models;
using BC = BCrypt.Net.BCrypt;
using X.PagedList;
namespace devpro_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        //Khai baos đối tượng thao tác csdl
        public MyDbContext db = new MyDbContext();
        public IActionResult Index()
        {
            return Redirect("/Admin/Users/Read");
        }
        public IActionResult Read(int? page)
        {
            //soos ban ghi trn 1 trang
            int pageSize = 4;
            //so trang
            int pageNumber = page ?? 1;
            List<ItemUser> users = db.Users.OrderByDescending(item => item.Id).ToList();
            return View("Read", users.ToPagedList(pageNumber, pageSize));
        }
        //update
        public IActionResult Update(int id)
        {
            ViewBag.action = "/Admin/Users/UpdatePost/" + id;
            ItemUser record = db.Users.FirstOrDefault(item => item.Id == id);

            return View("CreateUpdate", record);
        }
        [HttpPost]
        public IActionResult UpdatePost(IFormCollection fc, int id)
        {
            string _Name = fc["Name"].ToString().Trim();
            string _Email = fc["Email"].ToString().Trim();
            string _Password = fc["Password"].ToString();
            ItemUser record = db.Users.FirstOrDefault(item => item.Id == id);
            if (record != null)
            {
                //kiểm tra xem Email đã tồn tại trong table user chưa, nếu chưa mới được thêm
                ItemUser record_check = db.Users.FirstOrDefault(item => item.Id != id && item.Email == _Email);
                if (record_check == null)
                {
                    record.Name = _Name;
                    record.Email = _Email;
                    if(!String.IsNullOrEmpty(_Password))
                    {
                        record.Password = _Password;
                    }
                    //cập nhật lại dữ liệu
                    db.Update(record);
                    db.SaveChanges();
                }
                else
                    return Redirect("/Admin/Users/Update/" + id + "?notify=email-exists");

            }
            return Redirect("/Admin/Users/Read");
        }
        //create
        public IActionResult Create()
        {
            ViewBag.action = "/Admin/Users/CreatePost/";
            return View("CreateUpdate");
        }
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            string _Email = fc["Email"].ToString().Trim();
            string _Password = fc["Password"].ToString();
            _Password = BC.HashPassword(_Password);
            //kieerm tra trong csdl đã tồn tại Email chưa, chưa thì mới Create
            ItemUser record_check = db.Users.FirstOrDefault(item=>item.Email == _Email);
            if(record_check == null)
            {
                ItemUser record = new ItemUser();
                record.Name = _Name;
                record.Email = _Email;
                record.Password = _Password;
                //cập nhật lại dữ liệu
                db.Users.Add(record);
                db.SaveChanges();
            }
            else
                return Redirect("/Admin/Users/Create/?notify=email-exists");
            return Redirect("/Admin/Users/Read");
        }
        //delete
        public IActionResult Delete(int id)
        {
            ItemUser record = db.Users.FirstOrDefault(item => item.Id == id);
            if(record != null)
            {
                db.Users.Remove(record);
                db.SaveChanges();
            }
            return Redirect("/Admin/Users/Read");
        }
    }
}
