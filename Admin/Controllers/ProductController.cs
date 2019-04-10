using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace Admin.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(int page = 1, int pageSize = 1)
        {
            var model = new ProductDao().GetAllPaging(page, pageSize);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Product entity)
        {
            long id;
            if (ModelState.IsValid)
            {
                try
                {
                    entity.CreatedOn = DateTime.Now;
                    id = new ProductDao().Create(entity);
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Product", "Create"));
                }
                if (id > 0)
                {
                    ModelState.AddModelError("", "Tạo thành công");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Tạo không thành công");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin");
                SetViewBag();
                return View();
            }

        }

        [HttpGet]
        public ActionResult Update(long id)
        {
            var model = new ProductDao().GetByID(id);

            SetViewBag(model.CategoryID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(Product entity)
        {
            bool result;
            if (ModelState.IsValid)
            {
                try
                {
                    entity.CreatedOn = DateTime.Now;
                    result = new ProductDao().Update(entity);
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Product", "Create"));
                }
                if (result)
                {
                    ModelState.AddModelError("", "Cập nhật thành công");
                    return RedirectToAction("Index", "Product");
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
                SetViewBag(entity.CategoryID);
                return View();
            }

        }

        [HttpDelete]
        public ActionResult Delete(long id)
        {
            bool result = new ProductDao().Delete(id);
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


        public void SetViewBag(long? selectedId = null)
        {
            ViewBag.CategoryID = new SelectList(new ProductCategoryDao().ListProductCtegory(), "ID", "Name", selectedId);
        }

        public JsonResult SaveImages(long id,string images)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var imageList = js.Deserialize<List<string>>(images);
            XElement x = new XElement("Images");

            foreach (var item in imageList)
            {
                var newItem = item.Substring(22);
                x.Add(new XElement("Image", newItem));
            }
            ProductDao dao = new ProductDao();
            try
            {
               
                dao.UpdateImage(id, x.ToString());
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false
                });
            }

            
        }

        public JsonResult LoadImage(long id)
        {
            var model = new ProductDao().GetByID(id);
            var images = model.MoreImages;
            XElement xImages = XElement.Parse(images);
            List<string> listImages = new List<string>();

            foreach (XElement item in xImages.Elements())
            {
                listImages.Add(item.Value);
            }
            return Json(new
            {
                data = listImages
            },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string Upload(HttpPostedFileBase uploadFile)
        {
            StringBuilder strValidations = new StringBuilder(string.Empty);
            try
            {
                if (uploadFile.ContentLength > 0)
                {
                    string filePath = Path.Combine(HttpContext.Server.MapPath("../Uploads"),
                   Path.GetFileName(uploadFile.FileName));
                    uploadFile.SaveAs(filePath);
                    DataSet ds = new DataSet();

                    //A 32-bit provider which enables the use of

                    string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;";

                    using (OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConnectionString))
                    {
                        conn.Open();
                        using (DataTable dtExcelSchema = conn.GetSchema("Tables"))
                        {
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            string query = "SELECT * FROM [" + sheetName + "]";
                            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                            //DataSet ds = new DataSet();
                            adapter.Fill(ds, "Items");
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        //Now we can insert this data to database...
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return "";
        }
    }
}
