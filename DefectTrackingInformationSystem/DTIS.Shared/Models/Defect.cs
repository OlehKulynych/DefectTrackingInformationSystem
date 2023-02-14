namespace DTIS.Shared.Models;

internal class Defect
{
    public int Id { get; set; }
    public uint RoomNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? ImageString { get; set; }
    public bool isClosed { get; set; }
}
