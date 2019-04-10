using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
   public class TopMenuDao
    {
        private DBModel db = null;

        public TopMenuDao()
        {
            db = new DBModel();
        }

        public int Insert(TopMenu entity)
        {
            try
            {
                db.TopMenus.Add(entity);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity.ID;
        }

        public IEnumerable<TopMenu> ListAllPaging(int page, int pageSize)
        {
            return db.TopMenus.OrderByDescending(x => x.CreatedOn).ToPagedList(page,pageSize);
        }

        public bool ChangesStatus(int id)
        {
            var model = db.TopMenus.FirstOrDefault(x => x.ID == id);
            model.Status = !model.Status;
            db.SaveChanges();
            return bool.Parse(model.Status.ToString());
        }

        public TopMenu GetById(int id)
        {
            return db.TopMenus.FirstOrDefault(x=>x.ID == id);
        }

        public bool Update(TopMenu entity)
        {
            try
            {
                var model = db.TopMenus.FirstOrDefault(x => x.ID == entity.ID);
                model.Image = entity.Image;
                model.Name = entity.Name;
                model.ModifiedBy = "";
                model.ModifiedOn = DateTime.Now;
                model.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var model = db.TopMenus.FirstOrDefault(x => x.ID == id);
                db.TopMenus.Remove(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
