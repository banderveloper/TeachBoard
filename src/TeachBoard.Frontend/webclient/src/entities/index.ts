////////////////////////////////////
//////////// ENUMERABLE ////////////
////////////////////////////////////

export {EnumUserRole} from './enums/EnumUserRole'

////////////////////////////////////
//////////// REQUESTS //////////////
////////////////////////////////////

export type {ILoginRequest} from './requests/auth/ILoginRequest';

////////////////////////////////////
//////////// RESPONSES /////////////
////////////////////////////////////

export type {IServerResponse} from './responses/IServerResponse';
export type {ILoginResponse} from './responses/auth/ILoginResponse';

////////////////////////////////////
//////////// SHARED STORES /////////////
////////////////////////////////////

export {useAuthStore} from './sharedStores/useAuthStore';

////////////////////////////////////
//////////// MODELS /////////////
////////////////////////////////////

export type {IGivenHomework} from './models/student/IGivenHomework';
export type {IScheduleItem} from './models/student/IScheduleItem';

