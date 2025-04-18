const container = document.querySelector('.sections-container');
const sections = document.querySelectorAll('.section');
let currentSection = 0;
let isScrolling = false;

// Molette (scroll souris)
window.addEventListener('wheel', (e) => {
    if (isScrolling) return;
    isScrolling = true;

    if (e.deltaY > 0 && currentSection < sections.length - 1) {
        currentSection++;
    } else if (e.deltaY < 0 && currentSection > 0) {
        currentSection--;
    }

    scrollToSection(currentSection);
});

// Touch (swipe mobile)
let startY = 0;
window.addEventListener('touchstart', (e) => {
    startY = e.touches[0].clientY;
});

window.addEventListener('touchend', (e) => {
    const endY = e.changedTouches[0].clientY;
    const deltaY = endY - startY;

    if (Math.abs(deltaY) < 50) return;

    if (deltaY < 0 && currentSection < sections.length - 1) {
        currentSection++;
    } else if (deltaY > 0 && currentSection > 0) {
        currentSection--;
    }

    scrollToSection(currentSection);
});

// Scroll vers la section avec animation
function scrollToSection(index) {
    container.style.transform = `translateY(-${index * window.innerHeight}px)`;
    setTimeout(() => {
        isScrolling = false;
    }, 600); // délai de scroll pour éviter les scrolls multiples
}

// Formatage de nombre avec séparateur si besoin
function formatNum(num) {
    return Number(num).toLocaleString('fr-FR');
}

async function refeshData() {
    const rawData = await fetch('/Home/GetKPI');
    const datas = await rawData.json();

    // Section Satisfaction Client
    document.querySelector('#FrcAnnee').textContent = formatNum(datas.FrcAnnee);
    document.querySelector('#FrcMois').textContent = formatNum(datas.FrcMois);
    document.querySelector('#ObjectifFrc').textContent = formatNum(datas.ObjectifFrc);

    const frcPercent = datas.ObjectifFrc ? (datas.FrcAnnee / datas.ObjectifFrc * 100) : 0;
    const frcBar = document.querySelector('#ObjectifFrcBar');
    frcBar.style.width = `${frcPercent}%`;
    frcBar.querySelector('p').textContent = `${formatNum(datas.FrcAnnee)} FRC`;

    // Section Sécurité
    document.querySelector('#NbJourSansAccident').textContent = formatNum(datas.NbJourSansAccident);
    document.querySelector('#NbSoinsBenins').textContent = formatNum(datas.NbSoinsBenins);
    document.querySelector('#NbPresqueAccident').textContent = formatNum(datas.NbPresqueAccident);
    document.querySelector('#NbSituationDangereuse').textContent = formatNum(datas.NbSituationDangereuse);
    document.querySelector('#NbActesDangereux').textContent = formatNum(datas.NbActesDangereux);


    // Section OTD/OTR
    document.querySelector('#OtdSemaine').textContent = formatNum(datas.OtdSemaine);
    document.querySelector('#OtdMois').textContent = formatNum(datas.OtdMois);
    document.querySelector('#OtrSemaine').textContent = formatNum(datas.OtrSemaine);
    document.querySelector('#OtrMois').textContent = formatNum(datas.OtrMois);
    document.querySelector('#ObjectifOtd').textContent = formatNum(datas.ObjectifOtd);
    document.querySelector('#ObjectifOtr').textContent = formatNum(datas.ObjectifOtr);

    const otdPercent = datas.ObjectifOtd ? (datas.OtdSemaine / datas.ObjectifOtd * 100) : 0;
    const otdBar = document.querySelector('#ObjectifOtdBar');
    otdBar.style.width = `${otdPercent}%`;
    //otdBar.querySelector('p').textContent = `${formatNum(datas.OtdSemaine)} %`;

    // Section Chiffre d'affaires
    document.querySelector('#CaMois').textContent = formatNum(datas.CaMois);
    document.querySelector('#CaMoisPrec').textContent = formatNum(datas.CaMoisPrec);
    document.querySelector('#CaAnnee').textContent = formatNum(datas.CaAnnee);


}

document.addEventListener("DOMContentLoaded", () => {
    refeshData();
});
