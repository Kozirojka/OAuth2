    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    namespace OAuth.Controllers;

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("verify-token")]
        public async Task<IActionResult> VerifyGoogleToken([FromBody] GoogleTokenRequest request)
        {   
            var client = new HttpClient();
            var googleApiUrl = $"https://oauth2.googleapis.com/tokeninfo?id_token={request.Token}";
            var response = await client.GetAsync(googleApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return Unauthorized("Invalid Google token.");
            }

            var payload = await response.Content.ReadAsStringAsync();
            return Ok(new { message = "Token verified", payload });
        }
        
    }

    public class GoogleTokenRequest
    {
        public string Token { get; set; }
    }