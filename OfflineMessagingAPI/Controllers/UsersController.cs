using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfflineMessagingAPI.Business.Abstract;
using OfflineMessagingAPI.Entities.Entities;
using OfflineMessagingAPI.Entities.Helpers;
using Serilog;

namespace OfflineMessagingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserServices _userServices;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, IUserServices userServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userServices = userServices;
        }

        /// <summary>
        /// User Register
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register([FromBody]RegisterHelper parameters)
        {
            if (parameters == null || string.IsNullOrWhiteSpace(parameters.UserName) || string.IsNullOrWhiteSpace(parameters.Password))
            {
                Log.Error($"Attempt to register with null parameters.");
                return BadRequest("Lütfen zorunlu alanları doldurunuz.");
            }

            if (_userServices.CheckUserExistsByUsername(parameters.UserName))
            {
                Log.Error($"Username {parameters.UserName} already exists.");
                return BadRequest("Kullanıcı adı sistemde mevcuttur, lütfen başka bir kullanıcı adı seçiniz.");
            }

            var user = new User() { UserName = parameters.UserName, Password = parameters.Password };
            await _userManager.CreateAsync(user, user.Password);
            Log.Information($"User with Username:{parameters.UserName} registered successfully.");
            return Ok(user);

        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody]LoginHelper parameters)
        {
            var loginUser = await _userManager.FindByNameAsync(parameters.UserName);

            if (loginUser != null)
            {
                var response = await _signInManager.PasswordSignInAsync(loginUser, parameters.Password, false, false);

                if (response.Succeeded)
                {
                    Log.Information($"User with username {parameters.UserName} logged in successfully.");
                    return Ok(loginUser);
                }
            }
            Log.Error($"Username {parameters.UserName} ,invalid login attempt.");
            return BadRequest("Hatalı kullanıcı adı veya parola girişi yaptınız.");
        }
    }
}