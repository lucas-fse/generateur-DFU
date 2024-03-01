
/*
  Script pour créer une charte qui affiche le nombre de FRC par service
*/
var chartNbFrcParService = document.getElementById("chartDataFrcParService1");
const chartParServiceElem = document.getElementById('chartNbFrcParService').getContext('2d');
var legendOn = chartNbFrcParService.getAttribute("legend");
const optionsParServiceAvecLegend = {
    plugins: {
        legend: {
            display: true
        },
     title: {
       display: true,
       text: 'Nombre de FRC par service'
     }
   },

    scales: {
        y: {
            beginAtZero: true, //pour commencer à partir de zero en ordonnée
            title :{
              display: true,
              align: 'end',
              text: 'Nombre de FRC'
            },
            stacked: true //on veut empiler les barres
        },
        x: {
          stacked: true //on veut empiler les barres
          , title: {
            display: true,
            align: 'end',
            text: 'mois'
          }
        }
    },

    animation: {
    //  duration: 1250,
    //  easing: 'easeOutBounce' // doc animation : https://www.chartjs.org/docs/latest/configuration/animations.html#easing

    },
    responsive: true //pour que la charte soit responsive
    , maintainAspectRatio: false
}
const optionsParServiceSansLegend = {
    plugins: {
        legend: {
            display: false
        },
        title: {
            display: true,
            text: 'Nombre de FRC par service'
        }
    },

    scales: {
        y: {
            beginAtZero: true, //pour commencer à partir de zero en ordonnée
            title: {
                display: true,
                align: 'end',
                text: 'Nombre de FRC'
            },
            stacked: true //on veut empiler les barres
        },
        x: {
            stacked: true //on veut empiler les barres
            , title: {
                display: true,
                align: 'end',
                text: 'mois'
            }
        }
    },

    animation: {
        //  duration: 1250,
        //  easing: 'easeOutBounce' // doc animation : https://www.chartjs.org/docs/latest/configuration/animations.html#easing

    },
    responsive: true //pour que la charte soit responsive
    , maintainAspectRatio: false
}

var labels = ['Janvier', 'Fevrier', 'Mars', 'Avril', 'Mai', 'Juin', 'Juillet', 'Août', 'Septembre', 'Octobre', 'Novembre', 'Décembre'];

var datasetsValue = [];
//dataParService2.datasets = new datasets();
var dnl = chartNbFrcParService.getElementsByTagName("li");
for (var i = 0; i < dnl.length; i++) {
    var item = dnl.item(i);
    var value = item.innerHTML;
    var content = value.split(":");
    var newDataset = {
        label: content[0],
        backgroundColor: content[1],
        borderColor: content[2],
        
        data: [parseInt(content[3]), parseInt(content[4]), parseInt(content[5]), parseInt(content[6]), parseInt(content[7]), parseInt(content[8]), parseInt(content[9]),
            parseInt(content[10]), parseInt(content[11]), parseInt(content[12]), parseInt(content[13]), parseInt(content[14])],
        type: content[15],

    }
    datasetsValue[i] = newDataset;
}

