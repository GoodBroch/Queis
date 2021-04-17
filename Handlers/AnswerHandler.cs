using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Queis
{

    public class AnswerHandler
    {
        public Message answer {get;}
        public bool needAnswer {get;}
        public AnswerHandler(Message message)
        {
            needAnswer = false;
            string messageText = message.text;
            Regex usage = new Regex($"[club{Bot.instance().GetClubId()}|*]");

            if (!usage.IsMatch(messageText))
                return;
            messageText = usage.Replace(messageText, "");

            usage = new Regex("начни очередь");
            if (usage.IsMatch(messageText))
                answer = new Message(usage.Replace(messageText, ""), message.peer_id, message.from_id);

            needAnswer = true;
        }

    }
}