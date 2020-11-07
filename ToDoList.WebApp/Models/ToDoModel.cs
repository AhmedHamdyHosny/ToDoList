using System;
using System.Collections.Generic;
using System.Linq;
using ToDoList.DataLayer;
using ToDoList.DataLayer.TableEntity;
using Classes.Utilities;
using System.Data.Entity;

namespace ToDoList.Models
{
    public class ToDoModel
    {
        public IEnumerable<ToDo> GetData(ref int totalItemCount, string ToDoTitle = null, int? CreateUserId = null, int? skipItemsCount = null, int? pageSize = null, string orderColumn = null, string orderDir = null)
        {
            IEnumerable<ToDo> result;
            using(var context = new ToDoListDBContext())
            {
                IQueryable<ToDo> query = context.Set<ToDo>();

                if (!string.IsNullOrEmpty(ToDoTitle) && !string.IsNullOrWhiteSpace(ToDoTitle))
                {
                    query = query.Where(x => x.ToDoTitle.Contains(ToDoTitle));
                }

                if (CreateUserId != null)
                {
                    query = query.Where(x => x.CreateUserId == CreateUserId);
                }

                totalItemCount = query.Count();

                //set order by
                Func<IQueryable<ToDo>, IOrderedQueryable<ToDo>> orderBy = null;
                if (!string.IsNullOrEmpty(orderColumn) && !string.IsNullOrEmpty(orderDir))
                {
                    orderBy = EntityUtility<ToDo>.GetOrderBy(orderColumn, orderDir);
                }
                query = orderBy != null ? orderBy(query) : query;

                //skip items
                if (skipItemsCount != null && pageSize != null)
                {
                    query = query.Skip((int)skipItemsCount).Take((int)pageSize);
                }

                result = query.ToList();
            }
            return result;

        }

        public IEnumerable<ToDo> Test_GetData()
        {
            List<ToDo> lst = new List<ToDo>();
            for (int i = 0; i < 50; i++)
            {
                lst.Add(new ToDo()
                {
                    ToDoId = i + 1,
                    ToDoTitle = "Task No " + (i + 1),
                    IsDone = ((i % 3) > 0)
                });
            }

            return lst;
        }

        public ToDo Insert(ToDo entity)
        {
            ToDo result;
            using (var context = new ToDoListDBContext())
            {
                DbSet<ToDo> dbSet = context.Set<ToDo>();
                result = dbSet.Add(entity);
                context.SaveChanges();
            }
            return result;
        }

        public ToDo Edit(ToDo entity)
        {
            ToDo result;
            using (var context = new ToDoListDBContext())
            {
                context.Entry(entity).State = EntityState.Detached;
                DbSet<ToDo> dbSet = context.Set<ToDo>();
                result = dbSet.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
            return result;
        }

        public ToDo Get(int id)
        {
            ToDo result;
            using (var context = new ToDoListDBContext())
            {
                DbSet<ToDo> dbSet = context.Set<ToDo>();
                result = dbSet.Find(id);
            }
            return result;
        }

        public bool Delete(int id)
        {
            ToDo result = null;
            using (var context = new ToDoListDBContext())
            {
                DbSet<ToDo> dbSet = context.Set<ToDo>();
                var entity = dbSet.Find(id);
                context.Entry(entity).State = EntityState.Detached;
                dbSet.Attach(entity);
                result = dbSet.Remove(entity);
                context.SaveChanges();
            }
            return result != null;
        }

        public bool MarkDone(int id, int userId)
        {
            ToDo result = null;
            using (var context = new ToDoListDBContext())
            {
                DbSet<ToDo> dbSet = context.Set<ToDo>();
                var entity = dbSet.Find(id);
                context.Entry(entity).State = EntityState.Detached;
                entity.IsDone = true;
                entity.ModifyUserId = userId;
                entity.ModifyDate = DateTime.Now;
                result = dbSet.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
            return result != null;
        }
    }

    public class ToDoIndexViewModel : ToDo
    {

    }

    [System.Web.Mvc.Bind(Exclude = "IsDone,CreateUserId,CreateDate,ModifyUserId,ModifyDate")]
    public class ToDoCreateBindModel : ToDo
    {

    }

    [System.Web.Mvc.Bind(Exclude = "ModifyUserId,ModifyDate")]
    public class ToDoEditBindModel : ToDo
    {

    }

    public class ToDoCreateModel
    {
        public ToDo Item { get; set; }
    }

    public class ToDoEditModel
    {
        public ToDo Item { get; set; }
    }


}