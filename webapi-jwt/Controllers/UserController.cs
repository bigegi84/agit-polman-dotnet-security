using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace webapi_jwt.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public UserController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    public string GenerateToken(int userId)
    {
        var mySecret = "asdv234234^&%&^%&^hjsdfb2%%%";
        var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

        var myIssuer = "http://mysite.com";
        var myAudience = "http://myaudience.com";

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = myIssuer,
            Audience = myAudience,
            SigningCredentials = new SigningCredentials(
                mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [HttpPost("/register")]
    public IEnumerable<object?> Register([FromBody] User body)
    {
        using var db = new DBContext();
        // Note: This sample requires the database to be created before running.
        Console.WriteLine($"Database path: {db.DbPath}.");

        // Create
        Console.WriteLine("Inserting a new blog");
        db.Add(body);
        db.SaveChanges();

        // Read
        Console.WriteLine("Querying for a blog");
        var data = db.Users
            .OrderBy(b => b.UserId)
            .ToList();
        return data;
    }
    [HttpPost("/login")]
    public object? Login([FromBody] User body)
    {
        using var db = new DBContext();
        // Note: This sample requires the database to be created before running.
        Console.WriteLine($"Database path: {db.DbPath}.");

        // Create
        Console.WriteLine("Inserting a new blog");
        db.Add(body);
        db.SaveChanges();

        // Read
        Console.WriteLine("Querying for a blog");
        var data = db.Users
            .Find(body.Username);
        if (data?.Password == body.Password)
        {
            return new
            {
                Message = "login berhasil",
                Token = "coba"
            };
        }
        else
        {
            return null;
        }

    }
}
