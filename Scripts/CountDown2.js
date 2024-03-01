var cptCD2 = 0;
var nb_boucleCD2 = 100;

var nb_cpt4 = document.getElementById('OTDcpt1').getAttribute("initvalue");
var nb_cpt5 = document.getElementById('OTDcpt2').getAttribute("initvalue");

var cpt4CD2 = 0;
var cpt5CD2 = 0;
var dureeCD2 = 1;
var deltaCD2 = Math.ceil((dureeCD2 * 1000) / nb_boucleCD2);

var elem_cpt4CD2 = document.getElementById('OTDcpt1');
var elem_cpt5CD2 = document.getElementById('OTDcpt2');
var elemsCD2 = document.getElementsByClassName('ValueCase')
var opaCD2 = 0;


function CountDown2() {
    cptCD2++;

    elem_cpt4CD2.innerHTML = Math.round(cpt4CD2) + "%";
    elem_cpt5CD2.innerHTML = Math.round(cpt5CD2) + "%";
    for (var e of elems) {
        e.style.opacity = opaCD2;
    }
    if (cptCD2 <= nb_boucleCD2) {

        cpt4CD2 = cpt4CD2 + (nb_cpt4 / nb_boucleCD2);
        cpt5CD2 = cpt5CD2 + (nb_cpt5 / nb_boucleCD2);
        opaCD2 = opaCD2 + (1 / nb_boucleCD2);
        setTimeout(CountDown2, deltaCD2);
    }
}

setTimeout(function () { CountDown2(); }, 500);


