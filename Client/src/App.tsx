import React from 'react';
import { RouterPaths } from 'utility/RouterPaths';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import AuditLogPage from 'pages/AuditLogPage';
import Navbar from 'components/Navbar';
import WorklistPage from 'pages/WorklistPage';
import WorkflowStatesContextProvider from 'contexts/WorkflowStatesContext';

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import 'react-toastify/dist/ReactToastify.css';
import { MsalAuthenticationTemplate, MsalProvider } from '@azure/msal-react';
import { InteractionType, IPublicClientApplication } from '@azure/msal-browser';

type IAppProps = {
  pca: IPublicClientApplication
};

const App : React.FC<IAppProps> = ({pca}) => {

  return (
    <>
    <MsalProvider instance={pca}>
      <MsalAuthenticationTemplate interactionType={InteractionType.Redirect}>
      <BrowserRouter>
          <Navbar>
            <WorkflowStatesContextProvider>
              <ToastContainer
                position='top-right'
                autoClose={5000}
                hideProgressBar={false}
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
              />
                <Routes>
                    <Route path={RouterPaths.WorklistPath} element={<WorklistPage/>}/>
                    <Route path={RouterPaths.AuditsPath} element={<AuditLogPage/>} />
                    <Route path={'/'} element={<Navigate to={RouterPaths.WorklistPath} />} />
                    <Route path={'*'} element={<Navigate to={RouterPaths.WorklistPath} />} />
                </Routes>
            </WorkflowStatesContextProvider>
          </Navbar>
        </BrowserRouter>
        </MsalAuthenticationTemplate>
      </MsalProvider>
    </>
  );

}

export default App;
