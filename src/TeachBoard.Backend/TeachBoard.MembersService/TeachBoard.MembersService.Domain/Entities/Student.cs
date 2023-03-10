using System.Text.Json.Serialization;

namespace TeachBoard.MembersService.Domain.Entities;

public class Student : BaseEntity
{
    public int UserId { get; set; }
    public int? GroupId { get; set; }
    
    [JsonIgnore] public Group? Group { get; set; }
}