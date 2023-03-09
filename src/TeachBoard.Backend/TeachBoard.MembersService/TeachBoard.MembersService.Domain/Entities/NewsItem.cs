namespace TeachBoard.MembersService.Domain.Entities;

public class NewsItem : BaseEntity
{
    public int AuthorId { get; set; }
    public string Text { get; set; } = string.Empty;
}