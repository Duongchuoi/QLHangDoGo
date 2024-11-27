using BaiTapLon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BaiTapLon.Controllers
{
    public class LoginController : Controller
    {
        Models.Model1 db = new Models.Model1();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // GET: Login/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string submitButton, RegisterViewModel obj)
        {
            if (submitButton == "Login")
            {
                // Thực hiện logic đăng nhập
                Models.taikhoan check = db.taikhoans.FirstOrDefault(model => model.tk == obj.Email && model.mk == obj.Password && model.quyen =="nguoidung");

                if (check != null)
                {
                    // Đăng nhập thành công
                    // Lưu trang thái đăng nhập vào session
                    var taikhoan = db.taikhoans.Find(obj.Email);
                    Session["nguoidung"] = taikhoan;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Đăng nhập thất bại
                    ModelState.AddModelError("", "Đăng nhập thất bại");
                }
            }
            else if (submitButton == "Register")
            {
                if (db.nguoidungs.Any(nd => nd.email == obj.Email))
                {
                    ViewBag.SignUpSuccess = true;
                    // Nếu đã tồn tại, hiển thị thông báo lỗi
                    ModelState.AddModelError("EmailRegister", "Email đã được sử dụng. Vui lòng chọn một email khác.");
                    return View(obj);
                }
                // Thực hiện logic đăng ký
                nguoidung nguoidung = new nguoidung();
                taikhoan taikhoan = new taikhoan();
                // Thiết lập thông tin người dùng
                nguoidung.TenND = obj.TenND;               
                nguoidung.Tuoi = obj.Tuoi;
                nguoidung.email = obj.Email;
                nguoidung.SDT = obj.SDT;
                nguoidung.DiaChi = obj.DiaChi;

                // Thiết lập thông tin cho tài khoản người dùng
                taikhoan.tk = obj.Email;
                taikhoan.mk = obj.Password;
                taikhoan.quyen = "nguoidung";

                // Lưu thông tin vào db
                db.nguoidungs.Add(nguoidung);
                db.taikhoans.Add(taikhoan);
                db.SaveChanges();

                // Điều hướng đến trang đăng nhập hoặc trang chính
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }

        
}
