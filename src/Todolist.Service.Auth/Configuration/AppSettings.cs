using System.Text;

namespace Todolist.Service.Auth.Configuration
{
    public class AppSettings
    {
        protected string JwtSecretKey { get; set; }

        public byte[] JwtSecret
        {
            get
            {
                return Encoding.UTF8.GetBytes(JwtSecretKey);
            }
        }

    }
}