import { IReferral, IReferralFilters, IReferralRequest, IReferralResult } from 'common/interfaces/referral.interface';
import DocumentsModal from 'components/DocumentsModal';
import Loading from 'components/Loading/Loading';
import Modal from 'components/Modal';
import Pagination from 'components/Pagination/Pagination';
import WorkflowStatusModal from 'components/WorkflowStatus/WorkflowStatusModal';
import moment from 'moment';
import React, { useEffect, useState } from 'react';
import { getWorklist } from 'services/worklist-service';
import { debounce } from 'ts-debounce';
import WorklistFilters from 'components/Worklist/WorklistFilters';
import { useWorkflowStates } from 'contexts/WorkflowStatesContext';
import WorklistContext from 'contexts/WorklistContext';

const WorklistPage : React.FC = () => {
    const { getStatusIcon } = useWorkflowStates();
    const [worklist, setWorklist] = useState<IReferralResult>();
    const [selectedRef, setSelectedRef] = useState<IReferral>();
    const [pageNumber, setPageNumber] = useState<number>(1);
    const [filters, setFilters] = useState<IReferralFilters>({});

    const [selectedDocUid, setSelectedDocUid] = useState<string|undefined>();

    const [showWorkflowModal, setShowWorkflowModal] = useState<boolean>(false);
    const [showDocumentsModal, setShowDocumentsModal] = useState<boolean>(false);

    const fetchWorklist = () => {
        const request : IReferralRequest = {
            pageNumber: pageNumber,
            filters: filters
        };

        getWorklist(request).then((response : IReferralResult | undefined) => {
            setWorklist(response);
        });
    };

    const fetchWorklistDebounce = debounce(async () => {
        fetchWorklist();
    }, 1000);

    useEffect(() => {
        fetchWorklistDebounce().then();
    }, [filters.consultant,
        filters.ersService]);

    useEffect(() => {
        fetchWorklist();
    }, [filters.investigationMode,
        filters.meditechPathway,
        filters.refReqSpecialty]);

    useEffect(() => {
        fetchWorklist();
    }, [pageNumber]);

    const openModal = (modalFunc : (show: boolean) => void) => {
        if (selectedRef) {
            modalFunc(true);
        }
    };

    const toggleSelect = (select: IReferral) => {
        const newSelectState = selectedRef?.refReqUniqueId === select.refReqUniqueId ? undefined : select;
        setSelectedRef(newSelectState);
    };

    const openDocument = (docUid: string | undefined) => {
        if (docUid) {
            setSelectedDocUid(docUid);
            setShowWorkflowModal(false);
            openModal(setShowDocumentsModal);
        }
    };

    const pagesTotal = worklist?.pageCount ?? 1;

    return (
        <WorklistContext.Provider value={{selectedReferral: selectedRef, handleReloadWorklist: () => fetchWorklist()}}>
            <h1>Worklist</h1>
            {selectedRef &&
            <>
                <Modal title={'Workflow Status'} show={showWorkflowModal} setShow={setShowWorkflowModal}>
                    <WorkflowStatusModal openDocument={openDocument} />
                </Modal>
                <Modal title={'Documents'} show={showDocumentsModal} setShow={setShowDocumentsModal}>
                    <DocumentsModal selectedDocUid={selectedDocUid} resetSelectedDocUid={() => setSelectedDocUid(undefined)} />
                </Modal>
            </>
            }
            <WorklistFilters filters={filters} setFilters={setFilters} />
            <div className='table-responsive-md'>
                <table className='table'>
                    <thead>
                        <tr>
                            <th scope='col'>UBRN</th>
                            {/*<th scope='col'>Hospital ID</th>*/}
                            <th scope='col'>NHS No.</th>
                            <th scope='col'>Name</th>
                            <th scope='col'>DOB</th>
                            <th scope='col'>Gender</th>
                            <th scope='col'>Meditech Speciality</th>
                            {/*
                            <th scope='col'>Referral Details</th>
                            <th scope='col'>Appointment Details</th>
                            */}
                            <th scope='col'>Referral Status</th>
                            <th scope='col'></th>
                        </tr>
                    </thead>
                    <tbody>
                        {worklist && worklist.results?.length > 0 && worklist.results.map((wl) => {
                            return (
                                <tr
                                key={wl.refReqUniqueId}
                                className={`cursor-pointer ${wl.refReqUniqueId === selectedRef?.refReqUniqueId && 'table-primary'}`}
                                onClick={() => toggleSelect(wl)}>
                                    <td>{wl.refReqUbrn}</td>
                                    {/*<td>{wl.hospitalId}</td>*/}
                                    <td>{wl.refReqNhsno}</td>
                                    <td>{wl.patient?.patGivenName} {wl.patient?.patFamilyName}</td>
                                    <td>{wl.patient?.patDob && moment(wl.patient.patDob).format('DD-MM-YYYY')}</td>
                                    <td>{wl.patient?.patSex}</td>
                                    <td>{wl.refReqSpecialty}</td>
                                    {/*
                                    <td>{wl.referralDetails}</td>
                                    <td>{wl.apptDetails}</td>
                                    */}
                                    <td>{wl.refReqStatus}</td>
                                    <td>
                                    {getStatusIcon(wl.wfsHistory?.statusCode)}
                                    </td>
                                </tr>
                            );
                        })
                        }
                    </tbody>
                </table>
            </div>
            <div className='d-flex justify-content-center'>
                {!worklist ? <Loading /> : worklist.results?.length === 0 && <p>No results found.</p>}
            </div>
            <div className='d-flex justify-content-center'>
                <Pagination
                pageCurrent={pageNumber}
                handlePageChange={setPageNumber}
                pagesTotal={pagesTotal}
                showPageInput={pagesTotal > 5}
                />
            </div>
            {selectedRef &&
                <div className='d-flex justify-content-end'>
                    <div className='mx-2'>
                        <button
                        className='btn btn-outline-primary'
                        onClick={() => openModal(setShowWorkflowModal)}>
                            Workflow Status
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
        </WorklistContext.Provider>
    );
}

export default WorklistPage;