export interface IStudentGroupData {
    id: number;
    name: string;
    createdAt: Date;
}

export interface IStudentMemberData {
    userId: number;
    studentId: number;
    group: IStudentGroupData | null;
}