using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Bll.Interfaces.University;
using LibraryManagementSystem.Common.Helpers;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Dtos.University;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class MonographBll(IMonographRepository repository, ICareerBll careerBll, IAuthorBll authorBll,
        IMonographAuthorBll monographAuthorBll, IMapper mapper) : IMonographBll
    {
        public async Task<ApiResponse> Create(MonographInsertDto entity) => await repository.Create(mapper.Map<Monograph>(entity));

        public async Task<ApiResponse> Delete(int id) => await repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            // Comprobar si hay monografias
            var response = await repository.GetAll();
            if (response.Result is not IEnumerable<Monograph> monographs)
                return response;

            // Obtener los ids de las carreras de cada monografia
            var careerIds = monographs.Select(m => m.CareerId).ToList();
            var monographDtos = mapper.Map<IEnumerable<MonographDto>>(monographs).ToList();

            // 1. Comprobar si hay carreras registradas
            var careersResponse = await careerBll.GetAll();
            if (careersResponse.Result is IEnumerable<CareerDto> careers)
            {
                var careersDictionary = careers.ToDictionary(c => c.CareerId);

                for (var i = 0; i < monographDtos.Count; i++)
                {
                    var monographDto = monographDtos[i];
                    var careerId = careerIds[i];

                    if (careersDictionary.TryGetValue(Convert.ToByte(careerId), out var careerDto))
                    {
                        monographDto.Career = careerDto;
                    }
                }
            }

            // 2. Comprobar si hay autores registrados
            var authorsResponse = authorBll.GetAll();
            var monographAuthorsResponse = monographAuthorBll.GetAll();
            if (authorsResponse.Result.Result is IEnumerable<AuthorDto> authors &&
                monographAuthorsResponse.Result.Result is IEnumerable<MonographAuthorDto> monographAuthors)
            {
                var authorsDictionary = authors.ToDictionary(a => a.AuthorId);
                var monographAuthorsDictionary = ListHelper.ListToDictionary(monographAuthors.ToList(), ba => ba.MonographId, ba => ba.AuthorId);

                foreach (var monographDto in monographDtos)
                {
                    var allAuthors = new List<AuthorDto>();

                    // Si no se puede obtener la lista con id de todos los autores de la monografia actual...
                    if (!monographAuthorsDictionary.TryGetValue(monographDto.MonographId, out var authorList))
                        continue;

                    // Recorrer lista de id de autores para obtener el AuthorDto
                    foreach (var authorId in authorList)
                    {
                        // Obtener autor en base a su id
                        if (authorsDictionary.TryGetValue(authorId, out var authorDto))
                        {
                            allAuthors.Add(authorDto);
                        }
                    }
                    // Asignar lista
                    monographDto.Authors = allAuthors;
                }
            }

            response.Result = monographDtos;

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is not Monograph monograph)
                return response;

            // Obtener carrera a la que pertenece
            var careerId = monograph.CareerId;
            var monographDto = mapper.Map<MonographDto>(monograph);

            // 1. Comprobar si existe la carrera
            var careerResponse = await careerBll.GetById(careerId);
            if (careerResponse.Result is CareerDto career)
            {
                monographDto.Career = career;
            }

            // 2. Comprobar si hay autores registrados
            var authorsResponse = authorBll.GetAll();
            var monographAuthorsResponse = monographAuthorBll.GetAll();
            if (authorsResponse.Result.Result is IEnumerable<AuthorDto> authors &&
                monographAuthorsResponse.Result.Result is IEnumerable<MonographAuthorDto> monographAuthors)
            {
                var authorsDictionary = authors.ToDictionary(a => a.AuthorId);
                var monographAuthorsDictionary = ListHelper.ListToDictionary(monographAuthors.ToList(), ba => ba.MonographId, ba => ba.AuthorId);

                var allAuthors = new List<AuthorDto>();

                // Si se puede obtener la lista con id de todos los autores de la monografia actual...
                if (monographAuthorsDictionary.TryGetValue(monographDto.MonographId, out var authorList))
                {
                    // Recorrer lista de id de autores para obtener el AuthorDto
                    foreach (var authorId in authorList)
                    {
                        // Obtener autor en base a su id
                        if (authorsDictionary.TryGetValue(authorId, out var authorDto))
                        {
                            allAuthors.Add(authorDto);
                        }
                    }

                    // Asignar lista
                    monographDto.Authors = allAuthors;
                }
            }

            response.Result = monographDto;

            return response;
        }

        public async Task<ApiResponse> GetFilteredMonograph(FilterMonographDto filterMonographDto)
        {
            var response = await repository.GetFilteredMonograph(filterMonographDto);
            if (response.Result is not IEnumerable<Monograph> monographs)
                return response;

            // Obtener los ids de las carreras de cada monografia
            var careerIds = monographs.Select(m => m.CareerId).ToList();
            var monographDtos = mapper.Map<IEnumerable<MonographDto>>(monographs).ToList();

            // 1. Comprobar si hay carreras registradas
            var careersResponse = await careerBll.GetAll();
            if (careersResponse.Result is IEnumerable<CareerDto> careers)
            {
                var careersDictionary = careers.ToDictionary(c => c.CareerId);

                for (var i = 0; i < monographDtos.Count; i++)
                {
                    var monographDto = monographDtos[i];
                    var careerId = careerIds[i];

                    if (careersDictionary.TryGetValue(Convert.ToByte(careerId), out var careerDto))
                    {
                        monographDto.Career = careerDto;
                    }
                }
            }

            // 2. Comprobar si hay autores registrados
            var authorsResponse = authorBll.GetAll();
            var monographAuthorsResponse = monographAuthorBll.GetAll();
            if (authorsResponse.Result.Result is IEnumerable<AuthorDto> authors &&
                monographAuthorsResponse.Result.Result is IEnumerable<MonographAuthorDto> monographAuthors)
            {
                var authorsDictionary = authors.ToDictionary(a => a.AuthorId);
                var monographAuthorsDictionary = ListHelper.ListToDictionary(monographAuthors.ToList(), ba => ba.MonographId, ba => ba.AuthorId);

                foreach (var monographDto in monographDtos)
                {
                    var allAuthors = new List<AuthorDto>();

                    // Si no se puede obtener la lista con id de todos los autores de la monografia actual...
                    if (!monographAuthorsDictionary.TryGetValue(monographDto.MonographId, out var authorList))
                        continue;

                    // Recorrer lista de id de autores para obtener el AuthorDto
                    foreach (var authorId in authorList)
                    {
                        // Obtener autor en base a su id
                        if (authorsDictionary.TryGetValue(authorId, out var authorDto))
                        {
                            allAuthors.Add(authorDto);
                        }
                    }
                    // Asignar lista
                    monographDto.Authors = allAuthors;
                }
            }

            response.Result = monographDtos;

            return response;
        }

        public async Task<ApiResponse> Update(MonographUpdateDto entity) => await repository.Update(mapper.Map<Monograph>(entity));
    }
}