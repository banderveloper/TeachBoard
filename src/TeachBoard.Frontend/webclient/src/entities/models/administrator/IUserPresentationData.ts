export interface IUserPresentationData {
    id: number;
    userName: string;
    role: string;
    firstName: string;
    lastName: string;
    patronymic: string;
    phoneNumber: string | null;
    homeAddress: string | null;
    email: string | null;
    dateOfBirth: Date | null;
    avatarImagePath: string | null;
}