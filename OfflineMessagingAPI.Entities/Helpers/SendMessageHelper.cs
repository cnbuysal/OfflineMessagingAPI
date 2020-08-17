using System;
using System.Collections.Generic;
using System.Text;

namespace OfflineMessagingAPI.Entities.Helpers
{
    public class SendMessageHelper
    {
        public int SenderId { get; set; }

        public string ReceiverUsername { get; set; }

        public string MessageContent { get; set; }
    }
}
