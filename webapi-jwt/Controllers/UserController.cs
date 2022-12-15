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
        string[] words = user.Role.Split(',');
        foreach (var word in words)
        {
            System.Console.WriteLine($"<{word}>");
            claims.Add(new Claim(ClaimTypes.Role, word));
        }
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
    public object? Register([FromBody] User body)
    {
        using var db = new DBContext();
        // Note: This sample requires the database to be created before running.
        Console.WriteLine($"Database path: {db.DbPath}.");

        // Create
        Console.WriteLine("Inserting a new blog");
        body.Password = BCrypt.Net.BCrypt.HashPassword(body.Password);
        db.Add(body);
        db.SaveChanges();

        // Read
        Console.WriteLine("Querying for a blog");
        var data = db.Users
            .OrderBy(b => b.UserId)
            .LastOrDefault();
        return data;
    }

    [AllowAnonymous]
    [HttpPost("/login")]
    public object? Login([FromBody] User body)
    {
        using var db = new DBContext();
        // Note: This sample requires the database to be created before running.
        Console.WriteLine($"Database path: {db.DbPath}.");

        // Read
        Console.WriteLine("Querying for a blog");
        var data = db.Users
            .FirstOrDefault(it => it.Username == body.Username);
        Console.WriteLine(data);

        if (BCrypt.Net.BCrypt.Verify(body.Password, data?.Password))
        {
            return new
            {
                Message = "login berhasil",
                Token = GenerateToken(data)
            };
        }
        else
        {
            return BadRequest();
        }

    }

    [AllowAnonymous]
    [HttpGet("/public")]
    public object? Public()
    {
        return new { Message = "Api ini bisa di akses tanpa login maupun login." };
    }
    [Authorize]
    [HttpGet("/secret")]
    public object? Secret()
    {
        return new { Message = "Api ini harus login rolenya bebas." };
    }
    [Authorize(Roles = "mahasiswa")]
    [HttpGet("/secret/mahasiswa")]
    public object? SecretMahasiswa()
    {
        return new { Message = "Api ini harus mahasiswa." };
    }
    [Authorize(Roles = "dosen")]
    [HttpGet("/secret/dosen")]
    public object? SecretDosen()
    {
        return new { Message = "Api ini harus dosen." };
    }
    [Authorize(Roles = "mahasiswa,dosen")]
    [HttpGet("/secret/mahasiswaAtauDosen")]
    public object? SecretMahasiswaAtauDosen()
    {
        return new { Message = "Api ini boleh mahasiswa atau dosen." };
    }

    // TODO
    // [HttpGet("/secret/petugas")]
    // [HttpGet("/secret/rektor")]
    // [HttpGet("/secret/rektorDanDosen")]
}
