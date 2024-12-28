namespace LibraryManagementSystem.Entities.Dtos.University
{
    public class CareerDto
    {
        private byte careerId;
        private string? name;

        public byte CareerId { get => careerId; set => careerId = value; }
        public string? Name { get => name; set => name = value; }
    }
}