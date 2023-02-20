using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Subject;

public class CreateSubjectCommand : IRequest<Domain.Entities.Subject>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int LessonsCount { get; set; }
}

public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, Domain.Entities.Subject>
{
    private readonly IApplicationDbContext _context;

    public CreateSubjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Subject> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
    {
        var existingSubject = await _context.Subjects
            .FirstOrDefaultAsync(s => s.Name.ToLower() == request.Name.ToLower(),
                cancellationToken);

        if (existingSubject is not null)
            throw new AlreadyExistsException
            {
                Error = "subject_already_exists",
                ErrorDescription = $"Subject with name '{request.Name}' already exists",
                ReasonField = "name"
            };

        var newSubject = new Domain.Entities.Subject
        {
            Name = request.Name.Normalize(),
            Description = request.Description,
            LessonsCount = request.LessonsCount
        };
        _context.Subjects.Add(newSubject);
        await _context.SaveChangesAsync(cancellationToken);

        return newSubject;
    }
}