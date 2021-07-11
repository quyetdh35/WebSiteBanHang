using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using WebSiteBanHang.Models;
namespace WebSiteBanHang.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang

        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            //lấy ra 1 list khach hang
            //cách 1 : lấy dữ liệu là 1 danh sách khách hàng
            var lstKH = from KH in db.KhachHangs select KH;

            //cách 2: ngắn gọn hơn,dùng phương thức hỗ trợ sẵn
            //var lstKH = db.KhachHangs;
            return View(lstKH);
        }

        public ActionResult TruyVanMotDoiTuong()
        {
            //cách 1 :truy vấn 1 dối tượng bằng cauau lệnh truy vấn
            //bước 1 :lấy ra danh sách khách hàng
            var lstKH = from kh in db.KhachHangs where kh.MaKH ==1 select kh;

            //bước 2: lấy ra đối tượng khách hàng
           // KhachHang kHang = lstKH.FirstOrDefault(); //FirstOrDefault lấy ra dòng đầu tiên từ câu lệnh truy vấn trên


            //cách 2: lấy đối tượng khách hàng dựa trên phương thức hỗ trợ
            KhachHang khach = db.KhachHangs.SingleOrDefault(n=>n.MaKH==2); //SingleOrDefault kiểm tra nếu cso điều kiện hợp lệ thì trả về giá trị k cso thfi trả về null
            //return View(kHang);
            return View(khach);
        }

        public ActionResult SortDulieu()
        {
            List<KhachHang> lstKH = db.KhachHangs.OrderBy(n => n.TenKH).ToList();
            return View(lstKH);
        }


        public ActionResult GroupDuLieu()
        {
            //Group du liệu
            List<ThanhVien> lstKH = db.ThanhViens.OrderByDescending(n => n.TaiKhoan).ToList();
            return View(lstKH);
        }
    }
}