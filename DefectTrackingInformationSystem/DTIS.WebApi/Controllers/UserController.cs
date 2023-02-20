using DTIS.Shared.DTO;
using DTIS.WebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DTIS.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<UserDTO>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();

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

    [HttpGet("role")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<UserDTO>> GetAllRoles()
    {
        var roles = await _roleRepository.GetAllRoles();

        return Ok(roles);
    }
}
