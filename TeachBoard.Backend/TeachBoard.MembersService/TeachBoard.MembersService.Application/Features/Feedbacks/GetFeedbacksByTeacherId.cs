using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Feedbacks;

// Query
public class GetFeedbacksByTeacherIdQuery : IRequest<IList<Feedback>>
{
    public int TeacherId { get; set; }
}

public class GetFeedbacksByTeacherIdQueryHandler : IRequestHandler<GetFeedbacksByTeacherIdQuery, IList<Feedback>>
{
    private readonly IApplicationDbContext _context;

    public GetFeedbacksByTeacherIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Feedback>> Handle(GetFeedbacksByTeacherIdQuery request,
        CancellationToken cancellationToken)
    {
        var feedbacks = await _context.Feedbacks
            .Where(f => f.TeacherId == request.TeacherId)
            .ToListAsync(cancellationToken);

        return feedbacks;
    }
}