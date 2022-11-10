import { IAttachment } from "common/interfaces/attachment.interface";
import { useEffect, useState } from "react";
import { addToWorkflowHistory, getAttachments } from "services/worklist-service";
import { Document, Page } from 'react-pdf'
import Pagination from "components/Pagination/Pagination";
import { FaCheck, FaExclamationCircle, FaDownload, FaPlus, FaMinus } from 'react-icons/fa';

import { IWorkflowHistory } from "common/interfaces/workflow-history.interface";
import moment from "moment";
import { useWorkflowStates } from "contexts/WorkflowStatesContext";
import { useWorklist } from "contexts/WorklistContext";
import DocumentStatus from "./DocumentStatus";
import { useUserDetails } from "contexts/SessionContext";

interface IDocumentsModalProps {
    selectedDocUid: string | undefined;
    resetSelectedDocUid: () => void;
}

const DocumentsModal : React.FC<IDocumentsModalProps> = ({selectedDocUid, resetSelectedDocUid}) => {
    const { selectedReferral: referral, handleReloadWorklist } = useWorklist();
    const { getStatus, getStatusIcon } = useWorkflowStates();
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

    const handleDownloadAttachment = (attach: IAttachment) => {
        const url = attach?.docDownloadUrl;
        const fileName = attach?.attachTitle;
        if (!url || !fileName) {
            return false;
        }

        fetch(url)
        .then((response) => response.blob())
        .then((blob) => {
            const url = window.URL.createObjectURL(new Blob([blob]));
            const downloadLink = document.createElement('a');
            downloadLink.href = url;
            downloadLink.setAttribute('download', fileName);
            downloadLink.click();
            downloadLink.remove();
        });
    };

    const handleDownloadAllAttachments = () => {

    };

    const patient = referral?.patient;
    const age = moment().diff(patient?.patDob, 'years');

    const statusCode = selectedAttachment?.wfsHistory?.statusCode;
    const status = getStatus(statusCode);
    const docHasError = status?.errorStatus;

    return (
        <>
            <div className='row'>
                <div className='col-md-5'>
                    <div className='row mb-4'>
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
                    <div className='d-flex justify-content-between'>
                        <div><h5>Attached Documents:</h5></div>
                        <div className='d-flex flex-row align-items-center'>
                            <div className='me-2'>Download All</div>
                            <button
                            className='btn btn-outline-success'
                            onClick={() => handleDownloadAllAttachments()}>
                                <FaDownload/>
                            </button>
                        </div>
                    </div>
                    <div className='table-responsive-md'>
                        <table className='table table-hover'>
                            <thead>
                                <tr>
                                    <th scope='col'>Document Ref</th>
                                    <th scope='col'>Filename</th>
                                    <th scope='col'>Status</th>
                                    <th></th>
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
                                            <td className='align-middle'>{a.attachId}</td>
                                            <td className='align-middle'>{a.attachTitle}</td>
                                            <td className='align-middle'>
                                                <div className='d-flex justify-content-center'>
                                                    { getStatusIcon(a.wfsHistory?.statusCode) }
                                                </div>
                                            </td>
                                            <td className='align-middle'>
                                                <div className='d-flex justify-content-center'>
                                                    <button
                                                    className='btn btn-outline-success'
                                                    onClick={() => handleDownloadAttachment(a)}>
                                                        <FaDownload/>
                                                    </button>
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
                        <div className='d-flex justify-content-between align-items-center'>
                            <div className='align-middle'>
                                <button
                                className={`btn m-2 ${statusMode ? 'btn-warning' : 'btn-outline-warning'}`}
                                onClick={() => setStatusMode(!statusMode)}>
                                    {docHasError ? 'Amend Comments' : 'Flag Issue'} <FaExclamationCircle />
                                </button>
                            </div>
                            <div>
                                {status?.wfsmDisplayValue}
                            </div>
                            <div className='align-self-middle'>
                                {statusCode !== 'D-QCEPR-SUCC' &&
                                <button
                                className='btn btn-outline-success m-2'
                                onClick={handleApproveDocument}>
                                    {docHasError ? 'Flag Issue as Resolved' : 'Approve'} <FaCheck />
                                </button>
                                }
                            </div>
                        </div>
                    </div>
                    {statusMode &&
                        <div className='row'>
                            <DocumentStatus
                            docUid={selectedAttachment.attachId}
                            handleOnAddStatus={handleStatusUpdate}
                            comments={selectedAttachment.wfsHistory?.statusComments}
                            />
                        </div>
                    }
                    <div className='d-flex justify-content-between'>
                        <h4>Document Viewer:</h4>
                        <div className='d-flex flex-row align-items-center'>
                            <div>
                                <button
                                onClick={() => setPdfScale(pdfScale - 0.1)}
                                className='btn btn-outline-secondary mx-2'>
                                    <FaMinus />
                                </button>
                            </div>
                            <div className='mx-2'>{(pdfScale * 100).toFixed(0)}%</div>
                            <div>
                                <button
                                onClick={() => setPdfScale(pdfScale + 0.1)}
                                className='btn btn-outline-secondary'>
                                    <FaPlus />
                                </button>
                                </div>
                        </div>
                    </div>
                    <div className='d-flex justify-content-center pdf-container'>
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