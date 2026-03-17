namespace Frontendtest1703.Services
{
    public class UserSession
    {
         private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? JwtToken => _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");

        public string? Role => _httpContextAccessor.HttpContext?.Session.GetString("Role");

        public string? UserName => _httpContextAccessor.HttpContext?.Session.GetString("UserName");

        public bool IsAuthenticated => !string.IsNullOrEmpty(JwtToken);
    }
}
