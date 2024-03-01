const draggables = document.querySelectorAll('.of')
const containers = document.querySelectorAll('.containerOF')
const containerListe = document.getElementById('ContainerListeOF')
var oldposition
var collectionop = new Map()
var ligneActive

/*Pour le principe général des fonctions drag and drop
 *https://www.youtube.com/watch?v=jfYWwQrtzzY/ */

draggables.forEach(draggable => {
    draggable.addEventListener('dragstart', e => {
        /*on récupère les valeurs de l'événement pour pouvoir créer une 
         * copie de l'of à sa position initial*/
        
        oldposition = e
        
        draggable.classList.add('dragging')
    })

    draggable.addEventListener('dragend', e => {
        draggable.classList.remove('dragging')

        /*On test si l'of à déjà été copié dans la liste
         si ce n'est pas le cas on crée une copie*/
        var testofliste = containerListe.getElementsByClassName(draggable.classList)

        if (testofliste.length == 0) {
            const afterElement = getDragAfterElementYDecaler(containerListe, oldposition.clientY)

            var clone = draggable.cloneNode(true)
            clone.id = clone.id + "tri"
            clone.classList.add('oftri')

            if (afterElement == null) {
                containerListe.appendChild(clone)
            } else {
                containerListe.insertBefore(clone, afterElement)
            }
        }
        oldposition = null

    })
})

containers.forEach(container => {
    container.addEventListener('dragover', e => {
        e.preventDefault()
        const afterElement = getDragAfterElementX(container, e.clientX)
        const draggable = document.querySelector('.dragging')


        if (afterElement == null) {
            container.appendChild(draggable)
        } else {
            container.insertBefore(draggable, afterElement)
        }

    })


    container.addEventListener('dragenter', e => {
        console.log(e.target)
        if (e.target.classList[0] != "containerOF") { return }

        ligneActive = container.parentElement.id
        const draggable = document.querySelector('.dragging')

        /*Pour gérer le calcul des temps opératoir de chaque opératrices:
         * 
         * On stock chaque ligne du tableau dans une collection
         * Clé = Nom de l'opératrice  ---  Valeur = la collection d'of
         * 
         * Pour la collection d'of on a :
         * Clé = numéro de l'of  ---  Valeur = Temps opératoire
         * 
         * Ainsi, on a une collection de toute les opératrices
         * et pour chaque opératrice on a une collection de tout
         * les of qui lui sont attribué
         /

        /*Récupération et passage en number la valeur du temps opératoire de l'of*/

        var tpsop = draggable.getElementsByClassName("tpsop")[0].innerHTML
        tpsop = parseFloat(tpsop.substring(5))

        /*Test si l'opératrice possède déjà une collection d'of
         si ce n'est pas le cas on en crée une sinon on la récupère
         puis on ajoute l'of avec son temps opératoir*/
        if (!collectionop.has(container.parentElement.id)) {
            var collectionof = new Map()
            collectionof.set(draggable.id, tpsop)
            collectionop.set(container.parentElement.id, collectionof)

        } else {
            var collectionof = collectionop.get(container.parentElement.id)

            collectionof.set(draggable.id, tpsop)
            collectionop.set(container.parentElement.id, collectionof)

        }

        TempsOperatoire(container.parentElement)

        
    })

    container.addEventListener('dragleave', e => {
        //if (e.target.parentElement.id != ligneActive) { return }
        if (e.target.classList[0] != "containerOF") { return }

        const draggable = document.querySelector('.dragging')

        var collectionof = collectionop.get(container.parentElement.id)

        collectionof.delete(draggable.id)
        collectionop.set(container.parentElement.id, collectionof)

        TempsOperatoire(container.parentElement)

        console.log("sortie")
        console.log(e.target)
        
    })
})

containerListe.addEventListener('dragover', e => {
    e.preventDefault()
    const afterElement = getDragAfterElementY(containerListe, e.clientY)
    const draggable = document.querySelector('.dragging')

    var classof = draggable.classList.toString()
    classof = classof.substring(0, 11)
    classof = classof + " oftri"

    var oftri = containerListe.getElementsByClassName(classof)
    console.log(oftri);
    if (oftri.item(0) != null) {
        oftri.item(0).parentNode.removeChild(oftri.item(0))
    }

    if (afterElement == null) {
        containerListe.appendChild(draggable)
    } else {
        containerListe.insertBefore(draggable, afterElement)
    }
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

function getDragAfterElementY(container, y) {
    const draggableElements = [...container.querySelectorAll('.of:not(.dragging)')]

    return draggableElements.reduce((closest, child) => {
        const box = child.getBoundingClientRect()
        const offset = y - box.top - box.height / 2
        if (offset < 0 && offset > closest.offset) {
            return { offset: offset, element: child }
        } else {
            return closest
        }
    }, { offset: Number.NEGATIVE_INFINITY }).element
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

function TempsOperatoire(container) {

    var collectionofTps = collectionop.get(container.id)
    var tpstotal = 0
    
    var HTMLtpstotal = container.getElementsByClassName("TpsTotal")[0]

    collectionofTps.forEach((tpsop, of) => {
        console.log(tpsop);
        tpstotal = parseFloat(tpstotal) + parseFloat(tpsop)

    })

    HTMLtpstotal.innerHTML = tpstotal.toFixed(2)
}