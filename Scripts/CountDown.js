var cpt = 0;
var nb_boucle = 100;
var nb_cpt1 = document.getElementById('AMcpt1').getAttribute("initvalue");
var nb_cpt2 = document.getElementById('AMcpt2').getAttribute("initvalue");
var nb_cpt3 = document.getElementById('AMcpt3').getAttribute("initvalue");

var cpt1 = 0;
var cpt2 = 0;
var cpt3 = 0;

var duree = 1;
var delta = Math.ceil((duree * 1000) / nb_boucle);
var elem_cpt1 = document.getElementById('AMcpt1');
var elem_cpt2 = document.getElementById('AMcpt2');
var elem_cpt3 = document.getElementById('AMcpt3');

var elems = document.getElementsByClassName('ValueCase')
var opa = 0;


function CountDown() {
    cpt++;
    elem_cpt1.innerHTML = Math.round(cpt1);
    elem_cpt2.innerHTML = Math.round(cpt2);
    elem_cpt3.innerHTML = Math.round(cpt3);

    for (var e of elems) {
        e.style.opacity = opa;
    }
    if (cpt <= nb_boucle) {
        cpt1 = cpt1 + (nb_cpt1 / nb_boucle);
        cpt2 = cpt2 + (nb_cpt2 / nb_boucle);
        cpt3 = cpt3 + (nb_cpt3 / nb_boucle);

        opa = opa + (1 / nb_boucle);
        setTimeout(CountDown, delta);
    }
}

setTimeout(function () { CountDown(); }, 500);


