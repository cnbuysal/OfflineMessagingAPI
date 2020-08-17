using OfflineMessagingAPI.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfflineMessagingAPI.DataAccess.Abstract
{
    public interface IUserRepository
    {
        List<User> GetUsers();

        User GetUserById(int id);

        User GetUserByUsername(string username);

        User CreateUser(User user);

        void UpdateUser(User user);

        void DeleteUser(int id);
    }
}
