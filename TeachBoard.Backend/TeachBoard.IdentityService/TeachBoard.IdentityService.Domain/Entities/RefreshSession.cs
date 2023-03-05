using System.Text.Json.Serialization;

namespace TeachBoard.IdentityService.Domain.Entities;

// Auth session, based of refresh tokens
public class RefreshSession : BaseEntity
{
    public int UserId { get; set; }
    public Guid RefreshToken { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    

    [JsonIgnore] public User? User { get; set; }
}