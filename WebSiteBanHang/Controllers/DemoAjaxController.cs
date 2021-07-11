using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class DemoAjaxController : Controller
    {
        // GET: DemoAjax
        public ActionResult DemoAjax()
        {
            return View();
        }

        //xử lý ajaxActionLink
        public ActionResult LoadAjaxActionLink()
        {
            //System.Threading.Thread.Sleep(2000); //cho load view này trong 2 giây mới hiện ra
            return Content("Hello Ajax");
        }


        //xử lý ajaxBeginForm
        public ActionResult LoadAjaxBeginForm(FormCollection f)
        {
            string kQ = f["txt1"].ToString();
            return Content(kQ);
        }


        //xử lý ajaxJquery
        public ActionResult LoadAjaxJquery(int a, int b)
        {
            return Content((a + b).ToString()); //content chri cho truyền vào tchuooix nên .Tostring()
        }

        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //xử dụng ajax load 1 partial view
        public ActionResult SanPhamPartial()
        {
            var listSanPhamLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2);
            return PartialView(listSanPhamLTM);
        }
    }
}