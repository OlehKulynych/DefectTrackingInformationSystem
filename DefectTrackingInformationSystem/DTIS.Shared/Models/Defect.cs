using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTIS.Shared.Models;

public class Defect
{
    public int Id { get; set; }
    public uint RoomNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    [NotMapped]
    public IFormFile? File { get; set; }
    public string? ImageString { get; set; }
    public bool isClosed { get; set; } = false;
}
