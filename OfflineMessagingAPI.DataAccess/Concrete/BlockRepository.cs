using OfflineMessagingAPI.DataAccess.Abstract;
using OfflineMessagingAPI.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OfflineMessagingAPI.DataAccess.Concrete
{
    public class BlockRepository : IBlockRepository

    {
        private readonly OfflineMessagingDbContext _context;
        public BlockRepository(OfflineMessagingDbContext context)
        {
            _context = context;
        }

        public void CreateBlock(Block block)
        {
            _context.Blocks.Add(block);
            _context.SaveChanges();
        }

        public void DeleteBlock(int id)
        {
            var deletedBlock = GetBlockById(id);
            _context.Blocks.Remove(deletedBlock);
            _context.SaveChanges();
        }

        public Block GetBlockById(int id)
        {
            return _context.Blocks.Find(id);
        }

        public List<Block> GetBlocksByUser(User user)
        {
            List<Block> blocksByUser = _context.Blocks.Where(x => x.BlockerUserId == user.Id).ToList();
            return blocksByUser;
        }
    }
}
