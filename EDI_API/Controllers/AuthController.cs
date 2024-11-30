using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        // Mock authentication logic
        if (loginRequest.Username == "admin" && loginRequest.Password == "password")
        {
            var token = Guid.NewGuid().ToString(); // Mock token
            return Ok(new { token });
        }
        return Unauthorized("Invalid username or password");
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
