using PaymentSystem.Controllers;
using QuanLyHocSinhDuHoc.CommonXuLy;
using QuanLyHocSinhDuHoc.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyHocSinhDuHoc.Controllers
{
    public class NamHocController : BaseController
    {
        dbXulyTThsEntities db = new dbXulyTThsEntities();

        // GET: NamHoc
        public ActionResult Themmoi()
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {
                return View();
            } return RedirectToAction("Index", "Home");
        }
        public ActionResult ThemmoiR(int id_hb)
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {
                Session["id_hocba"] = id_hb;
                return View(id_hb);
            } return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public JsonResult SaveNamhoc(NAMHOC namhoc)
        {
            if (ModelState.IsValid)
            {
                namhoc.id_HB = (int)Session["id_hocba"];
                db.NAMHOCs.Add(namhoc);
                db.SaveChanges();
                return Json(namhoc, JsonRequestBehavior.AllowGet);
            }
            return Json("NO", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadFormNhapDiem(int id) //id: id_namhoc tuong ung
        {
            Session["id_namhoc"] = id;
            return Json(id, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveDiem3Ky(KIHOC diemKyI, KIHOC diemKyII, KIHOC diemKyCN)
        {
            if (ModelState.IsValid)
            {
                diemKyI.id_NAMHOC = (int)Session["id_namhoc"];
                db.KIHOCs.Add(diemKyI);
                diemKyII.id_NAMHOC = (int)Session["id_namhoc"];
                db.KIHOCs.Add(diemKyII);
                diemKyCN.id_NAMHOC = (int)Session["id_namhoc"];
                db.KIHOCs.Add(diemKyCN);
                db.SaveChanges();
                //để cập nhật lại năm học đã nhập đủ điểm 3 kì
                List<KIHOC> diemnamhoc = db.KIHOCs.Where(n => n.id_NAMHOC == diemKyI.id_NAMHOC).ToList();
                if (diemnamhoc.Count > 0 && diemnamhoc.Count == 3)
                {
                    NAMHOC namhoc = db.NAMHOCs.Find(diemKyI.id_NAMHOC);
                    namhoc.StatusNH = true;
                    db.Entry(namhoc).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return Json("YES", JsonRequestBehavior.AllowGet);
            }
            return Json("NO", JsonRequestBehavior.AllowGet);
        }
        public ActionResult ThemmoiDiem(int id_hb)
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {
                var model = db.NAMHOCs.Where(n => n.id_HB == id_hb).ToList();
                if (model.Count > 0) return View(model);
                else return RedirectToAction("ThemmoiR", "NamHoc", id_hb);
            } return RedirectToAction("Index", "Home");
        }

        public ActionResult Suanamhoc(int id)
        {
            if (Session["id_hsDetail"]!=null)
               ViewBag.id_hs = (int)Session["id_hsDetail"];
            else
            {
                if (Session["id_HS"] != null)
                    ViewBag.id_hs = (int)Session["id_HS"];
            }
           
            return View(db.NAMHOCs.Find(id));
        }
        [HttpPost]
        public ActionResult Suanamhoc(NAMHOC namhoc)
        {
            if (ModelState.IsValid)
            {
                int id_hs =0;
                if (Session["id_hsDetail"] != null)
                    id_hs = (int)Session["id_hsDetail"];
                else
                {
                    if (Session["id_HS"] != null)
                       id_hs = (int)Session["id_HS"];
                }
           
               db.Entry(namhoc).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();             
                Session["chuyenTab"] = 5;
                return RedirectToAction("DetailChung/" + id_hs, "HocSinh");
            }
            return View(db.NAMHOCs.Find(namhoc.id));
        }
        public ActionResult Xoanamhoc(int id)
        {
            NAMHOC namhoc = db.NAMHOCs.Find(id);
            if(namhoc!=null)
            {
                List<KIHOC> listKiHoc = db.KIHOCs.Where(n => n.id_NAMHOC == namhoc.id).ToList();
                foreach(var item in listKiHoc)
                {
                    db.KIHOCs.Remove(item);
                }
                db.NAMHOCs.Remove(namhoc);
                db.SaveChanges();
            }
            Session["chuyenTab"] = 5;
            if(Session["id_hsDetail"]!=null )                    
            {
                int  id_hs = (int)Session["id_HS"];
                return RedirectToAction("DetailChung/" + id_hs, "HocSinh");
            }
            if (Session["id_HS"] != null)
            {
                int id_hs = (int)Session["id_HS"];
                return RedirectToAction("DetailChung/" + id_hs, "HocSinh");
            }
            return View();
             
        }


        public ActionResult Suadiem(int id_nh)
        {
            ViewBag.namhoc = db.NAMHOCs.Find(id_nh).ThoiGian;
            Session["capnhatDiem_idNH"] = id_nh;
            List<KIHOC> listkihoc = db.KIHOCs.Where(n => n.id_NAMHOC == id_nh).ToList();
            return View(listkihoc);
        }
        [HttpPost]
        public JsonResult SaveDiemCapnhat(KIHOC diemKyI, KIHOC diemKyII, KIHOC diemKyCN)
        {
            if (ModelState.IsValid)
            {
                
                diemKyI.id_NAMHOC = (int)Session["capnhatDiem_idNH"];              
                diemKyII.id_NAMHOC = (int)Session["capnhatDiem_idNH"];               
                diemKyCN.id_NAMHOC = (int)Session["capnhatDiem_idNH"];
                db.Entry(diemKyI).State = System.Data.Entity.EntityState.Modified;
                db.Entry(diemKyII).State = System.Data.Entity.EntityState.Modified;
                db.Entry(diemKyCN).State = System.Data.Entity.EntityState.Modified;                
                db.SaveChanges();
                if (Session["id_hsDetail"] != null)
                {
                    int id_hs = (int)Session["id_hsDetail"];
                    Session["chuyenTab"] = 5;
                    return Json(id_hs, JsonRequestBehavior.AllowGet);
                }                
            }
            return Json("NO", JsonRequestBehavior.AllowGet);
        }

    }
}