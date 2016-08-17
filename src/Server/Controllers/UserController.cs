using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    public class UserController : Controller
    {
        public AppDbContext _context { get; set; }

        public UserController (AppDbContext ctx)
        {
            _context = ctx;
        }

        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            var user = new User(model.Username, model.Email, model.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Json(user);
        }
    }
}
