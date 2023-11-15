using Microsoft.AspNetCore.Mvc;
using devpro_project.Models;
using BC = BCrypt.Net.BCrypt;
using X.PagedList;
namespace devpro_project.Controllers
{
    public class AccountController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View("LoginRegister");
        }
        [HttpPost]
        public IActionResult RegisterPost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            string _Email = fc["Email"].ToString().Trim();
            string _Password = fc["Password"].ToString().Trim();
            _Password = BC.HashPassword(_Password);
            //kieerm tra trong csdl đã tồn tại Email chưa, chưa thì mới Create
            ItemCustomer record_check = db.Customers.FirstOrDefault(item => item.Email == _Email);
            if (record_check == null)
            {
                ItemCustomer record = new ItemCustomer();
                record.Name = _Name;
                record.Email = _Email;
                record.Password = _Password;
                //cập nhật lại dữ liệu
                db.Customers.Add(record);
                db.SaveChanges();
            }
            else
                return Redirect("/Account/Register?notify=email-exists");
            return Redirect("/Account/Register?notify=success");
        }
        public IActionResult Login()
        {
            return View("LoginRegister");
        }
        [HttpPost]
        public IActionResult LoginPost(IFormCollection fc)
        {
            string _Email = fc["Email"].ToString().Trim();
            string _Password = fc["Password"].ToString().Trim();
            //lay mot ban ghi tuong ung voi email truyen vao
            var record = db.Customers.Where(item => item.Email == _Email).FirstOrDefault();

            if (record != null)
            {
                if (BC.Verify(_Password, record.Password))
                {
                    //khoi tao session UserId
                    HttpContext.Session.SetString("customer-id", record.Id.ToString());
                    //khoi tao session Email
                    HttpContext.Session.SetString("customer-email", _Email);
                    //di chuyen den url
                    return Redirect("/Home");
                }
                else
                    return Redirect("/Account/Login?notify=invalid");
            }
            else
                return Redirect("/Account/Login?notify=invalid");
            return Redirect("/Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("customer-email");
            HttpContext.Session.Remove("customer-id");
            return Redirect("/Home");
        }
        //danh sách đơn hàng
        public IActionResult Orders(int? page)
        {
            //lay customer id de hien thi cac don hang
            int customer_id = 0;
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("customer-id"))){
                return Redirect("/Account/Login");
            }
            else
            {
                customer_id = Convert.ToInt32(HttpContext.Session.GetString("customer-id"));
            }
            int _CurrentPage = page ?? 1;
            //dinh nghia so ban ghi tren mot trang
            int _RecordPerPage = 20;
           

            //lay tat ca cac ban ghi trong table News
            List<ItemOrder> listRecord = db.Orders.Where(item=>item.CustomerId == customer_id).OrderByDescending(item => item.Id).ToList();
            //truyen gia tri ra view co phan trang
            //return Content(HttpContext.Session.GetString("id"));
            return View("Orders", listRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }
        public IActionResult Detail(int? id)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("customer-id")))
            {
                return Redirect("/Account/Login");
            }
            int _OrderId = id ?? 0;
            ViewBag.OrderId = _OrderId;
            //lay danh sach cac san pham thuoc don hang
            List<ItemOrderDetail> _ListRecord = db.OrdersDetail.Where(tbl => tbl.OrderId == _OrderId).ToList();
            return View("Detail", _ListRecord);
        }
        public IActionResult Canncel(int? id)
        {
            int _OrderId = id ?? 0;
            ItemOrder record = db.Orders.Where(tbl => tbl.Id == _OrderId).FirstOrDefault();
            if (record != null)
            {
                record.Status = 2;
                //cap nhat du lieu
                db.SaveChanges();
            }
            return Redirect("/Account/Orders");
        }
    }
}
