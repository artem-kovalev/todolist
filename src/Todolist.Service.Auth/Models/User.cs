namespace Todolist.Service.Auth.Models
{
    using AspNetCore.Identity.Mongo.Model;

    public class User : MongoUser
    {
        public User(string name, string email)
        {
            UserName = name;
            NormalizedUserName = name.ToUpper();

            Email = email;
            NormalizedEmail = email.ToUpper();
        }
    }
}