using BaiTapLon.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaiTapLon.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        Model1 dbConect = new Model1();

        public ActionResult Index(string sortOrder)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.MaSPSortParm = String.IsNullOrEmpty(sortOrder) ? "maSP_desc" : "";
            ViewBag.TenSPSortParm = sortOrder == "tenSP" ? "tenSP_desc" : "tenSP";
            ViewBag.MoTaSortParm = sortOrder == "moTa" ? "moTa_desc" : "moTa";
            ViewBag.LoaiSortParm = sortOrder == "loai" ? "loai_desc" : "loai";
            ViewBag.SoLuongSortParm = sortOrder == "soLuong" ? "soLuong_desc" : "soLuong";
            ViewBag.GiaSortParm = sortOrder == "gia" ? "gia_desc" : "gia";

            var sanphams = from s in dbConect.sanphams select s;

            switch (sortOrder)
            {
                case "maSP_desc":
                    sanphams = sanphams.OrderByDescending(s => s.MaSP);
                    break;
                case "tenSP":
                    sanphams = sanphams.OrderBy(s => s.TenSP);
                    break;
                case "tenSP_desc":
                    sanphams = sanphams.OrderByDescending(s => s.TenSP);
                    break;
                case "moTa":
                    sanphams = sanphams.OrderBy(s => s.MoTa);
                    break;
                case "moTa_desc":
                    sanphams = sanphams.OrderByDescending(s => s.MoTa);
                    break;
                case "loai":
                    sanphams = sanphams.OrderBy(s => s.Loai);
                    break;
                case "loai_desc":
                    sanphams = sanphams.OrderByDescending(s => s.Loai);
                    break;
                case "soLuong":
                    sanphams = sanphams.OrderBy(s => s.SoLuong);
                    break;
                case "soLuong_desc":
                    sanphams = sanphams.OrderByDescending(s => s.SoLuong);
                    break;
                case "gia":
                    sanphams = sanphams.OrderBy(s => s.Gia);
                    break;
                case "gia_desc":
                    sanphams = sanphams.OrderByDescending(s => s.Gia);
                    break;
                default:
                    sanphams = sanphams.OrderBy(s => s.MaSP);
                    break;
            }

            return View(sanphams.ToList());
        }


        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create( sanpham model)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }

            return View(model);
        }

        // POST: Product/Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(sanpham model,FormCollection collection)
        {
            try
            {
                HttpPostedFileBase Image = Request.Files["Image"];
                if (Image != null && Image.ContentLength > 0)
                {
                    string fileName = Image.FileName;
                    string urlImage = Server.MapPath("~/assets/images/" + fileName);
                    Image.SaveAs(urlImage);
                    model.Anh = fileName;
                }
                // TODO: Add delete logic here
                if (ModelState.IsValid)
                {
                    dbConect.sanphams.Add(model);
                    dbConect.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(string MaSP)
        {
            if (MaSP == null)
            {
                // Xử lý khi MaNV là null, có thể làm redirect hoặc hiển thị thông báo lỗi
                return RedirectToAction("Index");
            }
            var item = dbConect.sanphams.Find(MaSP);
            
            return View(item);
        }

        // POST: Product/Edit/5
        [HttpPost, ActionName("Save")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(sanpham model)
        {
            
                if (ModelState.IsValid)
                {
                    // TODO: Add update logic here
                    dbConect.sanphams.Attach(model);
                    dbConect.Entry(model).Property(x => x.TenSP).IsModified = true;
                    dbConect.Entry(model).Property(x => x.MoTa).IsModified = true;
                    HttpPostedFileBase Image = Request.Files["Image"];
                    if (Image != null && Image.ContentLength > 0)
                    {
                        string fileName = Image.FileName;
                        string urlImage = Server.MapPath("~/assets/images/" + fileName);
                        Image.SaveAs(urlImage);
                        model.Anh = fileName;
                    }
                    else
                    {
                        dbConect.Entry(model).Property(x => x.Anh).IsModified = false;
                    }
                    dbConect.Entry(model).Property(x => x.Loai).IsModified = true;
                    dbConect.Entry(model).Property(x => x.SoLuong).IsModified = true;
                    dbConect.Entry(model).Property(x => x.Gia).IsModified = true;
                    dbConect.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(model);

        }

        // GET: Product/Delete/5
        public ActionResult Delete(string MaSP)
        {
            if (MaSP == null)
            {
                // Xử lý khi MaNV là null, có thể làm redirect hoặc hiển thị thông báo lỗi
                return RedirectToAction("Index");
            }
            sanpham item = dbConect.sanphams.Find(MaSP);
            return View(item);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(string MaSP)
        {
            if (ModelState.IsValid)
            {
                sanpham item = dbConect.sanphams.Find(MaSP);
                dbConect.sanphams.Remove(item);
                dbConect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
