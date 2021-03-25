using System.Collections.Generic;

namespace Queis
{
    class Queue
    {
        public string name {get;}
        List<long> persons;
        public Queue(string name)
        {
            this.name = name;
            persons = new List<long>();
        }

        public void Add(long id)
        {
            if (!persons.Contains(id))
                persons.Add(id);
        }

        public void Remove(long id)
        {
            if (persons.Contains(id))
                persons.Remove(id);
        }
        public List<long> GetQueue()
        {
            List<long> result = persons;
            return result;
        }
    }
}