using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using ToDoList.DataLayer.TableEntity;

namespace ToDoList.DataLayer
{
    public partial class ToDoListDBContext : IdentityDbContext<SystemUser, Role, int, UserLogin, UserRole, UserClaim>
    {
        public ToDoListDBContext() : base("ToDoListDBEntities")
        {

        }
        public static ToDoListDBContext Create()
        {
            return new ToDoListDBContext();
        }

        #region Tables
        public virtual DbSet<ToDo> ToDo { get; set; }
        #endregion

    }
}
