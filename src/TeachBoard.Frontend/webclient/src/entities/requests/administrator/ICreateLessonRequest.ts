export interface ICreateLessonRequest {
    subjectId: number;
    teacherId: number;
    groupId: number;
    classroom: string;
    startsAt: Date | null;
}