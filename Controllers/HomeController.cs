using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Login_Reg.Models;

namespace Login_Reg.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        //This Index will be the page with a registration form
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User fromForm)
        {
            // Check initial ModelState
            if(ModelState.IsValid)
            {
                // If a User exists with provided email
                if(_context.Users.Any(user => user.Email == fromForm.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");
                    
                    // You may consider returning to the View at this point
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    fromForm.Password = Hasher.HashPassword(fromForm, fromForm.Password);
                    _context.Add(fromForm);
                    _context.SaveChanges();

                    System.Console.WriteLine("The User's Id is " + fromForm.UserId);
                    HttpContext.Session.SetInt32("UserId", fromForm.UserId);

                    return RedirectToAction("Success", new { userId = fromForm.UserId});
                }
            }
            else
            {
                return View("Index");
            }
        } 

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost("loginpost")]
        public IActionResult LoginPost(LoginUser submission)
        {
            if(ModelState.IsValid)
            {
                // If a User exists with provided email
                var userInDb = _context.Users.FirstOrDefault(u => u.Email == submission.LogEmail);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return Index();
                }
                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();
                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(submission, userInDb.Password, submission.LogPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("Password", "Invalid Email/Password");
                    return Login();
                }
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                return RedirectToAction("Success", new { userId = userInDb.UserId});
            }
            return RedirectToAction("Index");
        }


        [HttpGet("success")]
        public IActionResult Success()
        {
            // To retrieve a string from session we use ".GetString"
            int? SessionUserId = HttpContext.Session.GetInt32("UserId");
            if (SessionUserId != null)
            {
                User LoggedInUser = _context.Users.FirstOrDefault(user => user.UserId == SessionUserId);
                return View(LoggedInUser);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
