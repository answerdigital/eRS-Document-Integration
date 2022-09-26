import { IWorkflowHistory } from 'common/interfaces/workflow-history.interface';
import { IWorkflowStatus } from 'common/interfaces/workflow-status.interface';
import { useUserDetails } from 'contexts/SessionContext';
import { useWorkflowStates } from 'contexts/WorkflowStatesContext';
import { useWorklist } from 'contexts/WorklistContext';
import React, { useState } from 'react';
import Select from 'react-select';
import { addToWorkflowHistory } from 'services/worklist-service';

interface DocumentStatus {
    docUid?: string;
    handleOnAddStatus: () => void;
}

const DocumentStatus: React.FC<DocumentStatus> = ({docUid, handleOnAddStatus}) => {
    const { selectedReferral } = useWorklist();
    const {states: workflowStates, getStatus}= useWorkflowStates();
    const { userDetails } = useUserDetails();
    const [status, setStatus] = useState<string | undefined>('');
    const [comment, setComment] = useState<string>('');

    const handleAddToWfh = () => {
        const update: IWorkflowHistory = {
            erstrnsUid: selectedReferral?.refReqUniqueId,
            doctrnsUid: docUid,
            //statusCode: status,
            statusCode: 'D-QCEPR-FAIL',
            statusComments: comment,
            recInsertedBy: userDetails?.userEmail
        };

        addToWorkflowHistory(update).then((response : IWorkflowHistory[]) => {
            handleOnAddStatus();
        });
    };

    return (
        <>
            <h4>Issue Details</h4>
            <div className='d-flex flex-column mb-3'>
                {
                    /*
                    <div className='mb-3'>
                        <Select
                            value={status ? {label: status, value: status} : null}
                            options={workflowStates?.refDocStates?.map((wfs: IWorkflowStatus) => ({label: getStatus(wfs.wfsmCode)?.wfsmDisplayValue ?? '', value: wfs.wfsmCode ?? '' }))}
                            onChange={(opts) => setStatus(opts?.value ?? '')}
                            />
                    </div>
                    */
                }
                
                <div className='input-group mb-3'>
                    <textarea
                    className='form-control'
                    placeholder={'Comments'}
                    value={comment}
                    onChange={(e) => setComment(e.target.value)}
                    />
                </div>
                <div className='d-flex flex-row-reverse'>
                    <button
                    className='btn btn-warning'
                    onClick={() => handleAddToWfh()}>
                        Submit
                    </button>
                </div>
            </div>
        </>
    );
}

export default DocumentStatus;