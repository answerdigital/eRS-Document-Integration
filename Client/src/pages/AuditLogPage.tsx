import { IAuditFilters, IAuditLog, IAuditRequest, IAuditResult } from 'common/interfaces/audit.interface';
import Modal from 'components/Modal/Modal';
import Pagination from 'components/Pagination/Pagination';
import Tooltip from 'components/Tooltip/Tooltip';
import moment from 'moment';
import React, { useEffect, useState } from 'react';
import { getAudits } from 'services/audit-service';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

const testAudits : IAuditLog[]  = [
    {
        auditRowId: 1,
        eventDttm: new Date(),
        eventCode: '1',
        eventDescription: 'Description',
        eventDetails: 'Details',
        recStatus: 'Status',
        recInserted: new Date(),
        recInsertedBy: 'Steve'
    }
];

const AuditLogPage : React.FC = () => {
    const [auditLogs, setAuditLogs] = useState<IAuditResult>();
    const [modal, setModal] = useState<boolean>(false);
    const [pageNumber, setPageNumber] = useState<number>(1);
    const [auditFilters, setAuditFilters] = useState<IAuditFilters>();

    useEffect(() => {
        fetchAudits();
    }, []);

    const fetchAudits = () => {
        const request : IAuditRequest = {
            pageNumber: pageNumber,
            filters: {}
        };

        getAudits(request).then((response : IAuditResult) => {
            setAuditLogs(response);
        })
    };

    return (
        <>
            <h1>Audits</h1>
            <Modal title={'Test'} show={modal} setShow={setModal}
                footer={
                    <>
                        <button type='button' className='btn btn-secondary'>Close</button>
                        <button type='button' className='btn btn-primary'>Save changes</button>
                    </>
                }>
                Test
            </Modal>
            <DatePicker
                selected={auditFilters?.filterByRecInsertedFrom}
                onChange={(date: Date) => {setAuditFilters({...auditFilters, filterByRecInsertedFrom: date})}}
            />
            {auditLogs && auditLogs.results.length > 0 ?
            <>
                <div className='table-responsive-md'>
                    <table className='table'>
                        <thead>
                            <tr>
                            <th scope='col'>Event Date</th>
                            <th scope='col'>Event Code</th>
                            <th scope='col'>Description</th>
                            <th scope='col'>Details</th>
                            <th scope='col'>Inserted</th>
                            <th scope='col'>Inserted By</th>
                            </tr>
                        </thead>
                        <tbody>
                            {auditLogs.results.map((log) => {
                                return (
                                    <tr key={log.auditRowId}>
                                        <td>{moment(log.eventDttm).format('DD-MM-YYYY')}</td>
                                        <td>{log.eventCode}</td>
                                        <td>{log.eventDescription}</td>
                                        <td>
                                            <Tooltip text={log.eventDetails} >
                                                {log.eventDetails}
                                            </Tooltip>
                                        </td>
                                        <td>{moment(log.recInserted).format('DD-MM-YYYY')}</td>
                                        <td>{log.recInsertedBy}</td>
                                    </tr> 
                                )
                            })
                            }
                        </tbody>
                    </table>
                </div>
                <div className="d-flex justify-content-center">
                    <Pagination pageCurrent={pageNumber} handlePageChange={setPageNumber} pagesTotal={auditLogs.pageCount ?? 1} />
                </div>
            </>
            
        : <p>No Audit Logs available</p>
        }
        </>
    );
}

export default AuditLogPage;