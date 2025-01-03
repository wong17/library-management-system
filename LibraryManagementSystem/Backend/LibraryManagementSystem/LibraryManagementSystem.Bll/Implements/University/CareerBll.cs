﻿using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.University;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.University;
using LibraryManagementSystem.Entities.Dtos.University;
using LibraryManagementSystem.Entities.Models.University;

namespace LibraryManagementSystem.Bll.Implements.University
{
    public class CareerBll(ICareerRepository repository, IMapper mapper) : ICareerBll
    {
        public async Task<ApiResponse> GetAll()
        {
            var response = await repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<Career> careers)
                return response;
            // Retornar Dtos
            response.Result = mapper.Map<IEnumerable<CareerDto>>(careers);

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is not Career career)
                return response;
            // Retornar Dto
            response.Result = mapper.Map<CareerDto>(career);

            return response;
        }
    }
}