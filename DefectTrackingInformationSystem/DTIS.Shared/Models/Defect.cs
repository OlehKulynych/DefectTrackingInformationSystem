using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTIS.Shared.Models;

public class Defect
{
    public int Id { get; set; }
    [Required]
    [Range(1,uint.MaxValue)]
    public uint RoomNumber { get; set; }
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;
    [NotMapped]
    public IFormFile? File { get; set; }
    public string? ImageString { get; set; }
    public bool isClosed { get; set; } = false;
}
