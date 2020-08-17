using Newtonsoft.Json;
using OfflineMessagingAPI.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OfflineMessagingAPI.Entities.Entities
{
    public class Block : BaseEntity
    {
        [Required]
        public int BlockerUserId { get; set; }

        [Required]
        public int BlockedUserId { get; set; }

        [JsonIgnore]
        public virtual User BlockerUser { get; set; }

        [JsonIgnore]
        public virtual User BlockedUser { get; set; }
    }
}
