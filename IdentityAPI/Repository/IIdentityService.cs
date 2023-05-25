using IdentityAPI.Commands;
using IdentityAPI.Results;

namespace IdentityAPI.Repository
{
    public interface IIdentityService
    {
        Task<string> GenerateToken(User user);
        Task<User> RegisterUser(UserRegisterCommand userRegisterCommand, string password);

    }
}
