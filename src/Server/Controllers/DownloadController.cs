using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("[controller]")]
    public class DownloadController : Controller
    {

        private AppDbContext _context;

        public DownloadController (AppDbContext ctx)
        {
            _context = ctx;
        }

        [HttpGet("{id}")]
        public IActionResult Index(int id)
        {
            var file = _context.Files.FirstOrDefault(f => f.FileId == id);

            return View(file);
        }
    }
}
