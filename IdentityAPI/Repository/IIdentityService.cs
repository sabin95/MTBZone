using IdentityAPI.Commands;
using IdentityAPI.Results;

namespace IdentityAPI.Repository
{
    public interface IIdentityService
    {
        Task<string> GenerateToken(UserGenerateTokenCommand userGenerateTokenCommand);
        Task<User> RegisterUser(UserRegisterCommand userRegisterCommand);

    }
}
