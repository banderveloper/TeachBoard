using System.Text.Json.Serialization;

namespace TeachBoard.IdentityService.Domain.Entities;

// User role (such as student, teacher, admin)
public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    //
    [JsonIgnore] public IList<User>? Users { get; set; }
}