using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Dtos.Security;
using LibraryManagementSystem.Entities.Dtos.University;
using LibraryManagementSystem.Entities.Models.Library;
using LibraryManagementSystem.Entities.Models.Security;
using LibraryManagementSystem.Entities.Models.University;
using Riok.Mapperly.Abstractions;

namespace LibraryManagementSystem.Bll.Mapping
{
    [Mapper]
    public partial class MapperleyMapper
    {
        /* Models - Dtos Security */
        [MapperIgnoreTarget(nameof(User.AccessToken))]
        [MapperIgnoreTarget(nameof(User.AccessFailedCount))]
        [MapperIgnoreTarget(nameof(User.RefreshToken))]
        [MapperIgnoreTarget(nameof(User.LockoutEnabled))]
        [MapperIgnoreTarget(nameof(User.RefreshTokenExpiryTime))]
        [MapperIgnoreTarget(nameof(User.UserId))]
        public partial User UserInsertDtoToUser(UserInsertDto source);

        [MapperIgnoreTarget(nameof(User.AccessToken))]
        [MapperIgnoreTarget(nameof(User.AccessFailedCount))]
        [MapperIgnoreTarget(nameof(User.RefreshToken))]
        [MapperIgnoreTarget(nameof(User.LockoutEnabled))]
        [MapperIgnoreTarget(nameof(User.RefreshTokenExpiryTime))]
        [MapperIgnoreTarget(nameof(User.UserName))]
        public partial User UserUpdateDtoToUser(UserUpdateDto source);

        [MapperIgnoreTarget(nameof(Role.RoleId))]
        public partial Role RoleInsertDtoToRole(RoleInsertDto source);
        public partial Role RoleUpdateDtoToRole(RoleUpdateDto source);

        public partial UserRole UserRoleInsertDtoToUserRole(UserRoleInsertDto source);

        /* Models - Dtos Library */
        [MapperIgnoreTarget(nameof(Publisher.PublisherId))]
        public partial Publisher PublisherInsertDtoToPublisher(PublisherInsertDto source);
        public partial Publisher PublisherUpdateDtoToPublisher(PublisherUpdateDto source);
        public partial PublisherDto PublisherToPublisherDto(Publisher source);
        public partial IEnumerable<PublisherDto> IEnumerablePublisherToPublisherDto(IEnumerable<Publisher> source);

        [MapperIgnoreTarget(nameof(Category.CategoryId))]
        public partial Category CategoryInsertDtoToCategory(CategoryInsertDto source);
        public partial Category CategoryUpdateDtoToCategory(CategoryUpdateDto source);
        public partial CategoryDto CategoryToCategoryDto(Category source);
        public partial IEnumerable<CategoryDto> IEnumerableCategoryToCategoryDto(IEnumerable<Category> source);

        [MapperIgnoreTarget(nameof(SubCategory.SubCategoryId))]
        public partial SubCategory SubCategoryInsertDtoToSubCategory(SubCategoryInsertDto source);
        public partial SubCategory SubCategoryUpdateDtoToSubCategory(SubCategoryUpdateDto source);
        public partial SubCategoryDto SubCategoryToSubCategoryDto(SubCategory source);
        public partial IEnumerable<SubCategoryDto> IEnumerableSubCategoryToSubCategoryDto(IEnumerable<SubCategory> source);

        [MapperIgnoreTarget(nameof(Author.AuthorId))]
        public partial Author AuthorInsertDtoToAuthor(AuthorInsertDto source);
        public partial Author AuthorUpdateDtoToAuthor(AuthorUpdateDto source);
        public partial AuthorDto AuthorToAuthorDto(Author source);
        public partial IEnumerable<AuthorDto> IEnumerableAuthorToAuthorDto(IEnumerable<Author> source);

        [MapperIgnoreTarget(nameof(Book.BookId))]
        [MapperIgnoreTarget(nameof(Book.IsActive))]
        [MapperIgnoreTarget(nameof(Book.BorrowedCopies))]
        public partial Book BookInsertDtoToBook(BookInsertDto source);

        [MapperIgnoreTarget(nameof(Book.IsActive))]
        [MapperIgnoreTarget(nameof(Book.BorrowedCopies))]
        public partial Book BookUpdateDtoToBook(BookUpdateDto source);

        [MapperIgnoreTarget(nameof(BookDto.Publisher))]
        [MapperIgnoreTarget(nameof(BookDto.Category))]
        [MapperIgnoreTarget(nameof(BookDto.Authors))]
        [MapperIgnoreTarget(nameof(BookDto.SubCategories))]
        [MapperIgnoreTarget(nameof(BookDto.Category))]
        [MapperIgnoreSource(nameof(Book.PublisherId))]
        [MapperIgnoreSource(nameof(Book.CategoryId))]
        public partial BookDto BookToBookDto(Book source);
        public partial IEnumerable<BookDto> IEnumerableBookToBookDto(IEnumerable<Book> source);

        [MapperIgnoreTarget(nameof(BookSubCategory.CreatedOn))]
        [MapperIgnoreTarget(nameof(BookSubCategory.ModifiedOn))]
        public partial BookSubCategory BookSubCategoryInsertDtoToBookSubCategory(BookSubCategoryInsertDto source);
        public partial BookSubCategoryDto BookSubCategoryToBookSubCategoryDto(BookSubCategory source);
        public partial IEnumerable<BookSubCategoryDto> IEnumerableBookSubCategoryToBookSubCategoryDto(IEnumerable<BookSubCategory> source);

        [MapperIgnoreTarget(nameof(BookAuthor.CreatedOn))]
        [MapperIgnoreTarget(nameof(BookAuthor.ModifiedOn))]
        public partial BookAuthor BookAuthorInsertDtoToBookAuthor(BookAuthorInsertDto source);
        public partial BookAuthorDto BookAuthorToBookAuthorDto(BookAuthor source);
        public partial IEnumerable<BookAuthorDto> IEnumerableBookAuthorToBookAuthorDto(IEnumerable<BookAuthor> source);

        [MapperIgnoreTarget(nameof(BookLoan.BookLoanId))]
        [MapperIgnoreTarget(nameof(BookLoan.State))]
        [MapperIgnoreTarget(nameof(BookLoan.LoanDate))]
        [MapperIgnoreTarget(nameof(BookLoan.DueDate))]
        [MapperIgnoreTarget(nameof(BookLoan.ReturnDate))]
        public partial BookLoan BookLoanInsertDtoToBookLoan(BookLoanInsertDto source);

        [MapperIgnoreTarget(nameof(BookLoanDto.Book))]
        [MapperIgnoreTarget(nameof(BookLoanDto.Student))]
        public partial IEnumerable<BookLoanDto> IEnumerableBookLoanToBookLoanDto(IEnumerable<BookLoan> source);

        [MapperIgnoreTarget(nameof(Monograph.MonographId))]
        [MapperIgnoreTarget(nameof(Monograph.IsActive))]
        public partial Monograph MonographInsertDtoToMonograph(MonographInsertDto source);
        public partial Monograph MonographUpdateDtoToMonograph(MonographUpdateDto source);
        public partial IEnumerable<MonographLoanDto> IEnumerableMonographLoanToMonographLoanDto(IEnumerable<MonographLoan> source);

        public partial CareerDto CareerToCareerGetDto(Career career);
        public partial IEnumerable<CareerDto> IEnumerableCareerToCareerDto(IEnumerable<Career> careers);
    }
}
