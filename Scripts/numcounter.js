const delta = 4;
let counter_actif = [];

function increment(elem, n) {
    elem.field.textContent = formatNum(Math.floor(n));

    if(n < elem.initial) {
        setTimeout(() => increment(elem, n + (elem.initial / 600)), delta);
    }
    if (n >= elem.initial) {
        elem.field.textContent = formatNum(Math.floor(elem.initial));
        counter_actif = counter_actif.filter(i => i !== elem.field);
    }
}


function formatNum(n) {
    let num = Math.round(n).toString(10);

    if (num.length <= 3) {
        return num;
    }

    return formatNum(num.substring(0, num.length - 3)) + " " + num.substring(num.length - 3);
}

function parseNum(str) {
    console.log('before', str);
    str = str.replaceAll(' ', '');
    console.log('after', str);
    console.log('');
    return Number.parseInt(str);
}


function animClass(className) {
    const counter_list = document.querySelectorAll(className);

    counter_list.forEach(i => {
        if(!counter_actif.includes(i)) {
            increment({ field: i, initial: parseNum(i.textContent)}, 0);
            counter_actif.push(i);
        }
    });
}

