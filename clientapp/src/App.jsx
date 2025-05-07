import React, { useEffect, useState } from 'react';
import { Routes, Route } from 'react-router-dom';
import Production from './Production/Production.jsx';  
import Securite from './Production/Securite.jsx';
import IndexOFOperateur from './Production/IndexOFOperateur.jsx'


const App = () => {
    const [model, setModel] = useState(null);

    useEffect(() => {
        // Récupérer les données injectées depuis Razor
        setModel(window.reactData);  // Assurez-vous que window.reactData est bien disponible
    }, []);

    // Si les données ne sont pas encore chargées, afficher un message de chargement
    if (!model) return <p>Chargement...</p>;

    return (
        <Routes>
            {/* Définir des routes pour chaque page */}
            <Route path="/Production/Production" element={<Production model={model} />} />
            <Route path="/Production/Securite" element={<Securite model={model} />} />
            <Route path="/Production/indexofoperateur" element={<IndexOFOperateur model={model} />} />
        </Routes>
    );
};

export default App;
