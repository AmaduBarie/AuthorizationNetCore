using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


    
        public IActionResult Privacy()
        {
            return View();
        }


        [Authorize(Roles ="Admin")]
        public IActionResult Security()
        {
            return View();
        }



        [HttpGet("/login")]
        public IActionResult login(string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }


        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return View();
        }




        [HttpPost("/login")]
        public async Task<IActionResult> login(string name,string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            if (name == "barrie")
            {
                var claim = new List<Claim>();
                claim.Add(new Claim(ClaimTypes.NameIdentifier, name));
                claim.Add(new Claim("username", name));
                claim.Add(new Claim(ClaimTypes.Name, name));
                claim.Add(new Claim(ClaimTypes.Role, "Admin"));
                claim.Add(new Claim(ClaimTypes.GivenName, "alhaji barrie"));
                var claimIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrin = new ClaimsPrincipal(claimIdentity);
                await HttpContext.SignInAsync(claimPrin);

                return Redirect(ReturnUrl);

            }
            return BadRequest();
        }



        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();   
            return Redirect("/");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
