export interface ICreatePendingUserRequestModel{
    role: string;
    firstName: string;
    lastName: string;
    patronymic: string;
    phoneNumber: string;
    homeAddress: string;
    email: string;
    dateOfBirth: Date | null;
}