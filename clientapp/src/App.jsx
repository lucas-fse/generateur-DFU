import React, { useEffect, useState } from 'react';
import { Routes, Route } from 'react-router-dom';
import Production from './Production/Production.jsx';  
import Securite from './Production/Securite.jsx';
import IndexOFOperateur from './Production/IndexOFOperateur.jsx'
import Logistique from './Logistique/Logistique.jsx'
import PbCommandes from './Logistique/PbCommandes.jsx'
import Reassort from './Logistique/Reassort.jsx'



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
            <Route path="/Logistique/Index" element={<Logistique model={model} />} />
            <Route path="/Logistique/PbCommandesFournisseur" element={<PbCommandes model={model} titre="problèmes commandes fournisseur"/>} />
            <Route path="/Logistique/PbReceptionsFournisseur" element={<PbCommandes model={model} titre="problèmes réceptions fournisseur"/>} />
            <Route path="/Logistique/PbLivraisonsClient" element={<PbCommandes model={model} titre="problèmes livraisons client" />} />
            <Route path="/Logistique/Reassort" element={<Reassort model={model} />}/>
        </Routes>
    );
};

export default App;

