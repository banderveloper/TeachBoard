export interface IUpdateUserPublicRequest{
    id: number;
    userName: string;
    firstName: string;
    lastName: string;
    patronymic: string;
    phoneNumber: string | null;
    homeAddress: string | null;
    email: string | null;
    dateOfBirth : Date | null;
}