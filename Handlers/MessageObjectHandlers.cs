using System;
using System.Collections.Generic;
using System.Text.Json;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

namespace Queis
{
    class messageObjectHandlers
    {

        private IVkApi _vkApi;
        static private Dictionary<string, Queue> queues;

        private void SendMessage(long peer, string message)
        {
            _vkApi.Messages.Send(new MessagesSendParams
            {
                RandomId = new DateTime().Millisecond,
                PeerId = peer,
                Message = message
            });
        }
        public messageObjectHandlers(IVkApi _vkApi, string Object)
        {
            this._vkApi = _vkApi;
            Dictionary<string, object> messageObject = JsonSerializer.Deserialize<Dictionary<string, object>>(Object);
            if (queues == null)
                queues = new Dictionary<string, Queue>();
            
            if (!messageObject.ContainsKey("message"))
                return;
            
            Dictionary<string, object> message = JsonSerializer.Deserialize<Dictionary<string, object>>(messageObject["message"].ToString());

            if (!message.ContainsKey("text"))
                return;
            if (!message.ContainsKey("peer_id"))
                return;
            string messageText = message["text"].ToString();
            if (!messageText.StartsWith("[club203355401|"))
                return;
            messageText = messageText.Substring(messageText.IndexOf(']') + 1);

            if (messageText.Contains("старт") && message.ContainsKey("from_id") && message["from_id"].ToString() == "194465804")
                messageText = StartQueue(messageText);
            else if (messageText.Contains("стоп") && message.ContainsKey("from_id") && message["from_id"].ToString() == "194465804")
                messageText = StopQueue(messageText);
            else if (messageText.Contains("запись"))
                messageText = AddQueue(messageText, (message.ContainsKey("from_id")) ? message["from_id"].ToString()  : "");
            else if (messageText.Contains("отписаться"))
                messageText = RemoveQueue(messageText, (message.ContainsKey("from_id")) ? message["from_id"].ToString()  : "");
            else if (messageText.Contains("очередь"))
                messageText = StatusQueue(messageText);
            else
                messageText = "Чего хотел?";

            SendMessage(long.Parse(message["peer_id"].ToString()), messageText);
        }

        private string CheckQueue(string message, string command)
        {
            int point = message.IndexOf(command);
            if (point < 0)
                return "";
            message = message.Substring(point + command.Length);

            if (message.Length < 1)
                return "";

            if (message[0] == ' ')
                message = message.Substring(1);
            
            if (message.Length < 1)
                return "";
            return message;
        }

        private string StartQueue(string message)
        {
            message = CheckQueue(message, "старт");
            if (message.Length < 2)
                return "Не могу создать";
            
            queues.Add(message, new Queue(message));

            return $"Создал очередь на {message}";
        }

        private string StopQueue(string message)
        {
            message = CheckQueue(message, "стоп");

            if (!queues.ContainsKey(message))
                return "Такой очереди нет";
            queues.Remove(message);

            return $"Остановил очередь {message}";
        }
        
        private string AddQueue(string message, string id)
        {
            message = CheckQueue(message, "запись");
            if (!queues.ContainsKey(message))
                return "Такой очереди нет";
            
            long userId = 0;
            if (!long.TryParse(id, out userId))
                return "У тебя какой-то странный idшник";

            queues[message].Add(userId);

            return $"Записал на {message}";
        }

        private string RemoveQueue(string message, string id)
        {
            message = CheckQueue(message, "отписаться");
            if (!queues.ContainsKey(message))
                return "Такой очереди нет";
            
            long userId = 0;
            if (!long.TryParse(id, out userId))
                return "У тебя какой-то странный idшник";

            queues[message].Remove(userId);

            return $"Записал на {message}";
        }

        private string StatusQueue(string message)
        {
            message = CheckQueue(message, "очередь");

            if (!queues.ContainsKey(message))
                return "Не нашёл такую очередь";
            
            var ids =   _vkApi.Users.Get(queues[message].GetQueue());

            string result = $"Очередь {queues[message].name}:\n";
            foreach (var id in ids)
                result += $"{id.LastName} {id.FirstName[0]}.\n";

            return result;
        }
    }
}