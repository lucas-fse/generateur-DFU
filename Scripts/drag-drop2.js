const draggables = document.getElementById("ContainerListeOF").querySelectorAll('.of') /* list des of de liste de droites*/
const draggablesPlanifie = document.getElementById("OfPlanifieByOperateur").querySelectorAll('.of') /* list des of de liste de droites*/
const containers = document.querySelectorAll('.containerOF') /* list ensemble des operateurs*/
const containerListe = document.getElementById('ContainerListeOF')
var oldposition
var collectionop = new Map()
var ligneActive = null // position du bloc d'entree represente la ligne de l'operateur
var colonneActive = null
var draggableInProgress
var containeractif = null;
var calculok = null;
var memoiretime = null;


/*Pour le principe général des fonctions drag and drop
 *https://www.youtube.com/watch?v=jfYWwQrtzzY/ */


// drag start
console.log('draggables.length:', draggables.length);
// pour la Liste des of à droite de l'ecran
draggables.forEach(draggable => {

    draggable.addEventListener('dragstart', e => {
        Clignoterboutons();
        console.log("dragstart", e);

        /*on récupère les valeurs de l'événement pour pouvoir créer une 
         * copie de l'of à sa position initial*/
        oldposition = e
        
        draggableInProgress = draggable.cloneNode(true);
        draggable.classList.add('oftri') // permet de mettre element en mode transparent
        draggable.setAttribute('draggable', 'false');
        oldposition = draggable;
        draggableInProgress.setAttribute('Inprogress', "true");
        draggableInProgress.setAttribute('id', draggableInProgress.getAttribute('id') + 'PL');
        draggableInProgress.classList.add('dragging');
        draggableInProgress.addEventListener('dragstart', f => {
            console.log("dragstart operateurs",f);            
            draggableInProgress = f.target;
            oldposition = draggableInProgress.parentElement;
            console.log(oldposition);
            draggableInProgress.classList.add('dragging'); // permet de mettre element en mode transparent
            //oldposition = f;
            draggableInProgress.setAttribute('inprogress', "true");
            console.log(draggableInProgress);
        })

        draggableInProgress.addEventListener('dragend', f => {
            //deplacement d'un of d'une ligne a l'autre
            
            if (ligneActive == null && colonneActive == null) {
                if (containeractif != null) {
                    console.log("retire of ", draggableInProgress.getAttribute('traca'));
                    console.log(containeractif);
                    console.log(draggableInProgress.getAttribute('traca'));
                    containeractif.removeChild(draggableInProgress);
                }
                var ofnonplanifie = document.getElementById(draggableInProgress.getAttribute('traca'));
                console.log("ofnonplanifie", ofnonplanifie);
                if (ofnonplanifie != null) {
                    ofnonplanifie.classList.remove('oftri')
                    ofnonplanifie.setAttribute('draggable', 'true');
                }
            }
            else if (ligneActive != null ) {
                console.log("enddrag ligne a ligne");
                //oldposition.classList.remove('oftri');
                draggableInProgress.setAttribute('inprogress', "false");
                draggableInProgress.classList.remove('dragging');
                //ligneActive.appendChild(oldposition.cloneNode(true))
            }
            // retirer un of deja plannifie
            else if (colonneActive != null) {
                if (containeractif != null) {
                    console.log("retire of ", draggableInProgress.getAttribute('traca'));
                    console.log(containeractif);
                    if (containeractif != null) {
                        containeractif.removeChild(draggableInProgress);
                    }
                    var ofnonplanifie = document.getElementById(draggableInProgress.getAttribute('traca'));
                    console.log("ofnonplanifie", ofnonplanifie);
                    if (ofnonplanifie != null) {
                        ofnonplanifie.classList.remove('oftri')
                        ofnonplanifie.setAttribute('draggable', 'true');
                    }
                }
            }
            oldposition = null;
            draggableInProgress = null;
            containeractif = null;
            TempsOperatoire();
        })
        
    })
    // drag end
    draggable.addEventListener('dragend', e => {
        console.log("dragend id:",draggableInProgress.id, "inprogress",draggableInProgress.getAttribute('Inprogress'));
        oldposition.classList.remove('dragging')

        /*On test si l'of à déjà été copié dans la liste
         * 
         si ce n'est pas le cas on crée une copie*/
        console.log(ligneActive);
        console.log(colonneActive);
        console.log(oldposition);
        if (ligneActive == null && colonneActive == null)
        {
            if (containeractif != null) {
                console.log("retire of ", draggableInProgress.getAttribute('traca'));
                console.log(containeractif);
                console.log(draggableInProgress.getAttribute('traca'));
                containeractif.removeChild(draggableInProgress);                
            }
            var ofnonplanifie = document.getElementById(draggableInProgress.getAttribute('traca'));
            console.log("ofnonplanifie", ofnonplanifie);
            if (ofnonplanifie != null) {
                ofnonplanifie.classList.remove('oftri')
                ofnonplanifie.setAttribute('draggable', 'true');
            }
        }
        else if (ligneActive != null && oldposition != null) {
            //oldposition.classList.remove('oftri');
            draggableInProgress.setAttribute('Inprogress', "false");
            draggableInProgress.classList.remove('dragging');
            //ligneActive.appendChild(oldposition.cloneNode(true))
        }
        else if (colonneActive != null)
        {
            if (containeractif != null) {
                console.log("retire of ", draggableInProgress.getAttribute('traca'));
                console.log(containeractif);
                console.log(draggableInProgress.getAttribute('traca'));
                containeractif.removeChild(draggableInProgress);
                var ofnonplanifie = document.getElementById(draggableInProgress.getAttribute('traca'));
                console.log("ofnonplanifie", ofnonplanifie);
                if (ofnonplanifie != null) {
                    ofnonplanifie.classList.remove('oftri')
                    ofnonplanifie.setAttribute('draggable', 'true');
                }
            }
            else if (colonneActive != null) {
                console.log("retire of ", draggableInProgress.getAttribute('traca'));
                console.log(containeractif);
                console.log(draggableInProgress.getAttribute('traca'));
                
                var ofnonplanifie = document.getElementById(draggableInProgress.getAttribute('traca'));
                console.log("ofnonplanifie", ofnonplanifie);
                if (ofnonplanifie != null) {
                    ofnonplanifie.classList.remove('oftri')
                    ofnonplanifie.setAttribute('draggable', 'true');
                }
            }
            console.log("colonneactive");
        }
        oldposition = null;
        draggableInProgress = null;
    })

    draggable.addEventListener('mouseenter', e => {
        memoiretime = draggable;
        setTimeout(() => {
            if (memoiretime == draggable) {
                document.getElementById('ofover').innerHTML = draggable.getAttribute('traca') + ' - ' + draggable.getAttribute('description');
                document.getElementById('defilover').innerHTML = draggable.firstElementChild.innerHTML;
            }
        }, 1000);
    })
    draggable.addEventListener('mouseleave', e => {
        memoiretime = null;
        
    })
})

