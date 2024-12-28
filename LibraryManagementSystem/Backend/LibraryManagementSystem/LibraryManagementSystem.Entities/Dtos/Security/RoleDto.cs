namespace LibraryManagementSystem.Entities.Dtos.Security
{
    public class RoleDto
    {
        private int roleId;
        private string? name;
        private string? description;

        public int RoleId { get => roleId; set => roleId = value; }
        public string? Name { get => name; set => name = value; }
        public string? Description { get => description; set => description = value; }
    }
}