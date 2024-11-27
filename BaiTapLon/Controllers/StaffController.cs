using BaiTapLon.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaiTapLon.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff
        Model1 db = new Model1();
        public ActionResult Index(string sortOrder)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.TenNVSortParm = String.IsNullOrEmpty(sortOrder) ? "tenNV_desc" : "";
            ViewBag.TuoiSortParm = sortOrder == "tuoi" ? "tuoi_desc" : "tuoi";
            ViewBag.ChucVuSortParm = sortOrder == "chucvu" ? "chucvu_desc" : "chucvu";
            ViewBag.EmailSortParm = sortOrder == "email" ? "email_desc" : "email";
            ViewBag.SDTSortParm = sortOrder == "sdt" ? "sdt_desc" : "sdt";
            ViewBag.DiaChiSortParm = sortOrder == "diachi" ? "diachi_desc" : "diachi";

            var nhanviens = from nv in db.nhanviens select nv;

            switch (sortOrder)
            {
                case "tenNV_desc":
                    nhanviens = nhanviens.OrderByDescending(nv => nv.TenNV);
                    break;
                case "tuoi":
                    nhanviens = nhanviens.OrderBy(nv => nv.Tuoi);
                    break;
                case "tuoi_desc":
                    nhanviens = nhanviens.OrderByDescending(nv => nv.Tuoi);
                    break;
                case "chucvu":
                    nhanviens = nhanviens.OrderBy(nv => nv.chucvu.TenCV);
                    break;
                case "chucvu_desc":
                    nhanviens = nhanviens.OrderByDescending(nv => nv.chucvu.TenCV);
                    break;
                case "email":
                    nhanviens = nhanviens.OrderBy(nv => nv.email);
                    break;
                case "email_desc":
                    nhanviens = nhanviens.OrderByDescending(nv => nv.email);
                    break;
                case "sdt":
                    nhanviens = nhanviens.OrderBy(nv => nv.SDT);
                    break;
                case "sdt_desc":
                    nhanviens = nhanviens.OrderByDescending(nv => nv.SDT);
                    break;
                case "diachi":
                    nhanviens = nhanviens.OrderBy(nv => nv.DiaChi);
                    break;
                case "diachi_desc":
                    nhanviens = nhanviens.OrderByDescending(nv => nv.DiaChi);
                    break;
                default:
                    nhanviens = nhanviens.OrderBy(nv => nv.TenNV);
                    break;
            }

            return View(nhanviens.ToList());

        }

        // GET: Staff/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Staff/Create
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            var chucVuList = db.chucvus.Select(c => new SelectListItem
            {
                Value = c.MaCV,
                Text = c.TenCV
            }).ToList();

            ViewBag.ChucVuList = chucVuList;

            return View();
        }

        // POST: Staff/Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StaffViewModel model)
        {
            nhanvien nhanvien = new nhanvien();
            taikhoan taikhoan = new taikhoan();
            // TODO: Add insert logic here
            HttpPostedFileBase Image = Request.Files["Image"];
            if (Image != null && Image.ContentLength > 0)
            {
                string fileName = Image.FileName;
                string urlImage = Server.MapPath("~/assets/images/" + fileName);
                Image.SaveAs(urlImage);
                nhanvien.Avatar = fileName;
            }
            if (ModelState.IsValid)
            {
                // Thiết lập thông tin nhân viên
                nhanvien.TenNV = model.TenNV;
                nhanvien.Tuoi = model.Tuoi;
                nhanvien.MaCV = model.MaCV;
                nhanvien.email = model.email;
                nhanvien.SDT = model.SDT;
                nhanvien.DiaChi = model.DiaChi;

                // Thiết lập thông tin cho tài khoản nhân viên
                taikhoan.tk = model.email;
                taikhoan.mk = model.MatKhau;
                taikhoan.quyen = "admin";
                db.taikhoans.Add(taikhoan);
                // Lưu nhân viên vào cơ sở dữ liệu
                db.nhanviens.Add(nhanvien);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            var chucVuList = db.chucvus.Select(c => new SelectListItem
            {
                Value = c.MaCV,
                Text = c.TenCV
            }).ToList();

            ViewBag.ChucVuList = chucVuList;

            return View(model);
            
        }

        // GET: Staff/Edit/5
        public ActionResult Edit(string email)
        {
            if (email == null)
            {
                // Xử lý khi MaNV là null, có thể làm redirect hoặc hiển thị thông báo lỗi
                return RedirectToAction("Index");
            }
            var item = db.nhanviens.Find(email);
            var chucVuList = db.chucvus.Select(c => new SelectListItem
            {
                Value = c.MaCV,
                Text = c.TenCV
            }).ToList();

            ViewBag.ChucVuList = chucVuList;
            return View(item);
        }

        // POST: Staff/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(nhanvien model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add update logic here
                db.nhanviens.Attach(model);
                db.Entry(model).Property(x => x.TenNV).IsModified = true;
                
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
                db.Entry(model).Property(x => x.MaCV).IsModified = true;
                db.Entry(model).Property(x => x.SDT).IsModified = true;
                db.Entry(model).Property(x => x.DiaChi).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
          
        }

        // GET: Staff/Delete/5   
        public ActionResult Delete(string email)
        {
            if (email == null)
            {
                

                return RedirectToAction("Index");
            }
            // Xử lý khi MaNV là null, có thể làm redirect hoặc hiển thị thông báo lỗi

            nhanvien item = db.nhanviens.Find(email);
            var chucVuList = db.chucvus.Select(c => new SelectListItem
            {
                Value = c.MaCV,
                Text = c.TenCV
            }).ToList();

            ViewBag.ChucVuList = chucVuList;
            return View(item);
        }

        // POST: Staff/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(string email)
        {
            if (ModelState.IsValid)
            {
                nhanvien nhanvien = db.nhanviens.Find(email);
                taikhoan taikhoan = db.taikhoans.Find(email);
                db.nhanviens.Remove(nhanvien);
                db.taikhoans.Remove(taikhoan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
            
        }
       
    }
}
