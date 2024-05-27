using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class BookSubCategoryBll(IBookSubCategoryRepository repository, IMapper mapper) : IBookSubCategoryBll
    {
        private readonly IBookSubCategoryRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse> Create(BookSubCategory entity) => await _repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<BookSubCategory> entities) => await _repository.CreateMany(entities);

        public async Task<ApiResponse> Delete(int bookId, int subCategoryId) => await _repository.Delete(bookId, subCategoryId);

        public async Task<ApiResponse> GetAll()
        {
            var response = await _repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<BookSubCategory> bookSubCategories)
                return response;
            // Retornar Dtos
            response.Result = _mapper.Map<IEnumerable<BookSubCategoryDto>>(bookSubCategories);

            return response;
        }

        public async Task<ApiResponse> GetById(int bookId, int subCategoryId)
        {
            var response = await _repository.GetById(bookId, subCategoryId);
            // Comprobar si hay un elemento
            if (response.Result is null || response.Result is not BookSubCategory bookSubCategory)
                return response;
            // Retornar Dto
            response.Result = _mapper.Map<BookSubCategoryDto>(bookSubCategory);

            return response;
        }
    }
}
