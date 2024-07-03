namespace LibraryManagementSystem.Entities.Models.Library
{
    public class Publisher
    {
        private int publisherId;
        private string? name;

        public int PublisherId { get => publisherId; set => publisherId = value; }
        public string? Name { get => name; set => name = value; }
    }
}
