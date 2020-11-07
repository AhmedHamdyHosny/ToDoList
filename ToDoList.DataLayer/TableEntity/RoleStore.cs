using Microsoft.AspNet.Identity.EntityFramework;

namespace ToDoList.DataLayer.TableEntity
{
    public class RoleStore : RoleStore<Role, int, UserRole>
    {
        public RoleStore(ToDoListDBContext context) : base(context)
        {
        }
    }
}
