//permet de faire en sorte que la taille des textarea augmente automatiquement

$("textarea").each(function () {
  this.setAttribute("style", "height:" + (this.scrollHeight) + "px ;overflow-y: auto;")
}).on("input", function () {
  this.style.height = "auto";
  this.style.height = (this.scrollHeight) + "px"
});

//rendre les items de communications editables :

var edits = document.querySelectorAll("span.edit")

edits.forEach(
    (node, index) => {

        node.onclick = function () {

            var input = node.previousElementSibling

            if (node.getAttribute("confirmer") != "true") {
                node.setAttribute("confirmer", "true")
                node.innerHTML = 'Confirmer <img src="/image/validate.png"/>'
                
                input.disabled = false
            }
            else {
                node.textContent = "Editer"
                node.setAttribute("confirmer", "false")
                input.disabled = true
            }
       
        }
    }
)

//rendre les items de gestions des aléas éditables :

var editRows = document.querySelectorAll("span.editLine")

editRows.forEach(
    (node, index) => {
        node.onclick = function () {

            var editableInput

            if (node.getAttribute("confirmer") != "true") {
                node.setAttribute("confirmer", "true")
                node.innerHTML = 'Confirmer <img src="/image/validate.png"/>'

                editableInput = false
            }
            else {
                node.innerHTML = 'Editer'
                node.setAttribute("confirmer", "false")
                editableInput = true
            }

            var inputs = editRows[index].parentNode.parentNode.querySelectorAll("input, textarea")

            inputs.forEach(
                (node, index) => {
                    node.disabled = editableInput
                }
            )
        }
    }
)

document.querySelector("#submitButton").onclick = function () {
    var inputs = document.querySelectorAll('input[beforesendform="true"], textarea[beforesendform="true"]')

    inputs.forEach(
        (node, index) => {
            node.setAttribute("beforesendform", "false")
            node.disabled = false
            node.readOnly = true
        }
    )
}