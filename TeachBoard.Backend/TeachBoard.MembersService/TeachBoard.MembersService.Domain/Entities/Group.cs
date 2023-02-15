using System.Text.Json.Serialization;

namespace TeachBoard.MembersService.Domain.Entities;

public class Group : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    [JsonIgnore] public IList<Student>? Students { get; set; }
}