using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Core;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace LibraryManagementSystem.Dal.Repository.Implements.Library
{
    public class MonographLoanRepository(ISqlConnector sqlConnector) : IMonographLoanRepository
    {
        /* Para insertar una Solicitud de prestamo de monografia en la base de datos */

        public async Task<ApiResponse> Create(MonographLoan entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [
                new("@StudentId", entity.StudentId), new("@MonographId", entity.MonographId)
            ];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspInsertMonographLoan", CommandType.StoredProcedure, parameters);
                /* Convertir respuesta de la base de datos a objeto de tipo ApiResponse */
                response = sqlConnector.DataRowToObject<ApiResponse>(result.Rows[0]);
                /* Sino se pudo convertir la fila a un objeto de tipo ApiResponse */
                if (response is null)
                {
                    response = new ApiResponse
                    {
                        Message = "Error al obtener respuesta de la base de datos.",
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                    return response;
                }

                switch (response.IsSuccess)
                {
                    /* No paso una validación en el procedimiento almacenado */
                    case ApiResponseCode.ValidationError:
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;

                    case ApiResponseCode.ResourceNotFound:
                        response.StatusCode = HttpStatusCode.NotFound;
                        return response;
                    /* Ocurrio algún error en el procedimiento almacenado */
                    case ApiResponseCode.DatabaseError:
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;
                }

                /* Retornar código de éxito y objeto registrado */
                response.StatusCode = HttpStatusCode.OK;
                entity.MonographLoanId = Convert.ToInt32(response.Result);
                response.Result = entity;
            }
            catch (Exception ex)
            {
                response = new()
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            return response;
        }

        /* Para eliminar una Solicitud de prestamo de monografia en la base de datos */

        public async Task<ApiResponse> Delete(int id)
        {
            /* Parámetro que recibe el procedimiento almacenado para eliminar un registro */
            SqlParameter[] parameters = [new("@MonographLoanId", id)];
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado para eliminar registro por medio del ID */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspDeleteMonographLoan", CommandType.StoredProcedure, parameters);
                /* Convertir respuesta de la base de datos a objeto de tipo ApiResponse */
                response = sqlConnector.DataRowToObject<ApiResponse>(result.Rows[0]);
                /* Sino se pudo convertir la fila a un objeto de tipo ApiResponse */
                if (response is null)
                {
                    response = new()
                    {
                        Message = "Error al obtener respuesta de la base de datos.",
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                    return response;
                }

                switch (response.IsSuccess)
                {
                    /* No paso una validación en el procedimiento almacenado */
                    case ApiResponseCode.ValidationError:
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;

                    case ApiResponseCode.ResourceNotFound:
                        response.StatusCode = HttpStatusCode.NotFound;
                        return response;
                    /* Ocurrio algún error en el procedimiento almacenado */
                    case ApiResponseCode.DatabaseError:
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;

                    case ApiResponseCode.Success:
                    default:
                        /* Retornar código de éxito */
                        response.StatusCode = HttpStatusCode.OK;
                        break;
                }
            }
            catch (Exception ex)
            {
                response = new()
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            return response;
        }

        /* Para obtener todas las Solicitudes de prestamo de monografia en la base de datos */

        public async Task<ApiResponse> GetAll()
        {
            ApiResponse response = new();
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspGetMonographLoan", CommandType.StoredProcedure);
                /* Convertir DataTable a una Lista */
                var monographLoans = sqlConnector.DataTableToList<MonographLoan>(result);
                /* Retornar lista de elementos y código de éxito */
                response.IsSuccess = 0;
                response.Result = monographLoans;
                response.Message = monographLoans.Any() ? "Registros obtenidos exitosamente." : "No hay registros.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        /* Para obtener una Solicitud de prestamo de monografia en la base de datos */

        public async Task<ApiResponse> GetById(int id)
        {
            ApiResponse response = new();
            SqlParameter[] parameters = [new("@MonographLoanId", id)];
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspGetMonographLoan", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count <= 0)
                {
                    response.IsSuccess = ApiResponseCode.ResourceNotFound;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "No se encontro un registro con el ID ingresado.";
                    return response;
                }
                /* Convertir fila a un objeto */
                var monograph = sqlConnector.DataRowToObject<MonographLoan>(result.Rows[0]);
                /* Sino se pudo convertir la fila a un objeto */
                if (monograph is null)
                {
                    response.Message = "Error al obtener respuesta de la base de datos.";
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* Retorna código de éxito y registro encontrado */
                response.IsSuccess = 0;
                response.Result = monograph;
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

        public async Task<ApiResponse> GetMonographLoanByState(string? state)
        {
            ApiResponse response = new();
            SqlParameter[] parameters = [new("@State", state)];
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspGetMonographLoanByState", CommandType.StoredProcedure, parameters);
                /* Convertir DataTable a una Lista */
                var monographLoans = sqlConnector.DataTableToList<MonographLoan>(result);
                /* Retornar lista de elementos y código de éxito */
                response.IsSuccess = 0;
                response.Result = monographLoans;
                response.Message = monographLoans.Any() ? "Registros obtenidos exitosamente." : "No hay registros.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        public async Task<ApiResponse> GetMonographLoanByStudentCarnet(string? carnet)
        {
            ApiResponse response = new();
            SqlParameter[] parameters = [new("@Carnet", carnet)];
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspMonographLoanByStudentCarnet", CommandType.StoredProcedure, parameters);
                /* Convertir DataTable a una Lista */
                var monographLoans = sqlConnector.DataTableToList<MonographLoan>(result);
                /* Retornar lista de elementos y código de éxito */
                response.IsSuccess = 0;
                response.Result = monographLoans;
                response.Message = monographLoans.Any() ? "Registros obtenidos exitosamente." : "No hay registros.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        /* Para aprobar una Solicitud de prestamo de monografia en la base de datos */

        public async Task<ApiResponse> UpdateBorrowedMonographLoan(UpdateBorrowedMonographLoanDto updateBorrowedMonographLoanDto)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [
                new("@MonographLoanId", updateBorrowedMonographLoanDto.MonographLoanId), new("@DueDate", updateBorrowedMonographLoanDto.DueDate),
                new("@UserId", updateBorrowedMonographLoanDto.BorrowedUserId)
            ];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspUpdateBorrowedMonographLoan", CommandType.StoredProcedure, parameters);
                /* Convertir respuesta de la base de datos a objeto de tipo ApiResponse */
                response = sqlConnector.DataRowToObject<ApiResponse>(result.Rows[0]);
                /* Sino se pudo convertir la fila a un objeto de tipo ApiResponse */
                if (response is null)
                {
                    response = new()
                    {
                        Message = "Error al obtener respuesta de la base de datos.",
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                    return response;
                }

                switch (response.IsSuccess)
                {
                    /* No paso una validación en el procedimiento almacenado */
                    case ApiResponseCode.ValidationError:
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    /* No existe el registro a eliminar */
                    case ApiResponseCode.ResourceNotFound:
                        response.StatusCode = HttpStatusCode.NotFound;
                        return response;
                    /* Ocurrio algún error en el procedimiento almacenado */
                    case ApiResponseCode.DatabaseError:
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;

                    case ApiResponseCode.Success:
                    default:
                        /* Retornar código de éxito */
                        response.StatusCode = HttpStatusCode.OK;
                        break;
                }
            }
            catch (Exception ex)
            {
                response = new()
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            return response;
        }

        /* Para hacer una devolucion a una Solicitud de prestamo de monografia en la base de datos */

        public async Task<ApiResponse> UpdateReturnedMonographLoan(UpdateReturnedMonographLoanDto updateReturnedMonographLoanDto)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [new("@MonographLoanId", updateReturnedMonographLoanDto.MonographLoanId),
                                        new("@UserId", updateReturnedMonographLoanDto.ReturnedUserId)];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspUpdateReturnedMonographLoan", CommandType.StoredProcedure, parameters);
                /* Convertir respuesta de la base de datos a objeto de tipo ApiResponse */
                response = sqlConnector.DataRowToObject<ApiResponse>(result.Rows[0]);
                /* Sino se pudo convertir la fila a un objeto de tipo ApiResponse */
                if (response is null)
                {
                    response = new()
                    {
                        Message = "Error al obtener respuesta de la base de datos.",
                        StatusCode = HttpStatusCode.InternalServerError
                    };
                    return response;
                }

                switch (response.IsSuccess)
                {
                    /* No paso una validación en el procedimiento almacenado */
                    case ApiResponseCode.ValidationError:
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;

                    case ApiResponseCode.ResourceNotFound:
                        response.StatusCode = HttpStatusCode.NotFound;
                        return response;
                    /* Ocurrio algún error en el procedimiento almacenado */
                    case ApiResponseCode.DatabaseError:
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;

                    case ApiResponseCode.Success:
                    default:
                        /* Retornar código de éxito */
                        response.StatusCode = HttpStatusCode.OK;
                        break;
                }
            }
            catch (Exception ex)
            {
                response = new()
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            return response;
        }
    }
}