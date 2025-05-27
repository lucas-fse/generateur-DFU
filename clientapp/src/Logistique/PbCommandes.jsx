import React, { useEffect, useState } from 'react';
import './../../../Content/Logistique/logistiqueAleas.css'; // Importer le CSS
import './../../../Content/QCross.css';
import Button from '../component/Button';
import QCross from '../component/QCross';
import ProblemeBox from '../component/ProblemeBox';
import FormModalProbleme from '../component/FormModalProbleme';



const PbCommandes = ({ model, titre }) => {
    // D�structurer avec des valeurs par d�faut

    const {
        ListPbCommandesFournisseur = {},
        CasesQcross = [],
        DerniersProblemes = []
    } = model || {};

    const pbList = Object.values(DerniersProblemes);

    const QCrossCases = [
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 2,
            "Couleurstring": "vert"
        },
        {
            "Visible": true,
            "Couleur": 1,
            "Couleurstring": "gris"
        },
        {
            "Visible": true,
            "Couleur": 1,
            "Couleurstring": "gris"
        },
        {
            "Visible": true,
            "Couleur": 1,
            "Couleurstring": "gris"
        },
        {
            "Visible": true,
            "Couleur": 1,
            "Couleurstring": "gris"
        },
        {
            "Visible": true,
            "Couleur": 1,
            "Couleurstring": "gris"
        },
        {
            "Visible": true,
            "Couleur": 1,
            "Couleurstring": "gris"
        },
        {
            "Visible": true,
            "Couleur": 1,
            "Couleurstring": "gris"
        },
        {
            "Visible": true,
            "Couleur": 1,
            "Couleurstring": "gris"
        },
        {
            "Visible": true,
            "Couleur": 1,
            "Couleurstring": "gris"
        },
        {
            "Visible": true,
            "Couleur": 1,
            "Couleurstring": "gris"
        },
        {
            "Visible": true,
            "Couleur": 1,
            "Couleurstring": "rouge"
        }
    ]

    const problemes =
        [
            { numeroCommande : 'AC129513', date: '19/05/2025', natureProbleme: 'Prix', probleme:'erreur', resolution:'false' },
            { numeroCommande: 'AC129513', date: '19/05/2025', natureProbleme: 'Prix', probleme: 'erreur, réglé', resolution: 'true' },
            { numeroCommande: 'AC129513', date: '19/05/2025', natureProbleme: 'Prix', probleme: 'erreur', resolution: 'false' },
            { numeroCommande: 'AC129513', date: '19/05/2025', natureProbleme: 'Prix', probleme: 'erreur', resolution: 'false' },
            { numeroCommande: 'AC129513', date: '19/05/2025', natureProbleme: 'Prix', probleme: 'erreur', resolution: 'false' },
            { numeroCommande: 'AC129513', date: '19/05/2025', natureProbleme: 'Prix', probleme: 'erreur', resolution: 'false' },
            { numeroCommande: 'AC129513', date: '19/05/2025', natureProbleme: 'Prix', probleme: 'erreur', resolution: 'false' },
            { numeroCommande: 'AC129513', date: '19/05/2025', natureProbleme: 'Prix', probleme: 'erreur', resolution: 'false' },
            { numeroCommande: 'AC129513', date: '19/05/2025', natureProbleme: 'Prix', probleme: 'erreur', resolution: 'false' }
        ]

    const [isPopupOpen, setIsPopupOpen] = useState(false);
    console.log(pbList);
    console.log(CasesQcross);
    console.log(DerniersProblemes);
    return (
        <>
            <main>
                <div className="title">
                    <h2>Imprévus</h2>
                    <h3>· {titre}</h3>
                </div>
                <hr/>
                <div className="btnImprevu">
                    <button className ="btn btn-orange" onClick={() => setIsPopupOpen(true)}>Nouvel imprévu</button>
                    <FormModalProbleme isOpen={isPopupOpen} onClose={() => setIsPopupOpen(false)} />
                </div>
                <div className="main">
                    <div className="mainGauche">
                        <QCross CasesQcross={CasesQcross} />
                    </div>
                    <div className="mainDroit">
                        <div className="enTeteDroit">
                            <h2>Derniers imprévus</h2>
                            <div className="btnRechercher">
                                <Button label="Rechercher" onClick="#"/>
                            </div>
                        </div>
                        <div className="imprevusDroit">
                            {pbList.map((pb, index) => (
                                <ProblemeBox
                                    probleme = {pb}
                                />
                            ))}
                        </div>
                    </div>
                </div>
            </main>
        </>
    );
};

export default PbCommandes;