using Dapper;

namespace Referly.Data;

public class DataContextDapper(IConfiguration config)
{
    private readonly IConfiguration _config = config;

    public IEnumerable<T> LoadMethod<T>(string sql)
    {
        // Simula la conexión a la base de datos y ejecución de la consulta
        return new List<T>(); // Retorna una lista vacía como ejemplo
    }

    public T LoadSingleMethod<T>(string sql)
    {
        // Simula la ejecución de la consulta y retorno de un solo valor
        return default(T); // Retorna el valor por defecto del tipo T
    }

    public bool ExecuteSql(string sql)
    {
        // Simula la ejecución de una consulta SQL sin realizar ninguna acción
        return true; // Retorna true como si la ejecución fue exitosa
    }

    public IEnumerable<T> LoadMethodWithParameters<T>(string sql, object parameters)
    {
        // Simula la ejecución de una consulta con parámetros
        return new List<T>(); // Retorna una lista vacía
    }

    public bool ExecuteSqlWithParameters(string sql, object parameters)
    {
        // Simula la ejecución de una consulta con parámetros
        System.Console.WriteLine($"Simulado: {sql} con parámetros: {parameters}");
        return true; // Retorna true como si la ejecución fue exitosa
    }

    public T ExecuteStoredProcedureWithResult<T>(string sql, object parameters)
    {
        // Simula la ejecución de un procedimiento almacenado con un valor de retorno
        Console.WriteLine("Simulado: Ejecutando procedimiento almacenado con parámetros.");
        return default(T); // Retorna el valor por defecto de T
    }

    public string ExecuteStoredProcedureWithStringResult(string sql, object parameters)
    {
        // Simula la ejecución de un procedimiento almacenado con un resultado de tipo string
        Console.WriteLine("[DataContextDapper.cs]: Simulado procedimiento almacenado.");
        return "Resultado simulado"; // Retorna un mensaje simulado
    }

    public (IEnumerable<T> Items, int Total) ExecutePaginatedQuery<T>(
      string storedProcedure,
        DynamicParameters parameters)
    {
        // Simula una consulta paginada
        return (new List<T>(), 0); // Retorna una lista vacía y total 0
    }

    public string ExecuteSqlWithParametersString(string sql, object parameters)
    {
        // Simula la ejecución de una consulta SQL con parámetros y retorno de un string
        return "Resultado simulado"; // Retorna un string simulado
    }

    public int ExecuteSqlWithReturnValue(string sql, object parameters)
    {
        // Simula la ejecución de una consulta SQL con parámetros y retorno de un valor entero
        return 1; // Retorna un valor simulado
    }
}
