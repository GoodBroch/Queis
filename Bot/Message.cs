using System.Collections.Generic;

namespace Queis
{

    public class Message
    {
        public string text {get;}
        public long peer_id {get;}
        public long from_id {get;}
       public Message(string text, long peer_id = 0, long from_id = 0)
       {
            this.text = text;
            this.peer_id = peer_id;
            this.from_id = from_id;
       }

       public Message(Dictionary<string, object> message)
       {
            if (message.ContainsKey("text"))
                text = message["text"].ToString();
            if (message.ContainsKey("peer_id"))
                peer_id = long.Parse(message["peer_id"].ToString());
            if (message.ContainsKey("from_id"))
                from_id = long.Parse(message["from_id"].ToString());    
            
       }
    }
}