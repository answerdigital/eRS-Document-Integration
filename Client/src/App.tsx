import React from 'react';
import { RouterPaths } from 'utility/RouterPaths';
import HomePage from 'pages/HomePage';
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import AuditLogPage from 'pages/AuditLogPage';
import Navbar from 'components/Navbar/Navbar';

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

const App : React.FC = () => {

  return (
    <>
      <BrowserRouter>
      <Navbar>
        <Routes>
            <Route path={RouterPaths.HomePath} element={<HomePage />}/>
            <Route path={RouterPaths.AuditsPath} element={<AuditLogPage />} />
            <Route path="*" element={<Navigate to={RouterPaths.HomePath} />} />
          </Routes>
      </Navbar>
          
      </BrowserRouter>
    </>
  );

}

export default App;
