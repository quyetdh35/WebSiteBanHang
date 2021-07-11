using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class SanPhamController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: SanPham
        //Xay dung partial hien thi view style 1
        [ChildActionOnly] //ngan khong cho mo partial view len nhu cach mo 1 view
        public ActionResult SanPhamStyle1Partial()
        {
            return PartialView();
        }

        //Xay dung partial hien thi view style 2
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }


        public ActionResult XemChiTiet(int? id,string tensp)
        {
            //Kiem tra tham so truyen vao co  rong hay khong
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //neeu khong thif truy xuat csdl ra san pham tuowng ung
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            
            if(sp == null)
            {
                //thong bao neeu nhu khong co san pham
                return HttpNotFound();
            }

            return View(sp);
        }

        //xay dung trang danh muc san pham
        public ActionResult SanPham(int? MaLoaiSP,int? MaNSX)
        {
            if(MaLoaiSP == null  && MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lstSp = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);

            //kiem tra nêu listSp rỗng trả về lỗi k tìm thấy
            if(lstSp == null || lstSp.Count() == 0)
            {
                return HttpNotFound();
            }

            return View(lstSp);
        }
    }
}