export interface ICreateExaminationRequest {
    subjectId: number;
    checkingTeacherId: number;
    groupId: number;
    startsAt: Date;
    endsAt: Date;
}