export interface ITeacherUncheckedHomework {
    id: number;
    homeworkId: number;
    studentId: number;
    studentComment: string | null;
    createdAt: string;
}