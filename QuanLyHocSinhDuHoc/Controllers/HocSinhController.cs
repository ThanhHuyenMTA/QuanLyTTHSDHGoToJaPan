using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyHocSinhDuHoc.Models.Entities;
using System.Data.Entity;
using System.IO;
using QuanLyHocSinhDuHoc.CommonXuLy;
using System.Data.SqlClient;
using PaymentSystem.Controllers;

namespace QuanLyHocSinhDuHoc.Controllers
{
    public class HocSinhController : BaseController
    {
        // GET: HocSinh
        dbXulyTThsEntities db = new dbXulyTThsEntities();        
        public ActionResult Index(int? page)
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh"|| quyenNguoiDung.Quyen.Ten=="Admin"))
           {
               Session["chuyenTab"] = null;
               Xuly xuly = new Xuly();
               List<string> listNam = new List<string>();
               List<HOCSINH> lisths = db.HOCSINHs.OrderByDescending(n => n.timeStart).ToList();
               foreach (var item1 in lisths)
               {
                   if (!xuly.checkTrungTimeStart(item1.timeStart, listNam)) //chưa tồn tại trong list thì thêm vào list
                       listNam.Add(item1.timeStart);
               }
               ViewBag.listNam = listNam;
               int count = db.HOCSINHs.Where(n=>n.NguoiTao==quyenNguoiDung.Nhanvien.id).ToList().Count;
               ViewBag.All = count;
               Session["chiasotrang"] = count % 10 == 0 ? count / 10 : count / 10 + 1;
               page = page ?? 1;
               int lineStart = (int)(page - 1) * 10; //dòng bắt đầu
               int soBanGhi = 10; //số bản ghi cần hiện thị mỗi trang
               Session["trangdangload"] = page;

               var idParam0= new SqlParameter
               {
                   ParameterName = "NguoiTao",
                   Value = quyenNguoiDung.Nhanvien.id
               };
               var idParam1 = new SqlParameter
               {
                   ParameterName = "LineStart",
                   Value = lineStart
               };
               var idParam2 = new SqlParameter
               {
                   ParameterName = "soBanGhi",
                   Value = soBanGhi
               };
               var list = db.Database.SqlQuery<HOCSINH>("exec PhanTrang @NguoiTao,@LineStart,@soBanGhi ", idParam0, idParam1, idParam2).ToList<HOCSINH>();
               return View(list);       
                               
           }
            return RedirectToAction("Index", "Home");            
        }
        //xử lý phân trang ve cuoi cung     
        public JsonResult XulyPhanTrangVeCuoicung()
        {
            int trangcuoi = (int)Session["chiasotrang"];
            return Json(trangcuoi, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Themmoi()
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh"|| quyenNguoiDung.Quyen.Ten=="Admin"))
                 return View();
           return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public JsonResult Themmoi(HOCSINH hocsinh)
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh"|| quyenNguoiDung.Quyen.Ten=="Admin"))
            {
                if (ModelState.IsValid)
                {                  
                    DateTime today = DateTime.Now;
                    hocsinh.timeStart = today.ToString("yyyy");
                    hocsinh.NguoiTao = quyenNguoiDung.Nhanvien.id;
                    db.HOCSINHs.Add(hocsinh);
                    db.SaveChanges();
                    int id_HS = hocsinh.id;
                    Session["id_HS"] = id_HS;
                    return Json(id_HS, JsonRequestBehavior.AllowGet);
                }
                return Json("Thêm thất bại!", JsonRequestBehavior.AllowGet);
            }
            return Json("khong duoc quyen!", JsonRequestBehavior.AllowGet);
        }
         [HttpPost]
        public JsonResult CheckTrungSDT(string sodienthoai)
        {
            var model = db.HOCSINHs.Where(n => n.sdt == sodienthoai).ToList();
            if(model.Count>0)
                return Json("Trung", JsonRequestBehavior.AllowGet);
            else return Json("Khong trung", JsonRequestBehavior.AllowGet);
        }
           [HttpPost]
        public JsonResult CheckTrungEmail(string email)
        {
               foreach(var item in db.HOCSINHs.ToList())
               {
                   if(email ==(string)item.email)
                       return Json("Trung", JsonRequestBehavior.AllowGet);
               }               
               return Json("Khong trung", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpLoadImage()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var fileImg = Request.Files["HelpSectionImages"];
                //lưu tên file
                var fileName = Path.GetFileName(fileImg.FileName);
                //lưu đường dẫn
                var path = Path.Combine(Server.MapPath("~/Content/images/profile"), fileName);
                // file is uploaded
                var type = fileImg.ContentType;
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    if (type == "image/jpeg" || type == "image/jpg" || type == "image/png")
                        fileImg.SaveAs(path);
                }
                //dua ra hoc sinh can upload anh
                if(Session["id_hsSua"] !=null)
                {
                    HOCSINH hs = db.HOCSINHs.Find((int)Session["id_hsSua"]);
                    hs.anh = fileName;
                    db.Entry(hs).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                
                //end
                return Json(fileName, JsonRequestBehavior.AllowGet);

            }
            return Json("Khong", JsonRequestBehavior.AllowGet);
        }
        public ActionResult ThemmoiChung()
        {
             ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
             if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
                return View();
            else return RedirectToAction("Index", "Home");

        }
        public ActionResult SuaHocsinh(int id)
        {
             ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if ( quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh"|| quyenNguoiDung.Quyen.Ten=="Admin"))
            {
                HOCSINH hocsinh = db.HOCSINHs.Find(id);
                if (quyenNguoiDung.Nhanvien.id == hocsinh.NguoiTao)
                {
                    Session["id_hsSua"] = hocsinh.id;
                    return View(hocsinh);
                }
                
                else return RedirectToAction("Index", "HocSinh");
                
            } return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult SuaHocsinh(HOCSINH hocsinh)
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh"|| quyenNguoiDung.Quyen.Ten=="Admin"))
            {
                if (ModelState.IsValid)
                {
                    HOCSINH hocsinhupdate = db.HOCSINHs.Find(hocsinh.id);
                    
                    hocsinhupdate.TenHS = hocsinh.TenHS;
                    hocsinhupdate.sdt = hocsinh.sdt;
                    hocsinhupdate.email = hocsinh.email;
                    db.Entry(hocsinhupdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    Session["chuyenTab"] = null;
                    return RedirectToAction("DetailChung/" + hocsinhupdate.id);
                }
                return View(hocsinh);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DetailHocsinh(int id)
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh"|| quyenNguoiDung.Quyen.Ten=="Admin"))
            {
                Session["id_hsSua"] = id;
                HOCSINH hocsinh = db.HOCSINHs.Find(id);
                return View(hocsinh);
            } return RedirectToAction("Index", "Home");
        }
        public ActionResult DetailChung(int? id)
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh"|| quyenNguoiDung.Quyen.Ten=="Admin"))
            {
                Session["id_hsDetail"] = id; //được gọi khi thực hiện cập nhật thông tin học sinh(HS,CMT,GKS,BTN...)
                HOCSINH hocsinh = db.HOCSINHs.Find(id);
                return View(hocsinh);
            } return RedirectToAction("Index", "Home");
        }

        //GET: sOLUONG Load san pham
        public JsonResult SearchNam(string Namloc)
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {
                List<HOCSINH> list = db.HOCSINHs.Where(n => n.timeStart == Namloc).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            } 
            return Json("Khong co quyen", JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchKeyName(string keySearch)
        {
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten=="Admin"))
            {
                List<HOCSINH> list = db.HOCSINHs.Where(n => n.TenHS.Contains(keySearch) || n.sdt.Contains(keySearch) || n.email.Contains(keySearch)).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return Json("Khong co quyen", JsonRequestBehavior.AllowGet);
        }
        public ActionResult XoaHocSinh(int id)
        {
            //xóa dữ liệu trong bảng học sinh
            HOCSINH hocsinh = db.HOCSINHs.Find(id);
            db.HOCSINHs.Remove(hocsinh);
            TABLE_LOI tble = db.TABLE_LOI.SingleOrDefault(n => n.id_HS == id);
            if (tble != null)
                db.TABLE_LOI.Remove(tble);
            //xóa các dữ liệu liên quan
            string socmt = hocsinh.SoCMT;
            if(socmt!=null)
            {
                CMT cmt = db.CMTs.Find(socmt);
                TABLE_LOI tble1 = db.TABLE_LOI.SingleOrDefault(n => n.So_CMT == socmt);
                if (tble1 != null)
                    db.TABLE_LOI.Remove(tble1);
                db.CMTs.Remove(cmt);
            }
            int id_gks = hocsinh.id_GKS ==null ? 0 :(int)hocsinh.id_GKS;
            if(id_gks!=0)
            {
                GIAYKHAISINH gks = db.GIAYKHAISINHs.Find(id_gks);
                TABLE_LOI tble2 = db.TABLE_LOI.SingleOrDefault(n => n.id_GKS == id_gks);
                if (tble2 != null)
                    db.TABLE_LOI.Remove(tble2);
                db.GIAYKHAISINHs.Remove(gks);
            }
            int id_btn = hocsinh.id_BTN == null ? 0 : (int)hocsinh.id_BTN;
            if(id_btn!=0)
            {
                BANGTOTNGHIEP btn = db.BANGTOTNGHIEPs.Find(id_btn);
                TABLE_LOI tble3= db.TABLE_LOI.SingleOrDefault(n => n.id_BTN == id_btn);
                if (tble3 != null)
                    db.TABLE_LOI.Remove(tble3);
                db.BANGTOTNGHIEPs.Remove(btn);
            }
            int id_hb = hocsinh.id_HB == null ? 0 : (int)hocsinh.id_HB;
            if(id_hb!=0)
            {
                HOCBA hocba = db.HOCBAs.Find(id_hb);
                TABLE_LOI tble4 = db.TABLE_LOI.SingleOrDefault(n => n.id_HB == id_hb);
                if (tble4 != null)
                    db.TABLE_LOI.Remove(tble4);
                db.HOCBAs.Remove(hocba);
            }
            string socmtNGH = hocsinh.id_NgGiamHo;
            if(socmtNGH!=null)
            {
                NGUOIGIAMHO nggiamho = db.NGUOIGIAMHOes.Find(socmtNGH);
                db.NGUOIGIAMHOes.Remove(nggiamho);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "HocSinh");
        }
    }
}