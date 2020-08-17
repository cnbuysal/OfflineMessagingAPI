using OfflineMessagingAPI.DataAccess.Abstract;
using OfflineMessagingAPI.Entities.Entities;
using OfflineMessagingAPI.Entities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OfflineMessagingAPI.DataAccess.Concrete
{
    public class MessageRepository : IMessageRepository
    {
        private readonly OfflineMessagingDbContext _context;

        public MessageRepository(OfflineMessagingDbContext context)
        {
            _context = context;
        }

        public bool CreateMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
            return true;
        }

        public void DeleteMessage(int id)
        {
            var deletedMessage = GetMessageById(id);
            _context.Messages.Remove(deletedMessage);
            _context.SaveChanges();
        }

        public Message GetMessageById(int id)
        {
            return _context.Messages.Find(id);
        }

        public List<Message> GetAllMessagesHistory(GetMessageHistoryHelper parameters)
        {
            List<Message> allMessagesHistory = _context.Messages.Where(m => (m.SenderId == parameters.SenderId && m.ReceiverId == parameters.ReceiverId) || (m.SenderId == parameters.ReceiverId && m.ReceiverId == parameters.SenderId))
                .OrderByDescending(m => m.CreatedAt).ToList();

            return allMessagesHistory;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
