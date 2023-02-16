using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Feedbacks;

// Query
public class GetAllTeacherToStudentFeedbacksQuery : IRequest<TeacherToStudentFeedbacksListModel>
{
}

// Handler
public class GetAllTeacherToStudentFeedbacksQueryHandler : IRequestHandler<GetAllTeacherToStudentFeedbacksQuery,
    TeacherToStudentFeedbacksListModel>
{
    private readonly IApplicationDbContext _context;

    public GetAllTeacherToStudentFeedbacksQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TeacherToStudentFeedbacksListModel> Handle(GetAllTeacherToStudentFeedbacksQuery request,
        CancellationToken cancellationToken)
    {
        var feedbacks = await _context.TeacherToStudentFeedbacks.ToListAsync(cancellationToken);

        if (feedbacks.Count == 0)
            throw new NotFoundException
            {
                Error = "feedbacks_not_found",
                ErrorDescription = "Teacher to student feedbacks not found"
            };

        return new TeacherToStudentFeedbacksListModel
        {
            Feedbacks = feedbacks
        };
    }
}

// Result model
public class TeacherToStudentFeedbacksListModel
{
    public IList<TeacherToStudentFeedback> Feedbacks { get; set; }
}