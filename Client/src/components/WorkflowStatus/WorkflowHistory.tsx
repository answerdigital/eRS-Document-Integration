import { IWorkflowHistory } from "common/interfaces/workflow-history.interface";
import Loading from "components/Loading/Loading";
import moment from "moment";

interface WorkflowHistoryProps {
    workflowHistory?: IWorkflowHistory[];
    handleWfhOnClick: (wfsClicked?: IWorkflowHistory) => void;
    selectedHistoryItem?: IWorkflowHistory;
    setSelectedHistoryItem: (history?: IWorkflowHistory) => void;
}

const WorkflowHistory : React.FC<WorkflowHistoryProps> = ({workflowHistory, handleWfhOnClick, selectedHistoryItem, setSelectedHistoryItem}) => {

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
                                            <td>{moment(wfh.recInserted).format('DD-MM-YYYY')}</td>
                                            <td>{wfh.statusCode}</td>
                                            <td>{wfh.recInsertedBy}</td>
                                            <td>
                                            { wfh.doctrnsUid && <button className='btn btn-outline-primary'>{wfh.attachTitle}</button> }
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
                <p>{selectedHistoryItem?.statusComments}</p>
            </div>
        </>
    );
}

export default WorkflowHistory;