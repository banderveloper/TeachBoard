using System.Text.Json.Serialization;

namespace TeachBoard.Domain.Entities;

// Many-to-many user to roles
public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    //
    [JsonIgnore] public User? User { get; set; }
    [JsonIgnore] public Role? Role { get; set; }
}