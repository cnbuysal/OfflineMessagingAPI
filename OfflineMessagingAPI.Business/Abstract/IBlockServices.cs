using OfflineMessagingAPI.Entities.Entities;
using OfflineMessagingAPI.Entities.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfflineMessagingAPI.Business.Abstract
{
    public interface IBlockServices
    {
        bool CreateBlock(Block block);

        ResponseHelper BlockUser(string blockerUsername, string blockedUsername);

        bool CheckIfUserBlocked(int possibleBlockerId, int possibleBlockedId);
    }
}
