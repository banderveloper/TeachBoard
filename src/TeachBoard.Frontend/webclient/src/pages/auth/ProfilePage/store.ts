import {IGivenHomework, IServerResponse, IStudentProfileData} from "../../../entities";
import {create} from "zustand";
import {$api} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";

interface IStudentProfileStore {
    profileData: IStudentProfileData | null;
    isLoading: boolean;
    loadProfileData: () => Promise<void>;
}

export const useStudentProfileStore = create<IStudentProfileStore>(set => ({
    profileData: null,
    isLoading: false,
    loadProfileData: async () => {
        set({isLoading: true});
        const apiProfileData = await $api.get<IServerResponse<IStudentProfileData>>(Endpoints.STUDENT.GET_PROFILE_DATA);

        set({profileData: apiProfileData.data.data});

        set({isLoading: false});
    }
}));

