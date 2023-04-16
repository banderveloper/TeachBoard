import {ICreateLessonRequest, IGroup, IServerResponse, ISubject, ITeacherPresentation} from "../../../entities";
import {create} from "zustand";
import {$api} from "../../../shared";
import Endpoints from "../../../shared/api/endpoints";

interface IGroupsSubjectsUserPresentationsBundle {
    groups: IGroup[];
    subjects: ISubject[],
    teachers: ITeacherPresentation[]
}

interface ICreateLessonStore {
    selectedTeacherId: number;
    selectedSubjectId: number;
    selectedGroupId: number;
    classroom: string;
    startsAt: Date | null;

    selectData: IGroupsSubjectsUserPresentationsBundle;
    isSelectDataLoading: boolean;
    isLessonCreateLoading: boolean;
    error: string | null;

    setTeacherId: (teacherId: number) => void;
    setSubjectId: (subjectId: number) => void;
    setGroupId: (groupId: number) => void;
    setClassroom: (classroom: string) => void;
    setStartAt: (startsAt: Date | null) => void;

    loadSelectData: () => Promise<void>;
    sendCreateLessonRequest: () => Promise<void>;
}

export const useCreateLessonStore = create<ICreateLessonStore>((set, get) => ({

    isSelectDataLoading: false,
    isLessonCreateLoading: false,

    error: null,
    selectData: {groups: [], teachers: [], subjects: []},

    selectedGroupId: 1,
    selectedSubjectId: 1,
    selectedTeacherId: 1,
    classroom: '',
    startsAt: new Date(),

    setGroupId: (groupId) => {
        set({selectedGroupId: groupId})
    },
    setSubjectId: (subjectId) => {
        set({selectedSubjectId: subjectId})
    },
    setTeacherId: (teacherId) => {
        set({selectedTeacherId: teacherId})
    },
    setClassroom: (classroom) => {
        set({classroom: classroom})
    },
    setStartAt: (startsAt) => {
        set({startsAt: startsAt})
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

    sendCreateLessonRequest: async () => {

        set({isLessonCreateLoading: true});

        const {selectedTeacherId, selectedSubjectId, selectedGroupId, classroom, startsAt} = get();

        const request: ICreateLessonRequest = {
            classroom,
            startsAt: startsAt,
            groupId: selectedGroupId,
            subjectId: selectedSubjectId,
            teacherId: selectedTeacherId
        };

        const response = await $api.post<IServerResponse<any>>(Endpoints.ADMINISTRATOR.CREATE_LESSON, request);

        if(response.data.error)
            set({error: response.data.error});

        set({isLessonCreateLoading: false});
    }
}));