using VkNet;
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

        public void Send(Message message)
        {
            MessagesSendParams msp = new MessagesSendParams();
            msp.RandomId = System.DateTime.Now.Millisecond;
            msp.PeerId = message.peer_id;
            msp.Message = message.text;
            api.Messages.Send(msp);
        }

        public long GetClubId() => (long)bot_.api.UserId;

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