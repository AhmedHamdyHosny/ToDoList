using Microsoft.AspNet.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.DataLayer;
using ToDoList.DataLayer.TableEntity;
using System;

namespace ToDoList.WebApp
{
    public class SystemUserModel
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(SystemUser user, ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("UserFullName", user.UserFullName));

            return userIdentity;
        }

        public bool IsNotExists(string email)
        {
            bool result = false;
            using (var context = new ToDoListDBContext())
            {
                IQueryable<SystemUser> query = context.Set<SystemUser>();
                result = !query.Any(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
            return result;
        }
    }
}