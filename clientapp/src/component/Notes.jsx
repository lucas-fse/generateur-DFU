import React, { useEffect, useState, useRef } from 'react';
import './../../../Content/Logistique/logistique.css'; // Importer le CSS
import NoteBox from './../component/NoteBox';
import Envoyer from './../assets/envoyer.png';


const Notes = ({ messages }) => {

    const [nouveauTexte, setNouveauTexte] = useState('');

    const messagesEndRef = useRef(null);


    // Scroll vers le bas après chaque mise à jour de messages
    useEffect(() => {
        if (messagesEndRef.current) {
            messagesEndRef.current.scrollTop = messagesEndRef.current.scrollHeight;
        }
    }, [messages]); // Se déclenche à chaque changement de messages

    const envoyerMessage = () => {
        console.log(nouveauTexte);

        setNouveauTexte('');
    }

    return (
        <div className="notesBox">
            <div className=" notesHeader">
                <h2>Notes</h2>
                <h3>Informations pratiques</h3>
            </div>
            <div className="notesMessages" ref={messagesEndRef}>
                {messages.map((msg, index) => (
                <NoteBox
                    initiales={msg.initiales}
                    date={msg.date}
                    message={msg.message}
                    />
                ))}
            </div>
            <div className="nouvelleNote">
                <input className="nouveauMessage" id="nouveauMessage" placeholder="Ecrire un message" value={nouveauTexte} onChange={(e) => setNouveauTexte(e.target.value) }/>
                <img src={Envoyer} alt="Envoyer" onClick={envoyerMessage} style={{cursor : 'pointer'} }></img>
            </div>
        </div>
    );
};

export default Notes;