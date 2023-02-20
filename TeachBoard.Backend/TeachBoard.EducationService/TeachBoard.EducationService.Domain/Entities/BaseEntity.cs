using System.ComponentModel.DataAnnotations;

namespace TeachBoard.EducationService.Domain.Entities;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
}