/*
  script qui detecte quand la derniere bottomBox est "wrapped" (retour à ligne de l'élément),
  afin de lui appliquer la classe ".wrapped" pour la centrer

  note: ne fonctionne que si il-y-a exactement 4 bottomBox dans la div bottomActivityContent (voir l'html)
*/

const MIN_RESOLUTION_PC = 1081; /* debut resolution sur PC */
const MAX_RESOLUTION_TABLETTE = 1080; /* fin resolution tablette */
const TAILLE_MIN_MAIN_PC = 799; /* taille balise <main> min sur PC */
var mainWidth; /* largeur balise <main> */
var screenWidth; /* largeur fenetre */

/* enlever la classe wrapped aux éléments qui la possède */
function removeWrappedClass(screenWidth, mainWidth) {
  /* on vérifie quand il faut enlever la classe */
  if (screenWidth <= MAX_RESOLUTION_TABLETTE || mainWidth >= TAILLE_MIN_MAIN_PC) {
    var elems = document.querySelectorAll("#bottomActivityContent .bottomBox");

    for (var e of elems) {
      if (e.classList.contains("wrapped")) {
          e.classList.remove("wrapped");
      }
    }
  }
}
/* ajoute la classe wrapped aux éléments qui sont détectés "wrapped" (retour à la ligne) */
function addWrappedClass(screenWidth) {
  var wrappedItems = detectWrap('#bottomActivityContent .bottomBox');

  for (var k = 0; k < wrappedItems.length; k++) {
    /* on vérifie quand il faut ajouter la classe */
     if (screenWidth > MIN_RESOLUTION_PC)
        wrappedItems[k].classList.add("wrapped");
  }
}
/* itialise les variables globales screenWidth et mainWidth */
function loadWidth () {
  mainWidth = document.querySelector('main').offsetWidth;
  screenWidth = event.target.outerWidth;
}

var detectWrap = function(className) { /* retourne un Array des éléments détectés "wrapped" (retour à la ligne)*/
  var wrappedItems = [];
  var prevItem = {};
  var currItem = {};
  var items = document.querySelectorAll(className);

  for (var i = 0; i < items.length; i++) {
    currItem = items[i].getBoundingClientRect();
    if (prevItem && prevItem.top < currItem.top) {
      wrappedItems.push(items[i]);
    }
    prevItem = currItem;
  };

  return wrappedItems;
}

window.onresize = function(event){ /* quand l'utilisateur fait varier la taille de la fenêtre  */
    loadWidth();
    removeWrappedClass(screenWidth, mainWidth);
    addWrappedClass(screenWidth);
};

window.onload = function(event) { /* au chargement de la page */
  loadWidth();
  removeWrappedClass(screenWidth, mainWidth);
  addWrappedClass(screenWidth);
};
