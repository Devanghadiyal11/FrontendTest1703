using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Frontendtest1703.Models
{
    public class Login
    {
        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;

        [JsonProperty("user")]
        public UserDto User { get; set; } = new UserDto();
    }

    public class UserDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; } = null!;

        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("role")]
        public string Role { get; set; } = null!;

        [JsonProperty("clinicId")]
        public int ClinicId { get; set; }

        [JsonProperty("clinicName")]
        public string ClinicName { get; set; } = null!;

        [JsonProperty("clinicCode")]
        public string ClinicCode { get; set; } = null!;
    }
}
