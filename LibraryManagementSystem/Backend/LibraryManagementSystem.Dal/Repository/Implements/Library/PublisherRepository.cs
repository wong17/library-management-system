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
        private readonly ISqlConnector _sqlConnector = sqlConnector;

        /* Para insertar una Editorial en la base de datos */

        public async Task<ApiResponse> Create(Publisher entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [new("@Name", entity.Name)];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspInsertPublisher", CommandType.StoredProcedure, parameters);
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
                /* Ocurrio algún error o no paso una validación en el procedimiento almacenado */
                if (response.IsSuccess == 1)
                {
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
            DataTable table = _sqlConnector.ListToDataTable(entities);
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado que recibe tabla por parámetro */
                DataSet result = await _sqlConnector.ExecuteSPWithTVPMany(table, "[Library].PublisherType", "[Library].uspInsertManyPublisher", "@Publishers");
                /* Convertir respuesta de la base de datos a objeto de tipo ApiResponse */
                response = _sqlConnector.DataRowToObject<ApiResponse>(result.Tables[0].Rows[0]);
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
                /* Ocurrio algún error en el procedimiento almacenado */
                if (response.IsSuccess == 1)
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* Retornar código de éxito y objeto registrado */
                response.StatusCode = HttpStatusCode.OK;
                /* Obtener los IDs insertados del segundo DataTable */
                List<int> insertedIds = [.. result.Tables[1].AsEnumerable().Select(row => row.Field<int>("InsertedID"))];
                /* Asignar IDs a los elementos correspondientes en la lista de entidades */
                int index = 0;
                foreach (var entity in entities)
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
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspDeletePublisher", CommandType.StoredProcedure, parameters);
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
                /* Ocurrio algún error en el procedimiento almacenado */
                if (response.IsSuccess == 1)
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* No existe el registro a eliminar */
                if (response.IsSuccess == 2)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
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

        /* Para obtener todas las Editoriales de la base de datos */

        public async Task<ApiResponse> GetAll()
        {
            ApiResponse response = new();
            try
            {
                /* Ejecutar procedimiento almacenado */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspGetPublisher", CommandType.StoredProcedure);
                /* Convertir DataTable a una Lista */
                IEnumerable<Publisher> publishers = _sqlConnector.DataTableToList<Publisher>(result);
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
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspGetPublisher", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count <= 0)
                {
                    response.IsSuccess = 2;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "No se encontro un registro con el ID ingresado.";
                    return response;
                }
                /* Convertir fila a un objeto */
                Publisher? publisher = _sqlConnector.DataRowToObject<Publisher>(result.Rows[0]);
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
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspUpdatePublisher", CommandType.StoredProcedure, parameters);
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
                /* Ocurrio algún error o no paso una validación en el procedimiento almacenado */
                if (response.IsSuccess == 1)
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

        /* Para actualizar varias Editoriales en la base de datos */

        public async Task<ApiResponse> UpdateMany(IEnumerable<Publisher> entities)
        {
            /* Convertir lista a DataTable */
            DataTable table = _sqlConnector.ListToDataTable(entities);
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado que recibe tabla por parámetro */
                DataTable result = await _sqlConnector.ExecuteSPWithTVP(table, "[Library].PublisherType", "[Library].uspUpdateManyPublisher", "@Publishers");
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
                /* Ocurrio algún error en el procedimiento almacenado */
                if (response.IsSuccess == 1)
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* No existe el registro a actualizar */
                if (response.IsSuccess == 2)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }
                /* Retornar código de éxito y objeto registrado */
                response.StatusCode = HttpStatusCode.OK;
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
    }
}