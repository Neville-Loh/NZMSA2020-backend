using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentSIMS.Controllers
{

    /// <summary>
    ///  User login     form (React) ->> api .
    ///  user sign up        post --> api 
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Guard the action
        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Authenticate()
        {
            return RedirectToAction("Index");
        }


    }
}
