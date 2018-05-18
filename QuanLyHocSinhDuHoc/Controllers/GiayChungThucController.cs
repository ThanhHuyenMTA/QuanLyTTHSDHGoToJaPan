using QuanLyHocSinhDuHoc.CommonXuLy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyHocSinhDuHoc.Models.Entities;

namespace QuanLyHocSinhDuHoc.Controllers
{
    public class GiayChungThucController : Controller
    {
        dbXulyTThsEntities db = new dbXulyTThsEntities();

        // GET: GiayChungThuc
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
        //upload File
        [HttpPost]
        public JsonResult SaveGCT()
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
                int id_HS = (int)Session["id_HS"];
                if(id_HS!=null)
                {
                    GIAYCHUNGTHUC giay = new GIAYCHUNGTHUC();
                    giay.Anh = fileName;
                    giay.TenGiay = fileName;
                    giay.id_HSHS = id_HS;
                    db.GIAYCHUNGTHUCs.Add(giay);
                    db.SaveChanges();
                }

                return Json(fileName, JsonRequestBehavior.AllowGet);
            }
            Session["file"] = null;
            return Json("Khong", JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetailGCT()
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {
                int id_hs = (int)Session["id_hsDetail"];
                HOCSINH hocsinh = db.HOCSINHs.Find(id_hs);
                List<GIAYCHUNGTHUC> listGiay = db.GIAYCHUNGTHUCs.Where(n => n.id_HSHS == hocsinh.id).ToList();
                if (listGiay.Count > 0)
                {
                    ViewBag.ThongbaoBTN = "OK";
                    return View(listGiay);
                }
                ViewBag.ThongbaoBTN = "NO";
                return View();

            } return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult CapNhatGCT()
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
                int id_HS = (int)Session["id_hsDetail"];
                if (id_HS != null)
                {
                    GIAYCHUNGTHUC giay = new GIAYCHUNGTHUC();
                    giay.Anh = fileName;
                    giay.TenGiay = fileName;
                    giay.id_HSHS = id_HS;
                    db.GIAYCHUNGTHUCs.Add(giay);
                    db.SaveChanges();
                }

                return Json(fileName, JsonRequestBehavior.AllowGet);
            }
            Session["file"] = null;
            return Json("Khong", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Load()
        {
            int id_hs = (int)Session["id_hsDetail"];
            Session["chuyenTab"] = 8;
            return RedirectToAction("DetailChung", "HocSinh", new { id = id_hs });
        }
    }
}