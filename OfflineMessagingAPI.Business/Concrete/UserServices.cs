using Microsoft.Extensions.Logging;
using OfflineMessagingAPI.Business.Abstract;
using OfflineMessagingAPI.DataAccess.Abstract;
using OfflineMessagingAPI.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfflineMessagingAPI.Business.Concrete
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserServices> _logger;

        public UserServices(IUserRepository userRepository , ILogger<UserServices> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public List<User> GetUsers()
        {
           return  _userRepository.GetUsers();
        }

        public bool CheckUserExistsByUsername(string username)
        {
            if (GetUserByUsername(username) != null)
            {
                return true;
            }
            return false;
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public User GetUserByUsername(string username)
        {
            return _userRepository.GetUserByUsername(username);
        }

    }
}