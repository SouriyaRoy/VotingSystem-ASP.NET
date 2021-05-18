using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using votingSystem.Models;

namespace votingSystem.repositories
{
    public class UserRepo
    {
        private PollingSystemEntities entity = new PollingSystemEntities();

        public void Register(User user)
        {
            user.Password = Encryptpass(user.Password);
            entity.Users.Add(user);
            entity.SaveChanges();
        }

        public string Encryptpass(string password)
        {
            string msg = "";
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            msg = Convert.ToBase64String(encode);
            return msg;
        }
    }
}