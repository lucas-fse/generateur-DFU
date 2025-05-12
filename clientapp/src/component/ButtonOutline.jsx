import React from 'react';

const redirection = (acces) => {
    window.location.href = acces;
}


const ButtonOutline = ({ label ,onClick }) => {
    return (
        <button className="btn btn-outline-orange" onClick={() => redirection(onClick)}>
            {label}
        </button>
    );
};

export default ButtonOutline;