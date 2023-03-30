export interface IStudentLessonActivityItem {
    studentId: number;
    lessonId: number;
    lessonTopic: string;
    subjectName: string;
    attendanceStatus: string;
    grade: number | null;
    activityCreatedAt: string;
}