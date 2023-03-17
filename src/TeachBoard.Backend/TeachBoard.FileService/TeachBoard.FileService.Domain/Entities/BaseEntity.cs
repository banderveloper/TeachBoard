using System.ComponentModel.DataAnnotations;

namespace TeachBoard.FileService.Domain.Entities;

public abstract class BaseEntity
{
    [Key] public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
}