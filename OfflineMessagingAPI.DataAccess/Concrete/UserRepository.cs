using OfflineMessagingAPI.DataAccess.Abstract;
using OfflineMessagingAPI.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OfflineMessagingAPI.DataAccess.Concrete
{
    public class UserRepository : IUserRepository
    {
        private readonly OfflineMessagingDbContext _context;
        public UserRepository(OfflineMessagingDbContext context)
        {
            _context = context;
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public void DeleteUser(int id)
        {
            var removedUser = GetUserById(id);
            _context.Users.Remove(removedUser);
            _context.SaveChanges();
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == username);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
