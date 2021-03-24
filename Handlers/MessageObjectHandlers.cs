using System;
using System.Collections.Generic;
using System.Text.Json;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

namespace Queis
{
    class messageObjectHandlers
    {
        public messageObjectHandlers(IVkApi _vkApi, string Object)
        {
            Dictionary<string, object> messageObject = JsonSerializer.Deserialize<Dictionary<string, object>>(Object);
            
            if (!messageObject.ContainsKey("message"))
                return;
            
            Dictionary<string, object> message = JsonSerializer.Deserialize<Dictionary<string, object>>(messageObject["message"].ToString());

            if (!message.ContainsKey("text"))
                return;
            if (!message.ContainsKey("peer_id"))
                return;
            _vkApi.Messages.Send(new MessagesSendParams
            {
                RandomId = new DateTime().Millisecond,
                PeerId = long.Parse(message["peer_id"].ToString()),
                Message = message["text"].ToString()
            });
        }
    }
}