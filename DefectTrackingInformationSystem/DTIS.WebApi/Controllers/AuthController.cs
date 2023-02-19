using DTIS.Shared.DTO;
using DTIS.Shared.Models;
using DTIS.WebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DTIS.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public AuthController(IConfiguration configuration, 
        IUserRepository userRepository, 
        IRoleRepository roleRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> RegisterAsync(RegisterUserDTO registerUserDTO)
    {
        var oldUser = await _userRepository.GetUserByEmailAsync(registerUserDTO.Email);
        var defaultRole = await _roleRepository.GetRoleByNameAsync("None");

        if (oldUser != null)
        {
            return BadRequest("This mail is already taken.");
        }

        var user = new User();
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password);

        user.Email = registerUserDTO.Email;
        user.FirstName = registerUserDTO.FirstName;
        user.LastName = registerUserDTO.LastName;
        user.Role = defaultRole;
        user.ChatId = registerUserDTO.ChatId;
        user.PasswordHash = passwordHash;

        var result = await _userRepository.CreateUserAsync(user);

        if (!result)
        {
            return BadRequest("Wrong input data.");
        }

        return Ok();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<string?>> Login(LoginUserDTO loginUserDTO)
    {
        var user = await _userRepository.GetUserByEmailAsync(loginUserDTO.Email);

        if (user == null)
        {
            return BadRequest("User not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.PasswordHash))
        {
            return BadRequest("Wrong password.");
        }

        var token = CreateToken(user);

        return Ok(token);
    }

    [NonAction]
    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim (ClaimTypes.Email, user.Email),
            new Claim (ClaimTypes.Role, user.Role!.Name),
            new Claim (ClaimTypes.Name, user.FirstName),
            new Claim (ClaimTypes.Surname, user.LastName),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
