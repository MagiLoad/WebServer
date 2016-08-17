using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private AppDbContext _context;
        public UserController (AppDbContext ctx)
        {
            _context = ctx;
        }

        [HttpGet("signin/{username}/{password}")]
        public string SignIn(string username, string password)
        {
            // Find a user where Username or Email equals the requested username
            var user = _context.Users.First(u => u.Username == username || u.Email == username);

            // If no user was found, return nothing
            if (user == null)
                return "";

            // Ensure the passwords match
            if (user.ConfirmPassword(password))
                return user.Token;  // If match, return token

            // If passwords don't match, return nothing
            return "";
        }
    }
}
