
redirect(); //Appel de la fonction

//Code de la Fonction
function redirect() {
    var strinchemin = location.href;
    
    console.log(strinchemin);
    setTimeout("document.location = \'" + strinchemin + "'", 300000); //5 minutes
}
