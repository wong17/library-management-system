using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Core;
using LibraryManagementSystem.Entities.Models.Library;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;

namespace LibraryManagementSystem.Dal.Repository.Implements.Library
{
    public class MonographAutorRepository(ISqlConnector sqlConnector) : IMonographAuthorRepository
    {
        /* Para insertar un Autor de la monografia en la base de datos */

        public async Task<ApiResponse> Create(MonographAuthor entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [new("@MonographId", entity.MonographId), new("@AuthorId", entity.AuthorId)];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspInsertMonographAuthor", CommandType.StoredProcedure, parameters);
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
                    case 1:
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    /* No existe el registro a eliminar */
                    case 2:
                        response.StatusCode = HttpStatusCode.NotFound;
                        return response;
                    /* Ocurrio algún error en el procedimiento almacenado */
                    case 3:
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;
                    default:
                        /* Retornar código de éxito y objeto registrado */
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

        /* Para insertar varios Autores de la monografia en la base de datos */

        public async Task<ApiResponse> CreateMany(IEnumerable<MonographAuthor> entities)
        {
            /* Convertir lista a DataTable */
            var table = sqlConnector.ListToDataTable(entities);
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado que recibe tabla por parámetro */
                var result = await sqlConnector.ExecuteSpWithTvpMany(table, "[Library].MonographAuthorType", "[Library].uspInsertManyMonographAuthor", "@MonographAuthors");
                /* Convertir respuesta de la base de datos a objeto de tipo ApiResponse */
                response = sqlConnector.DataRowToObject<ApiResponse>(result.Tables[0].Rows[0]);
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
                    case 1:
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    /* No existe el registro a eliminar */
                    case 2:
                        response.StatusCode = HttpStatusCode.NotFound;
                        return response;
                    /* Ocurrio algún error en el procedimiento almacenado */
                    case 3:
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;
                    default:
                        /* Retornar código de éxito y objeto registrado */
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

        /* Para eliminar un Autor de la monografia en la base de datos */

        public async Task<ApiResponse> Delete(int monographId, int authorId)
        {
            /* Parámetro que recibe el procedimiento almacenado para eliminar un registro */
            SqlParameter[] parameters = [new("@MonographId", monographId), new("@AuthorId", authorId)];
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado para eliminar registro por medio del ID */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspDeleteMonographAuthor", CommandType.StoredProcedure, parameters);
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
                    case 1:
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    /* No existe el registro a eliminar */
                    case 2:
                        response.StatusCode = HttpStatusCode.NotFound;
                        return response;
                    /* Ocurrio algún error en el procedimiento almacenado */
                    case 3:
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;
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

        /* Para obtener todos los Autores de la monografia en la base de datos */

        public async Task<ApiResponse> GetAll()
        {
            ApiResponse response = new();
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspGetMonographAuthor", CommandType.StoredProcedure);
                /* Convertir DataTable a una Lista */
                var monographAuthors = sqlConnector.DataTableToList<MonographAuthor>(result);
                /* Retornar lista de elementos y código de éxito */
                response.IsSuccess = 0;
                response.Result = monographAuthors;
                response.Message = monographAuthors.Any() ? "Registros obtenidos exitosamente." : "No hay registros.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        /* Para obtener un Autor de la monografia en la base de datos */

        public async Task<ApiResponse> GetById(int monographId, int authorId)
        {
            ApiResponse response = new();
            SqlParameter[] parameters = [new("@MonographId", monographId), new("@AuthorId", authorId)];
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspGetMonographAuthor", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count <= 0)
                {
                    response.IsSuccess = 2;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "No se encontro un registro con el ID ingresado.";
                    return response;
                }
                /* Convertir fila a un objeto */
                var monographAuthors = sqlConnector.DataRowToObject<MonographAuthor>(result.Rows[0]);
                /* Sino se pudo convertir la fila a un objeto */
                if (monographAuthors is null)
                {
                    response.Message = "Error al obtener respuesta de la base de datos.";
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* Retorna código de éxito y registro encontrado */
                response.IsSuccess = 0;
                response.Result = monographAuthors;
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

        public async Task<ApiResponse> UpdateMany(IEnumerable<MonographAuthor> entities)
        {
            /* Convertir lista a DataTable */
            var table = sqlConnector.ListToDataTable(entities);
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado que recibe tabla por parámetro */
                var result = await sqlConnector.ExecuteSpWithTvp(table, "[Library].MonographAuthorType", "[Library].uspUpdateManyMonographAuthor", "@MonographAuthors");
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
                    case 1:
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    /* No existe el registro a eliminar */
                    case 2:
                        response.StatusCode = HttpStatusCode.NotFound;
                        return response;
                    /* Ocurrio algún error en el procedimiento almacenado */
                    case 3:
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;
                    default:
                        /* Retornar código de éxito y objeto registrado */
                        response.StatusCode = HttpStatusCode.OK;
                        response.Result = entities;
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
