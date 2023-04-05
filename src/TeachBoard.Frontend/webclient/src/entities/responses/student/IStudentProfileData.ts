interface IUserInfo {
    id: number;
    userName: string;
    role: string;
    firstName: string;
    lastName: string;
    patronymic: string,
    phoneNumber?: string,
    homeAddress?: string,
    email?: string,
    dateOfBirth?: string,
    avatarImagePath?: string,
    emailConfirmed: boolean,
    phoneNumberConfirmed: boolean
}

interface IGroupInfo {
    id: number;
    name: string;
    createdAt: string;
}

export interface IStudentProfileData {

    user: IUserInfo;
    group: IGroupInfo;
}