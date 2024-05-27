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
        private readonly IPublisherRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse> Create(Publisher entity) => await _repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<Publisher> entities)
        {
            var response = await _repository.CreateMany(entities);
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<Publisher> publishers)
                return response;
            // Retornar Dtos
            response.Result = _mapper.Map<IEnumerable<PublisherDto>>(publishers);

            return response;
        }

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            var response = await _repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<Publisher> publishers)
                return response;
            // Retornar Dtos
            response.Result = _mapper.Map<IEnumerable<PublisherDto>>(publishers);

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await _repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is null || response.Result is not Publisher publisher)
                return response;
            // Retornar Dto
            response.Result = _mapper.Map<PublisherDto>(publisher);

            return response;
        }

        public async Task<ApiResponse> Update(Publisher entity) => await _repository.Update(entity);

        public async Task<ApiResponse> UpdateMany(IEnumerable<Publisher> entities) => await _repository.UpdateMany(entities);
    }
}
