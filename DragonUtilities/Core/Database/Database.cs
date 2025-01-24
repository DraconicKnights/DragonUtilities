using DragonUtilities.Enums;
using DragonUtilities.Interfaces;

namespace DragonUtilities.Core.Database;

public class Database<TDatabase> : IDatabase where TDatabase : IDatabase, new()
{
    private readonly TDatabase _databaseImplementation;

    public Database(string connectionString)
    {
        // Ensure TDatabase has a matching constructor
        if (!typeof(TDatabase).GetConstructors().Any(c => c.GetParameters().Length == 1 && c.GetParameters()[0].ParameterType == typeof(string)))
        {
            throw new InvalidOperationException($"The type {typeof(TDatabase).Name} must have a constructor with a single string parameter.");
        }

        _databaseImplementation = (TDatabase)Activator.CreateInstance(typeof(TDatabase), connectionString)!;
    }

    public DatabaseType DatabaseType => _databaseImplementation.DatabaseType;

    public void OpenConnection() => _databaseImplementation.OpenConnection();

    public void CloseConnection() => _databaseImplementation.CloseConnection();

    public int ExecuteNonQuery(string query, IDictionary<string, object> parameters = null!) =>
        _databaseImplementation.ExecuteNonQuery(query, parameters);

    public T ExecuteScalar<T>(string query, IDictionary<string, object> parameters = null!) =>
        _databaseImplementation.ExecuteScalar<T>(query, parameters);

    public IEnumerable<IDictionary<string, object>> ExecuteQuery(string query, IDictionary<string, object> parameters = null!) =>
        _databaseImplementation.ExecuteQuery(query, parameters);

    public IDatabaseTransaction BeginTransaction() => _databaseImplementation.BeginTransaction();
}
