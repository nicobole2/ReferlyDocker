using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Referly.Models.Paged;

namespace Referly.Data;

public class DataContextDapper(IConfiguration config)
{
    private readonly IConfiguration _config = config;

    public IEnumerable<T> LoadMethod<T>(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")
        );
        return dbConnection.Query<T>(sql);
    }

    public T LoadSingleMethod<T>(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")
        );
        return dbConnection.QuerySingle<T>(sql);
    }

    public bool ExecuteSql(string sql)
    {
        IDbConnection dbConnection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")
        );
        return dbConnection.Execute(sql) > 0;
    }

    public IEnumerable<T> LoadMethodWithParameters<T>(string sql, object parameters)
    {
        
        IDbConnection dbConnection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")
        );
        return dbConnection.Query<T>(sql, parameters, commandType: CommandType.StoredProcedure);
    }

    public bool ExecuteSqlWithParameters(string sql, object parameters)
    {
        IDbConnection dbConnection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")
        );
        var result = dbConnection.Execute(sql, parameters, commandType: CommandType.StoredProcedure);
        System.Console.WriteLine($"Result: {result}");
        return dbConnection.Execute(sql, parameters, commandType: CommandType.StoredProcedure) > 0;
    }

    public T ExecuteStoredProcedureWithResult<T>(string sql, object parameters)
    {
        using IDbConnection dbConnection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")
        );
        dbConnection.Open();

        // Creación de parámetros de salida usando Dapper
        var dynamicParameters = new DynamicParameters(parameters);

        // Suponiendo que hay un parámetro OUTPUT en el procedimiento almacenado
        dynamicParameters.Add("@ResultMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

        // Ejecuta el procedimiento almacenado
        var result = dbConnection.QuerySingleOrDefault<T>(
            sql,
            dynamicParameters,
            commandType: CommandType.StoredProcedure
        );

        // Obtener el valor del parámetro OUTPUT
        var outputMessage = dynamicParameters.Get<string>("@ResultMessage");

        // Puedes hacer algo con el mensaje de salida aquí, por ejemplo, loguearlo o manejarlo
        Console.WriteLine("Resultado del procedimiento: " + outputMessage);

        return result;
    }

    public string ExecuteStoredProcedureWithStringResult(string sql, object parameters)
    {
        using IDbConnection dbConnection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")
        );
        dbConnection.Open();

        var dynamicParameters = new DynamicParameters(parameters);

        dynamicParameters.Add("@ResultMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

        dbConnection.QuerySingleOrDefault<string>(
            sql,
            dynamicParameters,
            commandType: CommandType.StoredProcedure
        );

        var outputMessage = dynamicParameters.Get<string>("@ResultMessage");

        Console.WriteLine("[DataContextDapper.cs]: Resultado del procedimiento: " + outputMessage);

        return outputMessage;
    }

    public (IEnumerable<T> Items, int Total) ExecutePaginatedQuery<T>(
      string storedProcedure,
        DynamicParameters parameters)
    {
        using IDbConnection dbConnection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")
        );
        dbConnection.Open();
        using var multi = dbConnection.QueryMultiple(
            storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure);

        var items = multi.Read<T>();
        var total = multi.ReadFirst<int>();

        return (items, total);
    }

    public string ExecuteSqlWithParametersString(string sql, object parameters)
    {
        IDbConnection dbConnection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")
        );
        return dbConnection.ExecuteScalar<string>(sql, parameters, commandType: CommandType.StoredProcedure);
    }

    public int ExecuteSqlWithReturnValue(string sql, object parameters)
    {
        IDbConnection dbConnection = new SqlConnection(
            _config.GetConnectionString("DefaultConnection")
        );
        return dbConnection.ExecuteScalar<int>(sql, parameters, commandType: CommandType.StoredProcedure);
    }


}
