import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom'; // Import du BrowserRouter
import App from './App'; // Ton composant principal

const rootElement = document.getElementById('root');
if (rootElement) {
    ReactDOM.createRoot(rootElement).render(
        <BrowserRouter>
            <App />
        </BrowserRouter>
    );
}
