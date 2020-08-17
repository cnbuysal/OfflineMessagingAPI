using OfflineMessagingAPI.Entities.Entities;
using OfflineMessagingAPI.Entities.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfflineMessagingAPI.DataAccess.Abstract
{
    public interface IMessageRepository
    {
        List<Message> GetAllMessagesHistory(GetMessageHistoryHelper parameters);

        Message GetMessageById(int id);

        bool CreateMessage(Message message);

        void DeleteMessage(int id);

        void SaveChanges();
    }
}
