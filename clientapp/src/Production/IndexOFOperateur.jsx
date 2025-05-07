// clientapp/src/components/Production/IndexOFOperateur.jsx
import React, { useState, useEffect } from 'react';
import OperatorCard from './../component/OperateurCard'; // Ajuste le chemin si nécessaire
import './../../../Content/IndexOFOperateur.css'; // Ajuste le chemin en fonction de ta structure

const IndexOFOperateur = ({ model }) => {
    // Déstructurer le modèle avec des valeurs par défaut
    const {
        ListOperateur = [], // Équivalent à DataOperateurs dans le modèle Razor
        CouleurCaseParJour = {},
        NbJourDuMois = 31,
        // Ajoute d'autres propriétés si elles sont nécessaires pour cet écran
    } = model || {};

    const [searchTerm, setSearchTerm] = useState('');
    const [filteredOperators, setFilteredOperators] = useState(ListOperateur);

    useEffect(() => {
        // Filtrer les opérateurs en fonction du terme de recherche
        const filtered = ListOperateur.filter(operator =>
            `${operator.Nom} ${operator.Prenom}`.toLowerCase().includes(searchTerm.toLowerCase())
        );
        setFilteredOperators(filtered);
    }, [searchTerm, ListOperateur]);

    // Calcul du nombre de cases vides pour compléter la grille
    const itemsPerRow = 5;
    const operatorCount = filteredOperators.length;
    const remainder = operatorCount % itemsPerRow;
    const emptySlots = remainder > 0 ? itemsPerRow - remainder : 0;

    return (
        <div>
            <img id="FondEcran" src="/image/swirl.png" alt="" />
            <div id="fondTransparent"></div>

            <div id="top">
                <h2 id="title">FabriZEN</h2>
                <div id="searchOF">
                    <label htmlFor="searched-OF">Référence de l'OF:</label>
                    <input
                        type="search"
                        id="searched-OF"
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        onFocus={() => document.getElementById("searched-OF").focus()}
                    />
                    <button type="button" id="submitButton">
                        <img src="/image/search.png" alt="Search" width="20" />
                    </button>
                </div>
            </div>
            <div id="BarreSeparation"></div>
            <div id="ListOperateur">
                {filteredOperators.map((operator, index) => (
                    <OperatorCard operator={operator} />
                ))}
                {Array.from({ length: emptySlots }).map((_, index) => (
                    <div key={`empty-${index}`} style={{ width: '300px' }}></div>
                ))}
            </div>
        </div>
    );
};

export default IndexOFOperateur;