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
        public async Task<ApiResponse> Create(Publisher entity) => await repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<Publisher> entities)
        {
            var response = await repository.CreateMany(entities);
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

        public async Task<ApiResponse> Update(Publisher entity) => await repository.Update(entity);

        public async Task<ApiResponse> UpdateMany(IEnumerable<Publisher> entities) => await repository.UpdateMany(entities);
    }
}
