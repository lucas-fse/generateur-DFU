// textarea auto-resize
$("textarea").each(function () {
    this.setAttribute("style", "height:" + this.scrollHeight + "px;overflow-y: auto;");
}).on("input", function () {
    this.style.height = "auto";
    this.style.height = (this.scrollHeight) + "px";
});

// gestion communication : span.edit (inchangé)
document.querySelectorAll("span.edit").forEach((node) => {
    node.onclick = function () {
        const input = node.previousElementSibling;
        const isConfirming = node.getAttribute("confirmer") === "true";

        if (!isConfirming) {
            node.setAttribute("confirmer", "true");
            node.innerHTML = 'Confirmer <img src="/image/validate.png"/>';
            input.disabled = false;
        } else {
            node.textContent = "Editer";
            node.setAttribute("confirmer", "false");
            input.disabled = true;
        }
    };
});

// gestion des aléas : button.editLine
document.querySelectorAll("button.editLine").forEach((btn) => {
    btn.addEventListener("click", () => {
        const isConfirming = btn.getAttribute("confirmer") === "true";
        const row = btn.closest("tr"); // plus sûr que parentNode.parentNode
        const inputs = row.querySelectorAll("input, textarea");

        if (!isConfirming) {
            btn.setAttribute("confirmer", "true");
            btn.innerHTML = 'Confirmer';
            btn.classList.remove("btn-outline-orange");
            btn.classList.add("btn-success");
            inputs.forEach((input) => (input.disabled = false));
        } else {
            btn.setAttribute("confirmer", "false");
            btn.innerHTML = "Editer";
            btn.classList.remove("btn-success");
            btn.classList.add("btn-outline-orange");
            inputs.forEach((input) => (input.disabled = true));
        }
    });
});

// déblocage avant submit
document.querySelector("#submitButton").onclick = function () {
    document.querySelectorAll('input[beforesendform="true"], textarea[beforesendform="true"]').forEach((node) => {
        node.setAttribute("beforesendform", "false");
        node.disabled = false;
        node.readOnly = true;
    });
};
