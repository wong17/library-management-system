using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos
{
    public class BookSubCategoryInsertDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del libro debe ser mayor que 0")]
        public int BookId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id de la Sub categoría debe ser mayor que 0")]
        public int SubCategoryId { get; set; }
    }
}
