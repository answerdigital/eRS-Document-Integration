import { IWorkflowHistory } from "common/interfaces/workflow-history.interface";
import { IWorkflowStatus } from "common/interfaces/workflow-status.interface";
import { useWorkflowStates } from "contexts/WorkflowStatesContext";
import React, { useEffect, useState } from "react";
import Select from "react-select";
import { addToWorkflowHistory, getWorkflowHistory, updateWorkflowHistory } from "services/worklist-service";
import WorkflowHistory from "./WorkflowHistory";

interface WorkflowStatusModalProps {
    refUid: string;
}

const WorkflowStatusModal : React.FC<WorkflowStatusModalProps> = ({refUid}) => {
    const [workflowHistory, setWorkflowHistory] = useState<IWorkflowHistory[]>();
    const [selectedHistoryItem, setSelectedHistoryItem] = useState<IWorkflowHistory>();

    const workflowStates= useWorkflowStates();
    const [status, setStatus] = useState<string | undefined>('');
    const [comment, setComment] = useState<string>('');

    useEffect(() => {
        getWorkflowHistory(refUid).then((response: IWorkflowHistory[]) => {
            setWorkflowHistory(response);
        });
    }, []);

    const handleSelectWfhToEdit = (wfh?: IWorkflowHistory) => {
        setStatus(wfh?.statusCode);
        setComment(wfh?.statusComments ?? '');
    };

    const resetStatusForm = () => {
        setComment('');
        setStatus('');
    };

    const handleAddToWfh = () => {
        const update: IWorkflowHistory = {
            erstrnsUid: refUid,
            statusCode: status,
            statusComments: comment,
            recInsertedBy: 'User'
        }

        if (selectedHistoryItem) {
            updateWorkflowHistory(update).then((response : IWorkflowHistory[]) => {
                setWorkflowHistory(response);
                resetStatusForm();
            });
        }else{
            addToWorkflowHistory(update).then((response : IWorkflowHistory[]) => {
                setWorkflowHistory(response);
                resetStatusForm();
            });
        }
    };

    return (
        <div className='container-fluid'>
            <div className='row'>
                <div className='col-md-6'>
                    <WorkflowHistory
                    workflowHistory={workflowHistory}
                    handleWfhOnClick={handleSelectWfhToEdit}
                    selectedHistoryItem={selectedHistoryItem}
                    setSelectedHistoryItem={setSelectedHistoryItem} />
                </div>
                <div className='col-md-6'>
                    <h4>{selectedHistoryItem ? 'Edit Status' : 'New Status'}</h4>
                    <div className='d-flex flex-column'>
                        <div className='mb-3'>
                            <Select
                                value={status ? {label: status, value: status} : null}
                                options={workflowStates?.map((wfs: IWorkflowStatus) => ({label: wfs.wfsmCode ?? '', value: wfs.wfsmCode ?? '' }))}
                                onChange={opts => setStatus(opts?.value ?? '')}
                                />
                        </div>
                        <div className='input-group mb-3'>
                            <textarea
                            className='form-control'
                            placeholder={'New Comments'}
                            value={comment}
                            onChange={e => setComment(e.target.value)}
                            />
                        </div>
                        <div className='d-flex flex-row-reverse'>
                            <button
                            className='btn btn-outline-success'
                            onClick={() => handleAddToWfh()}>
                                {selectedHistoryItem ? 'Update' : 'Add'}
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default WorkflowStatusModal;