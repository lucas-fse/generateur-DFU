import React from 'react';

const dummyData = Array(9).fill({
    date: 'XX/XX/XXXX',
    demandePar: 'XX',
    idPiece: 'XXX',
    nomPiece: 'XXXX',
    stock: 'XXXXX',
    localisation: 'XXXXXX',
});

function ReassortPage() {
    return (
        <div className="reassort-container">
            <h1>Réassort</h1>
            <div className="reassort-grid">
                <table className="reassort-table">
                    <thead>
                        <tr>
                            <th>Date</th>
                            <th>Demandé par</th>
                            <th>idPièce</th>
                            <th>Nom pièce</th>
                            <th>Stock</th>
                            <th>Localisation</th>
                            <th>Réassort</th>
                        </tr>
                    </thead>
                    <tbody>
                        {dummyData.map((row, index) => (
                            <tr key={index}>
                                <td>{row.date}</td>
                                <td>{row.demandePar}</td>
                                <td>{row.idPiece}</td>
                                <td>{row.nomPiece}</td>
                                <td>{row.stock}</td>
                                <td>{row.localisation}</td>
                                <td>
                                    <span className="badge green">OK</span>
                                    <span className="badge red">INDISPO</span>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>

                <div className="reassort-search">
                    <h3>Rechercher</h3>
                    <form>
                        <input type="text" placeholder="Date" />
                        <input type="text" placeholder="Demandé par" />
                        <input type="text" placeholder="idPièce" />
                        <input type="text" placeholder="Localisation" />
                        <button type="submit" className="btn btn-orange">Rechercher</button>
                    </form>
                </div>
            </div>
        </div>
    );
}

export default ReassortPage;
