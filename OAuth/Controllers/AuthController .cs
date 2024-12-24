    using System.Text.Json;
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
            
            var userInfo = JsonSerializer.Deserialize<GoogleUserInfo>(payload);
            
            
            return Ok(new 
            {
                message = "Token verified",
                email = userInfo.email,
                name = userInfo.name,
                picture = userInfo.picture
            });
        }
        
    }

    public class GoogleUserInfo
    {
        public string email { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
    }
    
    public class GoogleTokenRequest
    {
        public string Token { get; set; }
    }