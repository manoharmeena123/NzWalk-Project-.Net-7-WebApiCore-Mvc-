using Microsoft.AspNetCore.Identity;

namespace NzWalkWebApi.Repositories
{
    public interface ITokenRepository
    {
      string  CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
