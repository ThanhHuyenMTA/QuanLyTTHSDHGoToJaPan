﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyHocSinhDuHoc.Models.Entities;
using QuanLyHocSinhDuHoc.CommonXuLy;
using System.IO;
using PaymentSystem.Controllers;

namespace QuanLyHocSinhDuHoc.Controllers
{
    public class BangTotNghiepController : BaseController
    {       
        dbXulyTThsEntities db = new dbXulyTThsEntities();
        
        // GET: BangTotNghiep
        public ActionResult Themmoi(int? id_hs)
        {
             ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
             if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
             {
                 Session["file"] = null;
                 Session["id_hsDetail"] = null;
                 return View();
             }
             return RedirectToAction("Index", "Home");
        }
        public ActionResult ThemmoiR(int? id_hs)
        {
             ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
             if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
             {
                 Session["file"] = null;
                 Session["chuyenTab"] = 4;
                 Session["id_HS"] = id_hs;
                 return View(id_hs);
             } return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public JsonResult Themmoi(BANGTOTNGHIEP btn)
        {

            if (ModelState.IsValid)
            {
                if (Session["file"] != null)
                    btn.fileBTN = (string)Session["file"];
                db.BANGTOTNGHIEPs.Add(btn);
                db.SaveChanges();
                //Cập nhật lại bảng học sinh
                int id_HS = (int)Session["id_HS"];
                HOCSINH hocsinh = db.HOCSINHs.Find(id_HS);
                hocsinh.id_BTN = btn.id;
                db.Entry(hocsinh).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json("Thêm mới thành công", JsonRequestBehavior.AllowGet);
            }
            return Json("Thêm mới thất bại", JsonRequestBehavior.AllowGet);
        }
        public ActionResult DetailBTN()
        {
             ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
             if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
             {
                int id_hs = (int)Session["id_hsDetail"];                 
                HOCSINH hocsinh = db.HOCSINHs.Find(id_hs);
               
                if (hocsinh.id_BTN > 0)
                {
                    ViewBag.ThongbaoBTN = "OK";
                    BANGTOTNGHIEP btn = db.BANGTOTNGHIEPs.SingleOrDefault(n => n.id == hocsinh.id_BTN);
                    return View(btn);
                }
                ViewBag.ThongbaoBTN = "NO";
                return View();
                 
             } return RedirectToAction("Index", "Home");
        }
        //upload File -> them file trong phan chi tiet
        [HttpPost]
        public JsonResult UpLoadFileBTN()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var file = Request.Files["HelpSectionFile"];
                //lưu tên file
                var fileName = Path.GetFileName(file.FileName);
                //lưu đường dẫn
                var path = Path.Combine(Server.MapPath("~/Content/filePDF"), fileName);
                // file is uploaded
                var type = file.ContentType;
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Thongbao = "File đã tồn tại";
                }
                else
                {
                    if (type == "application/docx" || type == "application/pdf")
                        file.SaveAs(path);
                }
                Session["file"] = fileName;
                if (Session["id_hsDetail"] != null)
                {
                    int id_hs = (int)Session["id_hsDetail"];
                    HOCSINH hocsinh = db.HOCSINHs.Find(id_hs);
                    if (hocsinh.id_BTN != null)
                    {
                        BANGTOTNGHIEP btn = db.BANGTOTNGHIEPs.Find(hocsinh.id_BTN);
                        btn.fileBTN = fileName;
                        db.Entry(btn).State = System.Data.Entity.EntityState.Modified;
                        Session["chuyenTab"] = 4;
                        db.SaveChanges();
                        return Json(hocsinh.id, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(fileName, JsonRequestBehavior.AllowGet);
            }
            Session["file"] = null;
            return Json("Khong", JsonRequestBehavior.AllowGet);
        }
        public ActionResult SuaBTN(int id)
        {
             ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
             if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
             {
                 Session["file"] = null;
                 BANGTOTNGHIEP btn = db.BANGTOTNGHIEPs.Find(id);
                 HOCSINH hs = db.HOCSINHs.SingleOrDefault(n => n.id_BTN == btn.id);
                 ViewBag.id_hs = hs.id;
                 Session["chuyenTab"] = 4;
                 return View(btn);
             } return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult SuaBTN(BANGTOTNGHIEP btn)
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {

                if (ModelState.IsValid)
                {
                    if (Session["file"] != null)
                        btn.fileBTN = (string)Session["file"];
                    HOCSINH hs = db.HOCSINHs.SingleOrDefault(n => n.id_BTN == btn.id);
                    db.Entry(btn).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    Session["chuyenTab"] = 4;
                    return RedirectToAction("DetailChung/" + hs.id, "HocSinh");

                }
                return View(btn);
            } return RedirectToAction("Index", "Home");
        }

    }
}