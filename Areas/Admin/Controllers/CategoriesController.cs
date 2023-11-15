using Microsoft.AspNetCore.Mvc;
using devpro_project.Models;
using BC = BCrypt.Net.BCrypt;
using X.PagedList;
using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using devpro_project.Models;

namespace devpro_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        string strConnectionString;
        public CategoriesController()
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            this.strConnectionString = config.GetConnectionString("MyConnectionString").ToString();
        }
        public IActionResult Index()
        {
            return Redirect("/Admin/Categories/Read");
        }
        public IActionResult Read(int? page)
        {
            DataTable dtCategories = new DataTable();
            using(SqlConnection conn = new SqlConnection(strConnectionString))
            {
                //tao doi tuong DataApter der truyen cau lenh truy van
                SqlDataAdapter da = new SqlDataAdapter("select * from Categories where ParentId = 0 order by Id desc", conn);
                // Đổ dữ liệu từ da vào DataTable
                da.Fill(dtCategories);
            }
            //-----
            //do thư viên X.PagedList sử dụng List để phaan trang vì vậy cần chuyển DataTable có tên là dtCatrgories sang kiểu List
            // Tạo đối tượng List để lưu dữ liệu từ dtCategories chuyển sang
            List<ItemCategory> listCategory = new List<ItemCategory>();
                //duyeent các row của dtCatrgories sang row list để add vào List
                foreach (DataRow item in dtCategories.Rows)
                {
                    listCategory.Add(new ItemCategory() { Id = Convert.ToInt32(item["id"]), ParentId = Convert.ToInt32(item["ParentId"]),Name = item["Name"].ToString(), DisplayHomePage = Convert.ToInt32(item["DisplayHomePage"]) });
                }
                //-----
                int pageSize = 2;
                int pageNumber =page ?? 1;
                return View("Read", listCategory.ToPagedList(pageNumber, pageSize));
         
            
        }
        public IActionResult Update(int id)
        {
            ViewBag.action = "/Admin/Categories/UpdatePost/" + id;
            DataTable dtCategories = new DataTable();

            using(SqlConnection conn = new SqlConnection(strConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from Categories where Id = "+ id, conn);
                da.Fill(dtCategories);
            }

            return View("CreateUpdate", dtCategories);
        }
        [HttpPost]
        public IActionResult UpdatePost(int id, IFormCollection fc)
        {
            //su dung doi tuong IFormCollection de lay du lieu theo kieu POST
            string _Name = fc["Name"].ToString().Trim();
            //su dung doi tuong Request de lay du lieu theo kieu POST
            int _ParentId = Convert.ToInt32(Request.Form["ParentId"]);
            int _DisplayHomePage = !String.IsNullOrEmpty(Request.Form["DisplayHomePage"]) ? 1 : 0;
            //---
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                //chu y: voi Update, Create phai Open connection
                conn.Open();
                SqlCommand cmd = new SqlCommand("update Categories set Name=@var_name, ParentId = @var_parent_id, DisplayHomePage = @display_home_page where Id = @id", conn);
                //truyen cac gia tri vao chuoi truy van
                cmd.Parameters.AddWithValue("@var_name", _Name);
                cmd.Parameters.AddWithValue("@var_parent_id", _ParentId);
                cmd.Parameters.AddWithValue("@display_home_page", _DisplayHomePage);
                cmd.Parameters.AddWithValue("@id", id);
                //thuc thi cau truy van
                cmd.ExecuteNonQuery();
            }
            //---
            return Redirect("/Admin/Categories/Read");
        }
        //create
        public IActionResult Create()
        {
            ViewBag.action = "/Admin/Categories/CreatePost/";
            return View("CreateUpdate");
        }
        [HttpPost]
        public IActionResult CreatePost(IFormCollection fc)
        {
            //su dung doi tuong IFormCollection de lay du lieu theo kieu POST
            string _Name = fc["Name"].ToString().Trim();
            //su dung doi tuong Request de lay du lieu theo kieu POST
            int _ParentId = Convert.ToInt32(Request.Form["ParentId"]);
            int _DisplayHomePage = !String.IsNullOrEmpty(Request.Form["DisplayHomePage"]) ? 1 : 0;
            //---
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                //chu y: voi Update, Create phai Open connection
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into Categories(Name,ParentId,DisplayHomePage) values(@var_name,@var_parent_id,@display_home_page)", conn);
                //truyen cac gia tri vao chuoi truy van
                cmd.Parameters.AddWithValue("@var_name", _Name);
                cmd.Parameters.AddWithValue("@var_parent_id", _ParentId);
                cmd.Parameters.AddWithValue("@display_home_page", _DisplayHomePage);
                //thuc thi cau truy van
                cmd.ExecuteNonQuery();
            }
            //---
            return Redirect("/Admin/Categories/Read");
        }
        public IActionResult Delete(int id)
        {
            //---
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                //chu y: voi Update, Create phai Open connection
                conn.Open();
                SqlCommand cmd = new SqlCommand("delete from Categories  where Id = @id or ParentId =@id", conn);
                //truyen cac gia tri vao chuoi truy van
                cmd.Parameters.AddWithValue("@id", id);
                //thuc thi cau truy van
                cmd.ExecuteNonQuery();
            }
            return Redirect("/Admin/Categories/Read");
        }
    }
}
