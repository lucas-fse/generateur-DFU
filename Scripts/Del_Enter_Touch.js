var input = document.querySelectorAll("input");
input.forEach(
    (node, index) => {
        node.addEventListener("keydown", function (e) {
            if (e.keyCode == 13 & node.type == "text") {
                e.preventDefault();
            }
        })
    }
);