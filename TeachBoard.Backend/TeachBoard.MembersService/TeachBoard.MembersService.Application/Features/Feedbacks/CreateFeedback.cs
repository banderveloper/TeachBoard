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
        var existingTeacher = await _context.Teachers.FindAsync(request.TeacherId, cancellationToken);
        if (existingTeacher is null)
            throw new NotFoundException
            {
                Error = "teacher_not_found",
                ErrorDescription = $"Teacher with id {request.TeacherId} not found",
                ReasonField = "id"
            };

        // check student existing by id
        var existingStudent = await _context.Students.FindAsync(request.StudentId, cancellationToken);
        if (existingStudent is null)
            throw new NotFoundException
            {
                Error = "student_not_found",
                ErrorDescription = $"Student with id {request.StudentId} not found",
                ReasonField = "id"
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

