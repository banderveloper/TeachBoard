import {
    ISearchedUserPresentation, IServerResponse, IStudentMemberData, IUpdateUserPublicRequest, IUserPresentationData,
} from "../../../entities";
import {create} from "zustand";
import {$api, ENDPOINTS} from "../../../shared";

interface IUserEditStore {
    userId: number;
    isLoading: boolean;
    error: string | null;
    isSendingData: boolean;
    user: IUserPresentationData | null;
    userAsStudent: IStudentMemberData | null;

    setUserId: (userId: number) => void;
    setUser: (user: IUserPresentationData) => void;

    loadUserData: () => Promise<void>;
    sendUpdatePresentationRequest: () => Promise<void>;
}

interface IUserAsPossibleStudent{
    user: IUserPresentationData;
    member: IStudentMemberData | null;
}

export const useUserEditStore = create<IUserEditStore>((set, get) => ({
    userId: 0,
    isLoading: false,
    isSendingData: false,
    error: null,

    user: null,
    userAsStudent: null,

    setUserId: (userId) => set({userId: userId}),
    setUser: (user) => set({user: user}),

    loadUserData: async () => {
        set({isLoading: true});

        const {userId} = get();

        const response = await $api.get<IServerResponse<IUserAsPossibleStudent>>(ENDPOINTS.ADMINISTRATOR.GET_MEMBER_DATA+userId);

        if(response.data.error){
            set({error: response.data.error});
            set({isLoading: false});
            return;
        }

        if(response.data.data!.member)
            set({userAsStudent: response.data.data!.member});

        set({user: response.data.data!.user});

        set({isLoading: false});
    },

    sendUpdatePresentationRequest: async () => {

        set({isSendingData: true});

        const {user} = get();

        if(user){

            const request : IUpdateUserPublicRequest = {
                id: user.id,
                email: user.email,
                dateOfBirth: user.dateOfBirth,
                firstName: user.firstName,
                homeAddress: user.homeAddress,
                lastName: user.lastName,
                userName: user.userName,
                patronymic: user.patronymic,
                phoneNumber: user.phoneNumber
            };

            const response = await $api.put(ENDPOINTS.ADMINISTRATOR.UPDATE_USER_PUBLIC_DATA, request);
        }

        set({isSendingData: false});
    }
}));