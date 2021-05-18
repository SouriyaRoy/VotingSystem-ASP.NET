using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using votingSystem.Models;

namespace votingSystem
{
    public class UserAuthentication : IDisposable
    {
        private PollingSystemEntities context = new PollingSystemEntities();
        public User ValidateUser(string username, string password)
        {
            string encryptedPassword = Encryptpass(password);
            return context.Users.FirstOrDefault(user =>
                user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
                && user.Password == encryptedPassword);
        }

        public string Encryptpass(string password)
        {
            string msg = "";
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            msg = Convert.ToBase64String(encode);
            return msg;
        }

        public void Dispose()
        {
            // Dispose();
        }
    }
}