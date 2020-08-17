using OfflineMessagingAPI.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfflineMessagingAPI.Business.Abstract
{
    public interface IUserServices
    {
        #region CrudUserServices

        List<User> GetUsers();

        User GetUserById(int id);

        User GetUserByUsername(string username);

        #endregion

        #region CheckUserServices

        bool CheckUserExistsByUsername(string username);

        #endregion
    }
}
