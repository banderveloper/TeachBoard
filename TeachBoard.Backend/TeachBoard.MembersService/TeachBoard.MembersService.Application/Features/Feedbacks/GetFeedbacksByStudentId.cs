using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;

namespace TeachBoard.MembersService.Application.Features.Feedbacks;

// Query
public class GetFeedbacksByStudentIdQuery : IRequest<FeedbacksListModel>
{
    public int StudentId { get; set; }
}

public class GetFeedbacksByStudentIdQueryHandler : IRequestHandler<GetFeedbacksByStudentIdQuery, FeedbacksListModel>
{
    private readonly IApplicationDbContext _context;

    public GetFeedbacksByStudentIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FeedbacksListModel> Handle(GetFeedbacksByStudentIdQuery request,
        CancellationToken cancellationToken)
    {
        var feedbacks = await _context.Feedbacks
            .Where(f => f.StudentId == request.StudentId)
            .ToListAsync(cancellationToken);
        
        return new FeedbacksListModel { Feedbacks = feedbacks };
    }
}