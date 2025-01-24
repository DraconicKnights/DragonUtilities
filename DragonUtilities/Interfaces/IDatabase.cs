using DragonUtilities.Enums;

namespace DragonUtilities.Interfaces;

public interface IDatabase
{
    void OpenConnection();
    void CloseConnection();
    int ExecuteNonQuery(string query, IDictionary<string, object> parameters = null!);
    T ExecuteScalar<T>(string query, IDictionary<string, object> parameters = null!);
    IEnumerable<IDictionary<string, object>> ExecuteQuery(string query, IDictionary<string, object> parameters = null!);
    IDatabaseTransaction BeginTransaction();
    DatabaseType DatabaseType { get; }
}