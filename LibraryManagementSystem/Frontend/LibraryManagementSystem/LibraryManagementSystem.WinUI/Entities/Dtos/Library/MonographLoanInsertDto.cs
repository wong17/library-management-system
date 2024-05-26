using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.WinUi.Entities.Dtos.Library;

public class MonographLoanInsertDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Id del estudiante debe ser mayor que 0")]
    public int StudentId
    {
        get; set;
    }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Id de la monografía debe ser mayor que 0")]
    public int Monographid
    {
        get; set;
    }
}