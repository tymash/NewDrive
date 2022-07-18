using System.Runtime.Serialization;

namespace FileStorage.BLL.Validation;

/// <summary>

/// The file storage exception class

/// </summary>

/// <seealso cref="Exception"/>

[Serializable]
public class FileStorageException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileStorageException"/> class
    /// </summary>
    public FileStorageException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileStorageException"/> class
    /// </summary>
    /// <param name="message">The message</param>
    public FileStorageException(string message) : base(message)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="FileStorageException"/> class
    /// </summary>
    /// <param name="info">The info</param>
    /// <param name="context">The context</param>
    protected FileStorageException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}