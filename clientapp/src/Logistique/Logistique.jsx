import React, { useEffect } from 'react';
import './../../../Content/Logistique/logistique.css'; // Importer le CSS
import WhiteBox from './../component/WhiteBox';
import ButtonWhiteBox from './../component/ButtonWhiteBox';
import BoxInfo from './../component/BoxInfo';
import Button from './../component/Button';
import Imprevus from './../assets/imprevus.png';
import Livraison from './../assets/livraison.png';
import Reassort from './../assets/reassort.png';
import Stockage from './../assets/stockage.png';
import Notes from './../component/Notes';


const Logistique = ({ model }) => {
    // D�structurer avec des valeurs par d�faut



    const messages = 
        [
        { initiales: 'AB', date: '19/05/2025', message: 'Stock mis à jour' },
            { initiales: 'CD', date: '18/05/2025', message: 'Commande réceptionnée partiellement' },
         { initiales: 'CD', date: '18/05/2025', message: 'Commande réceptionnée partiellement' },
          { initiales: 'CD', date: '18/05/2025', message: 'Commande réceptionnée partiellement' },
           { initiales: 'CD', date: '18/05/2025', message: 'Commande réceptionnée partiellement' },
            { initiales: 'CD', date: '18/05/2025', message: 'Commande réceptionnée partiellement' },
            { initiales: 'CD', date: '18/05/2025', message: 'Commande réceptionnée partiellement' },
              { initiales: 'CD', date: '18/05/2025', message: 'Commande réceptionnée partiellement' },
               { initiales: 'CD', date: '18/05/2025', message: 'Commande réceptionnée partiellement' }
        ]
 




    return (
        <>
        <main>
                <h1>LogisZEN</h1>
                <hr />
                <div className="main">
                <div className="mainGauche">
                    <WhiteBox titre="Imprévus" sousTitre="Suivi des aléas" image={Imprevus}>
                        <ButtonWhiteBox texte="Problèmes commande fournisseur" onClick="/Logistique/PbCommandesFournisseur"></ButtonWhiteBox>
                        <ButtonWhiteBox texte="Problèmes réception fournisseurs" onClick="/Logistique/PbReceptionsFournisseur"></ButtonWhiteBox>
                        <ButtonWhiteBox texte="Problèmes livraison client" onClick="/Logistique/PbLivraisonsClient"></ButtonWhiteBox>
                    </WhiteBox>
                    <WhiteBox titre="Réassort" sousTitre="Demandes de suivi" image={Reassort}>
                        <div className="whiteBoxRight">
                            <div className="whiteInfo">
                                <BoxInfo texte="Nouvelles demandes" info="13"></BoxInfo>
                                <BoxInfo texte="En attente pour réception" info="13"></BoxInfo>
                            </div>
                            <div className="whiteBtn">
                                <Button label="Accéder" onClick="/Logistique/Reassort"></Button>
                            </div>
                        </div>
                    </WhiteBox>
                    <WhiteBox titre="Stockage" sousTitre="Mouvements de stock" image={Stockage}>
                        <div className="whiteBoxRight">
                            <div className="whiteInfo">
                                <BoxInfo texte="Dernier inventaire" info="13j"></BoxInfo>
                                <BoxInfo texte="Mouvements du mois" info="130"></BoxInfo>
                            </div>
                            <div className="whiteBtn">
                                <Button label="Accéder" onClick="#"></Button>
                            </div>
                        </div>
                    </WhiteBox>
                    <WhiteBox titre="Réception" sousTitre="Suivi des livraisons" image={Livraison}>
                        <div className="whiteBoxRight">
                            <div className="whiteInfo">
                                <BoxInfo texte="Livraisons attendues aujourd'hui" info="13"></BoxInfo>
                                <BoxInfo texte="Livraisons réceptionnées ce mois" info="130"></BoxInfo>
                            </div>
                            <div className="whiteBtn">
                                <Button label="Accéder" onClick="#"></Button>
                            </div>
                        </div>
                    </WhiteBox>
                </div>
                <div className="mainDroit">
                    <Notes messages={messages }></Notes>
                    </div>
                </div>
        </main>
        </>
    );
};

export default Logistique;