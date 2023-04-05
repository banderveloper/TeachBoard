////////////////////////////////////
//////////// ENUMERABLE ////////////
////////////////////////////////////

export {EnumUserRole} from './enums/EnumUserRole'
export {EnumStudentLessonAttendanceStatus} from './enums/EnumStudentLessonAttendanceStatus'

////////////////////////////////////
//////////// REQUESTS //////////////
////////////////////////////////////

export type {ILoginRequest} from './requests/auth/ILoginRequest';
export type {IApprovePendingStudentRequest} from './requests/auth/IApprovePendingStudentRequest'

////////////////////////////////////
//////////// RESPONSES /////////////
////////////////////////////////////

export type {IServerResponse} from './responses/IServerResponse';
export type {ILoginResponse} from './responses/auth/ILoginResponse';
export type {IStudentProfileData} from './responses/student/IStudentProfileData'

////////////////////////////////////
//////////// SHARED STORES /////////////
////////////////////////////////////

export {useAuthStore} from './sharedStores/useAuthStore';

////////////////////////////////////
//////////// MODELS /////////////
////////////////////////////////////

export type {IGivenHomework} from './models/student/IGivenHomework';
export type {IScheduleItem} from './models/student/IScheduleItem';
export type {IStudentLessonActivityItem} from './models/student/IStudentLessonActivityItem'
