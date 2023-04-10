import {create} from "zustand";
import {$api} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";
import {IServerResponse, ITeacherUncheckedHomework} from "../../../entities";

interface ITeacherHomeworksStore{
    homeworks: ITeacherUncheckedHomework[],
    isLoading: boolean,
    loadHomeworks: () => Promise<void>
}

export const useTeacherHomeworksStore = create<ITeacherHomeworksStore>(set => ({
    isLoading: false,
    homeworks: [],

    loadHomeworks: async() => {
        set({isLoading: true});
        const apiHomeworks = await $api.get<IServerResponse<ITeacherUncheckedHomework[]>>(Endpoints.TEACHER.GET_UNCHECKED_HOMEWORKS);

        set({homeworks: apiHomeworks.data.data});

        set({isLoading: false});
    }
}));