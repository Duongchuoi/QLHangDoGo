using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BaiTapLon.Models;
using Microsoft.AspNetCore.Mvc;
using WebGrease;

namespace BaiTapLon.Controllers
{
    public class HomeController : Controller
    {
        Model1 db = new Model1();
        public List<ProductViewModel> GetTopSellingProducts(int topCount)
        {
            var topSellingProductIds = db.donhangs
                .GroupBy(o => o.MaSP)
                .OrderByDescending(g => g.Count())
                .Take(topCount)
                .Select(g => g.Key)
                .ToList();

            var topSellingProducts = db.sanphams
                .Where(p => topSellingProductIds.Contains(p.MaSP))
                .Select(p => new ProductViewModel
                {
                    Product = p,
                    IsTopSelling = true
                })
                .ToList();

            var otherProducts = db.sanphams
                .Where(p => !topSellingProductIds.Contains(p.MaSP))
                .Select(p => new ProductViewModel
                {
                    Product = p,
                    IsTopSelling = false
                })
                .ToList();
            return topSellingProducts.Concat(otherProducts).ToList();
        }
        public ActionResult Index()
        {    
            var listProduct = GetTopSellingProducts(3);
            return View(listProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string MaSP)
        {
            if (Session["nguoidung"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            AntiForgery.Validate();
            if (ModelState.IsValid)
            {
                bool isProductInCart = db.giohangs.Any(g => g.MaSP == MaSP);

                if (isProductInCart)
                {
                    // Sản phẩm đã có trong giỏ hàng
                    // Thực hiện các hành động hoặc thông báo phù hợp ở đây
                    TempData["Message"] = "Sản phẩm đã có trong giỏ hàng!";
                }
                else
                {
                    // Sản phẩm chưa có trong giỏ hàng, thêm vào giỏ hàng
                    var item = db.sanphams.Find(MaSP);
                    giohang giohang = new giohang();
                    // Tạo id cho giỏ hàng
                    string uniqueMaND = GenerateUniqueMaND();
                    giohang.id = uniqueMaND;
                    giohang.MaSP = item.MaSP;
                    giohang.SoLuong = 1;
                    taikhoan taikhoan = Session["nguoidung"] as taikhoan;
                    giohang.tk = taikhoan.tk;

                    db.giohangs.Add(giohang);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpGet]
        public JsonResult GetCartItemCount()
        {
            // Lấy tài khoản từ session
            taikhoan taikhoan = Session["nguoidung"] as taikhoan;

            if (taikhoan != null)
            {
                // Thực hiện logic để đếm số lượng sản phẩm trong giỏ hàng của tài khoản hiện tại
                int cartItemCount = db.giohangs.Count(item => item.tk == taikhoan.tk);

                return Json(new { count = cartItemCount }, JsonRequestBehavior.AllowGet);
            }

            // Trả về 0 nếu không có tài khoản
            return Json(new { count = 0 }, JsonRequestBehavior.AllowGet);
        }
        public bool IsProductInCart(string MaSP)
        {
            // Thực hiện logic kiểm tra xem sản phẩm có trong giỏ hàng của người dùng hay không
            var isInCart = db.giohangs.Any(item => item.MaSP == MaSP);
            return isInCart;
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        // Get: /Home/Search
        [HttpGet]
        public JsonResult Search(string searchTerm)
        {
            // Nếu từ khóa tìm kiếm là rỗng hoặc null, trả về một danh sách sản phẩm trống
            if (string.IsNullOrEmpty(searchTerm))
            {
                return Json(new List<ProductViewModel>(), JsonRequestBehavior.AllowGet);
            }

            // Lấy danh sách sản phẩm từ cơ sở dữ liệu
            var products = db.sanphams
                                .Select(p => new ProductViewModel
                                {
                                    // Ánh xạ các thuộc tính của thực thể sang thuộc tính của ViewModel ở đây
                                    Product = p
                                })
                                .ToList();

            // Tìm kiếm sản phẩm sử dụng fuzzy search với từ khóa tìm kiếm
            var searchResults = FuzzySearchProducts(products, searchTerm.ToLower());

            return Json(searchResults, JsonRequestBehavior.AllowGet);
        }

        // Hàm thực hiện fuzzy search trên danh sách sản phẩm
        private List<ProductViewModel> FuzzySearchProducts(List<ProductViewModel> products, string searchTerm)
        {
            // Tính toán kết quả fuzzy search cho mỗi sản phẩm
            var results = products
                            .Where(p => FuzzySearch(p.Product.TenSP.ToLower(), searchTerm))
                            .ToList();

            return results;
        }

        // Phương thức thực hiện fuzzy match giữa hai chuỗi
        private bool FuzzySearch(string str1, string str2)
        {
            // Đặt ngưỡng cho fuzzy match
            var threshold = 0.5; // Ngưỡng tự chọn, có thể thay đổi

            // Chuyển đổi các chuỗi thành chữ thường để so sánh không phân biệt chữ hoa chữ thường
            str1 = str1.ToLower();
            str2 = str2.ToLower();

            // Tính toán độ tương đồng giữa hai chuỗi bằng cách sử dụng thuật toán Levenshtein distance
            var distance = ComputeLevenshteinDistance(str1, str2);

            // Tính toán độ tương đồng dựa trên độ dài của chuỗi và khoảng cách Levenshtein
            var maxLength = Math.Max(str1.Length, str2.Length);
            var similarity = 1 - (double)distance / maxLength;

            // Trả về true nếu độ tương đồng vượt qua ngưỡng
            return similarity >= threshold;
        }

        private int ComputeLevenshteinDistance(string str1, string str2)
        {
            int[,] distance = new int[str1.Length + 1, str2.Length + 1];

            for (int i = 0; i <= str1.Length; i++)
                distance[i, 0] = i;

            for (int j = 0; j <= str2.Length; j++)
                distance[0, j] = j;

            for (int i = 1; i <= str1.Length; i++)
            {
                for (int j = 1; j <= str2.Length; j++)
                {
                    int cost = (str1[i - 1] == str2[j - 1]) ? 0 : 1;

                    distance[i, j] = Math.Min(
                        Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                        distance[i - 1, j - 1] + cost
                    );
                }
            }

            return distance[str1.Length, str2.Length];
        }

     /*   public ActionResult User(nguoidung updatedUser)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["nguoidung"] == null)
            {
                // Nếu chưa đăng nhập, điều hướng về trang đăng nhập
                return RedirectToAction("Index", "Login");
            }
            else
            {
                // Lấy thông tin người dùng từ session
                var user = Session["nguoidung"] as taikhoan;

                // Kiểm tra xem thông tin người dùng được cung cấp là hợp lệ hay không
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Cập nhật thông tin người dùng trong cơ sở dữ liệu
                        var curUser = db.nguoidungs.FirstOrDefault(t => t.email == user.tk);
                        curUser.TenND = updatedUser.TenND;
                        curUser.Avatar = updatedUser.Avatar;
                        curUser.Tuoi = updatedUser.Tuoi;
                        curUser.SDT = updatedUser.SDT;
                        curUser.DiaChi = updatedUser.DiaChi;

                        // Lưu các thay đổi vào cơ sở dữ liệu
                        db.SaveChanges();

                        // Hiển thị thông báo thành công (tùy chọn)
                        TempData["SuccessMessage"] = "Thông tin người dùng đã được cập nhật thành công.";

                        // Điều hướng về trang User để hiển thị thông tin người dùng mới
                        return RedirectToAction("User");
                    }
                    catch (Exception ex)
                    {
                        // Xử lý ngoại lệ nếu có
                        TempData["ErrorMessage"] = "Đã xảy ra lỗi khi cập nhật thông tin người dùng: " + ex.Message;
                    }
                }

                // Nếu không thành công, hiển thị lại view User để người dùng có thể sửa lại thông tin
                return View(updatedUser);
            }
        }*/

        public ActionResult Cart()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["nguoidung"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // Lấy tài khoản từ session
            taikhoan taikhoan = Session["nguoidung"] as taikhoan;


            // Lấy danh sách sản phẩm trong giỏ hàng của người dùng hiện tại
            var listProduct = new Model1().giohangs.Where(item => item.tk == taikhoan.tk).ToList();
            return View(listProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cart(FormCollection collection)
        {
            string submitButton = collection["submitButton"];
            if (submitButton == "Pay")
            {
                return RedirectToAction("Index", "Pay");
            }    
            return View();
        }
        [HttpPost]
        public JsonResult UpdateCartItem(string iD, int soLuong)
        {
            var existingCartItem = db.giohangs.Find(iD);
            if (existingCartItem != null)
            {
                existingCartItem.SoLuong = soLuong;
                db.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult DeleteCartItem(string iD)
        {
            giohang giohang = db.giohangs.Find(iD); 
            if (giohang != null)
            {
                db.giohangs.Remove(giohang);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        private string GenerateUniqueMaND()
        {
            string uniqueID;

            do
            {
                uniqueID = GenerateMaND();
            } while (!IsMaNDUnique(uniqueID));

            return uniqueID;
        }

        private string GenerateMaND()
        {
            // Ví dụ đơn giản: sử dụng GUID và chỉ lấy 8 ký tự đầu tiên
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        private bool IsMaNDUnique(string id)
        {
            return !db.giohangs.Any(u => u.id == id);
        }
        public ActionResult User()
        {
            var user = Session["nguoidung"] as taikhoan;
            var curUser = db.nguoidungs.FirstOrDefault(t => t.email == user.tk);
            return View(curUser);



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult User(nguoidung model)
        {

            if (ModelState.IsValid)
            {
                // TODO: Add update logic here
                db.nguoidungs.Attach(model);
                db.Entry(model).Property(x => x.TenND).IsModified = true;
               
                HttpPostedFileBase Image = Request.Files["Image"];
                if (Image != null && Image.ContentLength > 0)
                {
                    string fileName = Image.FileName;
                    string urlImage = Server.MapPath("~/assets/images/" + fileName);
                    Image.SaveAs(urlImage);
                    model.Avatar = fileName;
                }
                else
                {
                    db.Entry(model).Property(x => x.Avatar).IsModified = false;
                }
                db.Entry(model).Property(x => x.Tuoi).IsModified = true;
                db.Entry(model).Property(x => x.email).IsModified = true;
                db.Entry(model).Property(x => x.DiaChi).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);

        }
        public ActionResult CheckSession()
        {
            if (Session["nguoidung"] == null)
            {
                // Nếu session "nguoidung" là null, trả về URL của trang đăng nhập
                return RedirectToAction("Index", "Login", new { id = 1 });
            }
            else
            {
                // Nếu session "nguoidung" tồn tại, trả về URL của trang người dùng
                return RedirectToAction("User", "Home");
            }
        }
    }

}