////////////////////////////////////////////////
//Script Webcam

//Méthode bouton "Prendre Photo"
function previewPhoto() {

    //Méthode lié à la bibliothèque de la webcam
    //Capture l'image et génère un lien stocké dans la variable "data_uri"
    //On affiche le résultat dans une image et on stocke le lien dans un champ input
    //pour le récupérer dans le controller via le formulaire


    //On masque la webcam et on affiche l'image comme résultat
    document.getElementById('Contenant-video').style.margin = "0";
    document.getElementById('video').style.display = "none";
    document.getElementById('video').style.position = "absolute";

    document.getElementById('photo').style.display = "inline-block";
    document.getElementById('photo').style.position = "static";

    takePicture();
    //Mise à jour des boutons du menu de la webcam
    document.getElementById("PrendreButtonPhoto").style.position = "absolute";
    document.getElementById("PrendreButtonPhoto").style.display = "none";

    document.getElementById("ReprendreButtonPhoto").style.position = "static";
    document.getElementById("ReprendreButtonPhoto").style.display = "block";

}

//Méthode bouton "Reprendre Photo"
function ReprendrePhoto() {

    //La webcam se réactive

    //On masque l'ancien résultat et on affiche le webcam
    document.getElementById('Contenant-video').style.margin = "0 10px 0 0";
    document.getElementById('video').style.position = "static";
    document.getElementById('video').style.display = "block";

    document.getElementById('photo').style.position = "absolute";
    document.getElementById('photo').style.display = "none";

    //Mise à jour des boutons du menu de la webcam
    document.getElementById("ReprendreButtonPhoto").style.position = "absolute";
    document.getElementById("ReprendreButtonPhoto").style.display = "none";

    document.getElementById("PrendreButtonPhoto").style.position = "static";
    document.getElementById("PrendreButtonPhoto").style.display = "block";
}

//Méthode bouton "Depuis la camera / webcam"
function Changepp() {

    //Masque l'ancienne photo de profil et affiche la webcam
    document.getElementById("FormContenantPhotoProfil").style.position = "absolute";
    document.getElementById("FormContenantPhotoProfil").style.display = "none";

    document.getElementById("WebcamOn").style.position = "static";
    document.getElementById("WebcamOn").style.display = "flex";

    //Masque le bouton importer une image depuis l'explo de fichiers
    document.getElementById("button-image").style.display = "none";
}

//Méthode pour cacher l'appareil photo et possibilité de changer l'image
function importImage() {
    //Masque le bouton de la webcam
    document.getElementById("ChangeppCamera").style.display = "none";
    document.getElementById("button-image").innerHTML = "<i class='fa-solid fa-rotate-left' style='color: #f26222;'></i> REMPLACER L'IMAGE";
    //Rendre visible le bouton annuler
    document.getElementById("annulation-image").style.display = "inline";
    document.getElementById('srcImgold').value = '';
}

//Méthode qui se déclenche lorsqu'on clique sur le bouton annuler
function cancelImage() {

    //On remplace l'intitulé du bouton ajouter une image et on masque le bouton annuler
    document.getElementById("button-image").innerHTML = "<i class='fa-solid fa-plus' style='color: #f26222;'></i> AJOUTER UNE IMAGE";
    document.getElementById("annulation-image").style.display = "none";

    document.getElementById("ChangeppCamera").style.display = "block";

    document.getElementById('preview').value = '';
    document.getElementById('blah').src = '';
    document.getElementById('srcImgold').value = '';
    
    /*
    document.getElementById('preview').value = '';
    document.getElementById('photo').src = '';*/
}


//Méthode bouton "Annuler" dans le menu webcam
function AnnulerChangepp() {

    //Suppression des potentiels données d'une photo
    document.getElementById('srcImg').value = '';
    document.getElementById('photo').src = '';

    //Masque la webcam et on affiche l'ancienne photo de profil
    document.getElementById("WebcamOn").style.position = "absolute";
    document.getElementById("WebcamOn").style.display = "none";

    document.getElementById("FormContenantPhotoProfil").style.position = "static";
    document.getElementById("FormContenantPhotoProfil").style.display = "flex";

    //Remise à l'état par défaut du menu de la webcam
    document.getElementById('photo').style.position = "absolute";
    document.getElementById('photo').style.display = "none";

    document.getElementById('Contenant-video').style.margin = "0 10px 0 0";
    document.getElementById('video').style.position = "static";
    document.getElementById('video').style.display = "block";


    document.getElementById("ReprendreButtonPhoto").style.position = "absolute";
    document.getElementById("ReprendreButtonPhoto").style.display = "none";

    document.getElementById("PrendreButtonPhoto").style.position = "static";
    document.getElementById("PrendreButtonPhoto").style.display = "block";

    //Réaffiche le bouton importer une image depuis l'explo de fichiers
    document.getElementById("button-image").style.display = "inline";
}