export interface ITeacherScheduleItem{
    subjectName: string;
    groupId: number;
    classroom: string | null;
    startsAt: string;
    endsAt: string;
}