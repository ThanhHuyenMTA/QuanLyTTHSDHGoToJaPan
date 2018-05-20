using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyHocSinhDuHoc.Models.Entities;
using QuanLyHocSinhDuHoc.CommonXuLy;

namespace QuanLyHocSinhDuHoc.Controllers
{
    public class DangNhapController : Controller
    {
        dbXulyTThsEntities db = new dbXulyTThsEntities();
        // GET: DangNhap
        public ActionResult Dangnhap()
        {
            Session["thongbaoDN"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(string tenDangNhap,string matKhau)          
        {
            Xuly xuly = new Xuly();
            string matKhauNew = xuly.chuoiMaHoa(matKhau);
            NHANVIEN Listnv = db.NHANVIENs.SingleOrDefault(n => n.TenDangNhap == tenDangNhap);
            NHANVIEN nv = Listnv != null ? Listnv : null;

            if (nv !=null)
            {
                if(nv.MatKhau ==matKhauNew) 
                {
                    List<PHANQUYEN> listPQ = db.PHANQUYENs.Where(n => n.id_quyen == nv.id_Quyen).ToList();
                    QUYEN quyen = db.QUYENs.Find(nv.id_Quyen);
                    ModelQuyenNguoiDung QuyenNguoiDung = new ModelQuyenNguoiDung(nv,quyen);
                    if (QuyenNguoiDung.Nhanvien.id_Quyen != null && QuyenNguoiDung.Nhanvien.id_Quyen>0)
                    {
                        Session["QuyenNguoiDung"] = QuyenNguoiDung;
                        Session["DangNhap"] = "OK";
                        Session["thongbaoDN"] = null;
                        Session["NguoiDung"] = nv.TenDangNhap;
                        Session["NguoiDungHT"] = nv;
                        
                        listTb();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Session["DangNhap"] = "NO";
                        Session["thongbaoDN"] = "Bạn không có quyền vào hệ thống";
                        return View();
                    }
                }
                Session["DangNhap"] = "NO";
                Session["thongbaoDN"] = "Đăng nhập thất bại";
                return View();
                
            }
            else
            {
                Session["DangNhap"] = "NO";
                Session["thongbaoDN"] = "Đăng nhập thất bại";
                return View();
               
            }         
        }
        public ActionResult DangXuat()
        {
            Session.RemoveAll();
            return RedirectToAction("Dangnhap", "DangNhap");
        }
        public void listTb()
        {
            List<TABLE_LOI> listVang = new List<TABLE_LOI>();
            List<TABLE_LOI> listDo = new List<TABLE_LOI>();
            List<TABLE_LOI> listXanh = new List<TABLE_LOI>();
            ModelQuyenNguoiDung quyenNguoiDung = Session["QuyenNguoiDung"] as ModelQuyenNguoiDung;
            if (quyenNguoiDung != null && (quyenNguoiDung.Quyen.Ten == "QuanLyThongTinHocSinh" || quyenNguoiDung.Quyen.Ten == "Admin"))
            {
                DateTime today = DateTime.Now;
                var listLoi = db.TABLE_LOI.ToList();
                foreach (var i in listLoi)
                {
                    if (i.TimeEnd > today)
                    {
                        TimeSpan a = ((DateTime)i.TimeEnd).Subtract(today);
                        double day = a.TotalDays;
                        if (day > 5)
                        {
                            i.TrangThai = "1"; //mức xanh
                            if (checkNguoiTao(i, quyenNguoiDung.Nhanvien.id) == true)
                            {
                                i.NguoiSua = quyenNguoiDung.Nhanvien.id;
                                listXanh.Add(i);
                            }
                               
                        }
                        else
                        {

                            i.TrangThai = "2";//mức vàng
                            if (checkNguoiTao(i, quyenNguoiDung.Nhanvien.id) == true)
                            {
                                i.NguoiSua = quyenNguoiDung.Nhanvien.id;
                                listVang.Add(i);
                            }
                               
                        }
                    }
                    else
                    {
                        i.TrangThai = "3"; //mức đỏ
                        if (checkNguoiTao(i, quyenNguoiDung.Nhanvien.id) == true)
                        {
                            i.NguoiSua = quyenNguoiDung.Nhanvien.id;
                            listDo.Add(i);
                        }
                           
                    }
                }
                Session["ThongBaoVang"] = listVang;
                Session["ThongBaoDo"] = listDo;
                Session["ThongBaoXanh"] = listXanh;
                db.SaveChanges();
            }
        }
        public bool checkNguoiTao(TABLE_LOI tableLoi, int id_nguoitao)
        {
            if (tableLoi.id_HS != 0 && tableLoi.id_HS != null)
            {
                HOCSINH hs = db.HOCSINHs.Find(tableLoi.id_HS);
                if (hs.NguoiTao == id_nguoitao)
                    return true;
            }
            string socmt = tableLoi.So_CMT;
            if (socmt != null)
            {
                HOCSINH hs = db.HOCSINHs.SingleOrDefault(n => n.SoCMT == socmt);
                if (hs.NguoiTao == id_nguoitao)
                    return true;
            }
            int id_gks = tableLoi.id_GKS == null ? 0 : (int)tableLoi.id_GKS;
            if (id_gks != 0)
            {
                HOCSINH hs = db.HOCSINHs.SingleOrDefault(n => n.id_GKS == id_gks);
                if (hs.NguoiTao == id_nguoitao)
                    return true;
            }
            int id_btn = tableLoi.id_BTN == null ? 0 : (int)tableLoi.id_BTN;
            if (id_btn != 0)
            {
                HOCSINH hs = db.HOCSINHs.SingleOrDefault(n => n.id_BTN == id_btn);
                if (hs.NguoiTao == id_nguoitao)
                    return true;
            }
            int id_hb = tableLoi.id_HB == null ? 0 : (int)tableLoi.id_HB;
            if (id_hb != 0)
            {
                HOCSINH hs = db.HOCSINHs.SingleOrDefault(n => n.id_HB == id_hb);
                if (hs.NguoiTao == id_nguoitao)
                    return true;
            }
            return false;
        }
        
    }
}