namespace LibraryManagementSystem.Entities.Models.University
{
    public class Career
    {
        private byte careerId;
        private string? name;

        public byte CareerId { get => careerId; set => careerId = value; }
        public string? Name { get => name; set => name = value; }
    }
}