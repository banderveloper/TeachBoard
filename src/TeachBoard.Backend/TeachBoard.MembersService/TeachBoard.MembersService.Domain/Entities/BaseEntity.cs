using System.ComponentModel.DataAnnotations;

namespace TeachBoard.MembersService.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}