var dataParService2 = {
    labels: labels,
    datasets: datasetsValue
}

 const dataParService = {
    labels: ['Janvier', 'Fevrier', 'Mars', 'Avril', 'Mai', 'Juin', 'Juillet', 'Août', 'Septembre', 'Octobre', 'Novembre', 'Décembre'], //axe (Ox) labels
    datasets: [
    {
        label: "Production", //y label
        data: [1, 2, 2, 3, 3, 3, 4, 4, 5, 5, 5, 6], //donnes pour chaque barre
        backgroundColor: [ //couleurs de fond des barres
            'rgba(255, 99, 132, 0.7)',
        ],
        borderColor: [ //couleurs des bordures des barres
            'rgba(255, 99, 132, 1)',
        ],
            borderWidth: 0 //épaisseur bordure des barres
    },
    {
      label: "Ventes", //y label
      data: [1, 2, 2, 3, 3, 3, 4, 4, 5, 5, 6, 7], //donnes pour chaque barre
      backgroundColor: [ //couleurs de fond des barres

          'rgba(54, 162, 235, 0.7)',
      ],
      borderColor: [ //couleurs des bordures des barres
          'rgba(54, 162, 235, 1)',
      ],
            borderWidth: 0 //épaisseur bordure des barres
    },
    {
        label: "Service 3", //y label
        data: [1, 1, 2, 2, 2, 2, 3, 3, 4, 4, 4, 4], //donnes pour chaque barre
        backgroundColor: [ //couleurs de fond des barres
            'rgba(255, 150, 0, 0.7)',

        ],
        borderColor: [ //couleurs des bordures des barres

            'rgba(255, 206, 86, 1)',
        ],
            borderWidth: 0 //épaisseur bordure des barres
    },
    {
        label: "Service 4", //y label
        data: [0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 3, 6], //donnes pour chaque barre
        backgroundColor: [ //couleurs de fond des barres


            'rgba(153, 102, 255, 0.7)',
        ],
        borderColor: [ //couleurs des bordures des barres
            'rgba(153, 102, 255, 1)',
        ],
            borderWidth: 0 //épaisseur bordure des barres
    },
    {
        label: "Support", //y label
        data: [1, 1, 1, 1, 1, 2, 2, 2, 2, 4, 5, 5], //donnes pour chaque barre
        backgroundColor: [ //couleurs de fond des barres

            'rgba(75, 192, 192, 0.7)',
        ],
        borderColor: [ //couleurs des bordures des barres
            'rgba(75, 192, 192, 1)',
        ],
        borderWidth: 0 //épaisseur bordure des barres
    },
    {
        label: "Service 6", //y label
        data: [0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 2], //donnes pour chaque barre
        backgroundColor: [ //couleurs de fond des barres
            'rgba(255, 0, 0, 0.7)',
        ],
        borderColor: [ //couleurs des bordures des barres
            'rgba(255, 159, 64, 1)',
        ],
            borderWidth: 0 //épaisseur bordure des barres
    }
    ,
    {
        label: "Service 7", //y label
        data: [0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 3, 4], //donnes pour chaque barre
        backgroundColor: [ //couleurs de fond des barres
            'rgba(0,255,255,0.7)',
        ],
        borderColor: [ //couleurs des bordures des barres
            'rgba(200, 200, 200, 1)',
        ],
            borderWidth: 0 //épaisseur bordure des barres
    },
    {
        label: "R&D", //y label
        data: [0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 3, 3], //donnes pour chaque barre
        backgroundColor: [ //couleurs de fond des barres
            'rgba(0, 0, 0, 0.65)',
        ],
        borderColor: [ //couleurs des bordures des barres
            'rgba(0, 0, 0, 1)',
        ],
        borderWidth: 0 //épaisseur bordure des barres
    },
    {
        label: "Logistique", //y label
        data: [0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 3], //donnes pour chaque barre
        backgroundColor: [ //couleurs de fond des barres

            'rgba(127, 0, 255, 0.8)',
            /*'rgba(0, 255, 0, 0.2)', */
        ],
        borderColor: [ //couleurs des bordures des barres
            'rgba(0, 255, 0, 1)',
        ],
        borderWidth: 0 //épaisseur bordure des barres
    },
    {
        label: "SAV", //y label
        data: [1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 4, 6], //donnes pour chaque barre
        backgroundColor: [ //couleurs de fond des barres




            'rgba(0, 255, 0, 0.4)',
        ],
        borderColor: [ //couleurs des bordures des barres

            /*'rgba(0, 255, 0, 1)',*/
        ],
        borderWidth: 0 //épaisseur bordure des barres
    },
    {
       type: 'line',
       label: 'Objectif',
       data: [5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60],
       backgroundColor: 'rgba(0, 0, 255, 0.2)',
       borderColor: 'rgba(0, 0, 255, 0.6)',
       lineTension: 0,
       pointRadius: 3
   }
  ]
}
chartNbFrcParService.style.display = "none";

if (legendOn == "true") {
    chartParServiceObj = new Chart(chartParServiceElem, {
        type: 'bar', //type de graphe
        data: dataParService2,
        options: optionsParServiceAvecLegend
    });
}
else {
    chartParServiceObj = new Chart(chartParServiceElem, {
        type: 'bar', //type de graphe
        data: dataParService2,
        options: optionsParServiceSansLegend
    });
}
