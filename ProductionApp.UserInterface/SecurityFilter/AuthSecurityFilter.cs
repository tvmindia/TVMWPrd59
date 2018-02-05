using Microsoft.Practices.Unity;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using ProductionApp.DataAccessObject.DTO;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Script.Serialization;
using System.Web.Security;
using Unity.Attributes;

namespace ProductionApp.UserInterface.SecurityFilter
{
    public class AuthSecurityFilter : ActionFilterAttribute, IAuthenticationFilter, IAuthorizationFilter
    {
        public string LoggedUserName { get; set; }
        public string ProjectObject { get; set; }
        public string Mode { get; set; }
        [Dependency]
        public IUserBusiness _userBusiness { get; set; }
      
        public void OnAuthentication(AuthenticationContext filterContext)
        {
           //var controllerName = filterContext.RouteData.Values["controller"];
           //var actionName = filterContext.RouteData.Values["action"];
           // if (filterContext.HttpContext.Request.IsAjaxRequest())
           // {
                if ((filterContext.HttpContext.Session == null) || (filterContext.HttpContext.Session["TvmValid"] == null))
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                    return;
                }
                var authCookie = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null)
                {
                    // Unauthorized
                    filterContext.Result = new HttpUnauthorizedResult();
                    return;
                }
                // Get the forms authentication ticket.
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket == null)
                {
                    filterContext.Result = new HttpUnauthorizedResult(); // mark unauthorized*/
                }
                else
                {

                    filterContext.HttpContext.User = new System.Security.Principal.GenericPrincipal(
                    new System.Security.Principal.GenericIdentity(authTicket.Name, "Forms"), authTicket.UserData.Split(',').Select(t => t.Trim()).ToArray());
                  
                  
                    UA _ua = (UA)filterContext.HttpContext.Session["TvmValid"];
                    AppUA appUA = new AppUA();
                    appUA.RolesCSV = authTicket.UserData;
                    appUA.UserName = _ua.UserName;
                    LoggedUserName = appUA.UserName;
                    ProductionApp.DataAccessObject.DTO.Common common = new ProductionApp.DataAccessObject.DTO.Common();
                    appUA.DateTime = common.GetCurrentDateTime();
                    appUA.AppID = _ua.AppID;
                    filterContext.HttpContext.Session.Add("AppUA", appUA);
            }
            //}
            //NON AJAX CALL
            //else
            //{

            //    if ((filterContext.HttpContext.Session == null) || (filterContext.HttpContext.Session["TvmValid"] == null))
            //    {
            //        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary() { { "controller", "Account" }, { "action", "Index" } });
            //        return;
            //    }
            //    ////
            //    var authCookie = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            //    if (authCookie == null)
            //    {
            //        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary() { { "controller", "Account" }, { "action", "Index" } });
            //        return;
            //    }
            //    // Get the forms authentication ticket.
            //    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //    //  object usercookie = JsonConvert.DeserializeObject(authTicket.UserData); // Up to you to write this Deserialize method -> it should be the reverse of what you did in your Login action
            //    if (authTicket == null)
            //    {
            //        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary() { { "controller", "Account" }, { "action", "Index" } });
            //    }
            //    else
            //    {

            //        filterContext.HttpContext.User = new System.Security.Principal.GenericPrincipal(
            //        new System.Security.Principal.GenericIdentity(authTicket.Name, "Forms"), authTicket.UserData.Split(',').Select(t => t.Trim()).ToArray());
            //    }
            //}

        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    JsonResult json = new JsonResult
                    {
                        Data = new { Result = "UNAUTH", Message = "Authorization failed!." }
                    };
                    //filterContext.Result= new JsonResult() { Data = new { Result = "UNAUTH", Message = "" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    filterContext.Result = new JsonResult() { Data = new JavaScriptSerializer().Serialize(json.Data), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    //filterContext.Result = new JsonResult() { Data = JsonConvert.SerializeObject(new { Result = "UNAUTH", Message = "" }), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary() { { "controller", "Account" }, { "action", "Index" } });
                }
            }
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
             Permission _permission =null;
            _permission = _userBusiness.GetSecurityCode(LoggedUserName, ProjectObject);
            if (_permission.AccessCode.Contains(Mode))
            {
                //Allows Permission
                filterContext.HttpContext.Session.Add("UserRights", _permission);
            }
            else
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else
               {    //unauthorized page show here 
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary() { { "controller", "Account" }, { "action", "NotAuthorized" } });
                }
                    
            }
        }

        //
    }
}