using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Models.ApiModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Xml.Serialization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {

        private AppDbContext _context;

        public FileController(AppDbContext ctx)
        {
            _context = ctx;
        }



        // POST api/file
        // Uploading files with a string in the body following the following template:
        //  <?xml version="1.0" encoding="UTF-8"?>
        //  <file>
        //      <name>{filename}</name>
        //      <user>{user token}</user>
        //      <content>{content base64 encoded}</content>
        //  </file>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            bool result = false;

            // Create a in memmory stream of all the content in the body
            var requestBody = new System.IO.MemoryStream();
            await Request.Body.CopyToAsync(requestBody);

            try {

                // Convert Memory Stream to String
                var requestString = System.Text.Encoding.UTF8.GetString(requestBody.ToArray());

                // Deserialize the request body
                var serializer = new XmlSerializer(typeof(FileUpload));
                var uploadedFile = (FileUpload)serializer.Deserialize(new System.IO.StringReader(requestString));

                // Convert content from base64 to byte[]
                var content = Convert.FromBase64String(uploadedFile.Content);

                // Get the user who uploaded the file
                var user = _context.Users.First(u => u.Token == uploadedFile.UserToken);

                // Create a file to store in the database
                var file = new File(uploadedFile.Name, user, content);

                // save file to database
                _context.Files.Add(file);
                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception e)
            {
                // throw (e);
            }
            return Json(new { success = result });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            // Find file by id
            var file = _context.Files.FirstOrDefault(f => f.FileId == id);

            // If no file was found, return 404
            if (file == null) return NotFound();

            // Send file to client so they can download it
            return File(file.Content, "application/x-msdownload", file.Name);
        }

        
    }
}
