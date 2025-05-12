import React, { useEffect, useState } from 'react';
import './../../../Content/Accident.css';
import sablier from './../../../image/sablier.png';
import coupe from './../../../image/coupe.png';
import pouceEnHaut from './../../../image/pouce_en_haut.png';
import BoxCounter from './../component/BoxCounter'; // Existing component
import GreenCross from './../component/GreenCross'; // Existing component
import Button from './../component/Button';

const Accident = ({ model }) => {
    // Destructure model with defaults
    const {
        CouleurCaseParJour = {},
        ListAccidentDuMois = [],
        NbJourSansAccident = 0,
        RecordSansAccident = 0,
        NbHumeurPositivestring = '- -%',
        CrossColor = '',
        TotalAccidentParType = {},
        AccidentsParMois = {},
        listAccidentsParMois = '',
        Day = 1,
    } = model || {};

    // State for smiley toggle
   

    // State for accident details
    const [accidentDetails, setAccidentDetails] = useState([]);

    // Parse listAccidentsParMois
    let parsedAccidentsParMois;
    try {
        const cleanedJson = listAccidentsParMois.replace(/(\r\n|\n|\r)/gm, '');
        // Remove <p> tags and extract JSON
        const jsonString = cleanedJson.replace(/^<p>|<\/p>$/g, '');
        parsedAccidentsParMois = JSON.parse(jsonString);
    } catch (e) {
        console.error('Error parsing listAccidentsParMois:', e);
        parsedAccidentsParMois = {};
    }



    // Load external scripts
    useEffect(() => {
        

        const scripts = [
            '/Scripts/jquery-3.6.0.slim.min.js',
            '/Scripts/ReloadTimer.js',
            '/Scripts/ViewImage.js',
            '/Scripts/Del_Enter_Touch.js',
            '/Scripts/Accident.js',
        ];

        scripts.forEach((src) => {
            const script = document.createElement('script');
            script.src = src;
            script.async = false;
            document.body.appendChild(script);
        });

       
        // Image upload logic
        const inputElement = document.getElementById('image-upload');
        if (inputElement) {
            const handleImageUpload = (event) => {
                const file = event.target.files[0];
                if (file) {
                    const reader = new FileReader();
                    reader.onload = (e) => {
                        const imgElement = document.createElement('img');
                        imgElement.src = e.target.result;
                        document.body.appendChild(imgElement);
                    };
                    reader.readAsDataURL(file);
                }
            };
            inputElement.addEventListener('change', handleImageUpload);
            return () => inputElement.removeEventListener('change', handleImageUpload);
        }

        return () => {
            scripts.forEach((src) => {
                const script = document.querySelector(`script[src="${src}"]`);
                if (script) script.remove();
            });
        };
    }, []);

    const DaysInCurrentMonth = () => {
        const today = new Date();
        const year = today.getFullYear();
        const month = today.getMonth() + 1; // +1 car getMonth() retourne 0 � 11

        const daysInMonth = new Date(year, month, 0).getDate();

        return daysInMonth;
    };

    // Smiley toggle functions


    // AfficherDetail function
    const afficherDetail = (type, mois) => {
        const details = parsedAccidentsParMois[mois]?.ListAccident[type] || [];
        const detailElements = details.map((accident, i) => (
            <div className="DetailA" key={`detail-${type}-${mois}-${i}`}>
                <div className="DivBandeau"></div>
                <div className="TextDateDetail">
                    <p>
                        Une déclaration a été faite le {accident.DateString} par {accident.Victime} de type: {accident.TypeString}
                    </p>
                </div>
                <div className="TextGrasDetail">
                    <label>Observations: </label>
                </div>
                <div className="TextDetail">
                    <p>{accident.Description}</p>
                </div>
                <div className="TextGrasDetail">
                    <label>Action immediate: </label>
                </div>
                <div className="TextDetail">
                    <p>{accident.Description2}</p>
                </div>
                <div className="TextGrasDetail">
                    <label>Image de l'accident: </label>
                </div>
                <div className="ImageDetail">
                    {accident.FullUrlImage && accident.FullUrlImage.length > 0 ? (
                        <img src={`/ImageAccident/${accident.FullUrlImage.split('\\')[1]}`} className="image_box" alt="Accident" />
                    ) : (
                        <p>Aucune photo de l'accident prise.</p>
                    )}
                </div>
            </div>
        ));
        setAccidentDetails(detailElements);
    };

    // toLiteral function
    const toLiteral = (str) => {
        const dict = { '\b': 'b', '\t': 't', '\n': 'n', '\v': 'v', '\f': 'f', '\r': 'r' };
        return str.replace(/([\\'"\b\t\n\v\f\r])/g, ($0, $1) => '\\' + (dict[$1] || $1));
    };

    // Placeholder for OuvrirPopUp
    const ouvrirPopUp = (niveau) => {
        window.location.href = '/Production/DeclAccident?id=' + niveau;
        // Implement modal logic if needed (e.g., Material-UI Modal)
    };

    return (
        <>
            <div id="fondTransparent"></div>
            <div id="AccidentDiv">
                <div id="DivCases">
                    <BoxCounter
                        image={sablier}
                        label="Nombre de Jours sans accidents"
                        valeur={NbJourSansAccident}
                    />
                    <BoxCounter
                        image={coupe}
                        label="Record de Jours sans accidents"
                        valeur={RecordSansAccident}
                    />
                    <BoxCounter
                        image={pouceEnHaut}
                        label="Moyenne des notes positives"
                        valeur={NbHumeurPositivestring}
                    />
                    <div className="boxCounter" id="SereniteLien">
                        <a href="/Production/serenite">Page de sérénité</a>
                    </div>
                </div>
                <div id="greenCross">
                    <GreenCross NbJourDuMois={DaysInCurrentMonth()} CouleurCaseParJour={CouleurCaseParJour} />
                </div>
                  
                <div id="Smiley">
                    <Button label="Signaler un incident" onClick={'/Production/DeclAccident?id=undefined'}/>
                </div>
                <div id="Total_Pyramide">
                    <div id="TotalAccident">
                        <div id="Total7"><p>Year</p></div>
                        {Array.from({ length: 6 }, (_, i) => 6 - i).map((niveau) => (
                            <div id={`Total${niveau}`} key={`Total${niveau}`}>
                                {TotalAccidentParType[niveau] !== 0 && <p>{TotalAccidentParType[niveau]}</p>}
                            </div>
                        ))}
                    </div>
                    <div id="Pyramide">
                        <button className="niveau" id="Niveau6" type="button" onClick={() => ouvrirPopUp('6')}>
                            <p>Accident Grave</p>
                        </button>
                        <button className="niveau" id="Niveau5" type="button" onClick={() => ouvrirPopUp('5')}>
                            <p>Accidents declarés</p>
                        </button>
                        <button className="niveau" id="Niveau4" type="button" onClick={() => ouvrirPopUp('4')}>
                            Soins benins
                        </button>
                        <button className="niveau" id="Niveau3" type="button" onClick={() => ouvrirPopUp('3')}>
                            Presque accident
                        </button>
                        <button className="niveau" id="Niveau2" type="button" onClick={() => ouvrirPopUp('2')}>
                            Situations dangereuses
                        </button>
                        <button className="niveau" id="Niveau1" type="button" onClick={() => ouvrirPopUp('1')}>
                            Actes dangeureux
                        </button>
                    </div>
                </div>
                <div id="Tableau">

                    <div id="ligne7" className="ligne">
                        {['J', 'F', 'M', 'A', 'M', 'J', 'J', 'A', 'S', 'O', 'N', 'D'].map((month) => (
                            <div key={month}><p>{month}</p></div>
                        ))}
                    </div>
                    {Array.from({ length: 6 }, (_, i) => 6 - i).map((niveau) => (
                        <div id={`ligne${niveau}`} className="ligne" key={`ligne${niveau}`}>
                            {Array.from({ length: 12 }, (_, i) => i + 1).map((mois) => (
                                <button
                                    className="buttonTableau"
                                    type="button"
                                    onClick={() => afficherDetail(niveau, mois)}
                                    key={`CaseTableau${niveau}_${mois}`}
                                >
                                    {AccidentsParMois[mois]?.ListAccident[niveau]?.length > 0 ? AccidentsParMois[mois].ListAccident[niveau].length  : ''}
                                </button>
                            ))}
                        </div>
                    ))}
                </div>
                <div id="DetailAccident">{accidentDetails}</div>
            </div>
        </>
    );
};

export default Accident;