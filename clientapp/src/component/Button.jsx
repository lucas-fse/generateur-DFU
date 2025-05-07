import React from 'react';

const redirection = (acces) => {
    window.location.href = acces;
}


const Button = ({ label ,onClick }) => {
    return (
        <button className="btn btn-orange" onClick={() => redirection(onClick)}>
            {label}
        </button>
    );
};

export default Button;