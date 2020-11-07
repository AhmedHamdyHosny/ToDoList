using System.ComponentModel.DataAnnotations;

namespace ToDoList.WebApp.Models
{
    public class AccountLoginViewModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resource))]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Resources.Resource))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "UserFullName", ResourceType = typeof(Resources.Resource))]
        [StringLength(50, ErrorMessageResourceName = "ErrorMsg_TextLength", ErrorMessageResourceType = typeof(Resources.Validation))]
        [RegularExpression(@".*\S+.*", ErrorMessageResourceName = "ErrorMsg_NoWhiteSpace", ErrorMessageResourceType = typeof(Resources.Validation))]
        public string UserFullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resource))]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessageResourceName = "ErrorMsg_TextLength", ErrorMessageResourceType = typeof(Resources.Validation), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\\]*+\\/|!\"£$%^&*()#[@~'?><,.=_-]).{6,}$", ErrorMessageResourceName = "ErrorMsg_PasswordValidation", ErrorMessageResourceType = typeof(Resources.Validation))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Resource))]
        [Compare("Password", ErrorMessageResourceName = "ErrorMsg_PasswordNotMatch", ErrorMessageResourceType = typeof(Resources.Validation))]
        public string ConfirmPassword { get; set; }
    }

}
