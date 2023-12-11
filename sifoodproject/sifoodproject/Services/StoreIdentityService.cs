using System.Security.Claims;

namespace sifoodproject.Services
{
    public class StoreIdentityService:IStoreIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        public StoreIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetStoreId()
        {
            Claim? store = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            return store?.Value;
        }
    }
}
