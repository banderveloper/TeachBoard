using System.Text.Json.Serialization;

namespace TeachBoard.Domain.Entities;

// Auth session, based of refresh tokens
public class RefreshSession : BaseEntity
{
    public int UserId { get; set; }
    public Guid RefreshToken { get; set; }
    public DateTime UpdatedAt { get; set; }

    //
    [JsonIgnore] public User? User { get; set; }
}