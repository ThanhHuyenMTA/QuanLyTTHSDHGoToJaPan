using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyHocSinhDuHoc.Models.Entities;

namespace QuanLyHocSinhDuHoc.Controllers
{
    public class SoHoKhauController : Controller
    {
        dbXulyTThsEntities db = new dbXulyTThsEntities();
        // GET: SoHoKhau
        public ActionResult Index()
        {
            return View();
        }
        #region //GET:Thêm mới
        public ActionResult Themmoi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Themmoi(HoKhau sohokhau)
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region //GET:Cập nhật-sửa
        public ActionResult Sua(int id)
        {
            //
            return View();
        }
         [HttpPost]
        public ActionResult Sua(HoKhau sohokhau)
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}