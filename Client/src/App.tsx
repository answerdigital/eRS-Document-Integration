import React from 'react';
import { RouterPaths } from 'utility/RouterPaths';
import HomePage from 'pages/HomePage';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import AuditLogPage from 'pages/AuditLogPage';
import Navbar from 'components/Navbar/Navbar';
import WorklistPage from 'pages/WorklistPage';
import WorkflowStatesContextProvider from 'contexts/WorkflowStatesContext';

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import 'react-toastify/dist/ReactToastify.css';


const App : React.FC = () => {

  return (
    <>
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
              <Route path={RouterPaths.HomePath} element={<HomePage />}/>
              <Route path={RouterPaths.WorklistPath} element={<WorklistPage />}/>
              <Route path={RouterPaths.AuditsPath} element={<AuditLogPage />} />
              <Route path={'*'} element={<Navigate to={RouterPaths.HomePath} />} />
            </Routes>
          </WorkflowStatesContextProvider>
        </Navbar>
      </BrowserRouter>
    </>
  );

}

export default App;
