using System.Net.Http.Headers;
using System.Text;
using Frontendtest1703.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Frontendtest1703.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://cmsback.sampaarsh.cloud/");
            _httpContextAccessor = httpContextAccessor;
        }

        private bool AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            _client.DefaultRequestHeaders.Authorization = null; // Clear existing
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return true;
        }

        #region GetAllUsers
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            if (!AddAuthorizationHeader())
            {
                return RedirectToAction("Login", "Login");
            }

            List<User> users = new();
            try
            {
                HttpResponseMessage response = await _client.GetAsync("admin/users");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<List<User>>(jsonData) ?? new List<User>();
                }
                else
                {
                    TempData["AdminErrorMessage"] = "Unable to load users. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                TempData["AdminErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return View(users);
        }
        #endregion

        #region Add User Form
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View(new CreateUserDTO());
        }
        #endregion

        #region Save User
        [HttpPost]
        public async Task<IActionResult> SaveUser(CreateUserDTO model)
        {
            if (!ModelState.IsValid)
            {
                TempData["AdminErrorMessage"] = "Please correct the errors in the form.";
                return View("CreateUser", model);
            }

            if (!AddAuthorizationHeader())
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync("admin/users", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AdminSuccessMessage"] = "User created successfully!";
                    return RedirectToAction("Users");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["AdminErrorMessage"] = $"Failed to create user. Error: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                TempData["AdminErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return View("CreateUser", model);
        }
        #endregion

        #region Clinic Information
        [HttpGet]
        public async Task<IActionResult> ClinicInfo()
        {
            if (!AddAuthorizationHeader())
            {
                return RedirectToAction("Login", "Login");
            }

            Clinic? clinic = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync("admin/clinic");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    clinic = JsonConvert.DeserializeObject<Clinic>(jsonData);
                }
                else
                {
                    TempData["AdminErrorMessage"] = "Failed to load clinic information.";
                }
            }
            catch (Exception ex)
            {
                TempData["AdminErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return View(clinic);
        }
        #endregion
    }
}
