using Microsoft.AspNetCore.Mvc;
using devpro_project.Models;
using Newtonsoft.Json;
namespace devpro_project.Controllers
{
    public class CartController : Controller
    {
        //hiển thị danh sách các sản phẩm trong giỏ hàng
        public IActionResult Index()
        {
           List<Item> shopping_cart = new List<Item>();
            string json_cart = HttpContext.Session.GetString("cart");
            if(!string.IsNullOrEmpty(json_cart))
            {
                shopping_cart=JsonConvert.DeserializeObject<List<Item>>(json_cart);
            }   
            return View(shopping_cart);
        }
        //thêm sản phẩm vào giỏ hàng
        public IActionResult Buy(int id)
        {
            Cart.CartAdd(HttpContext.Session, id);
            return RedirectToAction("Index");
        }
        //xóa sản phẩm khỏi giỏ hàng
        public IActionResult Remove(int id)
        {
            Cart.CartRemove(HttpContext.Session, id);
            return RedirectToAction("Index");
        }
        //update gio hang
        public IActionResult Update()
        {
            List<Item> shopping_cart = new List<Item>();
            string json_cart = HttpContext.Session.GetString("cart");
            if (!string.IsNullOrEmpty(json_cart))
            {
                shopping_cart = JsonConvert.DeserializeObject<List<Item>>(json_cart);
            }
            foreach(Item cart_item in shopping_cart)
            {
                int new_quantity = Convert.ToInt32(Request.Form["product_" + cart_item.ProductRecord.Id]);
                //goị hàm cập nhật số lượng sản phẩm
                Cart.CartUpdate(HttpContext.Session, cart_item.ProductRecord.Id, new_quantity);
            }
            return RedirectToAction("Index");
        }
        //Xóa toàn bộ sản phẩm trong giỏ hàng
        public IActionResult Destroy()
        {
            Cart.CartDestroy(HttpContext.Session);
            return Redirect("/Cart/Index/?notify=destroy-success");
        }
        //thanh toán giỏ hàng
        public IActionResult CheckOut()
        {
            //nếu user chưa đang nhập di chuyển đến url đăng nhập
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("customer-id"))){
                return Redirect("/Account/Login");
            }
            else
            {
                //laays id customer
                int customer_id = Convert.ToInt32(HttpContext.Session.GetString("customer-id"));
                Cart.CartCheckOut(HttpContext.Session, customer_id);
            }
            return Redirect("/Cart/Index/?notify=checkout-success");
        }
    }
}
