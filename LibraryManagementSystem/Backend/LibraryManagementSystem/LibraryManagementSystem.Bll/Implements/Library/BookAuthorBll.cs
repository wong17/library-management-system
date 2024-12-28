using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class BookAuthorBll(IBookAuthorRepository repository, IMapper mapper) : IBookAuthorBll
    {
        public async Task<ApiResponse> Create(BookAuthorInsertDto entity) => await repository.Create(mapper.Map<BookAuthor>(entity));

        public async Task<ApiResponse> CreateMany(IEnumerable<BookAuthorInsertDto> entities) =>
            await repository.CreateMany(mapper.Map<IEnumerable<BookAuthor>>(entities));

        public async Task<ApiResponse> Delete(int bookId, int authorId) => await repository.Delete(bookId, authorId);

        public async Task<ApiResponse> GetAll()
        {
            var response = await repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<BookAuthor> bookAuthors)
                return response;
            // Retornar Dtos
            response.Result = mapper.Map<IEnumerable<BookAuthorDto>>(bookAuthors);

            return response;
        }

        public async Task<ApiResponse> GetById(int bookId, int authorId)
        {
            var response = await repository.GetById(bookId, authorId);
            // Comprobar si hay un elemento
            if (response.Result is not BookAuthor bookAuthor)
                return response;
            // Retornar Dto
            response.Result = mapper.Map<BookAuthorDto>(bookAuthor);
            return response;
        }

        public async Task<ApiResponse> UpdateMany(IEnumerable<BookAuthorInsertDto> entities) =>
            await repository.UpdateMany(mapper.Map<IEnumerable<BookAuthor>>(entities));
    }
}