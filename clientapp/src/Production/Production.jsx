import React, { useEffect } from 'react';
import './../../../Content/homeProd.css'; // Importer le CSS
import logo from '../assets/logo.png'; // Chemin vers le logo
import sablier from './../../../image/sablier.png'; // Chemins vers les images
import coupe from './../../../image/coupe.png';
import pouceVert from './../../../image/pouceVert.png';
import pouceEnHaut from './../../../image/pouce_en_haut.png';
import conceptionPlanification from './../../../image/conception-planification.png';
import otd from './../../../image/OTD.png';
import idea from './../../../image/idea.png';
import aic from './../../../image/AIC.png';
import serenite from './../../../image/serenite.png';
import Button from './../component/Button';
import ButtonOutline from './../component/ButtonOutline';
import BoxCounter from './../component/BoxCounter';
import GreenCross from './../component/GreenCross';
import QCross from './../component/QCross';

const Production = ({ model }) => {
    // Déstructurer avec des valeurs par défaut
    const {
        CouleurCaseParJour = {},
        ListOperateur = [],
        NbJourDuMois = 31,
        NbJourSansAccident = 0,
        RecordSansAccident = 0,
        CasesQcross = [],
        DataFrcParService = {},
        OTDHebdomadaire = 0,
        OTRHebdomadaire = 0,
        NbAmeliorationDernierMois = 0,
        NbAmeliorationProduction = 0,
        NbAmeliorationProductionsecurite = 0,
        NbHumeurPositiveString = '0%',
        PDFAAfficher = '',
    } = model || {};



    // Charger les scripts externes
    useEffect(() => {
        const loadScripts = async () => {
            const moveScript = document.createElement('script');
            moveScript.src = '/Scripts/move.js';
            moveScript.async = false;
            moveScript.onload = () => {
                window.dispatchEvent(new Event('load')); // <= Force déclenchement
            };
            document.body.appendChild(moveScript);

            const otherScripts = [
                '/Scripts/chart.js',
                '/Scripts/compteursGreenCross.js',
                '/Scripts/chartNbFrcParService.js',
                '/Scripts/jquery-3.4.1.js',
                '/Scripts/CountDown.js',
                '/Scripts/CountDown2.js',
                '/Scripts/ReloadTimer.js',
            ];

            for (const src of otherScripts) {
                const script = document.createElement('script');
                script.src = src;
                script.async = false;
                document.body.appendChild(script);
            }
        };

        loadScripts();

        return () => {
            const allScripts = document.querySelectorAll('script[src]');
            allScripts.forEach((script) => {
                if (script.src.includes('/Scripts/')) {
                    script.remove();
                }
            });
        };
    }, [ListOperateur.length]);



 
    return (
        <>

            <main id="contenuPage">
                <div id="infoProd">
                    <div id="bandeau">
                        <div>
                            <img src={logo} alt="logo" width="350" />
                        </div>
                                              
                    </div>
                    <div id="topInfos">
                        <h1 id="mainTitle">Accueil ProductZEN</h1>
                    </div>
                    <hr />
                    <div id="securite-qualite">
                        <div id="securite">
                            <div id="green-cross">
                                <h3>Green Cross</h3>
                                <div id="greenCross">
                                    <GreenCross NbJourDuMois={NbJourDuMois} CouleurCaseParJour={CouleurCaseParJour} />
                                </div>

                            <h4 id="securiteTitle">Securite</h4>
                        </div>
                        <div id="infos-securite">
                                <div id="DivCases">

                                    <BoxCounter image={sablier} label="Nombre de Jours sans accidents" valeur={NbJourSansAccident} />

                                    <BoxCounter image={coupe} label="Record de Jours sans accidents" valeur={RecordSansAccident} />

                                </div>


                            <div class="enSavoirPlusContainer centered">
                                    <Button label="Accéder" onClick={'/Production/Securite'} />
                            </div>
             
                            </div>

                        </div>

                            <div id="qualite">
                                <div id="Q-cross">
                                    <h3 id="Q-CrossTitle">Q-Cross</h3>
                                <div id="Q-Cross1">
                                    <QCross CasesQcross={CasesQcross}/>
                                    </div>
                                <h4 id="qualiteTitle">Qualité</h4>
                                </div>
                            <div id="infos-qualite">
                                <div id="chartDataFrcParService1" legend="false">
                                    <ul>
                                        {Object.entries(DataFrcParService).map(([key, value]) => (
                                            <li key={key}>
                                                {key}:{value.backgroundColor}:{value.borderColor}:
                                                {value.Data.slice(0, 12).join(':')}:{value.Type}
                                            </li>
                                        ))}
                                    </ul>
                                </div>
                                <div id="chartNbFrcParService-container">
                                    <canvas id="chartNbFrcParService"></canvas>
                                </div>
                                <div className="enSavoirPlusContainer centered">
                                 <Button label="Accéder" onClick={'/Production/Qualite'} />
                                </div>
                            </div>
                        </div>

                      
                    </div>

                    <hr></hr>
                    <div id="production-idee">
                        <div id="Plannif">
                            <div id="Plannif-left">
                                <img src={conceptionPlanification} id="Plannif-pic" width="150" alt="img-plannification" />
                                <h4 id="Plannif-leftTitle">Plannification</h4>
                            </div>
                            <div id="Plannif-right">
                                <ButtonOutline label="Affectation aux Pôles" onClick={'/Production/AffectationOperateurs'} />
                        </div>
                    </div>
                    <div id="idee">
                            <div id="idee-left">
                                <img src={otd} id="otd-pic" />
                            <h4 id="idee-leftTitle">OTD</h4>
                        </div>
                        <div id="idee-right">
                                <div id="DivCases">

                                    <BoxCounter image={sablier} label="OTD Hebdomadaire" valeur={OTDHebdomadaire + " %"} />

                                </div>


                            <div class="enSavoirPlusContainer centered">
                                    <Button label="Accéder" onClick={'/Production/KPIProd'} />
                                </div>
                            </div>
                    </div>
                </div>
      
            <hr />

                    <div id="otd-AIC">
                        <div id="idee">
                            <div id="production-left">
                                <img src={idea} id="meeting"/>
                                    <h4 id="prof-left-title">Amélioration</h4>
                            </div>

                        <div id="production-right">

                            <div id="DivCases">

                                <BoxCounter image={sablier} label="Nb améliorations derniers mois" valeur={NbAmeliorationDernierMois} />
                                <BoxCounter image={coupe} label="Nb d'améliorations production sur l'année" valeur={NbAmeliorationProduction} />
                                                      
                                <BoxCounter image={pouceEnHaut} label="Nb d'améliorations de sécurité" valeur={NbAmeliorationProductionsecurite} />                             

                                                
                                <div className="enSavoirPlusContainer centered">
                                    <Button label="Accéder" onClick={'/Production/Amelioration'} />
                                    </div>
                                </div>

                        </div>
                    </div>
                    <div id="AIC">
                            <div id="AIC-left">
                                <img src={aic} id="AIC-pic" width="150" alt="Alternate Text" />
                            <h4 id="AIC-leftTitle">AIC2</h4>
                        </div>
                            <div id="AIC-right">
                                <ButtonOutline label="AIC 2 - Monodirectionnel" onClick={`/Production/AIC2?id=1`} />
                                <ButtonOutline label="AIC 2 - Bidirectionnel" onClick={`/Production/AIC2?id=2`} />
                                <ButtonOutline label="QRQC" onClick={`/Production/DataQRQC`} />

                                </div>
                            </div>
        </div >
            <hr />
            <div id="production">
                        <div id="production-left">
                            <img src={serenite} id="serenite-pic"/>
                    <h4 id="prof-left-title">Sérénité</h4>
                </div>

                        <div id="production-right">
                            <BoxCounter image={pouceVert} label="Moyenne de notes positives " valeur={NbHumeurPositiveString +" %"} />        
                   
                    <div className="enSavoirPlusContainer centered">
                        <Button label="Accéder" onClick={'/Production/serenite'} />
                    </div>
                    </div>
                </div >
                       
        </div >
    <div id="newsletter">
                 <embed id="pdfNewsletter" src={PDFAAfficher}/>
       
    </div>
            </main>
        </>
    );
};

export default Production;