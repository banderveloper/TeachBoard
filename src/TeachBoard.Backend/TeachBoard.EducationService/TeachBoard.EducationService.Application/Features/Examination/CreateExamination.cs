﻿using System.Security.Cryptography.X509Certificates;
using MediatR;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;

namespace TeachBoard.EducationService.Application.Features.Examination;

public class CreateExaminationCommand : IRequest<Domain.Entities.Examination>
{
    public int SubjectId { get; set; }
    public int GroupId { get; set; }
    public int CheckingTeacherId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}

public class CreateExaminationCommandHandler : IRequestHandler<CreateExaminationCommand, Domain.Entities.Examination>
{
    private readonly IApplicationDbContext _context;

    public CreateExaminationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Examination> Handle(CreateExaminationCommand request,
        CancellationToken cancellationToken)
    {
        // if end time later than start time
        if (request.StartsAt > request.EndsAt)
            throw new BadRequestApiException
            {
                ErrorCode = ErrorCode.InvalidDateTime,
                PublicErrorMessage = "Examination finish time cannot be later than start",
                ReasonField = "endsAt"
            };

        var existingSubject = await _context.Subjects.FindAsync(new object[] { request.SubjectId }, cancellationToken);
        if (existingSubject is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.SubjectNotFound,
                PublicErrorMessage = $"Subject not found",
                LogErrorMessage = $"CreateExaminationCommand error. Subject with id [{request.SubjectId}] not found"
            };

        // todo check existing examination to group?

        var examination = new Domain.Entities.Examination
        {
            EndsAt = request.EndsAt,
            StarsAt = request.StartsAt,
            SubjectId = request.SubjectId,
            GroupId = request.GroupId,
            CheckingTeacherId = request.CheckingTeacherId
        };
        _context.Examinations.Add(examination);
        await _context.SaveChangesAsync(cancellationToken);

        return examination;
    }
}