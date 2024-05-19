﻿using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos
{
    public class CategoryInsertDto
    {
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Nombre de la categoría debe tener entre 1 y 100 caracteres", MinimumLength = 1)]
        public string? Name { get; set; }
    }
}
