﻿using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Core;
using LibraryManagementSystem.Dal.Repository.Interfaces.Security;
using LibraryManagementSystem.Entities.Models.Security;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace LibraryManagementSystem.Dal.Repository.Implements.Security
{
    public class UserRepository(ISqlConnector sqlConnector) : IUserRepository
    {
        /* Para insertar un usuario en la base de datos */

        public async Task<ApiResponse> Create(User entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [new("@UserName", entity.UserName), new("@Email", entity.Email), new("@Password", entity.Password)];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                var result = await sqlConnector.ExecuteDataTableAsync("[Security].uspInsertUser", CommandType.StoredProcedure, parameters);
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
                    /* Ocurrio algún error en el procedimiento almacenado */
                    case ApiResponseCode.DatabaseError:
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;
                }

                /* Retornar código de éxito y objeto registrado */
                response.StatusCode = HttpStatusCode.OK;
                entity.UserId = Convert.ToInt32(response.Result);
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

        /* Para eliminar un usuario de la base de datos */

        public async Task<ApiResponse> Delete(int id)
        {
            /* Parámetro que recibe el procedimiento almacenado para eliminar un registro */
            SqlParameter[] parameters = [new("@UserId", id)];
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado para eliminar registro por medio del ID */
                var result = await sqlConnector.ExecuteDataTableAsync("[Security].uspDeleteUser", CommandType.StoredProcedure, parameters);
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

        /* Para obtener todos los usuarios de la base de datos */

        public async Task<ApiResponse> GetAll()
        {
            ApiResponse response = new();
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("[Security].uspGetUser", CommandType.StoredProcedure);
                /* Convertir DataTable a una Lista */
                var users = sqlConnector.DataTableToList<User>(result);
                /* Retornar lista de elementos y código de éxito */
                response.IsSuccess = 0;
                response.Result = users;
                response.Message = users.Any() ? "Registros obtenidos exitosamente." : "No hay registros.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        /* Para obtener un usuario de la base de datos */

        public async Task<ApiResponse> GetById(int id)
        {
            ApiResponse response = new();
            SqlParameter[] parameters = [new("@UserId", id)];
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("[Security].uspGetUser", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count <= 0)
                {
                    response.IsSuccess = ApiResponseCode.ResourceNotFound;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "No se encontro un registro con el ID ingresado.";
                    return response;
                }
                /* Convertir fila a un objeto */
                var user = sqlConnector.DataRowToObject<User>(result.Rows[0]);
                /* Sino se pudo convertir la fila a un objeto */
                if (user is null)
                {
                    response.Message = "Error al obtener respuesta de la base de datos.";
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* Retorna código de éxito y registro encontrado */
                response.IsSuccess = 0;
                response.Result = user;
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

        /* Para actualizar un usuario de la base de datos */

        public async Task<ApiResponse> Update(User entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [new("@UserId", entity.UserId), new("@Email", entity.Email), new("@Password", entity.Password)];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                var result = await sqlConnector.ExecuteDataTableAsync("[Security].uspUpdateUser", CommandType.StoredProcedure, parameters);
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

        /* Para iniciar sesión */

        public async Task<ApiResponse> LogIn(User entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [new("@UserName", entity.UserName), new("@Password", entity.Password)];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                var result = await sqlConnector.ExecuteDataTableAsync("[Security].uspLogInUser", CommandType.StoredProcedure, parameters);
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
                    /* Ocurrio algún error o no paso una validación en el procedimiento almacenado */
                    case ApiResponseCode.ValidationError:
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    /* No existe un usuario con el nombre ingresado */
                    case ApiResponseCode.ResourceNotFound:
                        response.StatusCode = HttpStatusCode.NotFound;
                        return response;
                }

                /* Retornar código de éxito y UserId */
                response.StatusCode = HttpStatusCode.OK;
                entity.UserId = Convert.ToInt32(response.Result);
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
    }
}