using Common.Interfaces;
using Common.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Utilities;
using Utilities.Models;
using WebApp.Models;

namespace WebApp
{
    public class HomeController : Controller
    {
        public ISolutionBusinessLayer SolutionBusinessLayer { get; set; }
        public HomeController(ISolutionBusinessLayer solutionBusinessLayer)
        {
            SolutionBusinessLayer = solutionBusinessLayer;
        }

        public async Task<ActionResult> Index()
        {
            var solutions = await SolutionBusinessLayer.GetSolutionsAsync();
            var model = new SolutionsViewModel
            {
                Solutions = solutions
            };
            return View(model);
        }

        public async Task<ActionResult> SelectSolution(string solutionId)
        {
            if (string.IsNullOrEmpty(solutionId))
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            var solution = await SolutionBusinessLayer.GetSolutionAsync(solutionId);
            if (solution == null)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            var session = new UserSession
            {
                SolutionId = solutionId
            };
            UserSession userSession = await SolutionBusinessLayer.UpdateUserSessionAsync(session);
            return RedirectToAction("Subscription", new RouteValueDictionary(new { controller = "Home", action = "Subscription", sessionId = userSession.RowKey }));
        }

        public async Task<ActionResult> Subscription(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }

            var session = await SolutionBusinessLayer.GetUserSessionByIdAsync(sessionId);
            if(session == null || session.ExpireTime> DateTime.Now)
            {
                return RedirectToAction("Subscription", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            ViewBag.sessionId = sessionId;
            return View();
        }

        public async Task<ActionResult> SelectSubscription(string subscriptionId, string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            var session = await SolutionBusinessLayer.GetUserSessionByIdAsync(sessionId);
            if (session == null || session.ExpireTime > DateTime.Now)
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else if(string.IsNullOrEmpty(subscriptionId) && session.SubscriptionId!=null)
            {
                subscriptionId = session.SubscriptionId;
            }
            string directoryId = string.Empty;
            if (!string.IsNullOrEmpty(subscriptionId))
                directoryId = await AzureResourceManagerUtil.GetDirectoryForSubscription(subscriptionId);
            if (!String.IsNullOrEmpty(directoryId))
            {
                if (!User.Identity.IsAuthenticated || !directoryId.Equals(ClaimsPrincipal.Current.FindFirst
                    ("http://schemas.microsoft.com/identity/claims/tenantid").Value))
                {
                    HttpContext.GetOwinContext().Environment.Add("Authority",
                        string.Format(ConfigurationManager.AppSettings["Authority"] + "OAuth2/Authorize", directoryId));

                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    session = new UserSession
                    {
                        SubscriptionId= subscriptionId,
                        RowKey = sessionId,
                        SolutionId=session.SolutionId
                    };
                    UserSession userSession = await SolutionBusinessLayer.UpdateUserSessionAsync(session);

                    dict["prompt"] = "select_account";
                    var redirectUrl = Url.Action("SelectSubscription", "Home") + "?sessionId=" + sessionId;
                    HttpContext.GetOwinContext().Authentication.Challenge(
                        new AuthenticationProperties(dict) {
                            RedirectUri = redirectUrl
                        },
                        OpenIdConnectAuthenticationDefaults.AuthenticationType);
                }
                else
                {
                    string objectIdOfCloudSenseServicePrincipalInDirectory = await
                        AzureADGraphAPIUtil.GetObjectIdOfServicePrincipalInDirectory(directoryId, ConfigurationManager.AppSettings["ClientID"]);

                    await AzureResourceManagerUtil.GrantRoleToServicePrincipalOnSubscription
                        (objectIdOfCloudSenseServicePrincipalInDirectory, subscriptionId, directoryId);
                    var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name).Value;
                    Subscription s = new Subscription()
                    {
                        Id = subscriptionId,
                        DirectoryId = directoryId,
                        ConnectedBy = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name).Value,
                        ConnectedOn = DateTime.Now
                    };

                    bool res = await SolutionBusinessLayer.UpdateSubscriptionAsync(s);
                    session = new UserSession
                    {
                        RowKey = sessionId,
                        UserId = userId,
                        SubscriptionId = subscriptionId,
                        TenantId = directoryId
                    };
                    UserSession userSession = await SolutionBusinessLayer.UpdateUserSessionAsync(session);
                    return RedirectToAction("Provision", new RouteValueDictionary(new { controller = "Home", action = "Provision", sessionId = sessionId }));
                }

            }
            return null;
        }

