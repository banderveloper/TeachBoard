using System.Text.Json.Serialization;

namespace TeachBoard.IdentityService.Domain.Entities;

// Many-to-many user to roles
public class UserRole : BaseEntity
{
    public int UserId { get; set; }
    public int RoleId { get; set; }

    //
    [JsonIgnore] public User? User { get; set; }
    [JsonIgnore] public Role? Role { get; set; }
}