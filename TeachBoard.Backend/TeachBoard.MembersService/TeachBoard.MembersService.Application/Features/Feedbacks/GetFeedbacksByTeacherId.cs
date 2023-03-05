using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;

namespace TeachBoard.MembersService.Application.Features.Feedbacks;

// Query
public class GetFeedbacksByTeacherIdQuery : IRequest<FeedbacksListModel>
{
    public int TeacherId { get; set; }
}

public class GetFeedbacksByTeacherIdQueryHandler : IRequestHandler<GetFeedbacksByTeacherIdQuery, FeedbacksListModel>
{
    private readonly IApplicationDbContext _context;

    public GetFeedbacksByTeacherIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FeedbacksListModel> Handle(GetFeedbacksByTeacherIdQuery request,
        CancellationToken cancellationToken)
    {
        var feedbacks = await _context.Feedbacks
            .Where(f => f.TeacherId == request.TeacherId)
            .ToListAsync(cancellationToken);

        return new FeedbacksListModel { Feedbacks = feedbacks };
    }
}