using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class MonographAuthorBll(IMonographAuthorRepository repository, IMapper mapper) : IMonographAuthorBll
    {
        public async Task<ApiResponse> Create(MonographAuthor entity) => await repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<MonographAuthor> entities) => await repository.CreateMany(entities);

        public async Task<ApiResponse> Delete(int monographId, int authorId) => await repository.Delete(monographId, authorId);

        public async Task<ApiResponse> GetAll()
        {
            var response = await repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<MonographAuthor> monographAuthors)
                return response;
            // Retornar Dtos
            response.Result = mapper.Map<IEnumerable<MonographAuthorDto>>(monographAuthors);

            return response;
        }

        public async Task<ApiResponse> GetById(int monographId, int authorId)
        {
            var response = await repository.GetById(monographId, authorId);
            // Comprobar si hay un elemento
            if (response.Result is not MonographAuthor monographAuthor)
                return response;
            // Retornar Dto
            response.Result = mapper.Map<MonographAuthorDto>(monographAuthor);

            return response;
        }

        public async Task<ApiResponse> UpdateMany(IEnumerable<MonographAuthor> entities) => await repository.UpdateMany(entities);
    }
}
