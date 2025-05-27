import React, { useState, useEffect } from 'react';

const initialFormState = {
    SaisieDate: '',
    TypeProbleme: '',
    NatureProbleme: '',
    Probleme: '',
    NumCommande: '',
    Resolu: '',
    Service: ''
};

const FormModalProbleme = ({ isOpen, onClose }) => {
    const [formData, setFormData] = useState(initialFormState);

    // ✅ Réinitialise le formulaire à chaque ouverture du modal
    useEffect(() => {
        if (isOpen) {
            setFormData(initialFormState);
        }
    }, [isOpen]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch('/Logistique/FormulaireDeclarationProbleme', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(formData)
            });

            if (response.ok) {
                window.showCustomAlert("success", "Problème enregistré avec succès");
                onClose(); // on ferme le modal après la fermeture du popup
                window.location.href = response.url;
            } else {
               window.showCustomAlert("error", "Erreur lors de l'envoie du formulaire");
            }
        } catch (error) {
             window.showCustomAlert("error", "Une erreur s'est produite.")
        }
    };


    if (!isOpen) return null;

    return (
        <div id="popupSignalement" className={isOpen ? "show" : ""}>
            <div className="popup-content">
                <div className="frm-title">Saisie d'un problème</div>
                <button className="close-btn" onClick={onClose}>×</button>
                <form className="frm-box" onSubmit={handleSubmit}>
                    <div className="frm-row">
                        <div className="frm-group">
                            <label>Date</label>
                            <input
                                type="date"
                                className="frm-input"
                                id="SaisieDate"
                                value={formData.SaisieDate}
                                name="SaisieDate"
                                onChange={handleChange}
                            />
                        </div>
                    </div>

                    <div className="frm-row">
                        <div className="frm-group">
                            <label>Type de problème</label>
                            <select
                                className="frm-select"
                                id="TypeProbleme"
                                name="TypeProbleme"
                                onChange={handleChange}
                                value={formData.TypeProbleme}
                            >
                                <option value="">-- Sélectionner --</option>
                                <option value="CommandeFournisseur">Problème de commande fournisseur</option>
                                <option value="LivraisonFournisseur">Problème de livraison fournisseur</option>
                                <option value="LivraisonClient">Problème de livraison client</option>
                            </select>
                        </div>
                    </div>

                    {formData.TypeProbleme === 'LivraisonFournisseur' && (
                        <>
                            <div className="frm-row">
                                <div className="frm-group">
                                    <label>Numéro de commande associé</label>
                                    <input
                                        type="text"
                                        className="frm-input"
                                        id="NumCommande"
                                        value={formData.NumCommande}
                                        name="NumCommande"
                                        onChange={handleChange}
                                    />
                                </div>
                            </div>
                            <div className="frm-row">
                                <div className="frm-group">
                                    <label>Nature du problème</label>
                                    <select
                                        className="frm-select"
                                        id="NatureProbleme"
                                        name="NatureProbleme"
                                        onChange={handleChange}
                                        value={formData.NatureProbleme}
                                    >
                                        <option value="">-- Sélectionner --</option>
                                        <option value="Erreur bon de livraison fournisseur">Erreur bon de livraison fournisseur</option>
                                        <option value="Problème saisie interne">Problème saisie interne</option>
                                        <option value="Autre">Autre</option>
                                    </select>
                                </div>
                            </div>
                        </>
                    )}

                    {formData.TypeProbleme === 'CommandeFournisseur' && (
                        <div className="frm-row">
                            <div className="frm-group">
                                <label>Nature du problème</label>
                                <select
                                    className="frm-select"
                                    id="NatureProbleme"
                                    name="NatureProbleme"
                                    onChange={handleChange}
                                    value={formData.NatureProbleme}
                                >
                                    <option value="">-- Sélectionner --</option>
                                    <option value="Prix">Prix</option>
                                    <option value="Données techniques">Données techniques</option>
                                    <option value="Autre">Autre</option>
                                </select>
                            </div>
                        </div>
                    )}

                    {formData.TypeProbleme === 'LivraisonClient' && (
                        <>
                            <div className="frm-row">
                                <div className="frm-group">
                                    <label>Numéro de commande associé</label>
                                    <input
                                        type="text"
                                        className="frm-input"
                                        id="NumCommande"
                                        value={formData.NumCommande}
                                        name="NumCommande"
                                        onChange={handleChange}
                                    />
                                </div>
                            </div>
                            <div className="frm-row">
                                <div className="frm-group">
                                    <label>Service</label>
                                    <select
                                        className="frm-select"
                                        id="Service"
                                        name="Service"
                                        onChange={handleChange}
                                        value={formData.Service}
                                    >
                                        <option value="">-- Sélectionner --</option>
                                        <option value="Logistique">Logistique</option>
                                        <option value="Informatique">Informatique</option>
                                        <option value="TCS">TCS</option>
                                        <option value="Autre">Autre</option>
                                    </select>
                                </div>
                            </div>
                        </>
                    )}

                    {formData.TypeProbleme === 'LivraisonClient' && formData.Service !== '' && (
                        <div className="frm-row">
                            <div className="frm-group">
                                <label>Nature du problème</label>
                                <select
                                    className="frm-select"
                                    id="NatureProbleme"
                                    name="NatureProbleme"
                                    onChange={handleChange}
                                    value={formData.NatureProbleme}
                                >
                                    <option value="">-- Sélectionner --</option>
                                    {formData.Service === 'Logistique' && (
                                        <option value="Logisticien">Logisticien</option>
                                    )}
                                    {formData.Service === 'Informatique' && (
                                        <option value="Edition">Édition</option>
                                    )}
                                    {formData.Service === 'TCS' && (
                                        <>
                                            <option value="Transporteur">Transporteur</option>
                                            <option value="Facture">Facture</option>
                                        </>
                                    )}
                                    <option value="Autre">Autre</option>
                                </select>
                            </div>
                        </div>
                    )}

                    {formData.TypeProbleme !== '' && (
                        <>
                            <div className="frm-row">
                                <div className="frm-group">
                                    <label>Problème</label>
                                    <textarea
                                        id="Probleme"
                                        className="frm-input"
                                        name="Probleme"
                                        placeholder="Écrivez le détail du problème ici..."
                                        value={formData.Probleme}
                                        onChange={handleChange}
                                    />
                                </div>
                            </div>
                            <div className="frm-row">
                                <div className="frm-group">
                                    <label>Résolu</label>
                                    <select
                                        className="frm-select"
                                        id="Resolu"
                                        name="Resolu"
                                        onChange={handleChange}
                                        value={formData.Resolu}
                                    >
                                        <option value="">-- Sélectionner --</option>
                                        <option value="Oui">Oui</option>
                                        <option value="Non">Non</option>
                                    </select>
                                </div>
                            </div>
                        </>
                    )}

                    <button className="btn btn-orange" type="submit">Valider</button>
                </form>
            </div>
        </div>
    );
};

export default FormModalProbleme;
