using LibraryManagementSystem.Dal.Enums;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace LibraryManagementSystem.Dal.Core
{
    public class SqlServerConnector : ISqlConnector
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public SqlServerConnector(IConfiguration configuration, TargetDatabase database = TargetDatabase.LibraryManagementDB)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString(database.ToString());
        }

        /* Para ejecutar un procedimiento almacenado que reciba una lista de parámetros y nos devuelva por medio de una sentencia select una respuesta */
        public virtual async Task<DataTable> ExecuteDataTableAsync(string commandText, CommandType commandType)
        {
            DataTable dataTable = new();
            using var connection = new SqlConnection(_connectionString);
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                await connection.OpenAsync();

                using var adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            return dataTable;
        }

        public virtual async Task<DataTable> ExecuteDataTableAsync(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            DataTable dataTable = new();
            using var connection = new SqlConnection(_connectionString);
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters is not null && parameters.Length > 0)
                    command.Parameters.AddRange(parameters);

                await connection.OpenAsync();

                using var adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            return dataTable;
        }

        /* Para ejecutar procedimientos almacenados que reciben por parámetro una tabla como tipo de dato y devuelven por medio de una o varias sentencia SELECT la respuesta */
        public virtual async Task<DataTable> ExecuteSPWithTVP(DataTable dataTable, string tableTypeName, string procedureName, string procedureParameterName)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(procedureName, connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter parameter = command.Parameters.AddWithValue(procedureParameterName, dataTable);
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.TypeName = tableTypeName;

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();
            DataTable resultTable = new();
            resultTable.Load(reader);

            return resultTable;
        }

        public virtual async Task<DataSet> ExecuteSPWithTVPMany(DataTable dataTable, string tableTypeName, string procedureName, string procedureParameterName)
        {
            DataSet resultDataSet = new();

            using (SqlConnection connection = new(_connectionString))
            {
                using SqlCommand command = new(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter parameter = command.Parameters.AddWithValue(procedureParameterName, dataTable);
                parameter.SqlDbType = SqlDbType.Structured;
                parameter.TypeName = tableTypeName;

                await connection.OpenAsync();

                using SqlDataAdapter adapter = new(command);
                adapter.Fill(resultDataSet);
            }

            return resultDataSet;
        }

        /* Convierte un objeto a un DataTable */
        public virtual DataTable ObjectToDataTable<T>(T obj) where T : class
        {
            DataTable dataTable = new(typeof(T).Name);
            PropertyInfo[] properties = typeof(T).GetProperties();

            DataRow fila = dataTable.NewRow();
            foreach (PropertyInfo property in properties)
            {
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                fila[property.Name] = property.GetValue(obj) ?? DBNull.Value;
            }
            dataTable.Rows.Add(fila);

            return dataTable;
        }

        /* Convierte un DataRow a un objeto, la fila de una DataTable a un objeto de tipo T */
        public virtual T? DataRowToObject<T>(DataRow row) where T : class, new()
        {
            if (row is null) return null;

            T obj = new();
            var properties = obj.GetType().GetProperties();

            foreach (DataColumn column in row.Table.Columns)
            {
                var property = properties.FirstOrDefault(p => p.Name == column.ColumnName);
                if (property != null && row[column] != DBNull.Value)
                {
                    Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    property.SetValue(obj, Convert.ChangeType(row[column], propertyType), null);
                }
            }

            return obj;
        }

        /* Convierte un DataTable a una lista de objetos de tipo T */
        public virtual IEnumerable<T> DataTableToList<T>(DataTable table) where T : class, new()
        {
            if (table is null || table.Rows.Count == 0)
                return Enumerable.Empty<T>();

            var list = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                T? obj = DataRowToObject<T>(row);
                if (obj is not null)
                    list.Add(obj);
            }

            return list;
        }

        /* Convierte una lista a un DataTable */
        public virtual DataTable ListToDataTable<T>(IEnumerable<T> list) where T : class
        {
            DataTable dataTable = new(typeof(T).Name);

            if (!list.Any())
                return dataTable;

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);

            foreach (T obj in list)
            {
                DataRow row = dataTable.NewRow();
                foreach (PropertyInfo property in properties)
                    row[property.Name] = property.GetValue(obj) ?? DBNull.Value;

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
