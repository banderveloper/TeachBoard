import React, {useEffect} from 'react';
import {GivenHomeworkCard} from "../../../features";
import {useHomeworksStore} from "./store";
import './index.css'

export const StudentHomeworksPage = () => {

    const {homeworks, loadHomeworks, isLoading} = useHomeworksStore();

    useEffect(() => {
        loadHomeworks();
    }, []);

    return (
        <div className='given-homework-card-container'>
            {
                isLoading
                    ? <h1>Loading</h1>
                    :
                    homeworks.map(hw => (
                        < GivenHomeworkCard
                            key={hw.homeworkId}
                            homeworkId={hw.homeworkId}
                            subjectName={hw.subjectName}
                            teacherId={hw.teacherId}
                            filePath={hw.filePath}
                            createdAt={hw.createdAt}
                        />
                    ))
            }
        </div>
    );
};
