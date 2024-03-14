using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositrories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser identityUser,List<string>roles);
    }
}
