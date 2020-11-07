using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.DataLayer.TableEntity
{
    public class SystemUser : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        [Required]
        [StringLength(50)]
        [RegularExpression(@".*\S+.*")]
        public string UserFullName { get; set; }
    }
}
