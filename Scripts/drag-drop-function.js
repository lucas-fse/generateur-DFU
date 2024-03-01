const draggables = document.querySelectorAll('.of')
const containers = document.querySelectorAll('.containerOF')
const containerListe = document.getElementById('ContainerListeOF')

function allowDrop(ev) {
    /* The default handling is to not allow dropping elements. */
    /* Here we allow it by preventing the default behaviour. */
    ev.preventDefault();
}

function ondragstart(ev) {
    /* Here is specified what should be dragged. */
    /* This data will be dropped at the place where the mouse button is released */
    /* Here, we want to drag the element itself, so we set it's ID. */

    ev.dataTransfer.setData("text/html", ev.target.id);

    console.log(ev.dataTransfer.getData())

    this.classList.add('dragging');
}

function ondragend(ev) {
    this.classList.remove('dragging');
}

function drop(ev) {
    /* The default handling is not to process a drop action and hand it to the next 
     higher html element in your DOM. */
    /* Here, we prevent the default behaviour in order to process the event within 
       this handler and to stop further propagation of the event. */
    ev.preventDefault();

    /*"text/html", now we read it out */
    var data = ev.dataTransfer.getData("text/html");


    var nodeCopy = document.getElementById(data).cloneNode(true);
    nodeCopy.id = data + "tri"; /* We cannot use the same ID */

    const afterElement = getDragAfterElementX(ev.target, ev.clientX)

    if (afterElement == null) {
        ev.target.appendChild(nodeCopy)
    } else {
        ev.target.insertBefore(nodeCopy, afterElement)
    }
}

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

draggables.forEach(draggable => {
    draggable.addEventListener('dragstart', ondragstart, false);

})