using VkNet;
using VkNet.Abstractions;
using VkNet.Model;
using System.Collections.Generic;
using VkNet.Model.RequestParams;

namespace Queis
{
    public class Bot
    {
        private static Bot bot_;
        private VkApi api;
        private Bot(string access)
        {
                api = new VkApi();
                api.Authorize(new ApiAuthParams { AccessToken = access });
        }

        public void Send(long peer, string message)
        {
            api.Messages.Send(new MessagesSendParams
            {
                RandomId = new System.DateTime().Millisecond,
                PeerId = peer,
                Message = message
            });
        }

        public List<User> GetUser(List<long> ids)
        {
            var users = api.Users.Get(ids);
            List<User> answer = new List<User>();
            foreach (var user in users)
                answer.Add(new User(user.Id, user.LastName, user.FirstName));
            return answer;
        }

        public static Bot instance(string AccessToken)
        {
            if (bot_ == null)
                bot_ = new Bot(AccessToken);
            return bot_;
        }

        public static Bot instance()
        {
            return bot_;
        }
    }
}