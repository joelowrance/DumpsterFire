using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.EntityFrameworkCore;

namespace MegaMercado.Infrastructure.Persistence.FileSystem;

public class FileSystemPersistence
{
    public void Serialize<T>(T obj, string path)
    {
        //use binary serialization to save the object to a file with compression
        using var fileStream = new FileStream(path, FileMode.Create);
        using var compressionStream = new GZipStream(fileStream, CompressionMode.Compress);
        var binaryFormatter = new BinaryFormatter();
#pragma warning disable CS8604
#pragma warning disable SYSLIB0011
        binaryFormatter.Serialize(compressionStream, obj);
#pragma warning restore SYSLIB0011
#pragma warning restore CS8604
    }

    public T Deserialize<T>(string path)
    {
        //use binary deserialization to read the object from a file with decompression
        using var fileStream = new FileStream(path, FileMode.Open);
        using var compressionStream = new GZipStream(fileStream, CompressionMode.Decompress);
        var binaryFormatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011
        return (T)binaryFormatter.Deserialize(compressionStream);
#pragma warning restore SYSLIB0011
    }
}