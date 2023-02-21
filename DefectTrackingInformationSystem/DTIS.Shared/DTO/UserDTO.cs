using DTIS.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace DTIS.Shared.DTO;

public class UserDTO
{
    public int Id { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public bool IsActivated { get; set; } = false;
    public Role? Role { get; set; }
    public string ChatId { get; set; } = string.Empty;
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string LastName { get; set; } = string.Empty;
}
