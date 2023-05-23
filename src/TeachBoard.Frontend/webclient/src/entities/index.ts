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
export type {ICheckHomework} from './requests/teacher/ICheckHomework'
export type {ICreatePendingUserRequestModel} from './requests/administrator/ICreatePendingUserRequestModel'
export type {IUpdateUserPublicRequest} from './requests/administrator/IUpdateUserPublicRequest'

////////////////////////////////////
//////////// RESPONSES /////////////
////////////////////////////////////

export type {IServerResponse} from './responses/IServerResponse';
export type {ILoginResponse} from './responses/auth/ILoginResponse';
export type {IStudentProfileData} from './responses/student/IStudentProfileData'
export type {ITeacherCurrentLessonFullInfo} from './responses/teacher/ITeacherCurrentLessonFullInfo'
export type {ICreatePendingUserResponse} from './responses/administrator/ICreatePendingUserResponse'
export type {IUpdateUserAvatarResponse} from './responses/administrator/IUpdateUserAvatarResponse'

////////////////////////////////////
//////////// SHARED STORES /////////////
////////////////////////////////////

export {useAuthStore} from './sharedStores/useAuthStore';

////////////////////////////////////
//////////// MODELS /////////////
////////////////////////////////////

export type {ITeacherScheduleItem} from './models/teacher/ITeacherScheduleItem'
export type {ITeacherUncheckedHomework} from './models/teacher/ITeacherUncheckedHomework'
export type {IGivenHomework} from './models/student/IGivenHomework';
export type {IScheduleItem} from './models/student/IScheduleItem';
export type {IStudentLessonActivityItem} from './models/student/IStudentLessonActivityItem'
export type {IStudentExaminationActivityItem} from './models/student/IStudentExaminationActivityItem'
export type {IStudentCurrentLessonActivityItem} from './models/teacher/IStudentCurrentLessonActivityItem'
export type {ITeacherCurrentLesson} from './models/teacher/ITeacherCurrentLesson'
export type {IGroup} from './models/administrator/IGroup'
export type {ISubject} from './models/administrator/ISubject'
export type {ITeacherPresentation} from './models/administrator/ITeacherPresentation'
export type {ICreateLessonRequest} from './requests/administrator/ICreateLessonRequest'
export type {IUncheckedHomeworksCountItem} from './models/administrator/IUncheckedHomeworksCountItem'
export type {ICreateExaminationRequest} from './requests/administrator/ICreateExaminationRequest'
export type {ISearchedUserPresentation} from './models/administrator/ISearchedUserPresentation'
export type {IUserPresentationData} from './models/administrator/IUserPresentationData'
export type {IStudentGroupData, IStudentMemberData} from './models/administrator/IStudentMemberData'