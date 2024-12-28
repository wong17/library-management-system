using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Core;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Models.Library;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace LibraryManagementSystem.Dal.Repository.Implements.Library
{
    public class PublisherRepository(ISqlConnector sqlConnector) : IPublisherRepository
    {
        /* Para insertar una Editorial en la base de datos */

        public async Task<ApiResponse> Create(Publisher entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [new("@Name", entity.Name)];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspInsertPublisher", CommandType.StoredProcedure, parameters);
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
                entity.PublisherId = Convert.ToInt32(response.Result);
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

        /* Para insertar varias Editoriales en la base de datos */

        public async Task<ApiResponse> CreateMany(IEnumerable<Publisher> entities)
        {
            /* Convertir lista a DataTable */
            var publishers = entities.ToList();
            var table = sqlConnector.ListToDataTable(publishers);
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado que recibe tabla por parámetro */
                var result = await sqlConnector.ExecuteSpWithTvpMany(table, "[Library].PublisherType", "[Library].uspInsertManyPublisher", "@Publishers");
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
                /* Obtener los IDs insertados del segundo DataTable */
                List<int> insertedIds = [.. result.Tables[1].AsEnumerable().Select(row => row.Field<int>("InsertedID"))];
                /* Asignar IDs a los elementos correspondientes en la lista de entidades */
                var index = 0;
                foreach (var entity in publishers)
                {
                    entity.PublisherId = insertedIds[index++];
                }
                response.Result = entities;
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

        /* Para eliminar una Editorial de la base de datos */

        public async Task<ApiResponse> Delete(int id)
        {
            /* Parámetro que recibe el procedimiento almacenado para eliminar un registro */
            SqlParameter[] parameters = [new("@PublisherId", id)];
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado para eliminar registro por medio del ID */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspDeletePublisher", CommandType.StoredProcedure, parameters);
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

        /* Para obtener todas las Editoriales de la base de datos */

        public async Task<ApiResponse> GetAll()
        {
            ApiResponse response = new();
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspGetPublisher", CommandType.StoredProcedure);
                /* Convertir DataTable a una Lista */
                var publishers = sqlConnector.DataTableToList<Publisher>(result);
                /* Retornar lista de elementos y código de éxito */
                response.IsSuccess = 0;
                response.Result = publishers;
                response.Message = publishers.Any() ? "Registros obtenidos exitosamente." : "No hay registros.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        /* Para obtener una Editorial de la base de datos */

        public async Task<ApiResponse> GetById(int id)
        {
            ApiResponse response = new();
            SqlParameter[] parameters = [new("@PublisherId", id)];
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspGetPublisher", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count <= 0)
                {
                    response.IsSuccess = ApiResponseCode.ResourceNotFound;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "No se encontro un registro con el ID ingresado.";
                    return response;
                }
                /* Convertir fila a un objeto */
                var publisher = sqlConnector.DataRowToObject<Publisher>(result.Rows[0]);
                /* Sino se pudo convertir la fila a un objeto */
                if (publisher is null)
                {
                    response.Message = "Error al obtener respuesta de la base de datos.";
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* Retorna código de éxito y registro encontrado */
                response.IsSuccess = 0;
                response.Result = publisher;
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

        /* Para actualizar una Editorial en la base de datos */

        public async Task<ApiResponse> Update(Publisher entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [new("@PublisherId", entity.PublisherId), new("@Name", entity.Name)];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspUpdatePublisher", CommandType.StoredProcedure, parameters);
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

        /* Para actualizar varias Editoriales en la base de datos */

        public async Task<ApiResponse> UpdateMany(IEnumerable<Publisher> entities)
        {
            /* Convertir lista a DataTable */
            var table = sqlConnector.ListToDataTable(entities);
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado que recibe tabla por parámetro */
                var result = await sqlConnector.ExecuteSpWithTvp(table, "[Library].PublisherType", "[Library].uspUpdateManyPublisher", "@Publishers");
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