// fin pour les of a droite de l'ecran

// pour les of planifie par operateur 
draggablesPlanifie.forEach(draggableof => {

    draggableof.addEventListener('dragstart', f => {
        Clignoterboutons();
        console.log("dragstart of deja palnifie", f);
        draggableInProgress = f.target;
        oldposition = draggableInProgress.parentElement;
        console.log(oldposition);
        draggableInProgress.classList.add('dragging'); // permet de mettre element en mode transparent
        //oldposition = f;
        draggableInProgress.setAttribute('inprogress', "true");
        console.log(draggableInProgress);
    })

    draggableof.addEventListener('dragend', f => {
        //deplacement d'un of d'une ligne a l'autre

        if (ligneActive == null && colonneActive == null) {
            if (containeractif != null) {
                console.log("retire of ", draggableInProgress.getAttribute('traca'));
                console.log(containeractif);
                console.log(draggableInProgress.getAttribute('traca'));
                containeractif.removeChild(draggableInProgress);
            }
            var ofnonplanifie = document.getElementById(draggableInProgress.getAttribute('traca'));
            console.log("ofnonplanifie", ofnonplanifie);
            if (ofnonplanifie != null) {
                ofnonplanifie.classList.remove('oftri')
                ofnonplanifie.setAttribute('draggableInProgress', 'true');
            }
        }
        else if (ligneActive != null) {
            console.log("enddrag ligne a ligne");
            //oldposition.classList.remove('oftri');
            draggableInProgress.setAttribute('inprogress', "false");
            draggableInProgress.classList.remove('dragging');
            //ligneActive.appendChild(oldposition.cloneNode(true))
        }
        // retirer un of deja plannifie
        else if (colonneActive != null) {
            if (containeractif != null) {
                console.log("retire of ", draggableInProgress.getAttribute('traca'));
                console.log(containeractif);
                if (containeractif != null) {
                    containeractif.removeChild(draggableInProgress);
                }
                var ofnonplanifie = document.getElementById(draggableInProgress.getAttribute('traca'));
                console.log("ofnonplanifie", ofnonplanifie);
                if (ofnonplanifie != null) {
                    ofnonplanifie.classList.remove('oftri')
                    ofnonplanifie.setAttribute('draggable', 'true');
                }
            }
        }
        oldposition = null;
        draggableInProgress = null;
        containeractif = null;
        TempsOperatoire();
    })

    draggableof.addEventListener('mouseenter', e => {
        memoiretime = draggableof;
        setTimeout(() => {
            if (memoiretime == draggableof) {
                document.getElementById('ofover').innerHTML = draggableof.getAttribute('traca') + ' - ' + draggableof.getAttribute('description');
                document.getElementById('defilover').innerHTML = draggableof.firstElementChild.innerHTML;
            }
        }, 1000);
    })
    draggableof.addEventListener('mouseleave', e => {
        memoiretime = null;

    })
})

