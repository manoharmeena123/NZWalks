using System;
using NZWalks.API.Models.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NZWalks.API.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        private List<User> Users = new List<User>()
        {
            //new User()
            //{
            //    FirstName = "Read Only",
            //    LastName = "User",
            //    EmailAddress = "readonly@gmail.com",
            //    Id = Guid.NewGuid(),
            //    Username = "readonly@gmail.com",
            //    Password = "readonly",
            //    Roles = new List<string>{"reader"}
            //},
            
            //new User()
            //{
            //    FirstName = "Read Write",
            //    LastName = "User",
            //    EmailAddress = "readwrite@gmail.com",
            //    Id = Guid.NewGuid(),
            //    Username = "readwrite@gmail.com",
            //    Password = "readwrite",
            //    Roles = new List<string>{"reader", "writer"}
            //}
        };

        public List<User> Users1 { get => Users; set => Users = value; }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = Users1.Find(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) && x.Password == password);
            return user;
        }
    }
}


//{
//"username": "readonly@gmail.com",
//  "password": "readonly"
//}