import { IWorkflowHistory } from "common/interfaces/workflow-history.interface";
import Loading from "components/Loading/Loading";
import { useWorkflowStates } from "contexts/WorkflowStatesContext";
import moment from "moment";
import { FaCheckCircle, FaExclamationTriangle, FaQuestionCircle } from "react-icons/fa";

interface WorkflowHistoryProps {
    workflowHistory?: IWorkflowHistory[];
    handleWfhOnClick: (wfsClicked?: IWorkflowHistory) => void;
    selectedHistoryItem?: IWorkflowHistory;
    setSelectedHistoryItem: (history?: IWorkflowHistory) => void;
    openDocument: (docUid: string | undefined) => void;
}

const WorkflowHistory : React.FC<WorkflowHistoryProps> = ({
    workflowHistory,
    handleWfhOnClick,
    selectedHistoryItem,
    setSelectedHistoryItem,
    openDocument}) => {

    const { getStatus } = useWorkflowStates();

    const toggleSelect = (select: IWorkflowHistory) => {
        const newSelectState = selectedHistoryItem === select ? undefined : select;
        setSelectedHistoryItem(newSelectState);
        handleWfhOnClick(newSelectState);
    };

    return (
        <>
            <div className='row'>
                <div className='input-group mb-3'>
                    <div className='table-responsive-md'>
                        <table className='table table-hover'>
                            <thead>
                                <tr>
                                <th scope='col'>Date</th>
                                <th scope='col'>Status</th>
                                <th scope='col'>User</th>
                                <th scope='col'>Attachment</th>
                                </tr>
                            </thead>
                            <tbody>
                                {workflowHistory && workflowHistory.length > 0 &&
                                workflowHistory.map((wfh : IWorkflowHistory) => {
                                    return (
                                        <tr
                                        key={wfh.ersdocsRowid}
                                        className={`cursor-pointer ${wfh === selectedHistoryItem && 'table-primary'}`}
                                        onClick={() => toggleSelect(wfh)}>
                                            <td className='align-middle'>{moment(wfh.recInserted).format('DD-MM-YYYY')}</td>
                                            <td className='align-middle'>
                                                <div className='d-flex justify-content-center'>
                                                {
                                                    wfh.statusCode === undefined ? <FaQuestionCircle /> :
                                                    getStatus(wfh.statusCode)?.errorStatus ? <FaExclamationTriangle /> : <FaCheckCircle />
                                                }
                                                </div>
                                            </td>
                                            <td className='align-middle'>{wfh.recInsertedBy}</td>
                                            <td className='align-middle'>
                                            { wfh.doctrnsUid &&
                                                <button
                                                className='btn btn-link'
                                                onClick={() => openDocument(wfh.doctrnsUid)}>
                                                    {wfh.attachTitle}
                                                </button>
                                            }
                                            </td>
                                        </tr>
                                    );
                                })
                                }
                            </tbody>
                        </table>
                        {!workflowHistory ? <Loading /> : workflowHistory.length === 0 && <p>No previous Status updates found</p>}
                    </div>
                </div>
            </div>
            <div className='row'>
                <h5>Comments:</h5>
                <p>{selectedHistoryItem?.statusComments === undefined || selectedHistoryItem?.statusComments === ''
                ? 'None'
                : selectedHistoryItem?.statusComments}</p>
            </div>
        </>
    );
}

export default WorkflowHistory;