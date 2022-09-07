import { IAttachment } from "common/interfaces/attachment.interface";
import { useEffect, useState } from "react";
import { addToWorkflowHistory, getAttachments } from "services/worklist-service";
import { Document, Page } from 'react-pdf'
import Pagination from "components/Pagination/Pagination";
import { FaCheck, FaCheckCircle, FaExclamation, FaExclamationCircle, FaExclamationTriangle, FaQuestionCircle } from 'react-icons/fa';

import './DocumentsModal.scss';
import { IWorkflowHistory } from "common/interfaces/workflow-history.interface";
import DocumentStatus from "./DocumentStatus";

interface DocumentsModalProps {
    refUid: string;
}

const DocumentsModal : React.FC<DocumentsModalProps> = ({refUid}) => {
    const [attachments, setAttachments] = useState<IAttachment[]>();
    const [selectedAttachment, setSelectedAttachment] = useState<IAttachment>();
    const [isDocumentLoading, setIsDocumentLoading] = useState<boolean>(false);
    const [numPages, setNumPages] = useState<number>(0);
    const [pageNumber, setPageNumber] = useState<number>(1);

    const [statusMode, setStatusMode] = useState<boolean>(false);

    useEffect(() => {
        fetchAttachments();
    }, []);

    const fetchAttachments = () => {
        getAttachments(refUid).then((response: IAttachment[]) => {
            setAttachments(response);
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
        const newSelectState = selectedAttachment === select ? undefined : select;
        setIsDocumentLoading(true);
        setPageNumber(1);
        setSelectedAttachment(newSelectState);
        setStatusMode(false);
    };

    const handleOnAddStatus = () => {
        setPageNumber(1);
        setSelectedAttachment(undefined);
        setStatusMode(false);
        fetchAttachments();
    }

    const handleApproveDocument = () => {
        const update: IWorkflowHistory = {
            doctrnsUid: selectedAttachment?.attachId,
            statusCode: 'ERSDL-SUCC',
            statusComments: '',
            recInsertedBy: 'User'
        };

        addToWorkflowHistory(update).then((response : IWorkflowHistory[]) => {
            setSelectedAttachment(undefined);
            fetchAttachments();
        });
    };

    return (
        <>
            <div className='row'>
                <div className='input-group mb-3'>
                    <input className='form-control' placeholder={'Patient Name, Gender & Age, DOB'} />
                    <input className='form-control' placeholder={'Referral Context'} />
                </div>
            </div>
            <div className='row'>
                <div className='col-md-4'>
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
                                        className={`cursor-pointer ${a === selectedAttachment && 'table-primary'}`}
                                        onClick={() => toggleSelect(a)}>
                                            <td>{a.attachId}</td>
                                            <td>{a.attachTitle}</td>
                                            <td className='align-middle'>
                                                <div className='d-flex justify-content-center'>
                                                {
                                                    a.wfsHistory?.statusCode === undefined ? <FaQuestionCircle /> :
                                                    a.wfsHistory?.statusCode === 'ERSDL-SUCC' ? <FaCheckCircle /> : <FaExclamationTriangle />
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
                <div className='pdf-column col-md-8'>
                {selectedAttachment &&
                <>
                    <div className='row mb-3'>
                        <div className='d-flex justify-content-between align-middle'>
                            <button
                            className='btn btn-outline-warning m-2'
                            onClick={() => setStatusMode(!statusMode)}>
                                Flag Issue <FaExclamationCircle />
                            </button>
                            <p>{selectedAttachment?.wfsHistory?.statusCode}</p>
                            <button
                            className='btn btn-outline-success m-2'
                            onClick={handleApproveDocument}>
                                Approve <FaCheck />
                            </button>
                        </div>
                    </div>
                    {statusMode &&
                        <div className='row'>
                            <DocumentStatus docUid={selectedAttachment?.attachId} handleOnAddStatus={handleOnAddStatus} />
                        </div>
                    }
                    <div className='pdf-container mb-3'>
                        <Document
                            file={{url: selectedAttachment?.docDownloadUrl}}
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
                            />
                        </Document>
                    </div>
                    {
                    !isDocumentLoading && numPages > 1 &&
                        <div className='d-flex justify-content-center'>
                            <Pagination pageCurrent={pageNumber} handlePageChange={onPageChange} pagesTotal={numPages} />
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