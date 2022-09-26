import { IAttachment } from "common/interfaces/attachment.interface";
import { useEffect, useState } from "react";
import { addToWorkflowHistory, getAttachments } from "services/worklist-service";
import { Document, Page } from 'react-pdf'
import Pagination from "components/Pagination/Pagination";
import { FaCheck, FaCheckCircle, FaExclamationCircle, FaExclamationTriangle, FaQuestionCircle } from 'react-icons/fa';

import { IWorkflowHistory } from "common/interfaces/workflow-history.interface";
import moment from "moment";
import { useWorkflowStates } from "contexts/WorkflowStatesContext";
import { useWorklist } from "contexts/WorklistContext";
import DocumentStatus from "./DocumentStatus";
import { useUserDetails } from "contexts/SessionContext";

interface IDocumentsModal {
    selectedDocUid: string | undefined;
    resetSelectedDocUid: () => void;
}

const DocumentsModal : React.FC<IDocumentsModal> = ({selectedDocUid, resetSelectedDocUid}) => {
    const { selectedReferral: referral, handleReloadWorklist } = useWorklist();
    const { getStatus } = useWorkflowStates();
    const { userDetails } = useUserDetails();
    const [attachments, setAttachments] = useState<IAttachment[]>();
    const [selectedAttachment, setSelectedAttachment] = useState<IAttachment>();
    const [statusMode, setStatusMode] = useState<boolean>(false);

    const [isDocumentLoading, setIsDocumentLoading] = useState<boolean>(false);
    const [numPages, setNumPages] = useState<number>(0);
    const [pageNumber, setPageNumber] = useState<number>(1);
    const [pdfScale, setPdfScale] = useState<number>(1);

    useEffect(() => {
        fetchAttachments();
    }, []);

    useEffect(() => {
        if (selectedDocUid && !selectedAttachment) {
            const attach = attachments?.find(a => a.attachId === selectedDocUid);
            if (attach) {
                toggleSelect(attach);
                resetSelectedDocUid();
            }
        }
    }, [attachments]);

    const fetchAttachments = () => {
        if (referral === undefined) {
            return;
        }

        getAttachments(referral.refReqUniqueId).then((attachments: IAttachment[]) => {
            setAttachments(attachments);
        });
    };

    const onDocumentLoadSuccess = (params : any) => {
        setNumPages(params.numPages);
    };

    const onPageLoadSuccess = () => {
        setIsDocumentLoading(false);
    };

    const onPageChange = (newPage: number) => {
        setPageNumber(newPage);
        setIsDocumentLoading(true);
    };

    const toggleSelect = (select: IAttachment) => {
        const newSelectState = selectedAttachment?.attachId === select.attachId ? undefined : select;
        setIsDocumentLoading(true);
        setPageNumber(1);
        setSelectedAttachment(newSelectState);
        setStatusMode(false);
        setPdfScale(1);
    };

    const handleStatusUpdate = () => {
        setPageNumber(1);
        setSelectedAttachment(undefined);
        setStatusMode(false);
        fetchAttachments();
        setPdfScale(1);
        handleReloadWorklist();
    }

    const handleApproveDocument = () => {
        const update: IWorkflowHistory = {
            erstrnsUid: referral?.refReqUniqueId,
            doctrnsUid: selectedAttachment?.attachId,
            statusCode: 'D-QCEPR-SUCC',
            statusComments: '',
            recInsertedBy: userDetails?.userEmail
        };

        addToWorkflowHistory(update).then((response : IWorkflowHistory[]) => {
            setSelectedAttachment(undefined);
            handleStatusUpdate();
        });
    };

    const patient = referral?.patient;
    const age = moment().diff(patient?.patDob, 'years');

    return (
        <>
            
            <div className='row'>
                <div className='col-md-5'>
                    <div className='row mb-3'>
                        {patient &&
                        <>
                            <h5>Patient Details:</h5>
                            <div><b>Name:</b> {patient?.patGivenName}  {patient?.patFamilyName}</div>
                            <div><b>Sex:</b> {patient?.patSex}</div>
                            <div><b>Age:</b> {age}</div>
                            <div><b>DOB:</b> {moment(patient?.patDob).format('DD-MM-YYYY')}</div>
                        </>
                        }
                    </div>
                    <h5>Attached Documents:</h5>
                    <div className='table-responsive-md'>
                        <table className='table table-hover'>
                            <thead>
                                <tr>
                                    <th scope='col'>Document Ref</th>
                                    <th scope='col'>Filename</th>
                                    <th scope='col'>Status</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                {attachments && attachments.length > 0 && attachments.map((a : IAttachment) => {
                                    return (
                                        <tr
                                        key={a.attachId}
                                        className={`cursor-pointer ${a.attachId === selectedAttachment?.attachId && 'table-primary'}`}
                                        onClick={() => toggleSelect(a)}>
                                            <td>{a.attachId}</td>
                                            <td>{a.attachTitle}</td>
                                            <td className='align-middle'>
                                                <div className='d-flex justify-content-center'>
                                                {
                                                    a.wfsHistory?.statusCode === undefined ? <FaQuestionCircle /> :
                                                    getStatus(a.wfsHistory.statusCode)?.errorStatus ? <FaExclamationTriangle /> : <FaCheckCircle />
                                                }
                                                </div>
                                            </td>
                                        </tr>
                                    );
                                })
                                }
                            </tbody>
                        </table>
                    </div>
                </div>
                <div className='pdf-column col-md-7'>
                {selectedAttachment &&
                <>
                    <div className='row mb-3'>
                        <div className='d-flex justify-content-between align-middle'>
                            <button
                            className={`btn m-2 ${statusMode ? 'btn-warning' : 'btn-outline-warning'}`}
                            onClick={() => setStatusMode(!statusMode)}>
                                Flag Issue <FaExclamationCircle />
                            </button>
                            <button
                            className='btn btn-outline-success m-2'
                            onClick={handleApproveDocument}>
                                Approve <FaCheck />
                            </button>
                        </div>
                    </div>
                    {statusMode &&
                        <div className='row'>
                            <DocumentStatus docUid={selectedAttachment?.attachId} handleOnAddStatus={handleStatusUpdate} />
                        </div>
                    }
                    <div className='d-flex justify-content-between'>
                        <h4>Document Viewer:</h4>
                        <div className='d-flex flex-row align-items-center'>
                            <div>
                                <button
                                onClick={() => setPdfScale(pdfScale - 0.1)}
                                className='btn btn-outline-secondary mx-2'>
                                    -
                                </button>
                            </div>
                            <div className='mx-2'>{(pdfScale * 100).toFixed(0)}%</div>
                            <div>
                                <button
                                onClick={() => setPdfScale(pdfScale + 0.1)}
                                className='btn btn-outline-secondary'>
                                    +
                                </button>
                                </div>
                        </div>
                    </div>
                    <div className='pdf-container'>
                        <Document
                            file={selectedAttachment?.docDownloadUrl}
                            options={{
                                standardFontDataUrl: 'standard_fonts/',
                                workerSrc: '/pdf.worker.js'
                            }}
                            loading={''}
                            onLoadSuccess={onDocumentLoadSuccess}
                            >
                            <Page
                                pageNumber={pageNumber}
                                loading={''}
                                onLoadSuccess={onPageLoadSuccess}
                                scale={pdfScale}
                            />
                        </Document>
                    </div>
                    {
                    !isDocumentLoading && numPages > 1 &&
                        <div className='d-flex justify-content-center'>
                            <Pagination
                            pageCurrent={pageNumber}
                            handlePageChange={onPageChange}
                            pagesTotal={numPages}
                            showPageInput={numPages > 5}
                            />
                        </div>
                    }
                </>   
                }
                </div>
            </div>
            
        </>
    );
}

export default DocumentsModal;