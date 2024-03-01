let buffer = "";

const mask = new RegExp(/^[A-Z0-9;\-]$/, 'i');
const filterCode = /RSL;([0-9A-Z\-]+);([0-9A-Z]*);([0-9A-Z]*)(;([0-9]+))*;*#/; // Filtrer les chaines rsl
const filterOf = /F[0-9]{7}#/;
const filterOther = /([A-Z0-9]+)#/

let matchCode = [];
let matchOf = [];

const ScanControl = {
    onCodeScannedListeners: [],
    onOFScannedListeners: [],
    onScanListeners: [],


    addOnCodeScannedListener: function (f) { this.onCodeScannedListeners.push(f) },
    addOnOFScannedListener: function (f) { this.onOFScannedListeners.push(f) },
    addOnScanListener: function (f) { this.onScanListeners.push(f) },


    onCodeScanned: function (a, b) {
        this.onCodeScannedListeners.forEach(i => i(a, b));
    },

    onOFScanned: function (a) {
        this.onOFScannedListeners.forEach(i => i(a));
    },

    onScan: function (a) {
        this.onScanListeners.forEach(i => i(a));
    }

};

document.addEventListener('keydown', e => {

    // Permet de filtrer les shifts
    if (mask.test(e.key)) {
        buffer += e.key.toUpperCase();
    }

    //console.log(e.key);

    if (e.key == 'Enter') {
        buffer += '#';
    }

    // on cherche des codes
    const nv = buffer.match(filterCode);
    if (nv != null && nv.length > 0) {
        matchCode = [...matchCode, nv];

        // On interprete le code
        const code = nv[0];
        const result = code.match(filterCode);

        const article = {};

        article.ref = result[1];
        article.serial = result[2];
        article.lot = result[3];
        article.cpt = result[4];

        // On appel la fonction callback
        ScanControl.onCodeScanned(article, code.replace('#', ''));
        ScanControl.onScan(code.replace('#', ''));

        buffer = buffer.replace(filterCode, "");
    }
    else {
        // on cherche des OF
        const nv2 = buffer.match(filterOf);
        if (nv2 != null && nv2.length > 0) {
            matchOf = [...matchOf, nv2];

            ScanControl.onOFScanned(nv2[0]);
            ScanControl.onScan(nv2[0]);

            buffer = buffer.replace(filterOf, "");
        }
        else {
            // on cherche d'autres chose
            const nv3 = buffer.match(filterOther);
            if (nv3 != null && nv3.length > 0) {
                ScanControl.onCodeScanned({ ref: nv3[1] }, nv3[0].replace('#', ''));
                ScanControl.onScan(nv3[0].replace('#', ''));

                buffer = buffer.replace(filterOther, "");
            }
        }
    }
    
});