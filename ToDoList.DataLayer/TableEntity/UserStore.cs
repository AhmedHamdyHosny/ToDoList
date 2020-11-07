using Microsoft.AspNet.Identity.EntityFramework;

namespace ToDoList.DataLayer.TableEntity
{
    public class UserStore : UserStore<SystemUser, Role, int,
    UserLogin, UserRole, UserClaim>
    {
        public UserStore(ToDoListDBContext context) : base(context)
        {
        }
    }

}
