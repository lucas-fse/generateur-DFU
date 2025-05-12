import React, { useEffect, useState } from 'react';
import { Routes, Route } from 'react-router-dom';
import Production from './Production/Production.jsx';  
import Securite from './Production/Securite.jsx';
import IndexOFOperateur from './Production/IndexOFOperateur.jsx'


const App = () => {
    const [model, setModel] = useState(null);

    useEffect(() => {
        // R�cup�rer les donn�es inject�es depuis Razor
        setModel(window.reactData);  // Assurez-vous que window.reactData est bien disponible
    }, []);

    // Si les donn�es ne sont pas encore charg�es, afficher un message de chargement
    if (!model) return <p>Chargement...</p>;

    return (
        <Routes>
            {/* D�finir des routes pour chaque page */}
            <Route path="/Production/Production" element={<Production model={model} />} />
            <Route path="/Production/Securite" element={<Securite model={model} />} />
            <Route path="/Production/indexofoperateur" element={<IndexOFOperateur model={model} />} />
        </Routes>
    );
};

export default App;
