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
    public class BookRepository(ISqlConnector sqlConnector) : IBookRepository
    {
        private readonly ISqlConnector _sqlConnector = sqlConnector;

        /* Para insertar un Libro en la base de datos */

        public async Task<ApiResponse> Create(Book entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [
                new("@ISBN10", entity.ISBN10), new("@ISBN13", entity.ISBN13), new("@Classification", entity.Classification),
                new("@Title", entity.Title), new("@Description", entity.Description), new("@PublicationYear", entity.PublicationYear),
                new("@Image", entity.Image), new("@PublisherId", entity.PublisherId), new("@CategoryId", entity.CategoryId),
                new("@NumberOfCopies", entity.NumberOfCopies), new("@IsActive", entity.IsActive)
            ];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspInsertBook", CommandType.StoredProcedure, parameters);
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
                entity.BookId = Convert.ToInt32(response.Result);
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

        /* Para eliminar un Libro en la base de datos */

        public async Task<ApiResponse> Delete(int id)
        {
            /* Parámetro que recibe el procedimiento almacenado para eliminar un registro */
            SqlParameter[] parameters = [new("@BookId", id)];
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado para eliminar registro por medio del ID */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspDeleteBook", CommandType.StoredProcedure, parameters);
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

        /* Para obtener todos los Libros en la base de datos */

        public async Task<ApiResponse> GetAll()
        {
            ApiResponse response = new();
            try
            {
                /* Ejecutar procedimiento almacenado */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspGetBook", CommandType.StoredProcedure);
                /* Convertir DataTable a una Lista */
                IEnumerable<Book> books = _sqlConnector.DataTableToList<Book>(result);
                /* Retornar lista de elementos y código de éxito */
                response.IsSuccess = 0;
                response.Result = books;
                response.Message = books.Any() ? "Registros obtenidos exitosamente." : "No hay registros.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        /* Para obtener un Libro en la base de datos */

        public async Task<ApiResponse> GetById(int id)
        {
            ApiResponse response = new();
            SqlParameter[] parameters = [new("@BookId", id)];
            try
            {
                /* Ejecutar procedimiento almacenado */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspGetBook", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count <= 0)
                {
                    response.IsSuccess = 2;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "No se encontro un registro con el ID ingresado.";
                    return response;
                }
                /* Convertir fila a un objeto */
                Book? book = _sqlConnector.DataRowToObject<Book>(result.Rows[0]);
                /* Sino se pudo convertir la fila a un objeto */
                if (book is null)
                {
                    response.Message = "Error al obtener respuesta de la base de datos.";
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* Retorna código de éxito y registro encontrado */
                response.IsSuccess = 0;
                response.Result = book;
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

        public async Task<ApiResponse> GetFilteredBook(FilterBookDto filterBookDto)
        {
            ApiResponse response = new();
            /* Convertir lista a DataTable */
            DataTable authorsTable = _sqlConnector.ListToDataTable(filterBookDto.Authors ?? []);
            DataTable publishersTable = _sqlConnector.ListToDataTable(filterBookDto.Publishers ?? []);
            DataTable categoriesTable = _sqlConnector.ListToDataTable(filterBookDto.Categories ?? []);
            DataTable subCategoriesTable = _sqlConnector.ListToDataTable(filterBookDto.SubCategories ?? []);

            SqlParameter[] parameters = [
                new("@AuthorIds", SqlDbType.Structured)
                {
                    TypeName = "[Library].AuthorType", Value = authorsTable.Columns.Count == 0 ? null : authorsTable
                },
                new("@PublisherIds", SqlDbType.Structured)
                {
                    TypeName = "[Library].PublisherType", Value = publishersTable.Columns.Count == 0 ? null : publishersTable
                },
                new("@CategoryIds", SqlDbType.Structured)
                {
                    TypeName = "[Library].CategoryType", Value = categoriesTable.Columns.Count == 0 ? null : categoriesTable
                },
                new("@SubCategoryIds", SqlDbType.Structured)
                {
                    TypeName = "[Library].SubCategoryType", Value = subCategoriesTable.Columns.Count == 0 ? null : subCategoriesTable
                },
                new("@PublicationYear", SqlDbType.SmallInt)
                {
                    Value = filterBookDto.PublicationYear
                }
            ];

            try
            {
                /* Ejecutar procedimiento almacenado */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspGetFilteredBooks", CommandType.StoredProcedure, parameters);
                /* Convertir DataTable a una Lista */
                IEnumerable<Book> books = _sqlConnector.DataTableToList<Book>(result);
                /* Retornar lista de elementos y código de éxito */
                response.IsSuccess = 0;
                response.Result = books;
                response.Message = books.Any() ? "Registros obtenidos exitosamente." : "No hay registros.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        /* Para actualizar un Libro en la base de datos */

        public async Task<ApiResponse> Update(Book entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [
                new("@BookId", entity.BookId), new("@ISBN10", entity.ISBN10), new("@ISBN13", entity.ISBN13), new("@Classification", entity.Classification),
                new("@Title", entity.Title), new("@Description", entity.Description), new("@PublicationYear", entity.PublicationYear),
                new("@Image", entity.Image), new("@PublisherId", entity.PublisherId), new("@CategoryId", entity.CategoryId),
                new("@NumberOfCopies", entity.NumberOfCopies), new("@IsActive", entity.IsActive)
            ];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                DataTable result = await _sqlConnector.ExecuteDataTableAsync("[Library].uspUpdateBook", CommandType.StoredProcedure, parameters);
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