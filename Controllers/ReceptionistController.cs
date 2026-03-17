using System.Net.Http.Headers;
using System.Text;
using Frontendtest1703.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Frontendtest1703.Controllers
{
    [Authorize(Roles = "Receptionist")]
    public class ReceptionistController : Controller
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReceptionistController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
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

        #region Get Queue
        [HttpGet]
        public async Task<IActionResult> DailyQueue(DateTime? date)
        {
            var selectedDate = date ?? DateTime.Now;
            if (!AddAuthorizationHeader())
            {
                return RedirectToAction("Login", "Login");
            }

            List<QueueEntry> queue = new();
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"queue?date={selectedDate:yyyy-MM-dd}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    queue = JsonConvert.DeserializeObject<List<QueueEntry>>(jsonData) ?? new List<QueueEntry>();
                }
                else
                {
                    TempData["ReceptionistErrorMessage"] = "Unable to load queue. Please try again later.";
                }
            }
            catch (Exception ex)
            {
                TempData["ReceptionistErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            ViewBag.SelectedDate = selectedDate;
            return View(queue);
        }
        #endregion

        #region Update Queue Status
        [HttpPost]
        public async Task<IActionResult> UpdateQueueStatus(int id, string status)
        {
            if (!AddAuthorizationHeader())
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                var model = new { status = status };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PatchAsync($"queue/{id}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["ReceptionistSuccessMessage"] = "Queue status updated successfully!";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ReceptionistErrorMessage"] = $"Failed to update queue status. Error: {errorContent}";
                }
            }
            catch (Exception ex)
            {
                TempData["ReceptionistErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("DailyQueue");
        }
        #endregion
    }
}
