﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTIS.Shared.DTO;

public class LoginUserDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;
}
