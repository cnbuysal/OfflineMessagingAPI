using OfflineMessagingAPI.Business.Abstract;
using OfflineMessagingAPI.DataAccess.Abstract;
using OfflineMessagingAPI.Entities.Entities;
using OfflineMessagingAPI.Entities.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineMessagingAPI.Business.Concrete
{
    public class MessageServices : IMessageServices

    {
        private readonly IMessageRepository _messageRepository;
        private readonly IBlockServices _blockServices;
        private readonly IUserServices _userServices;


        public MessageServices(IMessageRepository messageRepository  , IBlockServices blockServices , IUserServices userServices)
        {
            _messageRepository = messageRepository;
            _blockServices = blockServices;
            _userServices = userServices;
        }

        #region CrudMessageServices

        public List<Message> GetAllMessagesHistory(GetMessageHistoryHelper parameters)
        {
            var allMessagesHistory = _messageRepository.GetAllMessagesHistory(parameters);
            return allMessagesHistory;
        }

        public Message GetMessageById(int id)
        {
            return _messageRepository.GetMessageById(id);
        }

        #endregion

        #region MessageServices

        public ResponseHelper Send(SendMessageHelper parameters)
        {
            var receiverUser = _userServices.GetUserByUsername(parameters.ReceiverUsername);
            var senderUser = _userServices.GetUserById(parameters.SenderId);

            if (receiverUser == null)
            {
                Log.Error($"There is no user in the database with the username of {parameters.ReceiverUsername}");
                return new ResponseHelper() {Success = false, Message = "Böyle bir kullanıcı sistemde kayıtlı değildir." };
            }

            var isUserBlocked = _blockServices.CheckIfUserBlocked(receiverUser.Id, parameters.SenderId);

            if (isUserBlocked || parameters.SenderId == receiverUser.Id)
            {
                Log.Error($"Send message attempt failed, user blocked!");
                return new ResponseHelper() { Success = false, Message = "Bu kullanıcıya mesaj gönderme yetkiniz yoktur." };
            }

            var createdMessage = new Message() { SenderId = parameters.SenderId, ReceiverId = receiverUser.Id, Content = parameters.MessageContent };
            var success = _messageRepository.CreateMessage(createdMessage);

            if (!success)
            {
                Log.Error($"Invalid send message attempt");
                return new ResponseHelper() { Success = false, Message = "Mesaj gönderilemedi, lütfen tekrar deneyiniz." };
            }
            Log.Information($"{senderUser.UserName} send message with id : {createdMessage.Id} to {parameters.ReceiverUsername} successfully.");
            return new ResponseHelper() { Success = true, Message = "Mesajınız başarıyla gönderilmiştir." };
        }
        #endregion
    }
}