        public async Task<ActionResult> Provision(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            var session = await SolutionBusinessLayer.GetUserSessionByIdAsync(sessionId);
            if (session == null || session.ExpireTime > DateTime.Now)
            {
                return RedirectToAction("Subscription", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            var solution = await SolutionBusinessLayer.GetSolutionAsync(session.SolutionId);
            var viewModel = new Provision
            {
                SolutionName = solution.DisplayName,
                SubscriptionId = session.SubscriptionId,
                TenantId = session.TenantId,
                SessionId = session.RowKey,
                AzureAccountOwnerName = session.UserId
            };
            ViewBag.sessionId = sessionId;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Provision(Provision provision)
        {
            if (string.IsNullOrEmpty(provision.SessionId))
            {
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            var session = await SolutionBusinessLayer.GetUserSessionByIdAsync(provision.SessionId);
            if (session == null || session.ExpireTime > DateTime.Now)
            {
                return RedirectToAction("Subscription", new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            var build = new Build
            {
                AzureAccountOwnerName = provision.AzureAccountOwnerName,
                AzureSubscriptionId = provision.SubscriptionId,
                AzureTenantId = provision.TenantId,
                DeploymentName = provision.DeploymentName,
                PresetAzureLocationName = provision.LocationName,
                SolutionId = session.SolutionId,
                VmAdminPassword = provision.VmAdminPassword,
                ServicePrincipalId = ConfigurationManager.AppSettings["ClientID"],
                ServicePrincipalPassword = ConfigurationManager.AppSettings["Password"]
            };
            build = await SolutionBusinessLayer.BuildSolutionAsync(build);
            return View("ProvisionStatus", build);
        }

        //public async Task DisconnectSubscription(string subscriptionId)
        //{
        //    string directoryId = await AzureResourceManagerUtil.GetDirectoryForSubscription(subscriptionId);

        //    string objectIdOfCloudSenseServicePrincipalInDirectory = await
        //        AzureADGraphAPIUtil.GetObjectIdOfServicePrincipalInDirectory(directoryId, ConfigurationManager.AppSettings["ClientID"]);

        //    await AzureResourceManagerUtil.RevokeRoleFromServicePrincipalOnSubscription
        //        (objectIdOfCloudSenseServicePrincipalInDirectory, subscriptionId, directoryId);

        //    //Subscription s = db.Subscriptions.Find(subscriptionId);
        //    //if (s != null)
        //    //{
        //    //    db.Subscriptions.Remove(s);
        //    //    db.SaveChanges();
        //    //}

        //    Response.Redirect(this.Url.Action("Index", "Home"));
        //}
        //public async Task RepairSubscriptionConnection(string subscriptionId)
        //{
        //    string directoryId = await AzureResourceManagerUtil.GetDirectoryForSubscription(subscriptionId);

        //    string objectIdOfCloudSenseServicePrincipalInDirectory = await
        //        AzureADGraphAPIUtil.GetObjectIdOfServicePrincipalInDirectory(directoryId, ConfigurationManager.AppSettings["ClientID"]);

        //    await AzureResourceManagerUtil.RevokeRoleFromServicePrincipalOnSubscription
        //        (objectIdOfCloudSenseServicePrincipalInDirectory, subscriptionId, directoryId);
        //    await AzureResourceManagerUtil.GrantRoleToServicePrincipalOnSubscription
        //        (objectIdOfCloudSenseServicePrincipalInDirectory, subscriptionId, directoryId);

        //    Response.Redirect(this.Url.Action("Index", "Home"));
        //}
    }
}