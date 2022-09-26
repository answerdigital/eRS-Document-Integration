import React, { useEffect } from 'react';
import { RouterPaths } from 'utility/RouterPaths';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import AuditLogPage from 'pages/AuditLogPage';
import Navbar from 'components/Navbar';
import WorklistPage from 'pages/WorklistPage';
import WorkflowStatesContextProvider from 'contexts/WorkflowStatesContext';
import LoginPage from 'pages/LoginPage';
import axios from 'axios';
import AuthenticatedRoutes from 'components/AuthenticatedRoutes';

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import 'react-toastify/dist/ReactToastify.css';
import SessionContextProvider from 'contexts/SessionContext';

const App : React.FC = () => {

  const setAuthToken = (token: string) => {
    if (token) {
        axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
    }
    else {
      delete axios.defaults.headers.common["Authorization"];
    }
  };

  useEffect(() => {
    const jwt = localStorage.getItem("jwt");
    if (jwt) {
      setAuthToken(jwt);
    }
  }, []);

  return (
    <>
      <BrowserRouter>
        <SessionContextProvider>
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
                <Route path='/' element={<AuthenticatedRoutes/>}>
                  <Route path={RouterPaths.WorklistPath} element={<WorklistPage />}/>
                  <Route path={RouterPaths.AuditsPath} element={<AuditLogPage />} />
                  <Route path={'/'} element={<Navigate to={RouterPaths.WorklistPath} />} />
                  <Route path={'*'} element={<Navigate to={RouterPaths.WorklistPath} />} />
                </Route>
                <Route path={RouterPaths.LoginPath} element={<LoginPage />} />
                <Route path={'*'} element={<Navigate to={RouterPaths.LoginPath} />} />
              </Routes>
            </WorkflowStatesContextProvider>
          </Navbar>
        </SessionContextProvider>
      </BrowserRouter>
    </>
  );

}

export default App;
