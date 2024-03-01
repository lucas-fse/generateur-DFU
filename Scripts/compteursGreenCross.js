var cptG = 0;
var nb_boucleG = 100;
var nb_jour_sans_accident = document.getElementById('cpt1').getAttribute("initvalue");
var record_sans_accident = document.getElementById('cpt2').getAttribute("initvalue");
var nb_humeur_positive = document.getElementById('cpt3').getAttribute("initvalue");
var cpt_nb_jour_sans_accident = 0;
var cpt_record_sans_accident = 0;
var cpt_nb_humeur_positive = 0;
var dureeG = 1.3;
var deltaG = Math.ceil((dureeG * 1000) / nb_boucleG);
var elem_cpt1G = document.getElementById('cpt1');
var elem_cpt2G = document.getElementById('cpt2');
var elem_cpt3G = document.getElementById('cpt3');


setTimeout(function () { CountDown3(); }, 500);


function CountDown3() {
    cptG++;
    elem_cpt1G.innerHTML = Math.round(cpt_nb_jour_sans_accident);
    elem_cpt2G.innerHTML = Math.round(cpt_record_sans_accident);
    if (nb_humeur_positive != "-") {
        elem_cpt3G.innerHTML = Math.round(cpt_nb_humeur_positive) + "%";
    }

    if (cptG <= nb_boucleG) {
        cpt_nb_jour_sans_accident = cpt_nb_jour_sans_accident + (nb_jour_sans_accident / nb_boucleG);
        cpt_record_sans_accident = cpt_record_sans_accident + (record_sans_accident / nb_boucleG);
        cpt_nb_humeur_positive = cpt_nb_humeur_positive + (nb_humeur_positive / nb_boucleG);
        
        setTimeout(CountDown3, deltaG);
    }
}