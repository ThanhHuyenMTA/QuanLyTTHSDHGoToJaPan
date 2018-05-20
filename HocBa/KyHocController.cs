using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyHocSinhDuHoc.Models.Entities;
using PaymentSystem.Controllers;
using QuanLyHocSinhDuHoc.CommonXuLy;

namespace QuanLyHocSinhDuHoc.Controllers
{
    public class KyHocController : BaseController
    {
        dbXulyTThsEntities db = new dbXulyTThsEntities();
        // GET: KyHoc
        //Thông tin kì học (Nhập điểm)
        [HttpGet]
        public JsonResult ViewNhapDiem(int id) //id: id_namhoc tuong ung
        {
            Session["id_namhoc"] = id;
            return Json(id, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult NhapDiem(KIHOC kihoc)
        {
            if (ModelState.IsValid)
            {
                kihoc.id_NAMHOC = (int)Session["id_namhoc"];
                db.KIHOCs.Add(kihoc);
                db.SaveChanges();
                //để cập nhật lại năm học đã nhập đủ điểm 3 kì
                List<KIHOC> diemnamhoc = db.KIHOCs.Where(n => n.id_NAMHOC == kihoc.id_NAMHOC).ToList();
                if(diemnamhoc.Count>0 && diemnamhoc.Count ==3)
                {
                    NAMHOC namhoc = db.NAMHOCs.Find(kihoc.id_NAMHOC);
                    namhoc.Status = true;
                    db.Entry(namhoc).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return Json(kihoc, JsonRequestBehavior.AllowGet);
            }
            return Json(kihoc, JsonRequestBehavior.AllowGet);
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
                if(diemnamhoc.Count>0 && diemnamhoc.Count ==3)
                {
                    NAMHOC namhoc = db.NAMHOCs.Find(diemKyI.id_NAMHOC);
                    namhoc.Status = true;
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
    }
}