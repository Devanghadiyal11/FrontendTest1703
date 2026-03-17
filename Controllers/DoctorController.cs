using System.Net.Http.Headers;
using System.Text;
using Frontendtest1703.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Frontendtest1703.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DoctorController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
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

        #region Add Prescription
        [HttpPost]
        public async Task<IActionResult> AddPrescription(int appointmentId, AddPrescriptionDTO model)
        {
            if (!AddAuthorizationHeader())
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync($"prescriptions/{appointmentId}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["DoctorSuccessMessage"] = "Prescription added successfully!";
                    ModelState.Clear();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["DoctorErrorMessage"] = $"Failed to add prescription. Error: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                TempData["DoctorErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("TodaysQueue");
        }
        #endregion

        #region Add Medical Report
        [HttpPost]
        public async Task<IActionResult> AddReport(int appointmentId, AddReportDTO model)
        {
            if (!AddAuthorizationHeader())
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync($"reports/{appointmentId}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["DoctorSuccessMessage"] = "Report added successfully!";
                    ModelState.Clear();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["DoctorErrorMessage"] = $"Failed to add report. Error: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                TempData["DoctorErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("TodaysQueue");
        }
        #endregion
        #region Today's Queue
        [HttpGet]
        public async Task<IActionResult> TodaysQueue()
        {
            if (!AddAuthorizationHeader())
            {
                return RedirectToAction("Login", "Login");
            }

            List<QueueEntry> queue = new();
            try
            {
                HttpResponseMessage response = await _client.GetAsync("doctor/queue");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    queue = JsonConvert.DeserializeObject<List<QueueEntry>>(jsonData) ?? new List<QueueEntry>();
                }
                else
                {
                    TempData["DoctorErrorMessage"] = "Unable to load today's queue. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                TempData["DoctorErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return View(queue);
        }
        #endregion
    }
}

