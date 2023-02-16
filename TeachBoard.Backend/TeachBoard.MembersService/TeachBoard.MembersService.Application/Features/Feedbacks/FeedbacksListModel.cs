using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Feedbacks;

// Return model
public class FeedbacksListModel
{
    public IList<Feedback> Feedbacks { get; set; }
}