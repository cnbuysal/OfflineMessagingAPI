using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using OfflineMessagingAPI.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace OfflineMessagingAPI.Entities.Entities
{
    public class User : IdentityUser<int>
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public override string UserName { get; set; }

        [JsonIgnore]
        public virtual List<Block> BlockerBlocks { get; set; }

        [JsonIgnore]
        public virtual List<Block> BlockedBlocks { get; set; }

        [JsonIgnore]
        public virtual List<Message> SenderMessages { get; set; }

        [JsonIgnore]
        public virtual List<Message> ReceiverMessages { get; set; }
    }
}
