import {
    IGroup,
    ISearchedUserPresentation,
    IServerResponse,
    IStudentMemberData,
    IUpdateUserAvatarResponse,
    IUpdateUserPublicRequest,
    IUserPresentationData,
} from "../../../entities";
import {create} from "zustand";
import {$api, ENDPOINTS} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";

interface IUserEditStore {
    userId: number;
    isLoading: boolean;
    error: string | null;
    isSendingData: boolean;
    user: IUserPresentationData | null;
    userAsStudent: IStudentMemberData | null;

    selectedAvatar: File | null;
    setSelectedAvatar: (avatar: File) => void;

    groups: IGroup[];
    selectedGroupId: number | null;
    setSelectedGroupId: (groupId: number) => void;

    setUserId: (userId: number) => void;
    setUser: (user: IUserPresentationData) => void;

    loadUserData: () => Promise<void>;
    loadGroups: () => Promise<void>;
    sendUpdatePresentationRequest: () => Promise<void>;
    sendNewAvatar: () => Promise<void>;
}

interface IUserAsPossibleStudent {
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

    selectedAvatar: null,
    setSelectedAvatar: (avatar) => set({selectedAvatar: avatar}),

    selectedGroupId: null,
    setSelectedGroupId: (groupId) => set({selectedGroupId: groupId}),
    groups: [],

    setUserId: (userId) => set({userId: userId}),
    setUser: (user) => set({user: user}),

    loadUserData: async () => {
        set({isLoading: true});

        const {userId} = get();

        const response = await $api.get<IServerResponse<IUserAsPossibleStudent>>(ENDPOINTS.ADMINISTRATOR.GET_MEMBER_DATA + userId);

        if (response.data.error) {
            set({error: response.data.error.message});
            set({isLoading: false});
            return;
        }

        if (response.data.data!.member) {

            set({userAsStudent: response.data.data!.member});
            const {userAsStudent, loadGroups} = get();

            if (userAsStudent?.studentId){
                await loadGroups();
                set({selectedGroupId: userAsStudent?.group?.id});
            }

        }

        set({user: response.data.data!.user});

        set({isLoading: false});
    },

    loadGroups: async () => {
        const response = await $api.get<IServerResponse<IGroup[]>>(ENDPOINTS.ADMINISTRATOR.GET_ALL_GROUPS);
        set({groups: response.data.data});
    },

    sendUpdatePresentationRequest: async () => {

        set({isSendingData: true});

        const {user} = get();

        if (user) {

            const updateUserPublicRequest: IUpdateUserPublicRequest = {
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

            await $api.put(ENDPOINTS.ADMINISTRATOR.UPDATE_USER_PUBLIC_DATA, updateUserPublicRequest);

            const {userAsStudent, selectedGroupId} = get();

            if (userAsStudent) {
                await $api.put(ENDPOINTS.ADMINISTRATOR.UPDATE_STUDENT_GROUP, {
                    studentId: userAsStudent.studentId,
                    groupId: selectedGroupId
                });
            }
        }

        set({isSendingData: false});
    },

    sendNewAvatar: async () => {
        const {selectedAvatar, userId, user} = get();

        if (selectedAvatar) {

            let formData = new FormData();
            formData.append('imageFile', selectedAvatar);

            set({isSendingData: true});

            const response = await $api.post<IServerResponse<IUpdateUserAvatarResponse>>(Endpoints.ADMINISTRATOR.UPDATE_USER_AVATAR + userId, formData);

            if (response.status === 200) {
                set({user: {...user!, avatarImagePath: response.data.data!.avatarImagePath}})
            } else {
                console.error(response);
            }

            set({isSendingData: false});
        }
    }
}));