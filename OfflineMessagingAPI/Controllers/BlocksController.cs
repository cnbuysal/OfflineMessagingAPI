using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfflineMessagingAPI.Business.Abstract;
using OfflineMessagingAPI.Entities.Helpers;
using Serilog;

namespace OfflineMessagingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlocksController : ControllerBase
    {
        private readonly IBlockServices _blockServices;
        private readonly IUserServices _userServices;
        public BlocksController(IBlockServices blockServices, IUserServices userServices)
        {
            _blockServices = blockServices;
            _userServices = userServices;
        }
        /// <summary>
        /// Block user by username 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BlockUser")]
        [ProducesResponseType(typeof(ResponseHelper), (int)HttpStatusCode.OK)]
        public IActionResult BlockUser([FromBody]BlockUserHelper parameters)
        {
            var blockerUser = _userServices.GetUserById(parameters.BlockerId);
            var checkIfUserExists = _userServices.CheckUserExistsByUsername(parameters.BlockedUsername);

            if (blockerUser == null || blockerUser.UserName == parameters.BlockedUsername)
            {
                Log.Warning($"Unauthorized block attempt.");
                return Unauthorized("Bu işlemi yapmak için yetkiniz bulunmamaktadır.");
            }

            if (parameters == null || string.IsNullOrWhiteSpace(parameters.BlockedUsername))
            {
                Log.Error($"Username invalid.");
                return BadRequest("Lütfen kullanıcı adını doğru giriniz.");
            }

            var response = _blockServices.BlockUser(blockerUser.UserName, parameters.BlockedUsername);
            Log.Information($"User with username {blockerUser.UserName} is blocked user with username {parameters.BlockedUsername}");
            return Ok(response);
        }
    }
}