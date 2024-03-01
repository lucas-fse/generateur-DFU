var bool = false;

function OuvrirDiv(value, id) {
    if (bool == true) {
        document.getElementById("fondTransparent").style.opacity = 0;
        document.getElementById("fondTransparent").style.pointerEvents = "none";
        document.getElementById("humeur").style.display = "none";
        bool = false;
    } else {
        document.getElementById("fondTransparent").style.opacity = 1;
        document.getElementById("fondTransparent").style.pointerEvents = "all";
        document.getElementById("humeur").style.display = "flex";
        document.getElementById("IdSalarie").setAttribute("value", id);
        bool = true;
    }

}
