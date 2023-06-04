using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace MegaMercado.Infrastructure.Persistence.FileSystem;

public class DbBlobPersistence
{
    private readonly string _connectionString;

    public DbBlobPersistence(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Serialize<T>(T obj, int batchId)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(obj);

        using var connection = new SqlConnection(_connectionString);
        using var command = connection.CreateCommand();
        
        command.CommandText = "INSERT INTO Blobs (Id, BlobType, BatchId, BlobData) VALUES (@Id, @BlobType, @BatchId, @BlobData)";
        command.Parameters.Add(new SqlParameter("@Id", Guid.NewGuid()));
        command.Parameters.Add(new SqlParameter("@BlobType", typeof(T).Name));
        command.Parameters.Add(new SqlParameter("@BatchId", batchId));
        command.Parameters.Add(new SqlParameter("@BlobData", bytes));
        
        connection.Open();
        command.ExecuteNonQuery();
    }

    public T Deserialize<T>(int batchId)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = connection.CreateCommand();
        
        
        command.CommandText = "SELECT BlobData FROM Blobs WHERE BatchId = @BatchId AND BlobType = @BlobType";
        command.Parameters.Add(new SqlParameter("@BatchId", batchId));
        command.Parameters.Add(new SqlParameter("@BlobType", typeof(T).Name));
        
        connection.Open();
        using var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
        reader.Read();
        var bytes = (byte[])reader["BlobData"];
        connection.Close();
        return JsonSerializer.Deserialize<T>(bytes)!; 
    }
}