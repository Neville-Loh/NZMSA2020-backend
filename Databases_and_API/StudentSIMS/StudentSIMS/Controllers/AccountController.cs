using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSIMS.Data;
using StudentSIMS.Models;
using Microsoft.AspNetCore.Identity;
using StudentSIMS.ViewModel;

namespace StudentSIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly StudentContext _context;

        public AccountController(UserManager<IdentityUser> userManager,
                                SignInManager<IdentityUser> signInManager,
                                StudentContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return null;
            //return View();
        }

        [HttpPost]
        //[HttpPut]
        //[ValidateAntiForgeryToken]
        //Register the user and set session cookie
        //public async Task<ActionResult<UserLogin>> RegisterUser(string email, string password)
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //if (ModelState.IsValid)
            //{
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    //redirect to home page
                }
            //}


            foreach (var error in result.Errors)
            {
                //log error message
                //ModelState.AddModelError(string.Empty, error.Description);
            }

            //var user = await _userManage
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
