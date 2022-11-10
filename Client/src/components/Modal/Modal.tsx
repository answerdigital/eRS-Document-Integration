import React from 'react';
import { BsBackspace } from 'react-icons/bs';

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
                            <div className='d-flex flex-row align-items-center'>
                                <div className='me-3'>
                                    <button type='button' className='btn-transparent' aria-label='Close' onClick={() => setShow(false)}><BsBackspace/></button>
                                </div>
                                <div>
                                    <h5 className='modal-title'>{title}</h5>
                                </div>
                            </div>

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