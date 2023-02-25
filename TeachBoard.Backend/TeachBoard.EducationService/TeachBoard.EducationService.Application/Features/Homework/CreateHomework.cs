using MediatR;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Homework;

public class CreateHomeworkCommand : IRequest<Domain.Entities.Homework>
{
    public int GroupId { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public string? FilePath { get; set; }
}

public class CreateHomeworkCommandHandler : IRequestHandler<CreateHomeworkCommand, Domain.Entities.Homework>
{
    private readonly IApplicationDbContext _context;

    public CreateHomeworkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Homework> Handle(CreateHomeworkCommand request, CancellationToken cancellationToken)
    {
        var homework = new Domain.Entities.Homework
        {
            GroupId = request.GroupId,
            FilePath = request.FilePath,
            SubjectId = request.SubjectId,
            TeacherId = request.TeacherId
        };

        _context.Homeworks.Add(homework);
        await _context.SaveChangesAsync(cancellationToken);

        return homework;
    }
}