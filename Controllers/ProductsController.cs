using Microsoft.AspNetCore.Mvc;
using devpro_project.Models;
using X.PagedList;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace devpro_project.Controllers
{
    public class ProductsController : Controller
    {
        public MyDbContext db = new MyDbContext();
        //shop
        public IActionResult Categories(int? id, int? page)
        {
            int CategoryId = id ?? 0;
            ViewBag.CategoryId = CategoryId;
            //soos ban ghi trn 1 trang
            int pageSize = 9;
            //so trang
            int pageNumber = page ?? 1;
            List<ItemProduct> list_record = new List<ItemProduct>();
            if (CategoryId > 0)
            {
                //lay cac san pham tuong ung voi CategoryId truyen vao
                list_record = (from category in db.Categories join category_product in db.CategoriesProducts on category.Id equals category_product.CategoryId join product in db.Products on category_product.ProductId equals product.Id where category_product.CategoryId == CategoryId select product).ToList();
            }
            else
                list_record = db.Products.OrderByDescending(item => item.Id).ToList();
            //----
            string order = "";
            if (!String.IsNullOrEmpty(Request.Query["order"]))
                order = Request.Query["order"];
            //truyen bien order ra ngoai View de thuc hien Seleted dropdown
            ViewBag.order = order;
            switch (order)
            {
                case "name-asc":
                    list_record = list_record.OrderBy(item => item.Id).ToList(); break;
                case "name-desc":
                    list_record = list_record.OrderByDescending(item => item.Id).ToList(); break;
                case "price-asc":
                    list_record = list_record.OrderBy(item => item.Price).ToList(); break;
                case "price-desc":
                    list_record = list_record.OrderByDescending(item => item.Id).ToList(); break;
            }
            //----
            return View("ProductsCategories", list_record.ToPagedList(pageNumber, pageSize));
        }
        //detail
        public IActionResult Detail(int id)
        {
            var record = db.Products.Where(item => item.Id == id).FirstOrDefault();
            return View("ProductsDetail", record);
        }
        public IActionResult Rate(int id)
        {
            int star = Convert.ToInt32(Request.Query["star"]);
            ItemRating record = new ItemRating();
            record.ProductId = id;
            record.Star = star;
            db.Rating.Add(record);
            db.SaveChanges();
            return Redirect("/Products/Detail" + id);

        }
        public IActionResult AJaxSearch()
        {
            string key = "";
            if (!String.IsNullOrEmpty(Request.Query["key"]))
                key = Request.Query["key"];
            List<ItemProduct> list_products = db.Products.Where(item => item.Name.Contains(key)).ToList();
            string str = "";
            foreach (var item in list_products)
            {
                str += "<li><a href='/Products/Detail/" + item.Id + "'><img src='/Upload/Products/" + item.Photo + "'>" + item.Name + "</a></li>";
            }
            return Content(str);
        }
        public IActionResult SearchName(int? page)
        {
            //khi lấy biến từ url thì mặc định biến này sẽ là kiểu string -> nếu là số thì cần convert nó
            string key = "";
            if (!String.IsNullOrEmpty(Request.Query["key"]))
                key = Request.Query["key"];
            //---
            //tạo 2 biến để đưa giá ra ngoài view
            ViewBag.key = key;
            //---
            //số bản ghi trên một trang
            int pageSize = 9;
            //số trang
            int pageNumber = page ?? 1;
            List<ItemProduct> list_record = db.Products.Where(item => item.Name.Contains(key)).ToList();
            return View("SearchName", list_record.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult SearchPrice(int? page)
        {
            int from_price = 0;
            int to_price = 0;
            if (!String.IsNullOrEmpty(Request.Query["from_price"]))
                from_price = Convert.ToInt32(Request.Query["from_price"]);
            if (!String.IsNullOrEmpty(Request.Query["to_price"]))
                to_price = Convert.ToInt32(Request.Query["to_price"]);
            ViewBag.from_price = from_price;
            ViewBag.to_price = to_price;    
            //số bản ghi trên một trang
            int pageSize = 9;
            //số trang
            int pageNumber = page ?? 1;
            List<ItemProduct> list_record = db.Products.Where(item => item.Price >= from_price && item.Price <=to_price).ToList();
            return View("SearchPrice", list_record.ToPagedList(pageNumber, pageSize));

        }
        public IActionResult Tag(int? id, int? page)
        {
            int tag_id = id?? 0;
            ViewBag.tag_id = tag_id;
            //số bản ghi trên một trang
            int pageSize = 9;
            //số trang
            int pageNumber = page ?? 1;
            List<ItemProduct> list_products = (from item_tag in db.Tags join item_tag_product in db.TagsProducts on item_tag.Id equals item_tag_product.TagId join item_product in db.Products on item_tag_product.ProductId equals item_product.Id where item_tag.Id == tag_id select item_product).ToList();
            return View("Tag", list_products.ToPagedList(pageNumber, pageSize));
        }
    }
}
