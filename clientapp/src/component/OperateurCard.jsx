// clientapp/src/components/OperatorCard.jsx
import React from 'react';


const OperatorCard = ({ operator }) => {
    const lien = `/Production/gestionOf/${operator.ID}`;

    
    return (
        <div className="Operateur">
            <a href={lien} style={{ textDecoration: 'none', color: 'inherit' }}>
                <img src={operator.Photo} alt={`${operator.Nom} ${operator.Prenom}`} />
                <div id="ctn-id">
                    <p>{operator.Nom}</p>
                    <p>{operator.Prenom}</p>
                </div>
            </a>
        </div>
    );
};

export default OperatorCard;