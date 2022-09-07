import { IWorkflowHistory } from 'common/interfaces/workflow-history.interface';
import { IWorkflowStatus } from 'common/interfaces/workflow-status.interface';
import { useWorkflowStates } from 'contexts/WorkflowStatesContext';
import React, { useState } from 'react';
import Select from 'react-select';
import { addToWorkflowHistory } from 'services/worklist-service';

interface DocumentStatus {
    docUid?: string;
    handleOnAddStatus: () => void;
}

const DocumentStatus: React.FC<DocumentStatus> = ({docUid, handleOnAddStatus}) => {
    const workflowStates= useWorkflowStates();
    const [status, setStatus] = useState<string | undefined>('');
    const [comment, setComment] = useState<string>('');

    const handleAddToWfh = () => {
        const update: IWorkflowHistory = {
            doctrnsUid: docUid,
            statusCode: status,
            statusComments: comment,
            recInsertedBy: 'User'
        };

        addToWorkflowHistory(update).then((response : IWorkflowHistory[]) => {
            handleOnAddStatus();
        });
    };

    return (
        <>
            <h4>Status</h4>
            <div className='d-flex flex-column'>
                <div className='mb-3'>
                    <Select
                        value={status ? {label: status, value: status} : null}
                        options={workflowStates?.map((wfs: IWorkflowStatus) => ({label: wfs.wfsmCode ?? '', value: wfs.wfsmCode ?? '' }))}
                        onChange={(opts) => setStatus(opts?.value ?? '')}
                        />
                </div>
                <div className='input-group mb-3'>
                    <textarea
                    className='form-control'
                    placeholder={'New Comments'}
                    value={comment}
                    onChange={(e) => setComment(e.target.value)}
                    />
                </div>
                <div className='d-flex flex-row-reverse'>
                    <button
                    className='btn btn-success'
                    onClick={() => handleAddToWfh()}>
                        Assign Status
                    </button>
                </div>
            </div>
        </>
    );
}

export default DocumentStatus;