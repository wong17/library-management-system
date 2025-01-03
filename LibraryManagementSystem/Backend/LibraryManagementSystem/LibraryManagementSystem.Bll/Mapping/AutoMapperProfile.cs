﻿using AutoMapper;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Dtos.Security;
using LibraryManagementSystem.Entities.Dtos.University;
using LibraryManagementSystem.Entities.Models.Library;
using LibraryManagementSystem.Entities.Models.Security;
using LibraryManagementSystem.Entities.Models.University;

namespace LibraryManagementSystem.Bll.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            /* Models - Dtos Security */
            CreateMap<User, UserInsertDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserLogInDto>().ReverseMap();

            CreateMap<Role, RoleInsertDto>().ReverseMap();
            CreateMap<Role, RoleUpdateDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();

            CreateMap<UserRole, UserRoleInsertDto>().ReverseMap();
            CreateMap<UserRole, UserRoleDto>().ReverseMap();

            /* Models - Dtos Library */
            CreateMap<Publisher, PublisherInsertDto>().ReverseMap();
            CreateMap<Publisher, PublisherUpdateDto>().ReverseMap();
            CreateMap<Publisher, PublisherDto>().ReverseMap();

            CreateMap<Category, CategoryInsertDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<SubCategory, SubCategoryInsertDto>().ReverseMap();
            CreateMap<SubCategory, SubCategoryUpdateDto>().ReverseMap();
            CreateMap<SubCategory, SubCategoryDto>().ReverseMap();
            CreateMap<SubCategory, SubCategoryFilterDto>().ReverseMap();
            CreateMap<SubCategoryDto, SubCategoryFilterDto>().ReverseMap();

            CreateMap<Author, AuthorInsertDto>().ReverseMap();
            CreateMap<Author, AuthorUpdateDto>().ReverseMap();
            CreateMap<Author, AuthorDto>().ReverseMap();

            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image ?? Array.Empty<byte>())))
                .ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.FromBase64String(src.Image ?? string.Empty)));

            CreateMap<BookInsertDto, Book>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.FromBase64String(src.Image ?? string.Empty)))
                .ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image ?? Array.Empty<byte>())));

            CreateMap<BookUpdateDto, Book>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.FromBase64String(src.Image ?? string.Empty)))
                .ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image ?? Array.Empty<byte>())));

            CreateMap<BookSubCategory, BookSubCategoryInsertDto>().ReverseMap();
            CreateMap<BookSubCategory, BookSubCategoryDto>().ReverseMap();

            CreateMap<BookAuthor, BookAuthorInsertDto>().ReverseMap();
            CreateMap<BookAuthor, BookAuthorDto>().ReverseMap();

            CreateMap<BookLoan, BookLoanInsertDto>().ReverseMap();
            CreateMap<BookLoan, BookLoanDto>().ReverseMap();

            CreateMap<Monograph, MonographDto>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image ?? Array.Empty<byte>())))
                .ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.FromBase64String(src.Image ?? string.Empty)));

            CreateMap<MonographInsertDto, Monograph>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.FromBase64String(src.Image ?? string.Empty)))
                .ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image ?? Array.Empty<byte>())));

            CreateMap<MonographUpdateDto, Monograph>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.FromBase64String(src.Image ?? string.Empty)))
                .ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image ?? Array.Empty<byte>())));

            CreateMap<MonographLoan, MonographLoanInsertDto>().ReverseMap();
            CreateMap<MonographLoan, MonographLoanDto>().ReverseMap();

            CreateMap<MonographAuthor, MonographAuthorInsertDto>().ReverseMap();
            CreateMap<MonographAuthor, MonographAuthorDto>().ReverseMap();

            /* Models - Dtos University */
            CreateMap<Career, CareerDto>().ReverseMap();
            CreateMap<Student, StudentDto>().ReverseMap();
        }
    }
}