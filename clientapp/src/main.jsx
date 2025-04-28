import React from 'react';
import { createRoot } from 'react-dom/client'; // attention à bien utiliser 'react-dom/client'
import Button from './component/Button.jsx'; // ou ton composant principal


const rootElement = document.getElementById('root'); // ou "contenuPage" si tu montes dans <main id="contenuPage">
if (rootElement) {
    createRoot(rootElement).render(<Button label="Accéder" />);
}