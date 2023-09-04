using TheJitu_Commerce_Auth.Model;

namespace TheJitu_Commerce_Auth.Services.IService
{
    public interface IJWtTokenGenerator 
    {

        string GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
