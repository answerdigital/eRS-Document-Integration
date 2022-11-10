using eRS.API.Models;
using eRS.Data.Entities;
using eRS.Models.Dtos;
using eRS.Models.Models.Users;
using eRS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eRS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : Controller
{
    private readonly IConfiguration configuration;
    private readonly IAccountService accountService;
    private readonly ILogger<AuditLogController> logger;

    public AccountController(
        IConfiguration configuration,
        IAccountService accountService,
        ILogger<AuditLogController> logger)
    {
        this.configuration = configuration;
        this.logger = logger;
        this.accountService = accountService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreate userCreate)
    {
        var newUser = await this.accountService.CreateUser(userCreate);

        if (newUser is null)
        {
            return this.NotFound();
        }

        return this.Ok(newUser);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
    {
        var user = await this.accountService.AuthenticateLogin(userLogin);

        if (user is null)
        {
            return this.NotFound();
        }

        var token = Generate(user);

        return this.Ok(
            new AuthenticatedResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo
            }
        );
    }

    [Authorize]
    [HttpGet("details")]
    public async Task<IActionResult> GetUser()
    {
        var user = await this.GetCurrentUser();

        return user != null ? this.Ok(user) : this.NotFound();
    }

    private JwtSecurityToken Generate(UserDto user)
    {
        var jwtSettings = this.configuration
                .GetSection("JwtBearer")
                .Get<JwtBearerConfiguration>();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserEmail),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.NameIdentifier, user.UserReference.ToString()),
            new (ClaimTypes.GivenName, user.UserForename),
            new (ClaimTypes.Surname, user.UserSurname)
        };

        var token = new JwtSecurityToken(
            jwtSettings.Issuer,
            jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddHours(jwtSettings.LifetimeHours),
            notBefore: DateTime.UtcNow,
            signingCredentials: credentials);

        return token;
    }

    private async Task<UserDto?> GetCurrentUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        if (identity is null)
        {
            return null;
        }

        var userClaims = identity.Claims;
        var userRef = userClaims.FirstOrDefault(uc => uc.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = userClaims.FirstOrDefault(uc => uc.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (email is null)
        {
            return null;
        }

        var user = await this.accountService.GetUser(email);

        return user;
    }
}
