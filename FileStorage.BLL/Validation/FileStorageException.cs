using System.Runtime.Serialization;

namespace FileStorage.BLL.Validation;

[Serializable]
public class FileStorageException : Exception
{
    public FileStorageException()
    {
    }

    public FileStorageException(string message) : base(message)
    {
    }
    
    protected FileStorageException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}