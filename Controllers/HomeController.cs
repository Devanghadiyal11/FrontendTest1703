using System.Diagnostics;
using System.Net.Http.Headers;
using Frontendtest1703.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Frontendtest1703.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
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

        #region Dashboard
        public async Task<IActionResult> Index()
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(role))
            {
                // This should not happen for an authenticated user, but as a safeguard:
                return RedirectToAction("Logout", "Login");
            }

            if (!AddAuthorizationHeader())
            {
                // If the session token is missing, the API calls will fail gracefully.
                // The view will then show an empty state.
                TempData["ErrorMessage"] = "Your session may have expired. Please log in again.";
            }

            var viewModel = new DashboardViewModel { Role = role };
            try
            {
                switch (role.ToLower())
                {
                    case "admin":
                        var usersResponse = await _client.GetAsync("admin/users");
                        if (usersResponse.IsSuccessStatusCode)
                        {
                            var usersJson = await usersResponse.Content.ReadAsStringAsync();
                            viewModel.Users = JsonConvert.DeserializeObject<List<User>>(usersJson);
                        }
                        var clinicResponse = await _client.GetAsync("admin/clinic");
                        if (clinicResponse.IsSuccessStatusCode)
                        {
                            var clinicJson = await clinicResponse.Content.ReadAsStringAsync();
                            viewModel.ClinicInfo = JsonConvert.DeserializeObject<Clinic>(clinicJson);
                        }
                        break;
                    case "doctor":
                        var doctorQueueResponse = await _client.GetAsync("doctor/queue");
                        if (doctorQueueResponse.IsSuccessStatusCode)
                        {
                            var queueJson = await doctorQueueResponse.Content.ReadAsStringAsync();
                            viewModel.Queue = JsonConvert.DeserializeObject<List<QueueEntry>>(queueJson);
                        }
                        break;
                    case "receptionist":
                        var receptionistQueueResponse = await _client.GetAsync($"queue?date={DateTime.Now:yyyy-MM-dd}");
                        if (receptionistQueueResponse.IsSuccessStatusCode)
                        {
                            var queueJson = await receptionistQueueResponse.Content.ReadAsStringAsync();
                            viewModel.Queue = JsonConvert.DeserializeObject<List<QueueEntry>>(queueJson);
                        }
                        break;
                    case "patient":
                        var patientAppointmentsResponse = await _client.GetAsync("appointments/my");
                        if (patientAppointmentsResponse.IsSuccessStatusCode)
                        {
                            var appointmentsJson = await patientAppointmentsResponse.Content.ReadAsStringAsync();
                            viewModel.Appointments = JsonConvert.DeserializeObject<List<Appointment>>(appointmentsJson);
                        }
                        var prescriptionsResponse = await _client.GetAsync("prescriptions/my");
                        if (prescriptionsResponse.IsSuccessStatusCode)
                        {
                            var prescriptionsJson = await prescriptionsResponse.Content.ReadAsStringAsync();
                            viewModel.Prescriptions = JsonConvert.DeserializeObject<List<Prescription>>(prescriptionsJson);
                        }
                        var reportsResponse = await _client.GetAsync("reports/my");
                        if (reportsResponse.IsSuccessStatusCode)
                        {
                            var reportsJson = await reportsResponse.Content.ReadAsStringAsync();
                            viewModel.Reports = JsonConvert.DeserializeObject<List<Report>>(reportsJson);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while loading dashboard: {ex.Message}";
            }

            return View(viewModel);
        }
        #endregion

        public IActionResult Privacy()
        {
            return View();
        }
    }
}


