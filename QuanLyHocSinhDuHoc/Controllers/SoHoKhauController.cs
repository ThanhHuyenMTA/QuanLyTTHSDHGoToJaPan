using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyHocSinhDuHoc.Models.Entities;
using PaymentSystem.Controllers;
using System.IO;

namespace QuanLyHocSinhDuHoc.Controllers
{
    public class SoHoKhauController : BaseController
    {
        dbXulyTThsEntities db = new dbXulyTThsEntities();
        // GET: SoHoKhau
        public ActionResult DetailSHK()
        {
            int id_hs = (int)Session["id_hsDetail"];
            List<HoKhau> listSHK = db.HoKhaus.Where(n => n.id_hs == id_hs).ToList();
            ViewBag.fileSHK = null;
            if(listSHK.Count>0)
            {
                foreach(var item in listSHK)
                {
                    if(item.fileSHK!=null && item.fileSHK!="")
                    {
                        ViewBag.fileSHK = item.fileSHK;
                        break;
                    }
                }
            }
            return View(listSHK);
        }
        //upload File
        [HttpPost]
        public JsonResult UpLoadFile()
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
                    if (type == "application/pdf")
                    {
                        file.SaveAs(path);
                    }
                }
                if (type == "application/pdf")
                {
                   // Session["file"] = fileName;
                    int id_hs = (int)Session["id_hsDetail"];
                    List<HoKhau> listSHK = db.HoKhaus.Where(n => n.id_hs == id_hs).ToList();
                    foreach(var item in listSHK)
                    {
                        item.fileSHK = fileName;
                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        Session["chuyenTab"] = 7;
                        db.SaveChanges();

                    }
                    return Json(id_hs, JsonRequestBehavior.AllowGet);
                }
            }
            //Session["file"] = null;
            return Json("NO", JsonRequestBehavior.AllowGet);
        }
        #region //GET:Thêm mới
        public ActionResult Themmoi(int id_hs)
        {
            Session["file"] = null;
            Session["id_hsSHK"] = id_hs;
            Session["chuyenTab"] = 7;
            return View();
        }
        [HttpPost]
        public ActionResult Themmoi(HoKhau sohokhau)
        {
            if (ModelState.IsValid)
            {
                if (Session["file"]!=null)
                    sohokhau.fileSHK = (string)Session["file"];                    
                sohokhau.id_hs = (int)Session["id_hsSHK"];
                db.HoKhaus.Add(sohokhau);
                db.SaveChanges();
                Session["chuyenTab"] = 7;
                return RedirectToAction("DetailChung/" + sohokhau.id_hs, "HocSinh");                
            }
            return View();
        }
        #endregion

        #region //GET:Cập nhật-sửa
        public ActionResult Sua(int id_SHK)
        {
            Session["chuyenTab"] = 7;
            HoKhau hokhau = db.HoKhaus.Find(id_SHK);
            return View(hokhau);
        }
        [HttpPost]
        public ActionResult Sua(HoKhau sohokhau)
        {
            if(ModelState.IsValid)
            {
                db.Entry(sohokhau).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Session["chuyenTab"] = 7;
                return RedirectToAction("DetailChung/" + sohokhau.id_hs, "HocSinh");
            }
            return View();
            
        }
        #endregion



    }
}