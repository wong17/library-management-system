using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.University;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.University;
using LibraryManagementSystem.Entities.Dtos.University;
using LibraryManagementSystem.Entities.Models.University;

namespace LibraryManagementSystem.Bll.Implements.University
{
    public class StudentBll(IStudentRepository repository, ICareerBll careerBll, IMapper mapper) : IStudentBll
    {
        public async Task<ApiResponse> GetAll()
        {
            // Comprobar si hay estudiantes registrados
            var response = await repository.GetAll();
            if (response.Result is not IEnumerable<Student> students)
                return response;

            // Comprobar si hay carreras registradas
            var careersResponse = await careerBll.GetAll();
            if (careersResponse.Result is not IEnumerable<CareerDto> careers)
                return response;
            
            // Crear diccionario de carreras
            var careerDictionary = careers.ToDictionary(c => c.CareerId);

            // Obtener ids de carreras de los estudiantes
            var careerIds = students.Select(s => s.CareerId).ToList();
            var studentDtos = mapper.Map<IEnumerable<StudentDto>>(students).ToList();

            // Asignar CareerDto a cada StudentDto basado en los ids de carreras
            for (var i = 0; i < studentDtos.Count; i++)
            {
                var studentDto = studentDtos[i];
                var careerId = careerIds[i];

                if (careerDictionary.TryGetValue(careerId, out var careerDto))
                {
                    studentDto.Career = careerDto;
                }
            }

            // Actualizar el resultado en la respuesta
            response.Result = studentDtos;

            return response;
        }

        public async Task<ApiResponse> GetByCarnet(string? carnet)
        {
            // Comprobar si existe el estudiante
            var response = await repository.GetByCarnet(carnet);
            if (response.Result is not Student student)
                return response;

            // Obtener ids de carreras de los estudiantes
            var careerId = student.CareerId;
            var studentDto = mapper.Map<StudentDto>(student);

            // Comprobar si hay carreras registradas
            var careerResponse = await careerBll.GetById(careerId);
            if (careerResponse.Result is not CareerDto career)
                return response;

            studentDto.Career = career;

            // Actualizar el resultado en la respuesta
            response.Result = studentDto;

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            // Comprobar si existe el estudiante
            var response = await repository.GetById(id);
            if (response.Result is not Student student)
                return response;

            // Obtener ids de carreras de los estudiantes
            var careerId = student.CareerId;
            var studentDto = mapper.Map<StudentDto>(student);

            // Comprobar si hay carreras registradas
            var careerResponse = await careerBll.GetById(careerId);
            if (careerResponse.Result is not CareerDto career)
                return response;

            studentDto.Career = career;

            // Actualizar el resultado en la respuesta
            response.Result = studentDto;

            return response;
        }
    }
}
