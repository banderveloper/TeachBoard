import {IServerResponse, IStudentExaminationActivityItem} from "../../../entities";
import {create} from "zustand";
import {$api} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";

interface IExaminationActivitiesStore {

    examinations: IStudentExaminationActivityItem[],
    isLoading: boolean,
    loadExaminationActivities: () => Promise<void>

}

export const useExaminationActivitiesStore = create<IExaminationActivitiesStore>(set => ({
    examinations: [],
    isLoading: false,

    loadExaminationActivities: async () => {
        set({isLoading: true});
        const apiExaminations = await $api.get<IServerResponse<IStudentExaminationActivityItem[]>>(Endpoints.STUDENT.GET_EXAMINATION_ACTIVITIES);

        set({examinations: apiExaminations.data.data});

        set({isLoading: false});
    }
}));