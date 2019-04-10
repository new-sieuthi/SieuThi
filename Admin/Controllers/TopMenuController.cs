using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class TopMenuController : Controller
    {
        // GET: TopMenu
        public ActionResult Index(int page = 1, int pageSize = 1)
        {
            var model = new TopMenuDao().ListAllPaging(page, pageSize);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TopMenu entity)
        {
            int id;
            if (ModelState.IsValid)
            {
                try
                {
                    entity.Status = true;
                    entity.CreatedOn = DateTime.Now;
                    id = new TopMenuDao().Insert(entity);
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "TopMenu", "Index"));
                }
                if (id > 0)
                {
                    ModelState.AddModelError("", "Thêm mới thành công");
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm mới không thành công");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin");
                return View();
            }
        }

        public JsonResult ChangeStatus(int id)
        {
            var result = new TopMenuDao().ChangesStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var model = new TopMenuDao().GetById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(TopMenu entity)
        {
            bool result;
            if (ModelState.IsValid)
            {
                try
                {
                    result = new TopMenuDao().Update(entity);
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "TopMenu", "Create"));
                }
                if (result)
                {
                    ModelState.AddModelError("", "Cập nhật thành công");
                    return RedirectToAction("Index", "TopMenu");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công");
                    return View();
                }

            }
            else
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin");
                return View();
            }
        }
        
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            bool result = new TopMenuDao().Delete(id);
            if (result)
            {
                ModelState.AddModelError("", "Xóa thành công");
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Xóa không thành công");
                return RedirectToAction("Index");
            }
        }
    }
}