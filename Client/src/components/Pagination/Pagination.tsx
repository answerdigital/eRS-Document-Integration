import React from 'react';

interface PaginationProps {
    pageCurrent: number;
    handlePageChange: (page: number) => void;
    pagesTotal: number;
}

const Pagination : React.FC<PaginationProps> = ({pageCurrent, handlePageChange, pagesTotal}) => {
    
    const changePage = (page: number) => {
        if (page > 0 && page <= pagesTotal) {
            handlePageChange(page);
        }
    };

    const visiblePageNumbers = Array.from({length: pagesTotal}, (x, i) => i);

    return (
        <>
        <nav aria-label='Page navigation example'>
            <ul className='pagination'>
                <li className='page-item'>
                <button className='page-link' aria-label='Previous' onClick={() => changePage(pageCurrent + 1)}>
                    <span aria-hidden='true'>&laquo;</span>
                </button>
                </li>
                {visiblePageNumbers.map((i) => {
                    return (
                        <li className='page-item'>
                            <button className={`page-link ${pageCurrent === i + 1 ? 'active' : ''}`}>{i + 1}</button>
                        </li>
                    );
                })}
                <li className='page-item'>
                <button className='page-link' aria-label='Next' onClick={() => changePage(pageCurrent + 1)}>
                    <span aria-hidden='true'>&raquo;</span>
                </button>
                </li>
            </ul>
            </nav>
        </>
    );
}

export default Pagination;