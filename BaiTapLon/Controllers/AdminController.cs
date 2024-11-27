using BaiTapLon.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaiTapLon.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        Models.Model1 db = new Models.Model1();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(taikhoan obj)
        {
            taikhoan check = db.taikhoans.FirstOrDefault(model => model.tk == obj.tk && model.mk == obj.mk && model.quyen == "admin");

            if (check != null)
            {
                // Đăng nhập thành công
                // Lưu trang thái đăng nhập vào session
                Session["admin"] = check;
                return RedirectToAction("Index", "Product");
            }
            else
            {
                // Đăng nhập thất bại
                ModelState.AddModelError("", "Đăng nhập thất bại");
            }
            return View();
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult RevenueStatistics()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            List<MonthlyRevenue> monthlyRevenues = new List<MonthlyRevenue>();

            // Lấy doanh thu hàng tháng từ cơ sở dữ liệu
            var revenueByMonth = db.donhangs
                .GroupBy(d => d.ThoiGian.Month)
                .Select(group => new
                {
                    Month = group.Key,
                    TotalRevenue = group.Sum(d => d.SoLuong * d.sanpham.Gia)
                })
                .OrderBy(r => r.Month);

            // Chuyển dữ liệu sang đối tượng MonthlyRevenue
            foreach (var item in revenueByMonth)
            {
                monthlyRevenues.Add(new MonthlyRevenue
                {
                    Month = item.Month,
                    Revenue = (decimal)item.TotalRevenue
                });
            }

            return View(monthlyRevenues);
        }

    }
}
