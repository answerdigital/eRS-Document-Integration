import React from 'react';
import './App.scss';
import { RouterPaths } from 'utility/RouterPaths';
import HomePage from 'pages/HomePage';
import { BrowserRouter, Route, Routes } from 'react-router-dom';


function App() {

  return (
    <>
      <BrowserRouter>
          <Routes>
            <Route path={RouterPaths.HomePath} element={<HomePage />}></Route>
          </Routes>
      </BrowserRouter>
    </>
  );

}

export default App;
