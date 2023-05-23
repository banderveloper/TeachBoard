export interface ITeacherPresentation {
    userId: number;
    teacherId: number;
    firstName: string;
    lastName: string;
    patronymic: string;
    avatarImagePath: string | null;
}