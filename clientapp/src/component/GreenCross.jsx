import React from 'react';

const GreenCross = ({ NbJourDuMois, CouleurCaseParJour }) => {
    const dynamicStyles = [];
    const currentDay = new Date().getDate();
    for (let i = 1; i <= currentDay; i++) {
        const backgroundColor = CouleurCaseParJour[i.toString()] || 'lightgrey';
        dynamicStyles.push(`
            #Case_${i} {
                background: ${backgroundColor};
                color: white;
            }
        `);
    }

    const days = NbJourDuMois;
    const rows = [];

    if (days >= 28) {
        rows.push(
            // Ligne 1: 1 2 3 4 5
            <div className="greenCrossRow" key="row1">
                <div className="greenCrossCase topleft" id="Case_1">1</div>
                <div className="greenCrossCase" id="Case_2">2</div>
                <div className="greenCrossCase" id="Case_3">3</div>
                <div className="greenCrossCase" id="Case_4">4</div>
                <div className="greenCrossCase topright" id="Case_5">5</div>
            </div>,
            // Ligne 2: 6 7 8 9 10
            <div className="greenCrossRow" key="row2">

                <div className="greenCrossCase" id="Case_6">6</div>
                <div className="greenCrossCase" id="Case_7">7</div>
                <div className="greenCrossCase" id="Case_8">8</div>
                <div className="greenCrossCase" id="Case_9">9</div>
                <div className="greenCrossCase bottomright" id="Case_10">10</div>
            </div>,
            // Ligne 3: 11 (doit être aligné avec la 4e colonne des lignes 2 et 4)
            <div className="greenCrossRow" key="row3">
                <div className="greenCrossCase" id="Case_11">11</div>
                <div className="hiddenCase"></div>
                <div className="hiddenCase"></div>
                <div className="hiddenCase"></div>
                <div className="hiddenCase"></div>

            </div>,
            // Ligne 4: 12 13 14 15 16
            <div className="greenCrossRow" key="row4">

                <div className="greenCrossCase " id="Case_12">12</div>
                <div className="greenCrossCase" id="Case_13">13</div>
                <div className="greenCrossCase" id="Case_14">14</div>
                <div className="greenCrossCase" id="Case_15">15</div>
                <div className="greenCrossCase topright" id="Case_16">16</div>

            </div>,
            // Ligne 5: 17 18 19 20 21
            <div className="greenCrossRow" key="row5">
                <div className="greenCrossCase bottomleft" id="Case_17">17</div>
                <div className="greenCrossCase" id="Case_18">18</div>
                <div className="greenCrossCase" id="Case_19">19</div>
                <div className="greenCrossCase" id="Case_20">20</div>
                <div className="greenCrossCase" id="Case_21">21</div>
            </div>,
            // Ligne 6: 22 (doit être aligné avec la 6e colonne des lignes 4 et 5)
            <div className="greenCrossRow" key="row6">
                <div className="hiddenCase"></div>
                <div className="hiddenCase"></div>
                <div className="hiddenCase"></div>
                <div className="hiddenCase"></div>
                <div className="greenCrossCase" id="Case_22">22</div>
            </div>,
            // Ligne 7: 23 24 25 26 27 [28 29 30 31]
            <div className="greenCrossRow" key="row7">
                <div className="greenCrossCase" id="Case_23">23</div>
                <div className="greenCrossCase" id="Case_24">24</div>
                <div className="greenCrossCase" id="Case_25">25</div>
                <div className="greenCrossCase" id="Case_26">26</div>
                <div className="greenCrossCase" id="Case_27">27</div>
            </div>,
            //Ligne 8
            <div className="greenCrossRow" key="row8">
                {[28, 29, 30, 31].map((day, index) => (
                    <div
                        key={`Case_${day}`}
                        className="greenCrossCase"
                        id={`Case_${day}`}
                    >
                        {day <= days ? day : ''}
                    </div>
                ))}
                <div className="greenCrossCase bottomright"></div>
            </div>
        );
    }

    return (
        <>
            <style>{dynamicStyles.join('\n')}</style>
            {rows}
        </>
    );
};

export default GreenCross;