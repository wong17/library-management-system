using LibraryManagementSystem.Dal.Enums;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibraryManagementSystem.Dal.Core
{
    public sealed class SqlServerConnector(
        IConfiguration configuration,
        TargetDatabase database = TargetDatabase.LibraryManagementDb)
        : ISqlConnector
    {
        private readonly string? _connectionString = configuration.GetConnectionString(database.ToString());

        /* Para ejecutar un procedimiento almacenado que reciba una lista de parámetros y nos devuelva por medio de una sentencia select una respuesta */

        public async Task<DataTable> ExecuteDataTableAsync(string commandText, CommandType commandType)
        {
            DataTable dataTable = new();
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(commandText, connection);
            command.CommandType = commandType;
            await connection.OpenAsync();

            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataTable);
            return dataTable;
        }

        public async Task<DataTable> ExecuteDataTableAsync(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            DataTable dataTable = new();
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(commandText, connection);
            command.CommandType = commandType;
            if (parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            await connection.OpenAsync();

            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataTable);
            return dataTable;
        }

        /* Para ejecutar procedimientos almacenados que reciben por parámetro una tabla como tipo de dato y devuelven por medio de una o varias sentencia SELECT la respuesta */

        public async Task<DataTable> ExecuteSpWithTvp(DataTable dataTable, string tableTypeName, string procedureName, string procedureParameterName)
        {
            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new(procedureName, connection);
            command.CommandType = CommandType.StoredProcedure;

            var parameter = command.Parameters.AddWithValue(procedureParameterName, dataTable);
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.TypeName = tableTypeName;

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();
            DataTable resultTable = new();
            resultTable.Load(reader);

            return resultTable;
        }

        public async Task<DataSet> ExecuteSpWithTvpMany(DataTable dataTable, string tableTypeName, string procedureName, string procedureParameterName)
        {
            DataSet resultDataSet = new();

            await using SqlConnection connection = new(_connectionString);
            await using SqlCommand command = new(procedureName, connection);
            command.CommandType = CommandType.StoredProcedure;

            var parameter = command.Parameters.AddWithValue(procedureParameterName, dataTable);
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.TypeName = tableTypeName;

            await connection.OpenAsync();

            using SqlDataAdapter adapter = new(command);
            adapter.Fill(resultDataSet);

            return resultDataSet;
        }

        /* Convierte un objeto a un DataTable */

        public DataTable ObjectToDataTable<T>(T obj) where T : class
        {
            DataTable dataTable = new(typeof(T).Name);
            var properties = typeof(T).GetProperties();

            var fila = dataTable.NewRow();
            foreach (var property in properties)
            {
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                fila[property.Name] = property.GetValue(obj) ?? DBNull.Value;
            }
            dataTable.Rows.Add(fila);

            return dataTable;
        }

        /* Convierte un DataRow a un objeto, la fila de una DataTable a un objeto de tipo T */

        public T? DataRowToObject<T>(DataRow row) where T : class, new()
        {
            T obj = new();
            var properties = obj.GetType().GetProperties();

            foreach (DataColumn column in row.Table.Columns)
            {
                var property = properties.FirstOrDefault(p => p.Name == column.ColumnName);
                if (property == null || row[column] == DBNull.Value) continue;
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                property.SetValue(obj, Convert.ChangeType(row[column], propertyType), null);
            }

            return obj;
        }

        /* Convierte un DataTable a una lista de objetos de tipo T */

        public IEnumerable<T> DataTableToList<T>(DataTable table) where T : class, new()
        {
            if (table.Rows.Count == 0)
                return [];

            var list = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                var obj = DataRowToObject<T>(row);
                if (obj is not null)
                    list.Add(obj);
            }

            return list;
        }

        /* Convierte una lista a un DataTable */

        public DataTable ListToDataTable<T>(IEnumerable<T> list) where T : class
        {
            DataTable dataTable = new(typeof(T).Name);

            var elements = list.ToList();
            if (elements.Count == 0)
                return dataTable;

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);

            foreach (var obj in elements)
            {
                var row = dataTable.NewRow();
                foreach (var property in properties)
                    row[property.Name] = property.GetValue(obj) ?? DBNull.Value;

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}