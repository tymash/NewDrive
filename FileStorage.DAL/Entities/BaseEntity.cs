using System.ComponentModel.DataAnnotations;

namespace FileStorage.DAL.Entities;

/// <summary>

/// The base entity class

/// </summary>

public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the id
    /// </summary>
    public int Id { get; set; }
}