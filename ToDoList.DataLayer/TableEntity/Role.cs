using Microsoft.AspNet.Identity.EntityFramework;

namespace ToDoList.DataLayer.TableEntity
{
    public class Role : IdentityRole<int, UserRole>
    {
        public Role() { }
        public Role(string name) { Name = name; }
    }
}
