using PaymentSystem.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyHocSinhDuHoc.Models.Entities;
using QuanLyHocSinhDuHoc.CommonXuLy;

namespace QuanLyHocSinhDuHoc.Controllers
{
    public class CaNhanNguoiDungController : BaseController
    {
        dbXulyTThsEntities db = new dbXulyTThsEntities();
        // GET: CaNhanNguoiDung
        public ActionResult DetailTaiKhoan(int id)
        {
            return View(db.NHANVIENs.Find(id));
        }
        public ActionResult CapNhatTaiKhoan(int id)
        {
            return View(db.NHANVIENs.Find(id));
        }
        [HttpPost]
        public ActionResult CapNhatTaiKhoan(NHANVIEN nhanvien)
        {
            if(ModelState.IsValid)
            {
                NHANVIEN nvOld = db.NHANVIENs.Find(nhanvien.id);
                nhanvien.MatKhau = nvOld.MatKhau;
                nhanvien.TenDangNhap = nvOld.TenDangNhap;
                db.Entry(nhanvien).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DetailTaiKhoan", "CaNhanNguoiDung", new {id =nhanvien.id});
            } return View();
           
        }
        public ActionResult DoiMatKhau(int id)
        {
            return View(db.NHANVIENs.Find(id));
        }
        public JsonResult CheckMatKhau(int id, string matkhauOld)
        {
            NHANVIEN nvOld = db.NHANVIENs.Find(id);
            Xuly xuly = new Xuly();
            if (nvOld != null && nvOld.MatKhau == xuly.chuoiMaHoa(matkhauOld))
            {
                return Json("YES", JsonRequestBehavior.AllowGet);
            }else
                return Json("NO", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DoiMatKhau(NHANVIEN nv)
        {
             if(ModelState.IsValid)
            {
                NHANVIEN nvOld = db.NHANVIENs.Find(nv.id);
                Xuly xuly = new Xuly();
                nvOld.MatKhau = xuly.chuoiMaHoa(nv.MatKhau);
                db.Entry(nvOld).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json("YES",JsonRequestBehavior.AllowGet);
            }
             return Json("NO", JsonRequestBehavior.AllowGet);
        }
    }
}