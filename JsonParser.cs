using System;

namespace Queis
{
    [Serializable]
    public class JsonParser
    {
        public string type { get; set; }
        public System.Text.Json.JsonElement Object { get; set;}
        public long group_id { get; set; }

        public string event_id { get; set; }
        public string secret { get; set; }

        public override string ToString()
        {
            return $"'type':{type}, 'group_id':{group_id}, 'object':{Object}";
        }
    }
}
