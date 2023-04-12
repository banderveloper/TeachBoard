export interface IStudentCurrentLessonActivityItem {
    userId: number;
    studentId: number;
    firstName: string;
    lastName: string;
    patronymic: string;
    avatarImagePath: string | null;
    grade: number | null;
    attendanceStatus: string | null;
}