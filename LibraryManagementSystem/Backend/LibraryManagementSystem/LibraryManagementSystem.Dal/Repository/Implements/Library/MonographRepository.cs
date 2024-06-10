﻿using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Core;
using LibraryManagementSystem.Entities.Models.Library;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;

namespace LibraryManagementSystem.Dal.Repository.Implements.Library
{
    public class MonographRepository(ISqlConnector sqlConnector) : IMonographRepository
    {
        private readonly ISqlConnector _sqlConnector = sqlConnector;

        /* Para insertar una Monografia en la base de datos */

        public async Task<ApiResponse> Create(Monograph entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [
                new("@Classification", entity.Classification), new("@Title", entity.Title), new("@Description", entity.Description),
                new("@Tutor", entity.Tutor), new("@PresentationDate", entity.PresentationDate), new("@Image", entity.Image),
                new("@CareerId", entity.CareerId), new("@IsActive", entity.IsActive)
            ];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspInsertMonograph", CommandType.StoredProcedure, parameters);
                /* Convertir respuesta de la base de datos a objeto de tipo ApiResponse */
                response = _sqlConnector.DataRowToObject<ApiResponse>(result.Rows[0]);
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
                /* No paso una validación en el procedimiento almacenado */
                if (response.IsSuccess == 1)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                /* No existe el registro a eliminar */
                if (response.IsSuccess == 2)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }
                /* Ocurrio algún error en el procedimiento almacenado */
                if (response.IsSuccess == 3)
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* Retornar código de éxito y objeto registrado */
                response.StatusCode = HttpStatusCode.OK;
                entity.MonographId = Convert.ToInt32(response.Result);
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

        /* Para eliminar una Monografia en la base de datos */

        public async Task<ApiResponse> Delete(int id)
        {
            /* Parámetro que recibe el procedimiento almacenado para eliminar un registro */
            SqlParameter[] parameters = [new("@MonographId", id)];
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado para eliminar registro por medio del ID */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspDeleteMonograph", CommandType.StoredProcedure, parameters);
                /* Convertir respuesta de la base de datos a objeto de tipo ApiResponse */
                response = _sqlConnector.DataRowToObject<ApiResponse>(result.Rows[0]);
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
                /* No paso una validación en el procedimiento almacenado */
                if (response.IsSuccess == 1)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                /* No existe el registro a eliminar */
                if (response.IsSuccess == 2)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }
                /* Ocurrio algún error en el procedimiento almacenado */
                if (response.IsSuccess == 3)
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* Retornar código de éxito */
                response.StatusCode = HttpStatusCode.OK;
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

        /* Para obtener todas las Monografias en la base de datos */

        public async Task<ApiResponse> GetAll()
        {
            ApiResponse response = new();
            try
            {
                /* Ejecutar procedimiento almacenado */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspGetMonograph", CommandType.StoredProcedure);
                /* Convertir DataTable a una Lista */
                IEnumerable<Monograph> monographs = _sqlConnector.DataTableToList<Monograph>(result);
                /* Retornar lista de elementos y código de éxito */
                response.IsSuccess = 0;
                response.Result = monographs;
                response.Message = monographs.Any() ? "Registros obtenidos exitosamente." : "No hay registros.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        /* Para obtener una Monografia de la base de datos */

        public async Task<ApiResponse> GetById(int id)
        {
            ApiResponse response = new();
            SqlParameter[] parameters = [new("@MonographId", id)];
            try
            {
                /* Ejecutar procedimiento almacenado */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspGetMonograph", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count <= 0)
                {
                    response.IsSuccess = 2;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "No se encontro un registro con el ID ingresado.";
                    return response;
                }
                /* Convertir fila a un objeto */
                Monograph? monograph = _sqlConnector.DataRowToObject<Monograph>(result.Rows[0]);
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

        /* Para actualizar una Monografia en la base de datos */

        public async Task<ApiResponse> Update(Monograph entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [
                new("@MonographId", entity.MonographId), new("@Classification", entity.Classification), new("@Title", entity.Title),
                new("@Description", entity.Description), new("@Tutor", entity.Tutor), new("@PresentationDate", entity.PresentationDate),
                new("@Image", entity.Image), new("@CareerId", entity.CareerId), new("@IsActive", entity.IsActive)
            ];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspUpdateMonograph", CommandType.StoredProcedure, parameters);
                /* Convertir respuesta de la base de datos a objeto de tipo ApiResponse */
                response = _sqlConnector.DataRowToObject<ApiResponse>(result.Rows[0]);
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
                /* No paso una validación en el procedimiento almacenado */
                if (response.IsSuccess == 1)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                /* No existe el registro a eliminar */
                if (response.IsSuccess == 2)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }
                /* Ocurrio algún error en el procedimiento almacenado */
                if (response.IsSuccess == 3)
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* Retornar código de éxito */
                response.StatusCode = HttpStatusCode.OK;
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
