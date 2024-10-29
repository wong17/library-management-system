using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Core;
using LibraryManagementSystem.Entities.Models.Library;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;

namespace LibraryManagementSystem.Dal.Repository.Implements.Library
{
    public class SubCategoryRepository(ISqlConnector sqlConnector) : ISubCategoryRepository
    {
        /* Para insertar una Sub categoria en la base de datos */

        public async Task<ApiResponse> Create(SubCategory entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [new("@CategoryId", entity.CategoryId), new("@Name", entity.Name)];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspInsertSubCategory", CommandType.StoredProcedure, parameters);
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
                }

                /* Retornar código de éxito y objeto registrado */
                response.StatusCode = HttpStatusCode.OK;
                entity.SubCategoryId = Convert.ToInt32(response.Result);
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

        /* Para insertar varias Sub categorias en la base de datos */

        public async Task<ApiResponse> CreateMany(IEnumerable<SubCategory> entities)
        {
            /* Convertir lista a DataTable */
            var subCategories = entities.ToList();
            var table = sqlConnector.ListToDataTable(subCategories);
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado que recibe tabla por parámetro */
                var result = await sqlConnector.ExecuteSpWithTvpMany(table, "[Library].SubCategoryType", "[Library].uspInsertManySubCategory", "@SubCategories");
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
                }

                /* Retornar código de éxito y objeto registrado */
                response.StatusCode = HttpStatusCode.OK;
                /* Obtener los IDs insertados del segundo DataTable */
                List<int> insertedIds = [.. result.Tables[1].AsEnumerable().Select(row => row.Field<int>("InsertedID"))];
                /* Asignar IDs a los elementos correspondientes en la lista de entidades */
                var index = 0;
                foreach (var entity in subCategories)
                {
                    entity.SubCategoryId = insertedIds[index++];
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

        /* Para eliminar una Sub categoria de la base de datos */

        public async Task<ApiResponse> Delete(int id)
        {
            /* Parámetro que recibe el procedimiento almacenado para eliminar un registro */
            SqlParameter[] parameters = [new("@SubCategoryId", id)];
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado para eliminar registro por medio del ID */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspDeleteSubCategory", CommandType.StoredProcedure, parameters);
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

        /* Para obtener todas las Sub categorias de la base de datos */

        public async Task<ApiResponse> GetAll()
        {
            ApiResponse response = new();
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspGetSubCategory", CommandType.StoredProcedure);
                /* Convertir DataTable a una Lista */
                var subCategories = sqlConnector.DataTableToList<SubCategory>(result);
                /* Retornar lista de elementos y código de éxito */
                response.IsSuccess = 0;
                response.Result = subCategories;
                response.Message = subCategories.Any() ? "Registros obtenidos exitosamente." : "No hay registros.";
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        /* Para obtener una Sub categoria de la base de datos */

        public async Task<ApiResponse> GetById(int id)
        {
            ApiResponse response = new();
            SqlParameter[] parameters = [new("@SubCategoryId", id)];
            try
            {
                /* Ejecutar procedimiento almacenado */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspGetSubCategory", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count <= 0)
                {
                    response.IsSuccess = 2;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "No se encontro un registro con el ID ingresado.";
                    return response;
                }
                /* Convertir fila a un objeto */
                var subCategory = sqlConnector.DataRowToObject<SubCategory>(result.Rows[0]);
                /* Sino se pudo convertir la fila a un objeto */
                if (subCategory is null)
                {
                    response.Message = "Error al obtener respuesta de la base de datos.";
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }
                /* Retorna código de éxito y registro encontrado */
                response.IsSuccess = 0;
                response.Result = subCategory;
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

        /* Para actualizar una Sub categoria de la base de datos */

        public async Task<ApiResponse> Update(SubCategory entity)
        {
            ApiResponse? response;
            /* Lista de parámetros que recibe el procedimiento almacenado */
            SqlParameter[] parameters = [new("@SubCategoryId", entity.SubCategoryId), new("@CategoryId", entity.CategoryId), new("@Name", entity.Name)];

            try
            {
                /* Ejecutar procedimiento almacenado que recibe cada atributo por parámetro */
                var result = await sqlConnector.ExecuteDataTableAsync("[Library].uspUpdateSubCategory", CommandType.StoredProcedure, parameters);
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

        /* Para actualizar varias Sub categorias de la base de datos */

        public async Task<ApiResponse> UpdateMany(IEnumerable<SubCategory> entities)
        {
            /* Convertir lista a DataTable */
            var table = sqlConnector.ListToDataTable(entities);
            ApiResponse? response;
            try
            {
                /* Ejecutar procedimiento almacenado que recibe tabla por parámetro */
                var result = await sqlConnector.ExecuteSpWithTvp(table, "[Library].SubCategoryType", "[Library].uspUpdateManySubCategory", "@SubCategories");
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