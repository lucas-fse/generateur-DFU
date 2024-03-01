var cpt = 0;
var nb_boucle = 100;
var nb_situation_dangereuse = document.getElementById('cpt1').getAttribute("initvalue");
var CA = document.getElementById('cpt2').getAttribute("initvalue");
var nb_FRC = document.getElementById('cpt4').getAttribute("initvalue");
var OTR = document.getElementById('cpt3').getAttribute("initvalue");
var cpt_situation_dangereuse = 0;
var cpt_CA = 0;
var cpt_FRC = 0;
var cpt_OTR = 0;
var opa = 0.0;
var duree = 1.2;
var delta = Math.ceil((duree * 1000) / nb_boucle);
var elem_situation_dangereuse = document.getElementById('cpt1');
var elem_CA = document.getElementById('cpt2');
var elem_FRC = document.getElementById('cpt4');
var elem_OTR = document.getElementById('cpt3');
var elems = document.getElementsByClassName('ValueKPI');


function CountDown() {
    cpt++;
    elem_situation_dangereuse.innerHTML = Math.round(cpt_situation_dangereuse);
    elem_CA.innerHTML = Math.round(cpt_CA);
    elem_FRC.innerHTML = Math.round(cpt_FRC);
    elem_OTR.innerHTML = Math.round(cpt_OTR) ;
    for (var e of elems) {
        e.style.opacity = opa;
    }
    if (cpt <= nb_boucle) {
        cpt_situation_dangereuse = cpt_situation_dangereuse + (nb_situation_dangereuse/ nb_boucle);
        cpt_FRC = cpt_FRC + (nb_FRC / nb_boucle);
        cpt_OTR = (cpt_OTR + (OTR / nb_boucle));
        cpt_CA = cpt_CA + (CA / nb_boucle);
        opa = opa + (1 / nb_boucle);
        setTimeout(CountDown, delta);
    }
}

setTimeout(function () { CountDown(); }, 100);
