using sifoodproject.Models;
using System.Security.Claims;

namespace sifoodproject.Services
{
    public class AdminIdentityService:IAdminService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AdminIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetAdminId()
        {
            string Admin = _httpContextAccessor.HttpContext?.User.Claims.Select(x => x.Value).First();
            
                return Admin;
        }
    }
}
