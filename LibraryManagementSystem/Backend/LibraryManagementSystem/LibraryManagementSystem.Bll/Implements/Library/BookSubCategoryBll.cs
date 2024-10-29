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
        public async Task<ApiResponse> Create(BookSubCategory entity) => await repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<BookSubCategory> entities) => await repository.CreateMany(entities);

        public async Task<ApiResponse> Delete(int bookId, int subCategoryId) => await repository.Delete(bookId, subCategoryId);

        public async Task<ApiResponse> GetAll()
        {
            var response = await repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<BookSubCategory> bookSubCategories)
                return response;
            // Retornar Dtos
            response.Result = mapper.Map<IEnumerable<BookSubCategoryDto>>(bookSubCategories);

            return response;
        }

        public async Task<ApiResponse> GetById(int bookId, int subCategoryId)
        {
            var response = await repository.GetById(bookId, subCategoryId);
            // Comprobar si hay un elemento
            if (response.Result is not BookSubCategory bookSubCategory)
                return response;
            // Retornar Dto
            response.Result = mapper.Map<BookSubCategoryDto>(bookSubCategory);

            return response;
        }

        public async Task<ApiResponse> UpdateMany(IEnumerable<BookSubCategory> entities) => await repository.UpdateMany(entities);
    }
}
