import React from 'react';

const QCross = ({ CasesQcross}) => {
    const rows = [];


    // Ligne 1: Cases 1 � 7 (index 0 � 6)
    rows.push(
        <div className="Q-CrossRow" key="row1">
            <div className={`Q-CrossCase left top bottom right arrondi-hg bg-case cliquable ${CasesQcross[0]?.Couleurstring || ''}`}>1</div>
            {Array.from({ length: 4 }, (_, i) => i + 1).map((i) => (
                <div className={`Q-CrossCase top bottom right bg-case cliquable ${CasesQcross[i]?.Couleurstring || ''}`} key={`case${i}`}>
                    {i + 1}
                </div>
            ))}
            <div className={`Q-CrossCase top bottom right bg-case cliquable ${CasesQcross[5]?.Couleurstring || ''}`}>6</div>
            <div className={`Q-CrossCase top bottom right arrondi-hd bg-case cliquable ${CasesQcross[6]?.Couleurstring || ''}`}>7</div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
        </div>
    );

    // Ligne 2: Cases 8 et 9 (index 7 et 8)
    rows.push(
        <div className="Q-CrossRow" key="row2">
            <div className={`Q-CrossCase left bottom right bg-case cliquable ${CasesQcross[7]?.Couleurstring || ''}`}>8</div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase right"></div>
            <div className={`Q-CrossCase right bg-case cliquable ${CasesQcross[8]?.Couleurstring || ''}`}>9</div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
        </div>
    );

    // Ligne 3: Cases 10 et 11 (index 9 et 10)
    rows.push(
        <div className="Q-CrossRow" key="row3">
            <div className={`Q-CrossCase left right bottom bg-case cliquable ${CasesQcross[9]?.Couleurstring || ''}`}>10</div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase right"></div>
            <div className={`Q-CrossCase top right bg-case cliquable ${CasesQcross[10]?.Couleurstring || ''}`}>11</div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
        </div>
    );

    // Ligne 4: Cases 12 et 13 (index 11 et 12)
    rows.push(
        <div className="Q-CrossRow" key="row4">
            <div className={`Q-CrossCase left right bottom bg-case cliquable ${CasesQcross[11]?.Couleurstring || ''}`}>12</div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase right"></div>
            <div className={`Q-CrossCase top right bg-case cliquable ${CasesQcross[12]?.Couleurstring || ''}`}>13</div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
        </div>
    );

    // Ligne 5: Cases 14 et 15 (index 13 et 14)
    rows.push(
        <div className="Q-CrossRow" key="row5">
            <div className={`Q-CrossCase left right bottom bg-case cliquable ${CasesQcross[13]?.Couleurstring || ''}`}>14</div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase right"></div>
            <div className={`Q-CrossCase top right bg-case cliquable ${CasesQcross[14]?.Couleurstring || ''}`}>15</div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
        </div>
    );

    // Ligne 6: Cases 16 et 17 (index 15 et 16)
    rows.push(
        <div className="Q-CrossRow" key="row6">
            <div className={`Q-CrossCase left right bottom bg-case cliquable ${CasesQcross[15]?.Couleurstring || ''}`}>16</div>
            <div className="Q-CrossCase bottom"></div>
            <div className="Q-CrossCase bottom"></div>
            <div className="Q-CrossCase bottom"></div>
            <div className="Q-CrossCase bottom"></div>
            <div className="Q-CrossCase right bottom"></div>
            <div className={`Q-CrossCase top bottom right bg-case cliquable ${CasesQcross[16]?.Couleurstring || ''}`}>17</div>
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
        </div>
    );

    // Ligne 7: Cases 18 � 24 (index 17 � 23)
    rows.push(
        <div className="Q-CrossRow" key="row7">
            <div className={`Q-CrossCase left right bg-case cliquable ${CasesQcross[17]?.Couleurstring || ''}`}>18</div>
            <div className={`Q-CrossCase bg-case cliquable ${CasesQcross[18]?.Couleurstring || ''}`}>19</div>
            <div className={`Q-CrossCase left right bg-case cliquable ${CasesQcross[19]?.Couleurstring || ''}`}>20</div>
            {Array.from({ length: 4 }, (_, i) => i + 20).map((i) => (
                <div className={`Q-CrossCase right bg-case cliquable ${CasesQcross[i]?.Couleurstring || ''}`} key={`case${i}`}>
                    {i + 1}
                </div>
            ))}
            <div className="Q-CrossCase"></div>
            <div className="Q-CrossCase"></div>
        </div>
    );

    // Ligne 8: Cases 25 � 31 (index 24 � 30)
    rows.push(
        <div className="Q-CrossRow" key="row8">
            <div className="Q-CrossCase top"></div>
            <div className="Q-CrossCase top"></div>
            <div className={`Q-CrossCase left top right bottom arrondi-bg bg-case cliquable ${CasesQcross[24]?.Couleurstring || ''}`}>25</div>
            {Array.from({ length: 5 }, (_, i) => i + 25).map((i) => (
                <div
                    className={`Q-CrossCase right top bottom bg-case ${CasesQcross[i]?.Visible ? 'cliquable' : 'jour-inexistant'} ${CasesQcross[i]?.Visible ? CasesQcross[i]?.Couleurstring : ''}`}
                    key={`case${i}`}
                >
                    {CasesQcross[i]?.Visible ? i + 1 : ''}
                </div>
            ))}
            <div
                className={`Q-CrossCase top right bottom arrondi-bd arrondi-hd-bd bg-case ${CasesQcross[30]?.Visible ? 'cliquable' : 'jour-inexistant'} ${CasesQcross[30]?.Visible ? CasesQcross[30]?.Couleurstring : ''}`}
            >
                {CasesQcross[30]?.Visible ? 31 : ''}
            </div>
        </div>
    );
    

    return (

        <>
            {rows}
        </>
    );
};

export default QCross;
