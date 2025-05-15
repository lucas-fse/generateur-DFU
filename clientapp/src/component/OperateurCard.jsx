// clientapp/src/components/OperatorCard.jsx
import React from 'react';


const OperatorCard = ({ operator }) => {
    const lien = `/Production/gestionOf/${operator.ID}`;

    
    return (
        <div className="Operateur" style={{ width: '300px' }}>
            <a href={lien} style={{ textDecoration: 'none', color: 'inherit' }}>
                <img src={operator.Photo} alt={`${operator.Nom} ${operator.Prenom}`} />
                <p>{operator.Nom} {operator.Prenom}</p>
            </a>
        </div>
    );
};

export default OperatorCard;