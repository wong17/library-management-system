﻿using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class CategoryInsertDto
    {
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Nombre de la Categoría debe tener entre 1 y 100 caracteres", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\- ]+$", ErrorMessage = "Nombre de la Categoría solo puede tener mayúsculas, minúsculas, guiones y espacios")]
        public string? Name { get; set; }
    }
}