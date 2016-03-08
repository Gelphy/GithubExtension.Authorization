using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using GithubExtensionClaim = GithubExtension.Security.DAL.Entities.Claim;

namespace GithubExtension.Security.WebApi.Controllers
{
    public class ClaimsController : Controller
    {
        public Security.DAL.Context.SecurityContext SecurityContext { get; set; }

        // GET: Claims
        public ActionResult Index()
        {
            // Get id from frontend project id
            int id = 1;

            var users = SecurityContext.Users.Where(u => u.Claims.Cast<GithubExtensionClaim>().Any(c => c.ClaimType == ClaimTypes.Role && (c.ProjectId == id)));
          
            return View();
        }


    }
}