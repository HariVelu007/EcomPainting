using EcomPainting.Dtos;
using EcomPainting.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;

namespace EcomPainting.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly EcomContext _context;
        public AccountController(EcomContext ecomContext)
        {
            _context = ecomContext;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginDto());
        }
        [HttpPost]
        public IActionResult Login(LoginDto dto)
        {
            if(!ModelState.IsValid)
            {
                return View(dto);
            }
            User user= _context.Users.Where(u=> u.UserMail==dto.UserMail).FirstOrDefault();
            if(user==null)
            {
                ModelState.AddModelError(nameof(dto.UserMail), "User mail doesn't exist");
                return View(dto);
            }
            if (user.Password != dto.Password)
            {
                ModelState.AddModelError(nameof(dto.Password), "Invalid password");
                return View(dto);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Email, user.UserMail),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,               
                IsPersistent = true,             
            };

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            
            return RedirectToAction(user.Role=="User"?"Index": "Items", user.Role == "User" ? "Home":"Admin");
        }
        [HttpGet]        
        public IActionResult Register()
        {
            return View(new UserDto());
        }
        [HttpPost]
        public IActionResult Register(UserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            User user = _context.Users.Where(u => u.UserMail == dto.UserMail).FirstOrDefault();
            if (user != null)
            {
                ModelState.AddModelError(nameof(dto.UserMail), "User mail already exists");
                return View(dto);
            }
            User newuser = new User
            {
                UserMail = dto.UserMail,
                Password = dto.Password,
                UserName = dto.UserName
            };
            _context.Users.Add(newuser);
            bool result = _context.SaveChanges()>0?true:false;
            ViewBag.Status = result;
            return View();
        }
    }
}
