using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Feedbacks;

// Query
public class GetFeedbacksByStudentIdQuery : IRequest<IList<Feedback>>
{
    public int StudentId { get; set; }
}

public class GetFeedbacksByStudentIdQueryHandler : IRequestHandler<GetFeedbacksByStudentIdQuery, IList<Feedback>>
{
    private readonly IApplicationDbContext _context;

    public GetFeedbacksByStudentIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Feedback>> Handle(GetFeedbacksByStudentIdQuery request,
        CancellationToken cancellationToken)
    {
        var feedbacks = await _context.Feedbacks
            .Where(f => f.StudentId == request.StudentId)
            .ToListAsync(cancellationToken);

        return feedbacks;
    }
}