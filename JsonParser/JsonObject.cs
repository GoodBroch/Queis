namespace Queis
{
    public class JsonObject
    {
        public enum types {none = 0, Long, container, array};
        protected types type;
        protected string name, value;
        public JsonObject(string name, string value)
        {
            this.name = name;
            this.type = types.none;
            this.value = value;
        }

        public string Name() => name;
        public types Type() => type;
        public virtual string Get() => value;

        public override string ToString()
        {
            return value;
        }
    }
}
