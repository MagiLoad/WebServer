using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Server.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
        public virtual List<File> Files { get; set; }

        public User() { }
        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            SetPassword(password);
            SetToken();
        }



        // ----------------
        // Password helpers
        // ----------------

        private void SetPassword(string password)
        {
            // Generate and store  a 512 bit long Salt
            var salt = new byte[512 / 8];
            RandomNumberGenerator.Create().GetBytes(salt);
            Salt = salt;
            
            // Hash and store the password
            Hash = HashPassword(password);

            // Check if hash is correct
            if (!ConfirmPassword(password))
                throw new Exception("Password hashing broke! Password hashes didn't match");
        }

        // Converts string to byte[] and hashes password
        private byte[] HashPassword(string value) => HashPassword(System.Text.Encoding.UTF8.GetBytes(value));

        // Hashes a byte[] and the users salt using SHA512
        private byte[] HashPassword(byte[] value)
        {
            // Compute hash once
            byte[] res = SHA512.Create().ComputeHash(value.Concat(Salt).ToArray());
            
            // Compute the hash 1000 times more to make it harder to brute force
            for (int i = 0; i < 1000; i++)
                res = SHA512.Create().ComputeHash(value.Concat(res).ToArray());
            
            // return the computed hash
            return res;
        }

        // Compares a password to the hashed password in the database
        public bool ConfirmPassword(string password) => Hash.SequenceEqual(HashPassword(password));



        // -------------
        // Token Helpers
        // -------------

        // Generates a random, unique token and saves it to the User
        private void SetToken ()
        {
            // Get the bytes from the username
            var userNameBytes = System.Text.Encoding.UTF8.GetBytes(Username);

            // Generate a random byte array with a length of 1024
            var token = new byte[1024];
            RandomNumberGenerator.Create().GetBytes(token);

            // Base64 Encode the token array and the userNameBytes and save it to the user
            Token = System.Convert.ToBase64String(userNameBytes.Concat(token).ToArray());
        }
    }
}
