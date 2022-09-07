import React from 'react';
import './Modal.scss';

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
            <div className='modal modal-xl' style={{'display':'block'}}>
                <div className='modal-dialog'>
                    <div className='modal-content'>
                        <div className='modal-header'>
                            <h5 className='modal-title'>{title}</h5>
                            <button type='button' className='btn-close' aria-label='Close' onClick={() => setShow(false)}></button>
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