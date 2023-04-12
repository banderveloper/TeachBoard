export interface ITeacherCurrentLesson{
    id: number;
    subjectName: string;
    teacherId: number;
    topic: string | null;
    classroom: string | null;
    groupId: number;
    startsAt: string;
    endsAt: string;
}