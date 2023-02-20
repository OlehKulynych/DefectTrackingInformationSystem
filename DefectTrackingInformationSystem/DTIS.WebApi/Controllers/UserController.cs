using DTIS.Shared.DTO;
using DTIS.Shared.Models;
using DTIS.WebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DTIS.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UserController(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsersAsync();

        var result = users.Select(x => new UserDTO
        {
            Id = x.Id,
            Email = x.Email,
            IsActivated = x.IsActivated,
            Role = x.Role,
            ChatId = x.ChatId,
            FirstName = x.FirstName,
            LastName = x.LastName
        });

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet("role")]
    public async Task<ActionResult<List<Role>>> GetAllRoles()
    {
        var roles = await _roleRepository.GetAllRolesAsync();

        return Ok(roles);
    }

    [HttpGet("role/{id:int}")]
    public async Task<ActionResult<Role>> GetRoleById(int id)
    {
        var role = await _roleRepository.GetRoleByIdAsync(id);

        if (role == null)
        {
            return NotFound();
        }

        return Ok(role);
    }
}
