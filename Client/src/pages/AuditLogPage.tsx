import { IAuditFilters, IAuditLog, IAuditRequest, IAuditResult } from 'common/interfaces/audit.interface';
import Pagination from 'components/Pagination/Pagination';
import Tooltip from 'components/Tooltip/Tooltip';
import moment from 'moment';
import React, { useEffect, useState } from 'react';
import { getAudits } from 'services/audit-service';
import "react-datepicker/dist/react-datepicker.css";
import Loading from 'components/Loading/Loading';
import { debounce } from 'ts-debounce';
import AuditFilters from 'components/AuditLogs/AuditFilters';

const AuditLogPage : React.FC = () => {
    const [auditLogs, setAuditLogs] = useState<IAuditResult>();
    const [pageNumber, setPageNumber] = useState<number>(1);
    const [filters, setFilters] = useState<IAuditFilters>({});

    const fetchAudits = debounce(async () => {
        const request : IAuditRequest = {
            pageNumber: pageNumber,
            filters: filters
        };

        getAudits(request).then((response : IAuditResult) => {
            setAuditLogs(response);
        })
    }, 1000);

    useEffect(() => {
        fetchAudits().then();
    }, [filters]);

    return (
        <>
            <div className='d-flex justify-content-between align-items-center'>
                <h1>Audits</h1>
                <div>
                <button className='btn btn-sm btn-success'>Export as CSV</button>

                </div>
            </div>
            <AuditFilters filters={filters} setFilters={setFilters} />
            <div className='table-responsive-md'>
                <table className='table'>
                    <thead>
                        <tr>
                        <th scope='col'>Event Date</th>
                        <th scope='col'>Event Code</th>
                        <th scope='col'>Description</th>
                        <th scope='col'>Details</th>
                        <th scope='col'>User</th>
                        </tr>
                    </thead>
                    <tbody>
                        {auditLogs && auditLogs.results?.length > 0 &&
                        auditLogs.results.map((log : IAuditLog) => {
                            return (
                                <tr key={log.auditRowId}>
                                    <td>{moment(log.eventDttm).format('DD-MM-YYYY')}</td>
                                    <td>{log.eventCode}</td>
                                    <td>{log.eventDescription}</td>
                                    <td>{log.eventDetails}</td>
                                    <td>{log.recInsertedBy}</td>
                                </tr>
                            );
                        })
                        }
                    </tbody>
                </table>
            </div>
            <div className='d-flex justify-content-center'>
                {!auditLogs ? <Loading /> : auditLogs.results?.length === 0 && <p>No Audit Logs available</p>}
            </div>
            <div className='d-flex justify-content-center'>
                <Pagination pageCurrent={pageNumber} handlePageChange={setPageNumber} pagesTotal={auditLogs?.pageCount ?? 1} />
            </div>
        </>
    );
}

export default AuditLogPage;