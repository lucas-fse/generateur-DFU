const frmView = {
    inputs: document.querySelectorAll('.frm-input'),
    submitBtn: document.querySelector('#frm-submit'),
    backBtn: document.querySelector('#frm-back'),
    form: document.querySelector('form'),

    testForSubmit: function () {
        let valid = true;

        this.inputs.forEach(i => {
            valid &= (!i.classList.contains('frm-error'));

            if (i.classList.contains('frm-required')) {
                valid &= (i.value.length > 0);
            }
        });

        this.submitBtn.disabled = !valid;
    },

    addError: function (id) {
        const e = document.querySelector(id);
        e.classList.add('frm-error');
        this.testForSubmit();
    },

    removeError: function (id) {
        const e = document.querySelector(id);
        e.classList.remove('frm-error');
        this.testForSubmit();
    }
}

frmView.inputs.forEach(i => {
    i.addEventListener('keyup', () => {
        frmView.testForSubmit();
    });
});

frmView.backBtn.addEventListener('click', () => {
    history.back();
});

frmView.submitBtn.addEventListener('click', () => {
    frmView.submitBtn.disabled = true;
    frmView.submitBtn.innerHTML = '<div style="font-size: 16pt"><i class="fa-solid fa-spinner fa-spin"></i></div>';
    frmView.form.submit();
});