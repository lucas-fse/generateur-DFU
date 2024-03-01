// Listes des éléments déplacables
const draggables = document.querySelectorAll(".draggable");

let dadEnabled = true;


const infosDraggable = { element: null };

const callbacks_fn = {
    cb: (v1, v2) => { return false },
    dadEvent: () => { return false },
    startDragEvent: (e) => { return false }
};

const setCallback = function (f) {
    callbacks_fn.cb = f;
}

const setDragAndDropEvent = function (f) {
    callbacks_fn.dadEvent = f;
}

const setStartDragEvent = function (f) {
    callbacks_fn.startDragEvent = f;
}

const disableDragAndDrop = function () {
    dadEnabled = false;
}

// Permet de positionner un élément en fonction de la souris et du reste
const setToPosition = (e, element) => {
    const mouseX = e.clientX || e.changedTouches[e.changedTouches.length - 1].pageX;
    const mouseY = e.clientY || e.changedTouches[e.changedTouches.length - 1].pageY;

    const dragareas = document.querySelectorAll('.dragarea');

    let dropzone;

    // On regarde dans quel dropzone on est
    dropzone = [...dragareas].filter(d => {
        const rect = d.getBoundingClientRect();
        return (Math.abs(mouseX - (rect.left + rect.width / 2)) < rect.width / 2 && Math.abs(mouseY - (rect.top + rect.height / 2)) < rect.height / 2);
    });

    // On regare s'il faut placer automatiquement l'élément
    if (infosDraggable.element.getAttribute('autoplace') != null) {

        // On regarde si on est sur la zone poubelle
        if (dropzone.length > 0 && dropzone[0].getAttribute('trash') == null) {
            dropzone = [document.querySelector('.dragarea[zone="' + infosDraggable.element.getAttribute('zone') + '"]')];
        }
    }

    // On regarde que la zone match bien ou qu'il s'agit d'une zone poubelle
    if (dropzone.length > 0 && (infosDraggable.element.getAttribute('zone') == dropzone[0].getAttribute('zone') || dropzone[0].getAttribute('trash') != null)) {

        const dg = dropzone[0].querySelectorAll('.draggable:not(.moving)');

        // On regarde si c'est une zone vertical pour le tri
        const vertical_zone = dropzone[0].classList.contains('dragarea-colonne');

        if (dg.length == 0) {
            dropzone[0].appendChild(element);
        }
        else {
            let dist_min = 100000;
            let plus_proche = null;

            // On trouve l'item le plus proche
            [...dg].forEach(d => {
                const rect = d.getBoundingClientRect();
                let dist = (Math.sqrt(Math.pow(mouseX - (rect.left + rect.width / 2), 2) + Math.pow(mouseY - (rect.top + rect.height / 2), 2) * 2));
                if (dist < dist_min && (Math.abs(!vertical_zone && mouseY - (rect.top + rect.height / 2)) < 50 || vertical_zone && Math.abs(mouseX - (rect.left + rect.width / 2)) < 50)) {
                    dist_min = dist;
                    plus_proche = d;
                }
            });

            // On insert juste à coté
            if (plus_proche) {
                const rect = plus_proche.getBoundingClientRect();
                if (vertical_zone) {
                    plus_proche.insertAdjacentElement((mouseY - (rect.top + rect.height / 2) < 0) ? 'beforebegin' : 'afterend', element);
                }
                else {
                    plus_proche.insertAdjacentElement((mouseX - (rect.left + rect.width / 2) < 0) ? 'beforebegin' : 'afterend', element);
                }
            }
            else {
                dropzone[0].appendChild(element);
            }

        }
        return dropzone[0].getAttribute('value') || true;

    }
    else {
        return null;
    }

}

// événement de clique
let eventTimeout;
let preDrag = false;


const drop = (e) => {

    console.log("DROP");
    clearTimeout(eventTimeout);

    if (preDrag) {
        preDrag = false;
    }
   
    else if (infosDraggable.element) {

        const val = setToPosition(e, infosDraggable.element);
        infosDraggable.element.classList.remove('moving');
        document.querySelector('.preview').style.display = 'none';

        console.log(infosDraggable.element.getAttribute('value') + " -> " + val);
        callbacks_fn.cb(infosDraggable.element.getAttribute('value'), val);



        infosDraggable.element = null;
        infosDraggable.parent.style.overflow = 'scroll';

        callbacks_fn.dadEvent();
        infosDraggable.parent.style.overflow = 'scroll';

    }

    infosDraggable.element = null;

}

let mX = 0;
let mY = 0;

const move = (e) => {
    clearTimeout(eventTimeout);

    if (preDrag) {
        infosDraggable.parent.style.overflow = 'hidden';
        infosDraggable.element.classList.add('moving');
        preDrag = false;

        callbacks_fn.startDragEvent(infosDraggable.element);

        move(e);
    }

    else if (infosDraggable.element) {

        let y = e.clientY || e.changedTouches[e.touches.length - 1].clientY;
        let x = e.clientX || e.changedTouches[e.touches.length - 1].clientX;

        infosDraggable.element.style.top = y + "px";
        infosDraggable.element.style.left = x + "px";

        if (setToPosition(e, document.querySelector('.preview'))) {
            document.querySelector('.preview').style.display = 'block';
        }
        else {
            document.querySelector('.preview').style.display = 'none';
        }
    }
}


draggables.forEach(i => {
    const take = (e) => {

        if (dadEnabled) {

            const parentNode = e.currentTarget.parentNode;

            const takeFn = () => {


                console.log("TAKE");

                infosDraggable.parent = parentNode;
                infosDraggable.element = i;

                infosDraggable.parent.style.overflow = 'hidden';
                infosDraggable.element.classList.add('moving');

                callbacks_fn.startDragEvent(infosDraggable.element);


                move(e);

            }

            // délai sur écran tactile pour permettre le scroll
            if (e.clientX == null) {
                console.log("TACTILE");
                eventTimeout = setTimeout(takeFn, 300);
            }
            else {
                console.log("PC");
                preDrag = true;

                infosDraggable.parent = parentNode;
                infosDraggable.element = i;
            }
        }


    }

    i.addEventListener('mousedown', e => {
        // On filtre les evenenement qui viennent d'un evenement tactile
        if (!e.sourceCapabilities.firesTouchEvents) {
            take(e)
        }
    });

    i.addEventListener('touchstart', e => {
        if (infosDraggable.element == null)
            take(e)
        else e.preventDefault()
    });
    
});

window.addEventListener('mouseup', e => drop(e));
window.addEventListener('touchend', e => drop(e));

window.addEventListener('mousemove', e => move(e));
window.addEventListener('touchmove', e => move(e)); 