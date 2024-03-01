let preview = document.querySelector('#preview');

preview.onchange = evt => {
    const [file] = preview.files
    if (file) {
        blah.src = URL.createObjectURL(file)
    }
}