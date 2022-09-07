import { IReferral, IReferralFilters, IReferralRequest, IReferralResult } from 'common/interfaces/referral.interface';
import DocumentsModal from 'components/Documents/DocumentsModal';
import Loading from 'components/Loading/Loading';
import Modal from 'components/Modal/Modal';
import Pagination from 'components/Pagination/Pagination';
import WorkflowStatusModal from 'components/WorkflowStatus/WorkflowStatusModal';
import moment from 'moment';
import React, { useEffect, useState } from 'react';
import { getWorklist } from 'services/worklist-service';
import { debounce } from 'ts-debounce';
import WorklistFilters from 'components/Worklist/WorklistFilters';

const WorklistPage : React.FC = () => {
    const [worklist, setWorklist] = useState<IReferralResult>();
    const [selectedRef, setSelectedRef] = useState<IReferral | null>();
    const [pageNumber, setPageNumber] = useState<number>(1);
    const [filters, setFilters] = useState<IReferralFilters>({});

    const [showWorkflowModal, setShowWorkflowModal] = useState<boolean>(false);
    const [showDocumentsModal, setShowDocumentsModal] = useState<boolean>(false);

    const fetchWorklist = debounce(async () => {
        const request : IReferralRequest = {
            pageNumber: pageNumber,
            filters: filters
        };

        getWorklist(request).then((response : IReferralResult | undefined) => {
            setWorklist(response);
        });
    }, 1000);

    useEffect(() => {
        fetchWorklist().then();
    }, [filters]);

    const openModal = (modalFunc : (show: boolean) => void) => {
        if (selectedRef) {
            modalFunc(true);
        }
    };

    const toggleSelect = (select: IReferral) => {
        const newSelectState = selectedRef === select ? undefined : select;
        setSelectedRef(newSelectState);
    };

    return (
        <>
            <h1>Worklist</h1>
            {selectedRef &&
            <>
                <Modal title={'Workflow Status Update'} show={showWorkflowModal} setShow={setShowWorkflowModal}>
                    <WorkflowStatusModal refUid={selectedRef?.refReqUniqueId} />
                </Modal>
                <Modal title={'Documents'} show={showDocumentsModal} setShow={setShowDocumentsModal}>
                    <DocumentsModal refUid={selectedRef?.refReqUniqueId} />
                </Modal>
            </>
            }
            <WorklistFilters filters={filters} setFilters={setFilters} />
            <div className='table-responsive-md'>
                <table className='table'>
                    <thead>
                        <tr>
                            <th scope='col'>Hospital ID</th>
                            <th scope='col'>NHS No.</th>
                            <th scope='col'>Name</th>
                            <th scope='col'>DOB</th>
                            <th scope='col'>Gender</th>
                            <th scope='col'>Referral Details</th>
                            <th scope='col'>Meditech Speciality</th>
                            <th scope='col'>UBRN</th>
                            <th scope='col'>USRN</th>
                            <th scope='col'>Appointment Details</th>
                            <th scope='col'>Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        {worklist && worklist.results?.length > 0 && worklist.results.map((wl) => {
                            return (
                                <tr
                                key={wl.refReqUniqueId}
                                className={`cursor-pointer ${wl === selectedRef && 'table-primary'}`}
                                onClick={() => toggleSelect(wl)}>
                                    <td>{wl.hospitalId}</td>
                                    <td>{wl.refReqNhsno}</td>
                                    <td>{wl.name}</td>
                                    <td>{moment(wl.dob).format('DD-MM-YYYY')}</td>
                                    <td>{wl.gender}</td>
                                    <td>{wl.referralDetails}</td>
                                    <td>{wl.refReqSpecialty}</td>
                                    <td>{wl.refReqUbrn}</td>
                                    <td>{wl.usrn}</td>
                                    <td>{wl.apptDetails}</td>
                                    <td>{wl.refReqStatus}</td>
                                </tr>
                            );
                        })
                        }
                    </tbody>
                </table>
            </div>
            <div className='d-flex justify-content-center'>
                {!worklist ? <Loading /> : worklist.results?.length === 0 && <p>Worklist is empty</p>}
            </div>
            <div className='d-flex justify-content-center'>
                <Pagination pageCurrent={pageNumber} handlePageChange={setPageNumber} pagesTotal={worklist?.pageCount ?? 1} />
            </div>
            {selectedRef &&
                <div className='d-flex justify-content-end'>
                    <div className='mx-2'>
                        <button
                        className='btn btn-outline-primary'
                        onClick={() => openModal(setShowWorkflowModal)}>
                            View Workflow Status
                        </button>
                    </div>
                    <div>
                        <button
                        className='btn btn-outline-primary'
                        onClick={() => openModal(setShowDocumentsModal)}>
                            Documents
                        </button>
                    </div>
                </div>
            }
        </>
    );
}

export default WorklistPage;