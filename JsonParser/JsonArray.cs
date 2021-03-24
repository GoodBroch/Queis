using System.Collections.Generic;

namespace Queis
{
    public class JsonArray : JsonObject
    {
        new List<JsonObject> value;
        public JsonArray(string name, string value) : base(name, value)
        {
            this.type = types.array;
            this.value = new List<JsonObject>();


            while (true)
            {
                int pos = value.IndexOf('"');
                if (pos < 0)
                    break;
                value = value.Substring(pos + 1);
                pos = value.IndexOf('"');
                if (pos < 0)
                    break;
                string nameOfObject = value.Substring(0, pos);
                value = value.Substring(pos + 1);

                long num = 0;
                if (long.TryParse(nameOfObject, out num))
                {
                    this.value.Add(new JsonLongObject(name, num));
                }
                else
                {
                    this.value.Add(new JsonObject(name, nameOfObject));
                }
            }
        }

        public new List<JsonObject> Get() => value;
    }
}
