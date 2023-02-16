using MediatR;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.Application.Features.Feedbacks;

// Query
public class CreateTeacherToStudentFeedbackCommand : IRequest<TeacherToStudentFeedback>
{
    public int StudentId { get; set; }
    public int TeacherId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Rating { get; set; }
}

// Handler
public class
    CreateTeacherToStudentFeedbackQueryHandler : IRequestHandler<CreateTeacherToStudentFeedbackCommand,
        TeacherToStudentFeedback>
{
    private readonly IApplicationDbContext _context;

    public CreateTeacherToStudentFeedbackQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TeacherToStudentFeedback> Handle(CreateTeacherToStudentFeedbackCommand request,
        CancellationToken cancellationToken)
    {
        // if student not found - exception
        var existingStudent = await _context.Students.FindAsync(request.StudentId, cancellationToken);
        if (existingStudent is null)
            throw new NotFoundException
            {
                Error = "student_not_found",
                ErrorDescription = $"Student with id {request.StudentId} not found",
                ReasonField = "id"
            };
        
        // if teacher not found - exception
        var existingTeacher = await _context.Teachers.FindAsync(request.TeacherId, cancellationToken);
        if (existingTeacher is null)
            throw new NotFoundException
            {
                Error = "teacher_not_found",
                ErrorDescription = $"Teacher with id {request.TeacherId} not found",
                ReasonField = "id"
            };

        var feedback = new TeacherToStudentFeedback
        {
            StudentId = request.StudentId,
            TeacherId = request.TeacherId,
            Rating = request.Rating,
            Text = request.Text
        };
        _context.TeacherToStudentFeedbacks.Add(feedback);
        await _context.SaveChangesAsync(cancellationToken);

        return feedback;
    }
}