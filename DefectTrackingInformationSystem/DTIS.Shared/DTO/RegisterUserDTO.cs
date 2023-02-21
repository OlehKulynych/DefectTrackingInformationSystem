﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTIS.Shared.DTO;

public class RegisterUserDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [PasswordPropertyText]
    [StringLength(36, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
    public string ChatId { get; set; } = string.Empty;
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string LastName { get; set; } = string.Empty;
}
