using System.Security.Claims;

namespace sifoodproject.Services
{
    public class UserIdentityService : IUserIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            Claim? user = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (user != null)
            {
                return user.Value;
            }
            throw new InvalidOperationException("找不到 User ID");
        }

    }
}
