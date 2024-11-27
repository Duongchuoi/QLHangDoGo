using BaiTapLon.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaiTapLon.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        Model1 db = new Model1();
        public ActionResult Index(string sortOrder)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.TenSPSortParm = sortOrder == "tenSP" ? "tenSP_desc" : "tenSP";
            ViewBag.TkSortParm = sortOrder == "tk" ? "tk_desc" : "tk";
            ViewBag.ThoiGianSortParm = sortOrder == "thoiGian" ? "thoiGian_desc" : "thoiGian";
            ViewBag.SoLuongSortParm = sortOrder == "soLuong" ? "soLuong_desc" : "soLuong";
            ViewBag.DiaChiSortParm = sortOrder == "diaChi" ? "diaChi_desc" : "diaChi";

            var donhangs = from s in db.donhangs select s;

            switch (sortOrder)
            {
                case "tenSP_desc":
                    donhangs = donhangs.OrderByDescending(s => s.sanpham.TenSP);
                    break;
                case "tk":
                    donhangs = donhangs.OrderBy(s => s.tk);
                    break;
                case "tk_desc":
                    donhangs = donhangs.OrderByDescending(s => s.tk);
                    break;
                case "thoiGian":
                    donhangs = donhangs.OrderBy(s => s.ThoiGian);
                    break;
                case "thoiGian_desc":
                    donhangs = donhangs.OrderByDescending(s => s.ThoiGian);
                    break;
                case "soLuong":
                    donhangs = donhangs.OrderBy(s => s.SoLuong);
                    break;
                case "soLuong_desc":
                    donhangs = donhangs.OrderByDescending(s => s.SoLuong);
                    break;
                case "diaChi":
                    donhangs = donhangs.OrderBy(s => s.DiaChi);
                    break;
                case "diaChi_desc":
                    donhangs = donhangs.OrderByDescending(s => s.DiaChi);
                    break;
                default:
                    donhangs = donhangs.OrderBy(s => s.ThoiGian);
                    break;
            }

            return View(donhangs.ToList());

        }
        
        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
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

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Order/Edit/5
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

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Order/Delete/5
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
    }
}