// drag over des operateurs
containers.forEach(container => {
   container.addEventListener('dragover', e => {
        console.log("dragover:", container.getAttribute('idoperateur'));
        ligneActive = container;
        colonneActive = null;
            e.preventDefault();
        const afterElement = getDragAfterElementX(container, e.clientX)
        
        //const draggable = document.querySelector('.dragging')
        if (afterElement == null) {
            containeractif = container;
            container.appendChild(draggableInProgress)
        }
        else {
            container.insertBefore(draggableInProgress, afterElement)
            containeractif = container;
            }

        console.log('container.id:', container.id);
        console.log('calculok:',calculok);
            if (calculok != container.getAttribute('idoperateur')) {
                calculok = container.getAttribute('idoperateur');
                TempsOperatoire();
            
        }
})

    // dragenter container => operateur 
    container.addEventListener('dragenter', e => {
        console.log('dragenter');
        //console.log(container)
        ligneActive = container;
        //TempsOperatoire(container.parentNode.parentNode)
    })

    //dragleave
    container.addEventListener('dragleave', e => {
        console.log('dragleave');
        //console.log(container);
        ligneActive = null;
        console.log(container.parentElement);
        //TempsOperatoire(container.parentNode.parentNode)
    })
})



// drag overlist of
containerListe.addEventListener('dragover', e => {
    console.log("dragover listeOF");
    colonneActive = containerListe;
    ligneActive = null;
    e.preventDefault();
    if (calculok != "listdesof") {
        calculok = "listdesof";
        TempsOperatoire();
    }
})
containerListe.addEventListener('dragleave', e => {
    console.log("dragleave listeOF");
    e.preventDefault();
    colonneActive = null;    
})




function getDragAfterElementX(container, x) {
    const draggableElements = [...container.querySelectorAll('.of:not(.dragging)')]

    return draggableElements.reduce((closest, child) => {
        const box = child.getBoundingClientRect()
        const offset = x - box.left - box.width / 2
        if (offset < 0 && offset > closest.offset) {
            return { offset: offset, element: child }
        } else {
            return closest
        }
    }, { offset: Number.NEGATIVE_INFINITY }).element
}

function Clignoterboutons()
{
    var el = document.getElementById('Valider');
    el.classList.add('image-clignote');
}

/*Cette fonctionne légèrement différente de la précedente permet de prendre
 * en compte l'of qui disparait de la liste lorsque qu'on fait le test pour
 * replacer son clone*/
