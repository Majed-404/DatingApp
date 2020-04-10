using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Core.IRepository
{
    public interface IAuthRepository
    {
        Task<User> Register(User _user, string _password);
        Task<User> Login(string _userName, string _password);
        Task<bool> UserExists(string _userName);
    }
}
