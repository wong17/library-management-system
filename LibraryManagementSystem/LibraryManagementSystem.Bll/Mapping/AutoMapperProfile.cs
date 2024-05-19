using AutoMapper;
using LibraryManagementSystem.Entities.Dtos;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Bll.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Publisher, PublisherInsertDto>().ReverseMap();
            CreateMap<Publisher, PublisherUpdateDto>().ReverseMap();

            CreateMap<Category, CategoryInsertDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();

            CreateMap<SubCategory, SubCategoryInsertDto>().ReverseMap();
            CreateMap<SubCategory, SubCategoryUpdateDto>().ReverseMap();

            CreateMap<Author, AuthorInsertDto>().ReverseMap();
            CreateMap<Author, AuthorUpdateDto>().ReverseMap();

            CreateMap<Book, BookInsertDto>().ReverseMap();
            CreateMap<Book, BookUpdateDto>().ReverseMap();

            CreateMap<BookSubCategory, BookSubCategoryInsertDto>().ReverseMap();

            CreateMap<BookAuthor, BookAuthorInsertDto>().ReverseMap();

            CreateMap<BookLoan, BookLoanInsertDto>().ReverseMap();

            CreateMap<Monograph, MonographInsertDto>().ReverseMap();
            CreateMap<Monograph, MonographUpdateDto>().ReverseMap();

            CreateMap<MonographLoan, MonographLoanInsertDto>().ReverseMap();

            CreateMap<MonographAuthor, MonographAuthorInsertDto>().ReverseMap();
        }
    }
}
