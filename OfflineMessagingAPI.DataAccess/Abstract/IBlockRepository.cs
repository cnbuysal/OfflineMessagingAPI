using OfflineMessagingAPI.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfflineMessagingAPI.DataAccess.Abstract
{
    public interface IBlockRepository
    {
        List<Block> GetBlocksByUser(User user);

        Block GetBlockById(int id);

        void CreateBlock(Block block);

        void DeleteBlock(int id);
    }
}
