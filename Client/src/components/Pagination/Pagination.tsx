import React, { useState } from 'react';

interface PaginationProps {
    pageCurrent: number;
    handlePageChange: (page: number) => void;
    pagesTotal: number;
    showPageInput?: boolean;
}

const Pagination : React.FC<PaginationProps> = ({pageCurrent, handlePageChange, pagesTotal, showPageInput}) => {
    const [inputPage, setInputPage] = useState<string>(pageCurrent.toString() ?? "1");

    const handleInputPage = () => {
        const pageInt = parseInt(inputPage);
        if (pageInt) {
            changePage(pageInt);
        }
    };

    const pageInBounds = (page: number): boolean => {
        return (page > 0 && page <= pagesTotal);
    }

    const inputPageInBounds = (): boolean => {
        const pageInt = parseInt(inputPage);
        if (pageInt) {
            return pageInBounds(pageInt);
        }
        return false;
    };

    const changePage = (page: number) => {
        if (pageInBounds(page)) {
            handlePageChange(page);
        }
    };

    const pagesToShow = 5;
    let visiblePageNumbers: number[];
    if (pagesTotal > pagesToShow + 1) {
        let firstSectionStartPage = pageCurrent < pagesToShow - 1 ? 0 : pageCurrent - 3;
        if (firstSectionStartPage > pagesTotal - pagesToShow) {
            firstSectionStartPage = pagesTotal - pagesToShow;
        }
        visiblePageNumbers = Array.from({length: pagesToShow}, (x, i) => i + firstSectionStartPage);
    }
    else {
        visiblePageNumbers = Array.from({length: pagesTotal}, (x, i) => i);
    }

    const isInputPageInBounds = inputPageInBounds();
    const isFirstPage = pageCurrent === 1;
    const isLastPage = pageCurrent === pagesTotal;

    return (
        <div>
            <ul className='pagination d-flex justify-content-center'>
                <li className='page-item'>
                    <button className={`page-link ${isFirstPage && 'disabled'}`} aria-label='Previous' disabled={isFirstPage} onClick={() => changePage(1)}>
                        <span aria-hidden='true'>&laquo;</span>
                    </button>
                </li>
                <li className='page-item'>
                    <button className={`page-link ${isFirstPage && 'disabled'}`} aria-label='Previous' disabled={isFirstPage} onClick={() => changePage(pageCurrent - 1)}>
                        <span aria-hidden='true'>&lsaquo;</span>
                    </button>
                </li>
                {visiblePageNumbers.map((i) => {
                    return (
                        <li key={i} className='page-item'>
                            <button
                            className={`page-link ${pageCurrent === i + 1 ? 'active' : ''}`}
                            onClick={() => changePage(i + 1)}>
                                {i + 1}
                            </button>
                        </li>
                    );
                })}
                <li className='page-item'>
                    <button className={`page-link ${isLastPage && 'disabled'}`} aria-label='Next' onClick={() => changePage(pageCurrent + 1)}>
                        <span aria-hidden='true'>&rsaquo;</span>
                    </button>
                </li>
                <li className='page-item'>
                    <button className={`page-link ${isLastPage && 'disabled'}`} aria-label='End' onClick={() => changePage(pagesTotal)}>
                        <span aria-hidden='true'>&raquo;</span>
                    </button>
                </li>
            </ul>

            {showPageInput &&
            <>
                <div className='d-flex justify-content-center mb-1'>
                    <div className='col-6'>
                        <div className='input-group'>
                            <input
                            type='number'
                            className='form-control'
                            value={inputPage}
                            onChange={(e) => setInputPage(e.target.value)}
                            />
                            <button
                            className={`btn ${isInputPageInBounds ? 'btn-outline-primary' : 'btn-outline-secondary'} `}
                            disabled={!isInputPageInBounds}
                            onClick={() => handleInputPage()}>
                                Go to
                            </button>
                        </div>
                    </div>
                </div>
                <div className='d-flex justify-content-center'>
                    Pages: {pagesTotal}
                </div>
            </>
        
            }
        </div>
    );
}

export default Pagination;