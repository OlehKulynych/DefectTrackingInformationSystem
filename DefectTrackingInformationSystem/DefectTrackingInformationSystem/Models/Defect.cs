namespace DefectTrackingInformationSystem.Models
{
    public class Defect
    {
        public int Id { get; set; }
        public uint RoomNumber { get; set; }
        public string Description { get; set; }
        public string? ImageString { get; set; }
        public bool isClosed { get; set; }

    }
}
