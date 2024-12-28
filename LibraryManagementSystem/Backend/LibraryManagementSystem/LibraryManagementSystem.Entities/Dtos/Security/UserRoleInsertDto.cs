using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos.Security
{
    public class UserRoleInsertDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del usuario debe ser mayor que 0")]
        public int UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del rol debe ser mayor que 0")]
        public int RoleId { get; set; }
    }
}