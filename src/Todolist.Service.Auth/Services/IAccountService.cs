using System.Threading.Tasks;

namespace Todolist.Service.Auth.Services
{
    public interface IAccountService
    {
        Task<string> LoginAsync(string name, string password);

    }
}