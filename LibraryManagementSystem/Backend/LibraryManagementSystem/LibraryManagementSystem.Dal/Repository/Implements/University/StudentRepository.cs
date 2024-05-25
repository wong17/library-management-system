﻿using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Core;
using LibraryManagementSystem.Dal.Repository.Interfaces.University;
using LibraryManagementSystem.Entities.Models.University;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace LibraryManagementSystem.Dal.Repository.Implements.University
{
    public class StudentRepository(ISqlConnector sqlConnector) : IStudentRepository
    {
        private readonly ISqlConnector _sqlConnector = sqlConnector;

        /* Para obtener a todos los estudiantes de la base de datos */

        public async Task<ApiResponse> GetAll()
        {
            ApiResponse response = new();
            try
            {
                /* Ejecutar procedimiento almacenado */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("University.uspGetStudent", CommandType.StoredProcedure);
                /* Convertir DataTable a una Lista */
                IEnumerable<Student> students = _sqlConnector.DataTableToList<Student>(result);
                /* Retornar lista de elementos y código de éxito */
                response.IsSuccess = 0;
                response.Result = students;
                response.Message = students.Any() ? "Registros obtenidos exitosamente." : "No hay registros.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        /* Para obtener a un estudiante de la base de datos */

        public async Task<ApiResponse> GetById(int id)
        {
            ApiResponse response = new();
            SqlParameter[] parameters = [new("@StudentId", id)];
            try
            {
                /* Ejecutar procedimiento almacenado */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("University.uspGetStudent", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count <= 0)
                {
                    response.IsSuccess = 2;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "No se encontro un registro con el ID ingresado.";
                    return response;
                }
                /* Convertir fila a un objeto */
                Student? student = _sqlConnector.DataRowToObject<Student>(result.Rows[0]);
                /* Sino se pudo convertir la fila a un objeto */
                if (student is null)
                {
                    response.Message = "Error al obtener respuesta de la base de datos.";
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* Retorna código de éxito y registro encontrado */
                response.IsSuccess = 0;
                response.Result = student;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Registro encontrado.";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}