using AutoMapper;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.UserInterface.Models;

namespace ProductionApp.UserInterface.Controllers
{
    public class AccountController : Controller
    {        
        Const _const = new Const();
        IUserBusiness _userBusiness;
        Guid AppID = Guid.Parse(ConfigurationManager.AppSettings["ApplicationID"]);
        public AccountController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel loginVM)
        {
            UserViewModel userVM = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    loginVM.IsFailure = true;
                    loginVM.Message = _const.LoginFailed;
                    return View("Index", loginVM);
                }
                userVM = Mapper.Map<User, UserViewModel>(_userBusiness.CheckUserCredentials(Mapper.Map<LoginViewModel, User>(loginVM)));
                if (userVM != null)
                {
                    if (userVM.RoleList == null || userVM.RoleList.Count == 0 && string.IsNullOrEmpty(userVM.RoleIDCSV))
                    {
                        loginVM.IsFailure = true;
                        loginVM.Message = _const.LoginFailedNoRoles;
                        return View("Index", loginVM);
                    }
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userVM.UserName, DateTime.Now, DateTime.Now.AddHours(24), true, userVM.RoleCSV);
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
                    //session setting
                    UA ua = new UA();
                    ua.UserName = userVM.LoginName;
                    ua.AppID = AppID;
                    Session.Add("TvmValid", ua);
                    if (userVM.RoleCSV.Contains("SAdmin") || userVM.RoleCSV.Contains("CEO"))
                    {
                        return RedirectToAdminDashboard();
                    }
                    else
                    {
                        return RedirectToLocal();
                    }

                }
                else
                {
                    loginVM.IsFailure = true;
                    loginVM.Message = _const.LoginFailed;
                    return View("Index", loginVM);
                }



            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #region Logout
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Remove("TvmValid");
                Session.Remove("UserRights");
                Session.Remove("AppUA");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToLogin();
        }

        #endregion Logout
        private ActionResult RedirectToLocal()
        {
            return RedirectToAction("Index", "DashBoard");
        }
        private ActionResult RedirectToLogin()
        {
            return RedirectToAction("Index", "Account");
        }
        private ActionResult RedirectToAdminDashboard()
        {
            return RedirectToAction("Index", "DashBoard");
        }

        [HttpGet]
        public ActionResult NotAuthorized()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Down()
        {
            return View();
        }
    }
}