using Newtonsoft.Json;

namespace Frontendtest1703.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;
        public int ClinicId { get; set; }
        [JsonIgnore]
        public string? ClinicName { get; set; }
        [JsonIgnore]
        public string? ClinicCode { get; set; }

        public string? Phone { get; set; }


    }

    public class CreateUserDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("email")]
        public string Email { get; set; } = null!;

        [JsonProperty("password")]
        public string Password { get; set; } = null!;

        [JsonProperty("role")]
        public string Role { get; set; } = null!; // doctor | receptionist | patient

        [JsonProperty("phone")]
        public string? Phone { get; set; }
    }
}
