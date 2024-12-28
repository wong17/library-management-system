namespace LibraryManagementSystem.Entities.Models.Library
{
    public class MonographAuthor
    {
        private int monographId;
        private int authorId;
        private DateTime createdOn = DateTime.Now;
        private DateTime modifiedOn = DateTime.Now;

        public int MonographId { get => monographId; set => monographId = value; }
        public int AuthorId { get => authorId; set => authorId = value; }
        /*
         * Para evitar que asigne el valor 0001/1/1 12:00:00 y de conflicto en la base de datos
         * diciendo que no puede convertir de DATETIME a DATETIME2 se asigna DateTime.Now aunque
         * luego no lo ocupemos al crear el registro porque la columna ya lo hace por defecto.
         */
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public DateTime ModifiedOn { get => modifiedOn; set => modifiedOn = value; }
    }
}