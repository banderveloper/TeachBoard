import {IGivenHomework, IServerResponse} from "../../../entities";
import {create} from "zustand";
import {$api} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";


interface IHomeworksStore {
    homeworks: IGivenHomework[],
    isLoading: boolean,
    loadHomeworks: () => Promise<void>
}

export const useHomeworksStore = create<IHomeworksStore>(set => ({
    homeworks: [],
    isLoading: false,

    loadHomeworks: async () => {
        set({isLoading: true});
        const apiHomeworks = await $api.get<IServerResponse<IGivenHomework[]>>(Endpoints.STUDENT.GET_UNCOMPLETED_HOMEWORKS);

        set({homeworks: apiHomeworks.data.data});

        set({isLoading: false});
    }
}));