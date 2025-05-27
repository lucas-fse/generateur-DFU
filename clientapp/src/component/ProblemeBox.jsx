import React, { useEffect, useState, useRef } from 'react';
import './../../../Content/Logistique/logistiqueAleas.css'; // Importer le CSS
import NoteBox from './NoteBox';
import Envoyer from './../assets/envoyer.png';


const ProblemeBox = ({ probleme }) => {



    return (
        <div className="problemeBox">
            <h4>{probleme.NaturePB} - {probleme.Probleme}</h4>
            <p>{probleme.Date}</p>

            <div className="badgeBox">
            {probleme.Resolution ===true ? (
                <div className="badge badge-resolu">
                    RESOLU
                </div>
            ) : (
                <div className="badge badge-nonresolu">NON RESOLU</div>
                )}
            </div>
        </div>
    );
};

export default ProblemeBox;