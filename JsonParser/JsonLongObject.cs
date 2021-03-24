namespace Queis
{
    public class JsonLongObject : JsonObject
    {
        new long value;
        public JsonLongObject(string name, long value) : base(name, value.ToString())
        {
            this.value = value;
            this.type = types.Long;
        }

        public new long Get() => value;
        public void Set(long value)
        {
            this.value = value;
        }
    }
}
