﻿using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.WinUi.Entities.Dtos.Library;

public class AuthorInsertDto
{
    [Required]
    [StringLength(maximumLength: 100, ErrorMessage = "Nombre del Autor debe tener entre 1 y 100 caracteres", MinimumLength = 1)]
    [RegularExpression(@"^[a-zA-Z. ]+$", ErrorMessage = "Nombre del Autor solo puede tener mayúsculas, minúsculas, puntos y espacios")]
    public string? Name
    {
        get; set;
    }

    [Required]
    public bool IsFormerGraduated
    {
        get; set;
    }
}