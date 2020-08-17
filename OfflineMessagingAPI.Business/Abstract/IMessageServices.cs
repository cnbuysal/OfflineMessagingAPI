using OfflineMessagingAPI.Entities.Entities;
using OfflineMessagingAPI.Entities.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OfflineMessagingAPI.Business.Abstract
{
    public interface IMessageServices
    {
        #region CrudMessageServices

        Message GetMessageById(int id);

        List<Message> GetAllMessagesHistory(GetMessageHistoryHelper parameters);

        #endregion

        #region MessageServices

        ResponseHelper Send(SendMessageHelper parameters);

        #endregion
    }
}
