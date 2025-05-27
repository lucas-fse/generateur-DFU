import './../../../Content/Logistique/logistique.css'; // Importer le CSS
import flecheRedirection from './../assets/fleche_redirection.png';


const redirection = (acces) => {
    window.location.href = acces;
}


const ButtonWhiteBox = ({ texte, onClick }) => {
    return (
        <div className="btnWhite" onClick={() => redirection(onClick)}>
            <div className="btnWhiteGauche">
                {texte} 
            </div>
            <div className="btnWhiteDroit">
                <img src={flecheRedirection}></img>
            </div>
        </div>
    );
};

export default ButtonWhiteBox;