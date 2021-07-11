using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }

        public List<ItemGioHang> LayGioHang()
        {
            List<ItemGioHang> lst = Session["GioHang"] as List<ItemGioHang>;

            //kiểm tra xem list giỏ hàng đã tồn tại hay chưa 
            if (lst == null)
            {
                //nếu chưa tồn tại thfi tạo listGiohang mới
                lst = new List<ItemGioHang>();
                Session["GioHang"] = lst;
            }
            return lst;
        }

        //Thêm mới giỏ hàng
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            //kiểm tra xem sản phẩm đã tồn tại trong csdl hay chưa
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);

            if (sp == null)
            {
                //Trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }

            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();

            //kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng hay chưa
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);

            if (spCheck != null)
            {
                if (spCheck.SoLuong > sp.SoLuongTon)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;

                //cập nhật lại thành tiền
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }

            //nếu sp chưa có trong giỏ hàng thì tạo mới
            ItemGioHang lstAdd = new ItemGioHang(MaSP);
            if (lstAdd.SoLuong > sp.SoLuongTon)
            {
                return View("ThongBao");
            }


            //thêm vào giỏ hàng
            lstGioHang.Add(lstAdd);

            return Redirect(strURL);
        }



        //Tính tổng số lượng
        public double TongSoLuong()
        {
            //Lấy giỏ hàng
            List<ItemGioHang> lst = Session["GioHang"] as List<ItemGioHang>;
            if (lst == null)
            {
                return 0;
            }
            else
            {
                return lst.Sum(n => n.SoLuong);
            }
        }

        //Tính tổng tiền
        public decimal TinhTongTien()
        {
            //Lấy giỏ hàng
            List<ItemGioHang> lst = Session["GioHang"] as List<ItemGioHang>;
            if (lst == null)
            {
                return 0;
            }
            else
            {
                return lst.Sum(n => n.ThanhTien);
            }
        }

        //Tạo 1 partialView cho giỏ hàng tách từ layout
        public ActionResult GioHangPartial()
        {
            List<ItemGioHang> lstGh = Session["GioHang"] as List<ItemGioHang>;
            if (lstGh == null)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongThanhTien = 0;
                return PartialView();
            }

            //Nếu tổng sản phẩm lớn hơn 0 ta bắt đầu tính 
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TinhTongTien();
            return PartialView();
        }

        //Xây dựng view hiển thị giỏ hàng
        public ActionResult XemGioHang()
        {
            //Lấy ra list giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();

            return View(lstGioHang);
        }

        //xây dựng view hiển thị Sửa giỏ hàng

        public ActionResult SuaGioHang(int MaSP)
        {
            //kiểm tra sesion giỏ hàng đã tồn tại hay chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //kiểm tra xem sản phẩm đã tồn tại tron db hay chưa
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //trả về trang đường dẫn k hợp lệ
                Response.StatusCode = 404;
                return null;
            }

            //Lấy list Giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();

            //kiểm tra sp có trong list giỏ hàng hay chưa
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //Lấy listGioHang truyền sang View hiển thị
            ViewBag.lstGioHang = lstGioHang;

            return View(spCheck);
        }

        //xấy dựng action xử lý sửa giỏ hàng
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGh)
        {
            //kiểm tra số lượng tồn
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == itemGh.MaSP);
            if (sp.SoLuongTon < itemGh.SoLuong)
            {
                return View("ThongBao");
            }
            //Lấy ra listGioHang
            List<ItemGioHang> lstGioHang = LayGioHang();

            //cập nhật lại giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == itemGh.MaSP);

            spCheck.SoLuong = itemGh.SoLuong;
            spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;

            return RedirectToAction("XemGioHang");
        }

        //xây dựng action xóa giỏ hàng
        public ActionResult XoaGioHang(int MaSP)
        {
            //kiểm tra xem sp có trong db hay k
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //trả về trang lỗi 404
                Response.StatusCode = 404;
                return null;
            }

            //lấy ra list giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();

            ItemGioHang delSp = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);

            //kiểm tra xem sp có trong list Giỏ hàng k
            if (delSp == null)
            {
                return RedirectToAction("Index", "Home");
            }

            lstGioHang.Remove(delSp);
            return RedirectToAction("XemGioHang");
        }

        //xử lý actionresult đặt hàng
        public ActionResult DatHang(KhachHang kh)
        {
            //kiểm tra xem giỏ hàng đã tồn tại hay chưa 
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            KhachHang _khachHang = new KhachHang();
            if (Session["KhachHang"] == null)
            {
                //gán giá trị khách hàng lấy đc bên form view
                _khachHang = kh;
                //thêm vào db
                db.KhachHangs.Add(_khachHang);
                //lưu lại thay đổi db
                db.SaveChanges();
            }
            else
            {
                //dối vói thành viên là khách hàng
                ThanhVien tv = Session["KhachHang"] as ThanhVien;
                _khachHang.TenKH = tv.HoTen;
                _khachHang.DiaChi = tv.DiaChi;
                _khachHang.Email = tv.Email;
                _khachHang.SoDienThoai = tv.SoDienThoai;
                _khachHang.MaThanhVien = tv.MaLoaiTV;

                //thêm vào db
                db.KhachHangs.Add(_khachHang);

                db.SaveChanges();
            }

            //khi đã tồn tại thì thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = _khachHang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            ddh.DaThanhToan = false;

            db.DonDatHangs.Add(ddh);

            db.SaveChanges();


            //thêm chi tiết đơn đặt hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            foreach(var item in lstGioHang)
            {
                ChiTietDonDatHang ctDonHang = new ChiTietDonDatHang();
                ctDonHang.MaDDH = ddh.MaDDH;
                ctDonHang.MaSP = item.MaSP;
                ctDonHang.TenSP = item.TenSP;
                ctDonHang.SoLuong = item.SoLuong;
                ctDonHang.DonGia = item.DonGia;

                db.ChiTietDonDatHangs.Add(ctDonHang);

                
            }

            //cập nhật lại đơn hàng
            db.SaveChanges();

            //sau khi cập nhật lại đơn hàng thì làm trống giỏ hàng
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang", "GioHang");
        }

        //xây dựng chức năng thêm giỏ hàng bằng ajax
        public ActionResult ThemGioHangAjax(int MaSP,string strURL)
        {
            //kiểm tra xem sản phẩm cso trong db hay k
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //lấy giỏ hàng để keierm tra
            List<ItemGioHang> lstGioHang = LayGioHang();
            //kiểm tra xem sản phẩm có trong giỏ hàng hayc hưua
            ItemGioHang spcheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);

            //nếu sản phẩm đã có trong giỏ hàng
            if(spcheck != null)
            {
                //kiểm tra số lượng
                if(sp.SoLuongTon< spcheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spcheck.SoLuong++;
                spcheck.ThanhTien = spcheck.SoLuong * spcheck.DonGia;
                ViewBag.TongSoLuong = TongSoLuong();
                ViewBag.TongThanhTien = TinhTongTien();

                return PartialView("GioHangPartial");
            }
            //nếu sản phẩm chưa có trong giỏ hàng thì tạo mới sản phẩm
            ItemGioHang item = new ItemGioHang(MaSP);
            //kiểm tra số lượng
            if (sp.SoLuongTon < item.SoLuong)
            {
                return View("ThongBao");
            }
           
            lstGioHang.Add(item);
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TinhTongTien();
            return PartialView("GioHangPartial");

        }
    }
}