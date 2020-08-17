using Newtonsoft.Json;
using OfflineMessagingAPI.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OfflineMessagingAPI.Entities.Entities
{
    public class Message : BaseEntity
    {
        [Required]
        [StringLength(250)]
        public string Content { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [JsonIgnore]
        public virtual User Sender { get; set; }

        [JsonIgnore]
        public virtual User Receiver { get; set; }
    }
}
