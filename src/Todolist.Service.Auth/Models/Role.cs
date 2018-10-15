using AspNetCore.Identity.Mongo.Model;

namespace Todolist.Service.Auth.Models
{
    public class Role : MongoRole
    {
        public Role(string name) : base(name)
        {

        }
    }
}