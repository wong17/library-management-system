namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class UpdateReturnedBookLoanDto
    {
        public int BookLoanId { get; set; }
        public int ReturnedUserId { get; set; }
    }
}
