using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;
namespace DoAnWeb.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Giohang
        QLBanhKeoDataContext data = new QLBanhKeoDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public List<Giohang> layGiohang()
        {
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang == null)
            {
                listGiohang = new List<Giohang>();
                Session["Giohang"] = listGiohang;
            }
            return listGiohang;
        }
        public ActionResult ThemGiohang(int id, string strURL)
        {
            List<Giohang> listGiohang = layGiohang();
            Giohang sanPham = listGiohang.Find(n => n.iMasp == id);
            if (sanPham == null)
            {
                sanPham = new Giohang(id);
                listGiohang.Add(sanPham);
                return Redirect(strURL);
            }
            else
            {
                sanPham.iSoluong++;
                return Redirect(strURL);
            }

        }
        private int Tongsoluong()
        {
            int iTongsoluong = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang != null)
            {
                iTongsoluong = listGiohang.Sum(n => n.iSoluong);

            }
            return iTongsoluong;
        }

        private double Tongtien()
        {
            double iTongtien = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if (listGiohang != null)
            {
                iTongtien = listGiohang.Sum(n => n.dThanhtien);

            }
            return iTongtien;
        }
        public ActionResult Giohang()
        {
            List<Giohang> listGioHang = layGiohang();
            if (listGioHang.Count == 0)
            {
                return RedirectToAction("Index", "BanhKeo");
            }
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.TongTien = Tongtien();
            return View(listGioHang);
        }
        public ActionResult Giohangpartial()
        {
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.TongTien = Tongtien();
            return PartialView();
        }
        public ActionResult Xoasanpham(int id)
        {
            List<Giohang> listGiohang = layGiohang();
            Giohang sanPham = listGiohang.SingleOrDefault(n => n.iMasp == id);
            if (sanPham != null)
            {
              
                listGiohang.RemoveAll(n => n.iMasp == id);
                return RedirectToAction("Giohang");
            }
            if(listGiohang.Count == 0)
            {

                return RedirectToAction("Index","BanhKeo");
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult Xoatatcagiohang()
        {
            List<Giohang> listGiohang = layGiohang();
            listGiohang.Clear();
            return RedirectToAction("Index","BanhKeo");
        }
        public ActionResult Capnhat(int id, FormCollection f)
        {
            List<Giohang> listGiohang = layGiohang();
            Giohang sanPham = listGiohang.SingleOrDefault(n => n.iMasp == id);
            if (sanPham != null) 
            { 
                sanPham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        [HttpGet]
        public ActionResult Dathang()
        {
            if(Session["TaiKhoan"] ==null || Session["TaiKhoan"].ToString()== "")
            {
                return RedirectToAction("DangNhap", "BanhKeo");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "BanhKeo");
            }
            List<Giohang> listGioHang = layGiohang();
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.TongTien = Tongtien();
            return View(listGioHang);
        }
        public ActionResult Dathang(FormCollection collection)
        {
            Donhang dh = new Donhang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<Giohang> gh = layGiohang();
            dh.MaKH = kh.MaKH;
            dh.ngaydat = DateTime.Now;
            var Ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            dh.ngaygiao = DateTime.Parse(Ngaygiao);
            dh.Tinhtranggiaohang = false;
            dh.Dathanhtoan = false;
            data.Donhangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach( var sanPham in gh)
            {
                Chitietdonhang ctdh = new Chitietdonhang();
                ctdh.Madonhang = dh.Madonhang;
                ctdh.Masp = sanPham.iMasp;
                ctdh.soluong = sanPham.iSoluong;
                ctdh.dongia = (int)(decimal) sanPham.dGia;
                data.Chitietdonhangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}