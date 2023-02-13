namespace DefectTrackingInformationSystem.Models
{
    public class User
    {
        public string Id { get; set; }
        public string ChatId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

    }
}
