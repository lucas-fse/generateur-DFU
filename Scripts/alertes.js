// Fonction pour injecter le style une seule fois
function injectAlertStyles() {
    if (document.getElementById('custom-alert-style')) return; // déjà injecté

    const style = document.createElement('style');
    style.id = 'custom-alert-style';
    style.textContent = `
        #custom-alert {
            display: none;
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background-color: #fff;
            color: #333;
            border: 2px solid #FF6600;
            border-radius: 10px;
            padding: 20px 30px;
            box-shadow: 0 0 10px rgba(0,0,0,0.2);
            z-index: 9999;
            font-family: sans-serif;
            min-width: 300px;
            text-align: center;
        }

        #custom-alert.success { border-color: #28a745; }
        #custom-alert.error { border-color: #dc3545; }
        #custom-alert.neutre { border-color: #808080}

        #custom-alert .alert-title {
            font-size: 18px;
            font-weight: bold;
            margin-bottom: 10px;
        }
    `;
    document.head.appendChild(style);
}

// Fonction pour décoder les entités HTML
function decodeHTMLEntities(text) {
    var element = document.createElement('div');
    if (text) {
        element.innerHTML = text;
        text = element.innerText || element.textContent;
    }
    return text;
}

// Fonction d'affichage du pop-up
function showCustomAlert(type, message) {
    injectAlertStyles();

    let alertBox = document.getElementById('custom-alert');
    if (!alertBox) {
        alertBox = document.createElement('div');
        alertBox.id = 'custom-alert';
        document.body.appendChild(alertBox);

        alertBox.innerHTML = `
            <div class="alert-title" id="alert-title"></div>
            <button class="btn btn-orange" onclick="closeCustomAlert()">OK</button>
        `;
    }

    // Décode les entités HTML avant d'afficher le message
    let decodedMessage = decodeHTMLEntities(message);
    document.getElementById('alert-title').innerText = decodedMessage;

    alertBox.className = type;
    alertBox.style.display = 'block';

    setTimeout(closeCustomAlert, 5000);
}

// Fonction de fermeture
function closeCustomAlert() {
    const alertBox = document.getElementById('custom-alert');
    if (alertBox) {
        alertBox.style.display = 'none';
    }
}

window.showCustomAlert = showCustomAlert;