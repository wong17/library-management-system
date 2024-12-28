using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class PublisherBll(IPublisherRepository repository, IMapper mapper) : IPublisherBll
    {
        public async Task<ApiResponse> Create(PublisherInsertDto entity) => await repository.Create(mapper.Map<Publisher>(entity));

        public async Task<ApiResponse> CreateMany(IEnumerable<PublisherInsertDto> entities)
        {
            var response = await repository.CreateMany(mapper.Map<IEnumerable<Publisher>>(entities));
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<Publisher> publishers)
                return response;
            // Retornar Dtos
            response.Result = mapper.Map<IEnumerable<PublisherDto>>(publishers);

            return response;
        }

        public async Task<ApiResponse> Delete(int id) => await repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            var response = await repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<Publisher> publishers)
                return response;
            // Retornar Dtos
            response.Result = mapper.Map<IEnumerable<PublisherDto>>(publishers);

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is not Publisher publisher)
                return response;
            // Retornar Dto
            response.Result = mapper.Map<PublisherDto>(publisher);

            return response;
        }

        public async Task<ApiResponse> Update(PublisherUpdateDto entity) => await repository.Update(mapper.Map<Publisher>(entity));

        public async Task<ApiResponse> UpdateMany(IEnumerable<PublisherUpdateDto> entities) =>
            await repository.UpdateMany(mapper.Map<IEnumerable<Publisher>>(entities));
    }
}