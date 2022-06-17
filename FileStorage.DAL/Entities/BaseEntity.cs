using System.ComponentModel.DataAnnotations;

namespace FileStorage.DAL.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
}