import { IWorkflowHistory } from "common/interfaces/workflow-history.interface";
import { IWorkflowStatus } from "common/interfaces/workflow-status.interface";
import { useUserDetails } from "contexts/SessionContext";
import { useWorkflowStates } from "contexts/WorkflowStatesContext";
import { useWorklist } from "contexts/WorklistContext";
import { msalInstance } from "index";
import React, { useEffect, useState } from "react";
import Select from "react-select";
import { addToWorkflowHistory, getWorkflowHistory, updateWorkflowHistory } from "services/worklist-service";
import WorkflowHistory from "./WorkflowHistory";

interface IWorkflowStatusModalProps {
    openDocument: (docUid: string | undefined) => void;
}

const WorkflowStatusModal : React.FC<IWorkflowStatusModalProps> = ({openDocument}) => {
    const { selectedReferral } = useWorklist();
    const [workflowHistory, setWorkflowHistory] = useState<IWorkflowHistory[]>();
    const [selectedHistoryItem, setSelectedHistoryItem] = useState<IWorkflowHistory>();

    const {states: workflowStates, getStatus}= useWorkflowStates();
    const account = msalInstance.getAllAccounts()[0];
    const [status, setStatus] = useState<string | undefined>('');
    const [comment, setComment] = useState<string>('');

    const refUid = selectedReferral?.refReqUniqueId;

    useEffect(() => {
        fetchWfsHistory();
    }, []);

    const fetchWfsHistory = () => {
        if (refUid === undefined) {
            return;
        }
        getWorkflowHistory(refUid).then((response: IWorkflowHistory[]) => {
            setWorkflowHistory(response);
        });
    }

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
            recInsertedBy: account?.name
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

    const doesRefStatusExist = (): boolean => {
        return workflowHistory?.find(h => !h.doctrnsUid) !== undefined;
    };

    return (
        <div className='container-fluid'>
            <div className='row'>
                <div className='col-md-6'>
                    <WorkflowHistory
                    workflowHistory={workflowHistory}
                    handleWfhOnClick={handleSelectWfhToEdit}
                    selectedHistoryItem={selectedHistoryItem}
                    setSelectedHistoryItem={setSelectedHistoryItem}
                    openDocument={openDocument}
                    />
                </div>
                <div className='col-md-6'>
                    {selectedHistoryItem || !doesRefStatusExist() ?
                    <>
                        <h4>{selectedHistoryItem?.doctrnsUid ? 'Edit Status' : 'Set Referral Status'}</h4>
                        <div className='d-flex flex-column'>
                            <div className='mb-3'>
                                <Select
                                    value={status ? {label: getStatus(status)?.wfsmDisplayValue, value: status} : null}
                                    options={workflowStates?.refReqStates?.map((wfs: IWorkflowStatus) => ({label: getStatus(wfs.wfsmCode)?.wfsmDisplayValue, value: wfs.wfsmCode ?? '' }))}
                                    onChange={opts => setStatus(opts?.value ?? '')}
                                    />
                            </div>
                            <div className='input-group mb-3'>
                                <textarea
                                className='form-control'
                                placeholder={'Comments'}
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
                    </>
                    : null}
                </div>
            </div>
        </div>
    );
}

export default WorkflowStatusModal;