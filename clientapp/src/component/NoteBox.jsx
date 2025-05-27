import './../../../Content/Logistique/logistique.css'; // Importer le CSS



const Notes = ({ initiales, date, message }) => {
    return (
        <div className="noteBox">
            <div className="noteLeft">
                <div className="noteInitials">{initiales}</div>
                <div className="noteDate">{date}</div>
            </div>
            <div className="noteContent">
                <div className="noteText">{message}</div>
            </div>
        </div>
    );
};

export default Notes;