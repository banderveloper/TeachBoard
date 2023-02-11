namespace TeachBoard.Domain.Entities;

// Parent type for each entity
public class BaseEntity
{
    public int Id { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}