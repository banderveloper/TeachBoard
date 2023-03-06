using System.Text.Json.Serialization;
using TeachBoard.EducationService.Application.Converters;

namespace TeachBoard.EducationService.Application;

/// <summary>
/// Exception error code, sent to client with error model as snake_case_string
/// </summary>
/// <example>SubjectNotFound => subject_not_found</example>
[JsonConverter(typeof(SnakeCaseStringEnumConverter<ErrorCode>))]
public enum ErrorCode
{
    Unknown,
    
    InvalidDateTime,
    
    SubjectNotFound,
    SubjectAlreadyExists,
    
    ExaminationNotFound,
    
    CompletedHomeworkNotFound,
    CompletedHomeworkInvalidTeacher,
    CompletedHomeworkAlreadyExists,
    
    HomeworkNotFound,
    
    LessonNotFound,
    LessonNotStarted
}