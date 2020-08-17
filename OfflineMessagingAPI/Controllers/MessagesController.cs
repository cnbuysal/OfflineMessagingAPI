using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfflineMessagingAPI.Business.Abstract;
using OfflineMessagingAPI.Entities.Entities;
using OfflineMessagingAPI.Entities.Helpers;
using Serilog;

namespace OfflineMessagingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageServices _messageServices;
        private readonly IUserServices _userServices;
        public MessagesController(IMessageServices messageServices , IUserServices userServices)
        {
            _messageServices = messageServices;
            _userServices = userServices;
        }

        /// <summary>
        /// Send message to another user with username
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Send")]
        [ProducesResponseType(typeof(ResponseHelper), (int)HttpStatusCode.OK)]
        public IActionResult Send([FromBody]SendMessageHelper parameters)
        {

            if (parameters == null || string.IsNullOrWhiteSpace(parameters.ReceiverUsername))
            {
                Log.Error($"Username is invalid.");
                return BadRequest("Lütfen kullanıcı adını doğru giriniz.");
            }

            var response = _messageServices.Send(parameters);
            if (response.Success)
            {
                Log.Error($"Message sent successfully.");
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Get message history between two users in order from latest to oldest
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fromUserName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/{fromUserName}")]
        [ProducesResponseType(typeof(Message), (int)HttpStatusCode.OK)]
        public IActionResult GetMessagesHistory([FromRoute]int id, string fromUserName)
        {
            var userChecked = _userServices.GetUserById(id);
            var userFrom = _userServices.GetUserByUsername(fromUserName);

            if (userChecked == null)
            {
                Log.Error($"Invalid request.");
                return BadRequest("Geçersiz işlem."); 
            }

            if (userFrom == null)
            {
                Log.Error($"There is no user in the database with the username of {fromUserName}");
                return BadRequest("Böyle bir kullanıcı sistemde kayıtlı değildir.");
            }

            GetMessageHistoryHelper parameters = new GetMessageHistoryHelper() { SenderId = id, From = fromUserName, ReceiverId = userFrom.Id };

            var response = _messageServices.GetAllMessagesHistory(parameters);
            Log.Information($"{userChecked.UserName} viewed his/her message history with {fromUserName}");
            return Ok(response);
        }

    }
}