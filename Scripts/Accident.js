var cpt = 0;
var nb_boucle = 100;
var nb_jour_sans_accident = document.getElementById('cpt1').getAttribute("initvalue");
var record_sans_accident = document.getElementById('cpt2').getAttribute("initvalue");
var nb_humeur_positive = document.getElementById('cpt3').getAttribute("initvalue");
var cpt_nb_jour_sans_accident = 0;
var cpt_record_sans_accident = 0;
var cpt_nb_humeur_positive = 0;
var duree = 1.3;
var delta = Math.ceil((duree * 1000) / nb_boucle);
var elem_cpt1 = document.getElementById('cpt1');
var elem_cpt2 = document.getElementById('cpt2');
var elem_cpt3 = document.getElementById('cpt3');
var b1 = document.getElementById("b1div");
var b2 = document.getElementById("b2div");
var content = document.getElementById("content");
var pasContent = document.getElementById("pasContent");
var Smiley = document.getElementById("Smiley");
var SereniteStat = document.getElementById("SereniteStat");
var SereniteLien = document.getElementById("SereniteLien");
var now = new Date();
var colorCase = document.getElementById("Case_" + now.getDate().toString()).getAttribute("color");

if (colorCase == "#303030" && colorCase == "#CF0A1D" && colorCase =="#FF8C00") {
    AfficheSmileyPasContentInit();
}


setTimeout(function () { CountDown(); }, 500);

SereniteStat.addEventListener("mouseenter", function () {
    SereniteStat.style.display = "none";
    SereniteLien.style.display = "table";
});

SereniteLien.addEventListener("mouseleave", function () {
    SereniteStat.style.display = "flex";
    SereniteLien.style.display = "none";
});

var item = sessionStorage.getItem("DateSave");
if (now.getDay() == item) {
    sessionStorage.clear();
} else {
    if (sessionStorage.getItem("autosave1")) {
        b1.style.display = sessionStorage.getItem("autosave1");
    }

    if (sessionStorage.getItem("autosave2")) {
        b2.style.display = sessionStorage.getItem("autosave2");
    }

    if (sessionStorage.getItem("autosave3")) {
        content.style.display = sessionStorage.getItem("autosave3");
    }

    if (sessionStorage.getItem("autosave4")) {
        pasContent.style.display = sessionStorage.getItem("autosave4");
    }

    if (sessionStorage.getItem("autosave5")) {
        Smiley.style.border = sessionStorage.getItem("autosave5");
    }

}
if (sessionStorage.length != 0) {
    
    document.getElementById("Case_" + now.getDate().toString()).style.background = colorCase;
    document.getElementById("Case_" + now.getDate().toString()).style.color = "white";
    
}


function CountDown() {
    cpt++;
    elem_cpt1.innerHTML = Math.round(cpt_nb_jour_sans_accident);
    elem_cpt2.innerHTML = Math.round(cpt_record_sans_accident);
   
    if (nb_humeur_positive != "-") {
        elem_cpt3.innerHTML = Math.round(cpt_nb_humeur_positive) + "%";
    }

    if (cpt <= nb_boucle) {
        cpt_nb_jour_sans_accident = cpt_nb_jour_sans_accident + (nb_jour_sans_accident / nb_boucle);
        cpt_record_sans_accident = cpt_record_sans_accident + (record_sans_accident / nb_boucle);
        cpt_nb_humeur_positive = cpt_nb_humeur_positive + (nb_humeur_positive / nb_boucle);
        setTimeout(CountDown, delta);
    }
}

function OuvrirPopUp(id) {
    /*document.getElementById("SaisieNiveau").setAttribute("value", id);*/

    window.location.href = "/Production/DeclAccident?id=" + id;
    OuvrirPopUp.onload = CheckList(id);

}

function CheckList() {

    const urlSearchParams = new URLSearchParams(window.location.search);
    let id = urlSearchParams.get("id");

    if (id == 1) {
        document.getElementById("list1").selectedIndex = "5";
    } else if (id == 2) {
        document.getElementById("list1").selectedIndex = "4";
    } else if (id == 3) {
        document.getElementById("list1").selectedIndex = "3";
    } else if (id == 4) {
        document.getElementById("list1").selectedIndex = "2";
    } else if (id == 5) {
        document.getElementById("list1").selectedIndex = "1";
    } else if (id == 6) {
        document.getElementById("list1").selectedIndex = "0";
    }
}

function AfficheSmileyContent() {
    var now = new Date()
    var item = now.getDay() + 1;
    sessionStorage.setItem("DateSave", item);
    b1.style.display = "none";
    b2.style.display = "none";
    content.style.display = "block";
    Smiley.style.border = "1px solid black";

    sessionStorage.setItem("autosave1", "none");

    sessionStorage.setItem("autosave2", "none");

    sessionStorage.setItem("autosave3", "block");

    sessionStorage.setItem("autosave5", "1px solid black");

    if (colorCase != "") {

    }
    document.getElementById("Case_" + now.getDate().toString()).style.background = colorCase;
    document.getElementById("Case_" + now.getDate().toString()).style.color = "white";
}

function AfficheSmileyPasContent() {
    var now = new Date()
    var item = now.getTime() + 5000;
    sessionStorage.setItem("DateSave", item);

    b1.style.display = "none"
    b2.style.display = "none"
    pasContent.style.display = "block";
    Smiley.style.border = "1px solid black";

    sessionStorage.setItem("autosave1", "none");

    sessionStorage.setItem("autosave2", "none");

    sessionStorage.setItem("autosave4", "block");

    sessionStorage.setItem("autosave5", "1px solid black");

    OuvrirPopUp();
}
function AfficheSmileyPasContentInit() {
    var now = new Date()
    var item = now.getTime() + 5000;
    sessionStorage.setItem("DateSave", item);

    b1.style.display = "none"
    b2.style.display = "none"
    pasContent.style.display = "block";
    Smiley.style.border = "1px solid black";

    sessionStorage.setItem("autosave1", "none");

    sessionStorage.setItem("autosave2", "none");

    sessionStorage.setItem("autosave4", "block");

    sessionStorage.setItem("autosave5", "1px solid black");

}

function ValiderPopUp() {
    if (document.getElementById("AccidentGrave").checked) {
        document.getElementById("SaisieNiveau").setAttribute("value", 6);
    }
    if (document.getElementById("AccidentDeclare").checked) {
        document.getElementById("SaisieNiveau").setAttribute("value", 5);
    }
    if (document.getElementById("SoinsBenins").checked) {
        document.getElementById("SaisieNiveau").setAttribute("value", 4);
    }
    if (document.getElementById("PresqueAccident").checked) {
        document.getElementById("SaisieNiveau").setAttribute("value", 3);
    }
    if (document.getElementById("SituationsDangereuses").checked) {
        document.getElementById("SaisieNiveau").setAttribute("value", 2);
    }
    if (document.getElementById("ActesDangeureux").checked) {
        document.getElementById("SaisieNiveau").setAttribute("value", 1);
    }
}

function AnnulerPage() {
    // On renvoie a la page /Production/Securite si on clique sur le boutton annuler
    window.location.href = "/Production/Securite";
}


