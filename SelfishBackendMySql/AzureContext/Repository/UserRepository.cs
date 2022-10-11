using AzureContext;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly sellfish_dbContext _context;
        public UserRepository(sellfish_dbContext context)
        {
            _context = context;
        }

        public bool AddUser(User User)
        {


            _context.Users.Add(User);
            return Save();
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUser(int Id)
        {
            return _context.Users.FirstOrDefault(o => o.Id == Id);
        }

        public bool Save()
        {
            var IsSaved = _context.SaveChanges();

            return IsSaved > 0 ? true : false;
        }
    }
}
