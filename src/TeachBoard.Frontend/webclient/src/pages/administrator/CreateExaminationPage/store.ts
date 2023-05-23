import {
    ICreateExaminationRequest,
    IGroup,
    IServerResponse,
    ISubject,
    ITeacherPresentation
} from "../../../entities";
import {create} from "zustand";
import {$api} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";

interface IGroupsSubjectsUserPresentationsBundle {
    groups: IGroup[];
    subjects: ISubject[],
    teachers: ITeacherPresentation[]
}

interface ICreateExaminationStore {
    selectedTeacherId: number;
    selectedSubjectId: number;
    selectedGroupId: number;
    startsAt: Date | null;
    endsAt: Date | null;

    selectData: IGroupsSubjectsUserPresentationsBundle;
    isSelectDataLoading: boolean;
    isExaminationCreateLoading: boolean;
    error: string | null;

    setTeacherId: (teacherId: number) => void;
    setSubjectId: (subjectId: number) => void;
    setGroupId: (groupId: number) => void;
    setStartAt: (startsAt: Date | null) => void;
    setEndsAt: (endsAt: Date | null) => void;

    loadSelectData: () => Promise<void>;
    sendCreateExaminationRequest: () => Promise<void>;
}

export const useCreateExaminationStore = create<ICreateExaminationStore>((set, get) => ({

    isSelectDataLoading: false,
    isExaminationCreateLoading: false,
    selectData: {groups: [], teachers: [], subjects: []},

    selectedGroupId: 1,
    selectedSubjectId: 1,
    selectedTeacherId: 1,

    startsAt: new Date(),
    endsAt: new Date(),

    error: null,


    setGroupId: (groupId) => {
        set({selectedGroupId: groupId})
    },
    setSubjectId: (subjectId) => {
        set({selectedSubjectId: subjectId})
    },
    setTeacherId: (teacherId) => {
        set({selectedTeacherId: teacherId})
    },
    setStartAt: (startsAt) => {
        set({startsAt: startsAt})
    },
    setEndsAt: (endsAt) => {
        set({endsAt: endsAt})
    },

    loadSelectData: async () => {
        set({isSelectDataLoading: true});

        const response = await $api.get<IServerResponse<IGroupsSubjectsUserPresentationsBundle>>(Endpoints.ADMINISTRATOR.GET_CREATE_LESSON_SELECT_DATA);

        if (response.data.error) {
            set({error: response.data.error.errorMessage});
        } else {
            set({selectData: response.data.data});
        }

        set({isSelectDataLoading: false});
    },

    sendCreateExaminationRequest: async () => {

        set({isExaminationCreateLoading: true});

        const {selectedTeacherId, selectedSubjectId, selectedGroupId, startsAt, endsAt} = get();

        const request: ICreateExaminationRequest = {
            startsAt: startsAt!,
            endsAt: endsAt!,
            groupId: selectedGroupId,
            subjectId: selectedSubjectId,
            checkingTeacherId: selectedTeacherId
        };

        const response = await $api.post<IServerResponse<any>>(Endpoints.ADMINISTRATOR.CREATE_EXAMINATION, request);

        if(response.data.error)
            set({error: response.data.error});

        set({isExaminationCreateLoading: false});
    }
}));