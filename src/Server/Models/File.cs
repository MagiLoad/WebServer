using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class File
    {
        [Key]
        public int FileId { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public DateTimeOffset Upploaded { get; set; }

        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public User Owner { get
            {
                if (_owner != null)
                    return _owner;
                using (var db = new AppDbContext())
                    _owner = db.Users.First(u => u.UserId == OwnerId);
                return _owner;
            }
            set { _owner = value; } }

        private User _owner;

        // Constructor for EF
        public File () { }

        // Constructor for creating new files
        public File(string name, User owner, byte[] content)
        {
            Name = name;
            Content = content;
            Owner = owner;
            Upploaded = DateTimeOffset.UtcNow;
        }



        // -----------------------------------------
        // Public functions for retreaving meta data
        // -----------------------------------------

        // Returns the file extention
        public string FileExtention { get { return Name.Split('.').LastOrDefault(); } }

        // If the file extention is a "raw" text format, return true, else return 
        public bool ShowAsRawText {
            get {
                return hasExtention(new string[] {
                    ".txt", ".json", ".xml", ".bat", ".sh"
                });
            } }

        // If the file extention is a image format, return true, else return false
        public bool ShowAsImage {
            get {
                return hasExtention(new string[] {
                    ".jpg", ".png", ".gif"
                });
            } }


        // ---------------
        // Helper functions
        // ---------------

        private bool hasExtention (IEnumerable<string> extentions)
        {
            foreach (var extention in extentions)
                if (Name.EndsWith(extention))
                    return true;
            return false;
        }

    }
}
