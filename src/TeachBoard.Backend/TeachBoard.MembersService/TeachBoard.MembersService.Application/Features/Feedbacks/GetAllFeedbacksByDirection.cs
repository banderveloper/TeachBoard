using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.Domain.Enums;

namespace TeachBoard.MembersService.Application.Features.Feedbacks;

// Query
public class GetAllFeedbacksByDirectionQuery : IRequest<IList<Feedback>>
{
    public FeedbackDirection Direction { get; set; }
}

// Handler
public class
    GetAllFeedbacksByDirectionQueryHandler : IRequestHandler<GetAllFeedbacksByDirectionQuery, IList<Feedback>>
{
    private readonly IApplicationDbContext _context;

    public GetAllFeedbacksByDirectionQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Feedback>> Handle(GetAllFeedbacksByDirectionQuery request,
        CancellationToken cancellationToken)
    {
        var feedbacks = await _context.Feedbacks
            .Where(f => f.Direction == request.Direction)
            .ToListAsync(cancellationToken);

        return feedbacks;
    }
}