using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Core;
using LibraryManagementSystem.Entities.Models.Library;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;

namespace LibraryManagementSystem.Dal.Repository.Implements.Library
{
    public class AuthorRepository(ISqlConnector sqlConnector) : IAuthorRepository
    {
        /* Para insertar un Autor en la base de datos */

        public async Task<ApiResponse> Create(Author entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters =
                [new("@Name", entity.Name), new("@IsFormerGraduated", entity.IsFormerGraduated)];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspInsertAuthor",
                    CommandType.StoredProcedure, parameters);
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
                    case 1:
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    /* Ocurrio algún error en el procedimiento almacenado */
                    case 3:
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;
                }

                /* Retornar código de éxito y objeto registrado */
                response.StatusCode = HttpStatusCode.OK;
                entity.AuthorId = Convert.ToInt32(response.Result);
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

        /* Para insertar varios Autores en la base de datos */

        public async Task<ApiResponse> CreateMany(IEnumerable<Author> entities)
        {
            /* Convertir lista a DataTable */
            var enumerable = entities.ToList();
            var table = sqlConnector.ListToDataTable(enumerable);
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado que recibe tabla por parámetro */
                var result = await sqlConnector.ExecuteSpWithTvpMany(table, "[Library].AuthorType",
                    "[Library].uspInsertManyAuthor", "@Authors");
                /* Convertir respuesta de la base de datos a objeto de tipo ApiResponse */
                response = sqlConnector.DataRowToObject<ApiResponse>(result.Tables[0].Rows[0]);
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
                    case 1:
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    /* Ocurrio algún error en el procedimiento almacenado */
                    case 3:
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;
                }

                /* Retornar código de éxito y objeto registrado */
                response.StatusCode = HttpStatusCode.OK;
                /* Obtener los IDs insertados del segundo DataTable */
                List<int> insertedIds =
                    [.. result.Tables[1].AsEnumerable().Select(row => row.Field<int>("InsertedID"))];
                /* Asignar IDs a los elementos correspondientes en la lista de entidades */
                var index = 0;
                foreach (var entity in enumerable)
                {
                    entity.AuthorId = insertedIds[index++];
                }

                response.Result = entities;
            }
            catch (Exception ex)
            {
                response = new ()
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            return response;
        }

        /* Para eliminar un Autor en la base de datos */

        public async Task<ApiResponse> Delete(int id)
        {
            /* Parámetro que recibe el procedimiento almacenado para eliminar un registro */
            SqlParameter[] parameters = [new("@AuthorId", id)];
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado para eliminar registro por medio del ID */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspDeleteAuthor",
                    CommandType.StoredProcedure, parameters);
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
                response = new ()
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            return response;
        }

        /* Para obtener todos los Autores en la base de datos */

        public async Task<ApiResponse> GetAll()
        {
            ApiResponse response = new();
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result =
                    await sqlConnector.ExecuteDataTableAsync("[Library].uspGetAuthor", CommandType.StoredProcedure);
                /* Convertir DataTable a una Lista */
                var authors = sqlConnector.DataTableToList<Author>(result);
                /* Retornar lista de elementos y código de éxito */
                response.IsSuccess = 0;
                response.Result = authors;
                response.Message = authors.Any() ? "Registros obtenidos exitosamente." : "No hay registros.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        /* Para obtener un Autor en la base de datos */

        public async Task<ApiResponse> GetById(int id)
        {
            ApiResponse response = new();
            SqlParameter[] parameters = [new("@AuthorId", id)];
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspGetAuthor",
                    CommandType.StoredProcedure, parameters);
                if (result.Rows.Count <= 0)
                {
                    response.IsSuccess = 2;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "No se encontro un registro con el ID ingresado.";
                    return response;
                }

                /* Convertir fila a un objeto */
                var author = sqlConnector.DataRowToObject<Author>(result.Rows[0]);
                /* Sino se pudo convertir la fila a un objeto */
                if (author is null)
                {
                    response.Message = "Error al obtener respuesta de la base de datos.";
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }

                /* Retorna código de éxito y registro encontrado */
                response.IsSuccess = 0;
                response.Result = author;
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

        /* Para actualizar un Autor en la base de datos */

        public async Task<ApiResponse> Update(Author entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters =
            [
                new("@AuthorId", entity.AuthorId), new("@Name", entity.Name),
                new("@IsFormerGraduated", entity.IsFormerGraduated)
            ];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspUpdateAuthor",
                    CommandType.StoredProcedure, parameters);
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
                response = new ()
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            return response;
        }

        /* Para actualizar varios Autores en la base de datos */

        public async Task<ApiResponse> UpdateMany(IEnumerable<Author> entities)
        {
            /* Convertir lista a DataTable */
            var table = sqlConnector.ListToDataTable(entities);
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado que recibe tabla por parámetro */
                var result = await sqlConnector.ExecuteSpWithTvp(table, "[Library].AuthorType",
                    "[Library].uspUpdateManyAuthor", "@Authors");
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
                response = new ()
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            return response;
        }
    }
}