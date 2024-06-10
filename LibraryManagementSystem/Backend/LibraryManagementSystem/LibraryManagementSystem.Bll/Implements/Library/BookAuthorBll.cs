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
        private readonly IBookAuthorRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse> Create(BookAuthor entity) => await _repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<BookAuthor> entities) => await _repository.CreateMany(entities);

        public async Task<ApiResponse> Delete(int bookId, int authorId) => await _repository.Delete(bookId, authorId);

        public async Task<ApiResponse> GetAll()
        {
            var response = await _repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<BookAuthor> bookAuthors)
                return response;
            // Retornar Dtos
            response.Result = _mapper.Map<IEnumerable<BookAuthorDto>>(bookAuthors);

            return response;
        }

        public async Task<ApiResponse> GetById(int bookId, int authorId)
        {
            var response = await _repository.GetById(bookId, authorId);
            // Comprobar si hay un elemento
            if (response.Result is null || response.Result is not BookAuthor bookAuthor)
                return response;
            // Retornar Dto
            response.Result = _mapper.Map<BookAuthorDto>(bookAuthor);

            return response;
        }

        public async Task<ApiResponse> UpdateMany(IEnumerable<BookAuthor> entities) => await _repository.UpdateMany(entities);
    }
}
