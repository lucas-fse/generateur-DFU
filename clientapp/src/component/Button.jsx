import React from 'react';

const Button = ({ label }) => {
    return (
        <button className="btn btn-orange">
            {label}
        </button>
    );
};

export default Button;