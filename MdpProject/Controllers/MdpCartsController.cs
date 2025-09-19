using Microsoft.AspNetCore.Mvc;

namespace MdpProject.Controllers
{
    public class CartController : Controller
    {
        // Thêm vào giỏ hàng
        public IActionResult AddToCart(int id)
        {
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
            {
                // Chưa đăng nhập → redirect về login
                return RedirectToAction("Index", "MDPHome");
            }

            // TODO: Logic thêm sản phẩm vào giỏ hàng
            // VD: lưu vào Session hoặc DB

            // Sau khi thêm → redirect thẳng tới trang giỏ hàng
            return RedirectToAction("Index", "MdpCart");
        }

        // Trang giỏ hàng
        public IActionResult Index()
        {
            // Lấy danh sách sản phẩm trong giỏ từ Session/DB
            return View();
        }
    }
}
