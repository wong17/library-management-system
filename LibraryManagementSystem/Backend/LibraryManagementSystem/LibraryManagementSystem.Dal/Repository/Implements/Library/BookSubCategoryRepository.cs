using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Core;
using LibraryManagementSystem.Entities.Models.Library;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;

namespace LibraryManagementSystem.Dal.Repository.Implements.Library
{
    public class BookSubCategoryRepository(ISqlConnector sqlConnector) : IBookSubCategoryRepository
    {
        private readonly ISqlConnector _sqlConnector = sqlConnector;

        /* Para insertar una Sub categoria del Libro en la base de datos */

        public async Task<ApiResponse> Create(BookSubCategory entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [new("@BookId", entity.BookId), new("@SubCategoryId", entity.SubCategoryId)];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspInsertBookSubCategory", CommandType.StoredProcedure, parameters);
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

        /* Para insertar varias Sub categorias del Libro en la base de datos */

        public async Task<ApiResponse> CreateMany(IEnumerable<BookSubCategory> entities)
        {
            /* Convertir lista a DataTable */
            DataTable table = _sqlConnector.ListToDataTable(entities);
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado que recibe tabla por parámetro */
                DataSet result = await _sqlConnector.ExecuteSPWithTVPMany(table, "[Library].BookSubCategoryType", "[Library].uspInsertManyBookSubCategory", "@BookSubCategories");
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

        /* Para eliminar una Sub categoria del Libro en la base de datos */

        public async Task<ApiResponse> Delete(int bookId, int subCategoryId)
        {
            /* Parámetro que recibe el procedimiento almacenado para eliminar un registro */
            SqlParameter[] parameters = [new("@BookId", bookId), new("@SubCategoryId", subCategoryId)];
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado para eliminar registro por medio del ID */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspDeleteBookSubCategory", CommandType.StoredProcedure, parameters);
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

        /* Para obtener todas las Sub categorias del Libro en la base de datos */

        public async Task<ApiResponse> GetAll()
        {
            ApiResponse response = new();
            try
            {
                /* Ejecutar procedimiento almacenado */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspGetBookSubCategory", CommandType.StoredProcedure);
                /* Convertir DataTable a una Lista */
                IEnumerable<BookSubCategory> bookSubCategories = _sqlConnector.DataTableToList<BookSubCategory>(result);
                /* Retornar lista de elementos y código de éxito */
                response.IsSuccess = 0;
                response.Result = bookSubCategories;
                response.Message = bookSubCategories.Any() ? "Registros obtenidos exitosamente." : "No hay registros.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        /* Para obtener una Sub categoria del Libro en la base de datos */

        public async Task<ApiResponse> GetById(int bookId, int subCategoryId)
        {
            ApiResponse response = new();
            SqlParameter[] parameters = [new("@BookId", bookId), new("@SubCategoryId", subCategoryId)];
            try
            {
                /* Ejecutar procedimiento almacenado */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspGetBookSubCategory", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count <= 0)
                {
                    response.IsSuccess = 2;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "No se encontro un registro con el ID ingresado.";
                    return response;
                }
                /* Convertir fila a un objeto */
                BookSubCategory? bookSubCategories = _sqlConnector.DataRowToObject<BookSubCategory>(result.Rows[0]);
                /* Sino se pudo convertir la fila a un objeto */
                if (bookSubCategories is null)
                {
                    response.Message = "Error al obtener respuesta de la base de datos.";
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* Retorna código de éxito y registro encontrado */
                response.IsSuccess = 0;
                response.Result = bookSubCategories;
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
