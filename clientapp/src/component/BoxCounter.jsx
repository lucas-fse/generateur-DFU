import React from 'react';



const BoxCounter = ({ image, label, valeur }) => {
    return (
        <div className="boxCounter">
            <div className="thumbnailCounter">
                <img src={image} className="imgIndicateur" />
            </div>
            <div className="counterContent">
                <h3>{label}</h3>
                <p id="cpt1" initvalue={valeur}><span>{valeur}</span></p>
            </div>
        </div>
    );
};

export default BoxCounter;