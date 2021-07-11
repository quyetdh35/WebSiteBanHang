using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSiteBanHang.Models
{
    public class ItemGioHang
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public string HinhAnh { get; set; }
        public decimal ThanhTien { get; set; }
        public ItemGioHang(int iMaSP)
        {
            using(QuanLyBanHangEntities db = new QuanLyBanHangEntities())
            {
                this.MaSP = iMaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaSP);
                TenSP = sp.TenSP;
                SoLuong = 1;
                DonGia = sp.DonGia.Value;
                HinhAnh = sp.HinhAnh;
                ThanhTien = SoLuong * DonGia;
            }
        }

        public ItemGioHang(int iMaSP,int sl)
        {
            using(QuanLyBanHangEntities db = new QuanLyBanHangEntities())
            {
                this.MaSP = iMaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaSP);
                TenSP = sp.TenSP;
                SoLuong = sl;
                DonGia = sp.DonGia.Value;
                HinhAnh = sp.HinhAnh;
                ThanhTien = SoLuong * DonGia;
            }
        }

        public ItemGioHang()
        {

        }
    }
}