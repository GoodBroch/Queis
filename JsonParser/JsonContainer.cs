using System.Collections.Generic;

namespace Queis
{
    public class JsonContainer : JsonObject
    {
        Dictionary<string, JsonObject> objects;
        public JsonContainer(string name, string value) : base(name, value.ToString())
        {
            objects = new Dictionary<string, JsonObject>();
            type = types.container;
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

                if (value.Length < 2 || value[0] != ':')
                    break;
                
                if (value[1] == '"')
                {
                    value = value.Substring(2);
                    pos = value.IndexOf('"');
                    if (pos < 0)
                        break;
                    objects.Add(nameOfObject, new JsonObject(nameOfObject, value.Substring(0, pos)));
                }
                else if (value[1] >= '0' && value[1] <= '9')
                {
                    value = value.Substring(1);
                    pos = value.IndexOf(',');
                    if (pos < 0)
                    {
                        pos = value.IndexOf('}');
                        if (pos < 0)
                            break;    
                    }
                    int num = 0;
                    if (!int.TryParse(value.Substring(0, pos), out num))
                        break;
                    objects.Add(nameOfObject, new JsonLongObject(nameOfObject, num));
                }
                else if (value[1] == '[')
                {
                    value = value.Substring(2);
                    pos = value.IndexOf(']');
                    if (pos < 0)
                        break;
                    objects.Add(nameOfObject, new JsonArray(nameOfObject, value.Substring(0, pos)));
                }
                else if (value[1] == '{')
                {
                    value = value.Substring(2);
                    pos = value.IndexOf('}');
                    if (pos < 0)
                        break;
                    objects.Add(nameOfObject, new JsonContainer(nameOfObject, value.Substring(0, pos)));
                }
                value = value.Substring(pos + 1);
            }
        }

        public bool Check(string value) => objects.ContainsKey(value);

        public JsonObject this[string name]
        {
            get {
                if (!Check(name))
                    return new JsonObject("Error", "Error");
                return objects[name];
            }

        }

        public override string ToString()
        {
            string result = "";
            foreach(var item in objects)
            {
                result += item.Key + ": " + item.Value + "\n";
            }
            return result;
        }

    }
}
