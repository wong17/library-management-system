using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Core;
using LibraryManagementSystem.Dal.Repository.Interfaces.University;
using LibraryManagementSystem.Entities.Models.University;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace LibraryManagementSystem.Dal.Repository.Implements.University
{
    public class CareerRepository(ISqlConnector sqlConnector) : ICareerRepository
    {
        /* Para obtener todas las carreras de la base de datos */

        public async Task<ApiResponse> GetAll()
        {
            ApiResponse response = new();
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("University.uspGetCareer", CommandType.StoredProcedure);
                /* Convertir DataTable a una Lista */
                var careers = sqlConnector.DataTableToList<Career>(result);
                /* Retornar lista de elementos y código de éxito */
                response.IsSuccess = 0;
                response.Result = careers;
                response.Message = careers.Any() ? "Registros obtenidos exitosamente." : "No hay registros.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        /* Para obtener una carrera de la base de datos */

        public async Task<ApiResponse> GetById(int id)
        {
            ApiResponse response = new();
            SqlParameter[] parameters = [new("@CareerId", id)];
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("University.uspGetCareer", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count <= 0)
                {
                    response.IsSuccess = ApiResponseCode.ResourceNotFound;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "No se encontro un registro con el ID ingresado.";
                    return response;
                }
                /* Convertir fila a un objeto */
                var career = sqlConnector.DataRowToObject<Career>(result.Rows[0]);
                /* Sino se pudo convertir la fila a un objeto */
                if (career is null)
                {
                    response.Message = "Error al obtener respuesta de la base de datos.";
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* Retorna código de éxito y registro encontrado */
                response.IsSuccess = 0;
                response.Result = career;
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