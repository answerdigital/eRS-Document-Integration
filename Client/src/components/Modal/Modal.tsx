import IconButton from 'components/IconButton';
import React from 'react';
import { BsBackspace, BsXSquare, BsXSquareFill } from 'react-icons/bs';

interface IModalProps {
    title?: string;
    show: boolean;
    setShow: (show: boolean) => void;
    children: React.ReactNode;
    footer?: React.ReactNode;
}

const Modal: React.FC<IModalProps> = ({title, show, setShow, children, footer}) => {

    return (
        <>
        {show &&
            <div className='modal' style={{'display':'block'}}>
                <div className='modal-dialog modal-fullscreen'>
                    <div className='modal-content'>
                        <div className='modal-header'>
                            <h5 className='modal-title'>{title}</h5>
                            <IconButton icon={<BsXSquare/>} iconHover={<BsXSquareFill/>} onClick={() => setShow(false)} aria-label='Close' />
                        </div>
                        <div className='modal-body'>
                            {children}
                        </div>
                        {footer &&
                            <div className='modal-footer'>
                                {footer}
                            </div>
                        }
                    </div>
                </div>
            </div>
        }
        </>
    )
};

export default Modal;