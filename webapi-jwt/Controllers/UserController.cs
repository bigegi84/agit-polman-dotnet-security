using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
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

    private string GenerateToken(User user)
    {
        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, user.Username));
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(State.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [AllowAnonymous]
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

    [AllowAnonymous]
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
            .FirstOrDefault(it => it.Username == body.Username);
        Console.WriteLine(data);
        if (data?.Password == body.Password)
        {
            return new
            {
                Message = "login berhasil",
                Token = GenerateToken(data)
            };
        }
        else
        {
            return null;
        }

    }

    [HttpGet("/secret")]
    public object? Secret()
    {
        return new { Message = "this is authorize api" };
    }
}
