using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSIMS.Data;
using StudentSIMS.Models;

namespace StudentSIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly StudentContext _context;
        public LoginController(UserManager<IdentityUser> userManager,
                                SignInManager<IdentityUser> signInManager,
                                StudentContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        //Get the list of users 
        public async Task<ActionResult<IEnumerable<UserLogin>>> GetUser()
        {
            return await _context.UserLogin.ToListAsync();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //Register the user and set session cookie
        public async Task<ActionResult<UserLogin>> RegisterUser(string email, string password)
        {

            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                //redirect to home page
            }

            foreach (var error in result.Errors)
            {
                //log error message
                //ModelState.AddModelError(string.Empty, error.Description);
            }

            //var user = await _userManage
            await _context.SaveChangesAsync();

            return null;
        }

        //Login User 
        //[HttpPost]
        //public async Task<ActionResult<UserLogin>> LoginUser(String email, string password)
        //{
        //    var result = await signInManager.PasswordSignInAsync(email, password, false, false);

        //    if (result.Succeeded)
        //    {
        //        //Redirect to home page
        //    }

        //    //Add code for error handling
        //    //Invalid user

        //    return null;
        //}
    }
}
