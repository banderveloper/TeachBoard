using MediatR;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Interfaces;
using TeachBoard.MembersService.Domain.Entities;
using TeachBoard.MembersService.Domain.Enums;

namespace TeachBoard.MembersService.Application.Features.Feedbacks;

// Command
public class CreateFeedbackCommand : IRequest<Feedback>
{
    public int TeacherId { get; set; }
    public int StudentId { get; set; }
    public FeedbackDirection Direction { get; set; }
    public string? Text { get; set; }
    public int Rating { get; set; }
}

public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand, Feedback>
{
    private readonly IApplicationDbContext _context;

    public CreateFeedbackCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Feedback> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        // check teacher existing by id
        var existingTeacher = await _context.Teachers.FindAsync(new object[] { request.TeacherId }, cancellationToken);
        if (existingTeacher is null)
            throw new ExpectedApiException
            {
                ErrorCode = "teacher_not_found",
                PublicErrorMessage = $"Teacher not found",
                LogErrorMessage = $"Create feedback error. Teacher with id [{request.TeacherId}] not found"
            };

        // check student existing by id
        var existingStudent = await _context.Students.FindAsync(new object[] { request.StudentId }, cancellationToken);
        if (existingStudent is null)
            throw new ExpectedApiException
            {
                ErrorCode = "student_not_found",
                PublicErrorMessage = $"Student not found",
                LogErrorMessage = $"CreateFeedback command error. Student with id [{request.StudentId}] not found"
            };

        // if ok - create
        var feedback = new Feedback
        {
            StudentId = request.StudentId,
            TeacherId = request.TeacherId,
            Direction = request.Direction,
            Rating = request.Rating,
            Text = request.Text
        };

        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync(cancellationToken);

        return feedback;
    }
}