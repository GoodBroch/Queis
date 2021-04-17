
namespace Queis
{
    public class User
    {
        public long id {get;}
        public string Surname {get;}
        public string Name {get;}
        public User(long id, string Surname, string Name)
        {
            this.id = id;
            this.Surname = Surname;
            this.Name = Name;
        }
    }
}