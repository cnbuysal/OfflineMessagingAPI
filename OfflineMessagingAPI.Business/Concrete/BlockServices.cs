using OfflineMessagingAPI.Business.Abstract;
using OfflineMessagingAPI.DataAccess.Abstract;
using OfflineMessagingAPI.Entities.Entities;
using OfflineMessagingAPI.Entities.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfflineMessagingAPI.Business.Concrete
{
    public class BlockServices : IBlockServices
    {
        private readonly IBlockRepository _blockRepository;
        private readonly IUserServices _userServices;
        public BlockServices(IBlockRepository blockRepository , IUserServices userServices)
        {
            _blockRepository = blockRepository;
            _userServices = userServices;
        }

        public bool CreateBlock(Block block)
        {
            _blockRepository.CreateBlock(block);
            return true;
        }

        public ResponseHelper BlockUser(string blockerUsername , string blockedUsername)
        {
            var blockerUser = _userServices.GetUserByUsername(blockerUsername);
            var blockedUser = _userServices.GetUserByUsername(blockedUsername);

            var checkUserExistsByUsername = _userServices.CheckUserExistsByUsername(blockedUsername);

            if(!checkUserExistsByUsername)
            {
                Log.Error($"There is no user in the database with the username of {blockedUsername}");
                return new ResponseHelper() { Success = false, Message = "Böyle bir kullanıcı sistemde kayıtlı değildir."};
            }

            var checkIfUserBlocked = CheckIfUserBlocked(blockerUser.Id, blockedUser.Id);
            if (checkIfUserBlocked)
            {
                Log.Error($"Block is already exists.");
                return new ResponseHelper() { Success = false, Message = "Geçersiz işlem ,kullanıcıyı daha önce blokladınız." };
            }

            if (blockerUser.Id == blockedUser.Id)
            {
                Log.Warning($"Unauthorized block attempt.");
                return new ResponseHelper() { Success = false, Message = "Bu işlemi yapmak için yetkiniz bulunmamaktadır." };
            }

            var response = CreateBlock(new Block() { BlockerUserId = blockerUser.Id, BlockedUserId = blockedUser.Id });
            if (!response)
            {
                Log.Error("Create block method failed.");
                return new ResponseHelper { Success = false, Message = "Bloklama sırasında bir hata oluştu. Lütfen tekrar deneyiniz." };
            }

            Log.Information($"{blockerUsername} blocked {blockedUsername} successfully.");
            return new ResponseHelper() { Success = true, Message = $"{blockedUsername} kullanıcı adlı kişiyi başarıyla blokladınız." };
        }

        public bool CheckIfUserBlocked(int possibleBlockerId, int possibleBlockedId)
        {
            var possibleBlockerUser = _userServices.GetUserById(possibleBlockerId);
            var blockerBlocks = _blockRepository.GetBlocksByUser(possibleBlockerUser);

            if (blockerBlocks != null)
            {
                foreach (var block in blockerBlocks)
                {
                    if (block.BlockedUserId == possibleBlockedId)
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
    }
}
