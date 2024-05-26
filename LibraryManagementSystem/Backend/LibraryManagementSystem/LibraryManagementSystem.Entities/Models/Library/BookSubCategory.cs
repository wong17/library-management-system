using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Models.Library
{
    public class BookSubCategory
    {
        [JsonPropertyName("BookId")]
        public int BookId { get; set; }

        [JsonPropertyName("SubCategoryId")]
        public int SubCategoryId { get; set; }
        /* 
         * Para evitar que asigne el valor 0001/1/1 12:00:00 y de conflicto en la base de datos 
         * diciendo que no puede convertir de DATETIME a DATETIME2 se asigna DateTime.Now aunque
         * luego no lo ocupemos al crear el registro porque la columna ya lo hace por defecto.
         */
        [JsonPropertyName("CreatedOn")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [JsonPropertyName("ModifiedOn")]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}
