using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class HomeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Home
        public ActionResult Index()
        {
            //Laasy ra dien thoai moi nhat
            var lstDTMoi = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.lstDTMoi = lstDTMoi;

            //lay ra lap top moi
            var lstLTMoi = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.lstLTMoi = lstLTMoi;

            //Lay ra may tinh bang moi
            var lstMTBMoi = db.SanPhams.Where(n => n.MaLoaiSP == 3 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.lstMTBMoi = lstMTBMoi;
            return View();
        }

        //xay dung menupartial
        public ActionResult MenuPartial()
        {
            var lstDanhMuc = db.SanPhams;
            return PartialView(lstDanhMuc);
        }

        //xay dựng Action đăng ký cho 
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            db.ThanhViens.Add(tv);

            ViewBag.ThongBao = "Thêm thành viên thành công";

            db.SaveChanges();
            return Content("<script>window.location.reload();alert('Đăng ký thành công')</script>");
        }

        //xây dựng action đăng nhập 
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            var taiKhoan = f["txtTenDangNhap"].ToString();
            var matKhau = f["txtMatKhau"].ToString();

            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == taiKhoan && n.MatKhau == matKhau);

            if(tv != null)
            {
                Session["Taikhoan"] = tv;
                //load lại trang web
                return Content("<script>window.location.reload();</script>");
            }
            return Content("Tài khoản hoặc tên đăng nhập không đúng !!");
        }

        //xây dựng action đăng xuât
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index");
        }
    }
}