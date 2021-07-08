using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAnWeb.Models;
namespace DoAnWeb.Models
{
    public class Giohang
    {
        QLBanhKeoDataContext data = new QLBanhKeoDataContext();

        public int iMasp;
        public string sTensanpham;
        public string sAnh;
        public Double dGia;
        public int iSoluong;

        public Double dThanhtien
        {
            get { return iSoluong * dGia; }
        }
        public Giohang(int Masp)
        {
            iMasp = Masp;
            SanPham sanPham = data.SanPhams.Single(n => n.Masp == iMasp);
            sTensanpham = sanPham.tensanpham;
            sAnh = sanPham.Anh;
            dGia = double.Parse(sanPham.Giaban.ToString());
            iSoluong = 1;
        }
    }
}