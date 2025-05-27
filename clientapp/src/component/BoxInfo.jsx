import './../../../Content/Logistique/logistique.css'; // Importer le CSS



const BoxInfo = ({ texte, info }) => {
    return (
        <div className="boxInfo">
            <div className="boxInfoGauche">
                {texte} 
            </div>
            <div className="boxInfoDroit">
                {info}
            </div>
        </div>
    );
};

export default BoxInfo;