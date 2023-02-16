using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Feedbacks;

// Query
public class GetAllStudentToTeacherFeedbacksQuery : IRequest<StudentToTeacherFeedbacksListModel>
{
}

// Handler
public class GetAllStudentToTeacherFeedbacksQueryHandler : IRequestHandler<GetAllStudentToTeacherFeedbacksQuery,
    StudentToTeacherFeedbacksListModel>
{
    private readonly IApplicationDbContext _context;

    public GetAllStudentToTeacherFeedbacksQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentToTeacherFeedbacksListModel> Handle(GetAllStudentToTeacherFeedbacksQuery request,
        CancellationToken cancellationToken)
    {
        var feedbacks = await _context.StudentToTeacherFeedbacks.ToListAsync(cancellationToken);

        if (feedbacks.Count == 0)
            throw new NotFoundException
            {
                Error = "feedbacks_not_found",
                ErrorDescription = "Student to teacher feedbacks not found"
            };

        return new StudentToTeacherFeedbacksListModel
        {
            Feedbacks = feedbacks
        };
    }
}

// Result model
public class StudentToTeacherFeedbacksListModel
{
    public IList<StudentToTeacherFeedback> Feedbacks { get; set; }
}