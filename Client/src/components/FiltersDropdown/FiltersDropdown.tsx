import React, { useState } from 'react';

interface FiltersDropdownProps {
    children: React.ReactNode;
}

const FiltersDropdown : React.FC<FiltersDropdownProps> = ({children}) => {
    const [show, setShow] = useState<boolean>(false);

    return (
        <div className='d-flex flex-column justify-content-center align-items-center'>
            <div>
                <button className='btn dropdown-toggle mb-3' onClick={() => setShow(!show)}>Filters</button>
            </div>
            <div className={`dropdown ${show && 'show'}`}>
                {children}
            </div>
        </div>
    );
}

export default FiltersDropdown;