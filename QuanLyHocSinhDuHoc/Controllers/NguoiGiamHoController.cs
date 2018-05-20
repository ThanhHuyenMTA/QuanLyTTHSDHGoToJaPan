using PaymentSystem.Controllers;
using QuanLyHocSinhDuHoc.CommonXuLy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyHocSinhDuHoc.Models.Entities;
using System.IO;

namespace QuanLyHocSinhDuHoc.Controllers
{
    public class NguoiGiamHoController : BaseController
    {
        dbXulyTThsEntities db = new dbXulyTThsEntities();
        // GET: NguoiGiamHo
        public ActionResult Themmoi(int? id_hs)
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {
                 HOCSINH hocsinh = db.HOCSINHs.Find(id_hs);
                
                     Session["file"] = null;
                     return View();
                 
            } return RedirectToAction("Index", "Home");
        }
        public ActionResult ThemmoiR(int? id_hs)
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {
                 HOCSINH hocsinh = db.HOCSINHs.Find(id_hs);
                 if (quyenNguoiDung.Nhanvien.id == hocsinh.NguoiTao)
                 {
                     Session["file"] = null;
                     Session["id_HS"] = id_hs;
                     return View();
                 }
            } return RedirectToAction("Index", "Home");
        }
        public ActionResult Loai1()
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {
                return View();
            } return RedirectToAction("Index", "Home");
        }
        public ActionResult Loai2()
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {
                return View();
            } return RedirectToAction("Index", "Home");
        }
        public ActionResult Loai3()
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {
                return View();
            } return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult Themmoi1(NGUOIGIAMHO cmt1)
        {
            if (ModelState.IsValid)
            {
                if (Session["file"] != null)
                    cmt1.file_CMTNGH = (string)Session["file"];
                db.NGUOIGIAMHOes.Add(cmt1);
                db.SaveChanges();
                //Cập nhật lại bảng học sinh
                if(Session["id_HS"]!=null)
                {
                    int id_HS = (int)Session["id_HS"];
                    HOCSINH hocsinh = db.HOCSINHs.Find(id_HS);
                    hocsinh.id_NgGiamHo = cmt1.SoCMT;
                    db.Entry(hocsinh).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                if (Session["id_hsDetail"] != null)
                {
                    Session["chuyenTab"] = 6;
                    return Json((int)Session["id_hsDetail"], JsonRequestBehavior.AllowGet);
                }
                
                return Json("Thêm thành công!", JsonRequestBehavior.AllowGet);
            }
            return Json("Thêm thất bại!", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Themmoi2(NGUOIGIAMHO cmt2)
        {
            if (ModelState.IsValid)
            {
                if (Session["file"] != null)
                    cmt2.file_CMTNGH = (string)Session["file"];
                db.NGUOIGIAMHOes.Add(cmt2);
                db.SaveChanges();
                //Cập nhật lại bảng học sinh
                if (Session["id_HS"] != null)
                {
                    int id_HS = (int)Session["id_HS"];
                    HOCSINH hocsinh = db.HOCSINHs.Find(id_HS);
                    hocsinh.id_NgGiamHo = cmt2.SoCMT;
                    db.Entry(hocsinh).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                if (Session["id_hsDetail"] != null)
                {
                    Session["chuyenTab"] = 6;
                    return Json((int)Session["id_hsDetail"], JsonRequestBehavior.AllowGet);
                }
                return Json("Thêm thành công!", JsonRequestBehavior.AllowGet);
            }
            return Json("Thêm thất bại!", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Themmoi3(NGUOIGIAMHO cmt3)
        {
            if (ModelState.IsValid)
            {
                if (Session["file"] != null)
                    cmt3.file_CMTNGH = (string)Session["file"];
                db.NGUOIGIAMHOes.Add(cmt3);
                db.SaveChanges();
                //Cập nhật lại bảng học sinh
                if (Session["id_HS"] != null)
                {
                    int id_HS = (int)Session["id_HS"];
                    HOCSINH hocsinh = db.HOCSINHs.Find(id_HS);
                    hocsinh.id_NgGiamHo = cmt3.SoCMT;
                    db.Entry(hocsinh).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                if (Session["id_hsDetail"] != null)
                {
                    Session["chuyenTab"] = 6;
                    return Json((int)Session["id_hsDetail"], JsonRequestBehavior.AllowGet);
                }
                return Json("Thêm thành công!", JsonRequestBehavior.AllowGet);
            }
            return Json("Thêm thất bại!", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail()
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {
                int id_hs = (int)Session["id_hsDetail"];
                HOCSINH hocsinh = db.HOCSINHs.Find(id_hs);

                NGUOIGIAMHO nguoigiamho = db.NGUOIGIAMHOes.Find(hocsinh.id_NgGiamHo);
                return View(nguoigiamho);

            } return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public JsonResult UpLoadFileCMT()
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
                    if (hocsinh.id_NgGiamHo != null)
                    {
                        NGUOIGIAMHO nguoigiamho = db.NGUOIGIAMHOes.Find(hocsinh.id_NgGiamHo);
                        nguoigiamho.file_CMTNGH = fileName;
                        db.Entry(nguoigiamho).State = System.Data.Entity.EntityState.Modified;
                        Session["chuyenTab"] = 6;
                        db.SaveChanges();
                        return Json(hocsinh.id, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(fileName, JsonRequestBehavior.AllowGet);
            }
            Session["file"] = null;
            return Json("Khong", JsonRequestBehavior.AllowGet);
        }
        public ActionResult SuaNGH(string soCMT, int loaiCMT)
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {
                Session["file"] = null;
                NGUOIGIAMHO nggiamho = db.NGUOIGIAMHOes.Find(soCMT);
                ViewBag.LoaiCMT = loaiCMT;
                HOCSINH hs = db.HOCSINHs.SingleOrDefault(n => n.id_NgGiamHo == soCMT);
                if ( hs!=null && quyenNguoiDung.Nhanvien.id == hs.NguoiTao)
                {
                    ViewBag.id_hs = hs.id;
                    Session["chuyenTab"] = 6;
                    return View(nggiamho);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult SuaNGH(NGUOIGIAMHO ngGiamho) //chú ý kh lấy đc id_hs=> update bảng học sinh khi đã chỉnh sửa
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {
                if (ModelState.IsValid)
                {
                    int id_hs = (int)Session["id_hsDetail"];
                    if (Session["file"] != null)
                    {
                        ngGiamho.file_CMTNGH = (string)Session["file"];
                    }
                    db.Entry(ngGiamho).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    Session["chuyenTab"] = 6;
                    return RedirectToAction("DetailChung/" + id_hs, "HocSinh");
                }
                return View(ngGiamho);
            } return RedirectToAction("Index", "Home");
        }

    }
}