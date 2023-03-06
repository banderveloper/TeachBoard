using MediatR;
using Microsoft.EntityFrameworkCore;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Interfaces;
using TeachBoard.EducationService.Domain.Entities;
using TeachBoard.EducationService.Domain.Enums;

namespace TeachBoard.EducationService.Application.Features.Examination;

public class SetStudentExaminationActivityCommand : IRequest<StudentExaminationActivity>
{
    public int StudentId { get; set; }
    public int ExaminationId { get; set; }
    public int? Grade { get; set; }
    public StudentExaminationStatus Status { get; set; }
}

public class
    SetStudentExaminationActivityCommandHandler : IRequestHandler<SetStudentExaminationActivityCommand,
        StudentExaminationActivity>
{
    private readonly IApplicationDbContext _context;

    public SetStudentExaminationActivityCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentExaminationActivity> Handle(SetStudentExaminationActivityCommand request,
        CancellationToken cancellationToken)
    {
        var existingExamination =
            await _context.Examinations
                .AsTracking()
                .FirstOrDefaultAsync(e => e.Id == request.ExaminationId, cancellationToken);
        
        if (existingExamination is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.ExaminationNotFound,
                PublicErrorMessage = "Examination not found",
                LogErrorMessage =
                    $"SetStudentExaminationActivityCommand error. Examination with id [{request.ExaminationId}] not found",
            };

        // If exam not set - create, otherwise update 

        var existingActivity = await _context.StudentExaminationActivities
            .FirstOrDefaultAsync(a => a.StudentId == request.StudentId &&
                                      a.ExaminationId == request.ExaminationId, cancellationToken);

        // if activity exists - update and return updated
        if (existingActivity is not null)
        {
            existingActivity.Status = request.Status;
            existingActivity.Grade = request.Status == StudentExaminationStatus.Passed ? request.Grade : null;
            await _context.SaveChangesAsync(cancellationToken);
            return existingActivity;
        }

        // if not exists - create new
        var newActivity = new StudentExaminationActivity
        {
            ExaminationId = request.ExaminationId,
            StudentId = request.StudentId,
            Status = request.Status,
            Grade = request.Status == StudentExaminationStatus.Passed ? request.Grade : null,
        };

        _context.StudentExaminationActivities.Add(newActivity);
        await _context.SaveChangesAsync(cancellationToken);

        return newActivity;
    }
}