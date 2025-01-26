
using Microsoft.Data.Sqlite;
using System.Data;


namespace FunRun.CodingTracker.Data;

public class SQLiteConnectionFactory
{
    private readonly string _connectionString;

    private readonly string TableCreateStatment = $@"
            CREATE TABLE IF NOT EXISTS CodingTracker(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime DATETIME NOT NULL,
                EndTime DATETIME NOT NULL,
                Duration INT NOT NULL
            );
        ";

    public SQLiteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }


    public void InitializeDatabase()
    {
        using var connection = CreateConnection();
        using var command = connection.CreateCommand();
        command.CommandText = TableCreateStatment;
        command.ExecuteNonQuery();
    }
}