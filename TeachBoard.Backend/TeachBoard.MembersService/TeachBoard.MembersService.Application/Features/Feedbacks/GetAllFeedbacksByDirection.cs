using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.Domain.Enums;

namespace TeachBoard.MembersService.Application.Features.Feedbacks;

// Query
public class GetAllFeedbacksByDirectionQuery : IRequest<FeedbacksListModel>
{
    public FeedbackDirection Direction { get; set; }
}

// Handler
public class
    GetAllFeedbacksByDirectionQueryHandler : IRequestHandler<GetAllFeedbacksByDirectionQuery, FeedbacksListModel>
{
    private readonly IApplicationDbContext _context;

    public GetAllFeedbacksByDirectionQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FeedbacksListModel> Handle(GetAllFeedbacksByDirectionQuery request,
        CancellationToken cancellationToken)
    {
        var feedbacks = await _context.Feedbacks
            .Where(f => f.Direction == request.Direction)
            .ToListAsync(cancellationToken);

        return new FeedbacksListModel
        {
            Feedbacks = feedbacks
        };
    }
}