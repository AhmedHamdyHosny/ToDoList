using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Classes.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ToDoList.DataLayer.TableEntity;
using ToDoList.WebApp.Models;

namespace ToDoList.WebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        SystemUserModel systemUserModel = new SystemUserModel();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AccountLoginViewModel model, string returnUrl)
        {
            AlertMessage alertMessage = new AlertMessage();
            string msg = "";
            bool success = false;

            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        success = true;
                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            returnUrl = Url.Action("Index", "ToDoList");
                        }
                        break;
                    case SignInStatus.LockedOut:
                        msg = Resources.Validation.ErrorMsg_UserLockedOut;
                        break;
                    case SignInStatus.Failure:
                        msg = Resources.Validation.ErrorMsg_LoginFail;
                        break;
                    default:
                        msg = Resources.Validation.ErrorMsg_LoginFail;
                        break;
                }
            }
            else
            {
                msg = Resources.Validation.ErrorMsg_LoginFail;
            }

            JsonResponse<SystemUser> response = new JsonResponse<SystemUser>().GetResponse(success, null, msg, returnUrl, alertMessage);
            return Json(response);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            AlertMessage alertMessage = new AlertMessage();
            bool success = false;
            string msg, redirectTo = "";
            if (ModelState.IsValid)
            {
                if(systemUserModel.IsNotExists( email: model.Email))
                {
                    var user = new SystemUser { UserName = model.Email, Email = model.Email, UserFullName = model.UserFullName };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        success = true;
                        msg = Resources.Resource.Alert_UserRegister_SuccessMessage;

                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        redirectTo = Url.Action("Login", "Account");
                    }
                    else
                    {
                        msg = string.Join("\n", result.Errors);
                    }
                }
                else
                {
                    msg = Resources.Validation.ErrorMsg_EmailExist;
                }
            }
            else
            {
                msg = Resources.Validation.ErrorMsg_InvalidInputs;
            }

            JsonResponse<SystemUser> response = new JsonResponse<SystemUser>().GetResponse(success, null, msg, redirectTo, alertMessage);
            return Json(response);
        }

        //
        // POST: /Account/SignOut
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            bool success = false;
            string msg = "", redirectTo = Url.Action("Login", "Account");
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AlertMessage alertMessage = new AlertMessage();
            success = true;
            JsonResponse<SystemUser> response = new JsonResponse<SystemUser>().GetResponse(success, null, msg, redirectTo, alertMessage);
            return Json(response);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}