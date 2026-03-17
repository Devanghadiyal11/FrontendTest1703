using Frontendtest1703.Models;
using Frontendtest1703.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Frontendtest1703.Controllers
{
    public class LoginController : Controller
    {
        #region Configuration Fields

        private readonly AuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _client;

        public LoginController(AuthService authService, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://cmsback.sampaarsh.cloud/");
        }
        #endregion

        #region LoginGet
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        #endregion
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string role)
        {
            var loginResponse = await _authService.AuthenticateUserAsync(userName, password, role);

            if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                ViewBag.Error = "Invalid username or password.";
                return View();
            }

            var userRole = loginResponse.User.Role ?? "";
            // Normalize role to TitleCase (Admin, Doctor, etc.) if it's lowercase from backend
            if (!string.IsNullOrEmpty(userRole) && char.IsLower(userRole[0]))
            {
                userRole = char.ToUpper(userRole[0]) + userRole.Substring(1).ToLower();
            }

            // ✅ Create Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginResponse.User.Name ?? ""),
                new Claim(ClaimTypes.Email, loginResponse.User.Email),
                new Claim(ClaimTypes.Role, userRole),
                new Claim("UserId", loginResponse.User.Id.ToString()),
                new Claim("JWTToken", loginResponse.Token),
                new Claim("ClinicName", loginResponse.User.ClinicName ?? ""),
                new Claim("ClinicCode", loginResponse.User.ClinicCode ?? ""),
                new Claim("ClinicId", loginResponse.User.ClinicId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // ✅ Maintain Session for existing code if needed
            HttpContext.Session.SetString("JWTToken", loginResponse.Token);
            HttpContext.Session.SetString("Role", userRole);
            HttpContext.Session.SetString("UserName", loginResponse.User.Name ?? "");
            HttpContext.Session.SetString("Email", loginResponse.User.Email);
            HttpContext.Session.SetString("UserId", loginResponse.User.Id.ToString());
            HttpContext.Session.SetString("ClinicName", loginResponse.User.ClinicName ?? "");
            HttpContext.Session.SetString("ClinicCode", loginResponse.User.ClinicCode ?? "");
            HttpContext.Session.SetInt32("ClinicId", loginResponse.User.ClinicId);

            return RedirectToAction("Index", "Home");
        }
        #region Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _httpContextAccessor.HttpContext?.Session.Clear();
            return RedirectToAction("Login");
        }
        #endregion
    }
}
