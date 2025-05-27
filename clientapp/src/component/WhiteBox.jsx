import './../../../Content/Logistique/logistique.css'; // Importer le CSS

const WhiteBox = ({ titre, sousTitre, image, children }) => {
    return (
        <div className="whiteBox">
            <div className="wGauche">
                <h2>{titre}</h2>
                <h3>{sousTitre}</h3>
                <img src={image} alt={titre}/>
            </div>
            <div className="wDroit">
                {children}
            </div>
        </div>
    );
};

export default WhiteBox;