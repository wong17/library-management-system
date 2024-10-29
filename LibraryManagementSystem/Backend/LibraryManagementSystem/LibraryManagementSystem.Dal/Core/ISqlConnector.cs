using System.Data;
using System.Data.SqlClient;

namespace LibraryManagementSystem.Dal.Core
{
    public interface ISqlConnector
    {
        /* Para ejecutar un procedimiento almacenado que reciba una lista de parametros y nos devuelva por medio de una sentencia select una respuesta */
        Task<DataTable> ExecuteDataTableAsync(string commandText, CommandType commandType);
        Task<DataTable> ExecuteDataTableAsync(string commandText, CommandType commandType, SqlParameter[] parameters);

        /* Para ejecutar procedimientos almacenados que reciben por parámetro una tabla como tipo de dato y devuelven por medio de una o varias sentencias SELECT la respuesta */
        Task<DataTable> ExecuteSpWithTvp(DataTable dataTable, string tableTypeName, string procedureName, string procedureParameterName);
        Task<DataSet> ExecuteSpWithTvpMany(DataTable dataTable, string tableTypeName, string procedureName, string procedureParameterName);

        /* Convierte un objeto T a un DataTable */
        DataTable ObjectToDataTable<T>(T obj) where T : class;
        
        /* Convierte un DataRow a un objeto, es decir una fila de un DataTable a un objeto de tipo T */
        T? DataRowToObject<T>(DataRow row) where T : class, new();

        /* Convierte un DataTable a una lista de objetos T */
        IEnumerable<T> DataTableToList<T>(DataTable table) where T : class, new();

        /* Convierte una Lista de T a un DataTable */
        DataTable ListToDataTable<T>(IEnumerable<T> list) where T : class;
    }
}
