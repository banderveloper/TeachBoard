namespace TeachBoard.EducationService.Application;

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