const sections = document.querySelectorAll('.section');

let diapo_index = 0;

let animation;

function startAnimation() {
    animation = setInterval(async () => {
        diapo_index = (diapo_index + 1) % 4;
        await refreshData();
        openOne(diapo_index);
    }, 10000);
}

function stopAnimation() {
    clearInterval(animation);
}

function restartAnimation() {
    stopAnimation();
    startAnimation();
}

sections.forEach((i, n) => {
    i.addEventListener('click', (e) => {
        stopAnimation();
        setTimeout(restartAnimation, 60000);
        if(!i.classList.contains("section-open")) {
            openOne(n);
        }
        else {
            i.classList.remove('section-open');
        }
    })
});

function openOne(n) {

    diapo_index = n;

    const i = document.querySelectorAll('.section')[n];

    closeAll();
    i.classList.add('section-open');
    animClass(".section-open .infossup .counter");
}

function closeAll() {
    sections.forEach(i => i.classList.remove('section-open'));
}

window.onload = async () => {
    await refreshData();
    startAnimation();
    animClass("h1 > .counter");

    document.querySelector('.conductix-logo').addEventListener('click', () => {
        diapo_index = (diapo_index + 1) % 4;
        openOne(diapo_index);
        restartAnimation();
    });
}

async function refreshData() {
    const rawData = await fetch('/Home/GetKPI');
    const datas = await rawData.json();

    

    document.querySelector('#FrcAnnee').textContent = formatNum(datas.FrcAnnee);
    document.querySelector('#FrcMois').textContent = formatNum(datas.FrcMois);
    document.querySelector('#ObjectifFrc').textContent = formatNum(datas.ObjectifFrc);
    document.querySelector('#ObjectifFrcBar').style.width = (datas.FrcAnnee / datas.ObjectifFrc)*100 + "%";

    document.querySelector('#NbJourSansAccident').textContent = formatNum(datas.NbJourSansAccident);
    document.querySelector('#NbSoinsBenins').textContent = formatNum(datas.NbSoinsBenins);
    document.querySelector('#NbPresqueAccident').textContent = formatNum(datas.NbPresqueAccident);
    document.querySelector('#NbSituationDangereuse').textContent = formatNum(datas.NbSituationDangereuse);
    document.querySelector('#NbActesDangereux').textContent = formatNum(datas.NbActesDangereux);

    document.querySelector('#OtdSemaine').textContent = formatNum(Number.parseInt(datas.OtdSemaine));
    document.querySelector('#OtdMois').textContent = formatNum(datas.OtdMois);

    document.querySelector('#OtrSemaine').textContent = formatNum(datas.OtrSemaine);
    document.querySelector('#OtrMois').textContent = formatNum(datas.OtrMois);

    document.querySelector('#ObjectifOtd').textContent = formatNum(datas.ObjectifOtd);
    document.querySelector('#ObjectifOtr').textContent = formatNum(datas.ObjectifOtr);

    document.querySelector('#ObjectifOtdBar').style.width = formatNum(datas.ObjectifOtd + "%");

    document.querySelector('#CaMoisPrec').textContent = formatNum(datas.CaMoisPrec);
    document.querySelector('#CaMois').textContent = formatNum(datas.CaMois);
    document.querySelector('#CaAnnee').textContent = formatNum(datas.CaAnnee);
    
}

