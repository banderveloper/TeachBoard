﻿namespace TeachBoard.EducationService.Application.Features.Lesson;

public class LessonPresentationDataModel
{
    public string SubjectName { get; set; }
    public int TeacherId { get; set; }
    public string? Topic { get; set; }
    public string? Classroom { get; set; }

    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}