function getDragAfterElementYDecaler(container, y) {
    const draggableElements = [...container.querySelectorAll('.of:not(.dragging)')]

    return draggableElements.reduce((closest, child) => {
        const box = child.getBoundingClientRect()
        /*"50" permet de prendre en compte le décalage*/
        const offset = y - 50 - box.top - box.height / 2
        if (offset < 0 && offset > closest.offset) {
            return { offset: offset, element: child }
        } else {
            return closest
        }
    }, { offset: Number.NEGATIVE_INFINITY }).element
}

function TempsOperatoire() {

    var toto = document.getElementsByClassName('LigneOp').length;
    console.log('longueur:', toto);
    for (let pas = 0; pas < toto; pas++) {
        var tpstotal = 0;
        var HTMLtpstotal = document.getElementsByClassName('LigneOp')[pas].getElementsByClassName("TpsTotal")[0];
        console.log('of:', document.getElementsByClassName('LigneOp')[pas]);
        console.log('nbof:', document.getElementsByClassName('LigneOp')[pas].getElementsByClassName('of').length);
        var tata = document.getElementsByClassName('LigneOp')[pas].getElementsByClassName('of').length;
        for (let nmrof = 0; nmrof < tata; nmrof++) {
            console.log('of', document.getElementsByClassName('LigneOp')[pas].getElementsByClassName('of')[nmrof]);
            var tpsop = document.getElementsByClassName('LigneOp')[pas].getElementsByClassName('of')[nmrof].getAttribute('temps');
            console.log("tempsof:", tpsop);
            tpstotal = parseFloat(tpstotal) + parseFloat(tpsop)
        }
        var heures = Math.trunc(tpstotal);
        var minutes = (tpstotal % 1) * 60;
        console.log("tt:", tpstotal, "h:", heures, "m:", minutes);
        HTMLtpstotal.innerHTML = Math.round(heures) + "h" + Math.round(minutes).toLocaleString('en-US', {
            minimumIntegerDigits: 2,
            useGrouping: false
        });
        console.log('longueur:', toto);
    }
    
}
function SauvegardePlanification()
{
    // fonction de sauvegarde de la planification
    var data = document.getElementById('SaisieEmetteur');
    var datestring  = data.getAttribute('date');
    // {"Date":"2015-06-02 23:33:00","ListofByOperateur":[{"ID":1,"ListeOf":["OF1","OF2"]},{"ID":2,"ListeOf":["OF1","OF2"]} ]  }
    var toto = document.getElementsByClassName('LigneOp').length;

    var objectidentifiant = '{"Date":"' + datestring +'","ListofByOperateur":[';
    var finobjectidentifiant = ']  }';
    for (let pas = 0; pas < toto; pas++) {
        var virgule = '';
        if (pas < (toto - 1)) {
            virgule = ',';
        }
        console.log('id operateur:', document.getElementsByClassName('containerOF')[pas].getAttribute('idoperateur'));
        var id = document.getElementsByClassName('containerOF')[pas].getAttribute('idoperateur');
        var IdOperateur = document.getElementsByClassName('containerOF')[pas].getAttribute('idoperateur');
        var identifiant = '{"ID":' + id + ',"ListeOf": [';
        var finidentiant = ']}';
        var nbof = document.getElementsByClassName('containerOF')[pas].getElementsByClassName('of').length;
        console.log(nbof, " of(s) pour l'operateur ", IdOperateur);
        var listeof = '';
        if (nbof > 0) {
            listeof = '"'+document.getElementsByClassName('containerOF')[pas].getElementsByClassName('of')[0].getAttribute('traca')+'"';
        }
        for (let of = 1; of < nbof; of++) {
            var ofid = '"'+document.getElementsByClassName('containerOF')[pas].getElementsByClassName('of')[of].getAttribute('traca')+'"';
            console.log(ofid);
            listeof = listeof + ',' + ofid;
        }
        objectidentifiant = objectidentifiant+ identifiant + listeof + finidentiant+ virgule;
    }
    var chainejson = objectidentifiant + finobjectidentifiant;
    console.log(chainejson);
    var data = document.getElementById('SaisieEmetteur');
    data.setAttribute('value', chainejson);
    data.click();
}

getResolution();
function getResolution() {
    console.log("Votre résolution d'écran est: " + screen.width + "x" + screen.height);
}