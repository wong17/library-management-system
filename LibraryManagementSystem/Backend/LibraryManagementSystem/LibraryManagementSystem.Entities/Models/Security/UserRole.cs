namespace LibraryManagementSystem.Entities.Models.Security
{
    public class UserRole
    {
        private int userId;
        private int roleId;

        public int UserId { get => userId; set => userId = value; }
        public int RoleId { get => roleId; set => roleId = value; }
    }
}