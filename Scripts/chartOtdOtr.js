/* ------------------------------------------------------------
                  Script pour la charte graphique OTD OTR
  ------------------------------------------------------------ */

/* début données datasets */
var container = document.getElementById("DataKPIProd")
var datasetsValue = [];
var dnl = container.getElementsByTagName("li");
for (var i = 0; i < dnl.length; i++) {
    var item = dnl.item(i);
    var value = item.innerHTML;
    var content = value.split(":");
    var newDataset = {
        yAxisID: content[content.length - 2].trim(),
        label: content[0].trim(),
        backgroundColor: content[1],
        borderColor: content[2],

    }
    newDataset.data = new Array();
    for (var j = 3; j < content.length - 3; j++)
    {
        newDataset.data.push(parseInt(content[j]));
    }    
    newDataset.type = content[content.length - 3];
    datasetsValue[i] = newDataset;
}


var xValues = new Array();


for (var i = 0; i < newDataset.data.length; i++) {
  xValues.push(i+1)

}

/* fin données datasets */

const chartParServiceElem = document.getElementById('chartNbFrcParService').getContext('2d');

const optionsParService = {
   plugins: {
     title: {
       display: true,
           text: 'OTD/OTR',
           font: {
               size: 20
           }
     },
     tooltip: { //info bulle
       interaction: {
         //mode: 'index' //ca permet d'afficher tous les labels d'un coup
       },
       filter: function (tooltipItem, data) { //filtrage des info bulle => permet de filtrer lesquelles on veut afficher ou non
            if (tooltipItem.dataset.label == "Objectif") {
              return false // => on affichage pas celle de objectif (car c'est tout le temps la même valeur et la taille des bordures des points est à zero)
            }
            else {
              return true //on affiche les info bulles des autres graphes
            }
       },
       usePointStyle: true,
       displayColors: true, //afficher le carré de couleur
       borderWidth: 0,
       callbacks: {
         title: function(context) {
           var numSemaine = context[0].label
           return "semaine n°" + numSemaine
         },
         label: function(context) {
           if (context.dataset.label == "OTD" || context.dataset.label == "OTR") {
             return context.dataset.label + ": " + context.formattedValue + ' %'
           }
           else {
             return context.dataset.label + ": " + context.formattedValue
           }
         }
       }
     }
   },

    scales: {
        y: [
        {
            id: 'Pourcent',
            position: "left",
            beginAtZero: true, //pour commencer à partir de zero en ordonnée
            title : {
              display: true, //afficher la légende axe (Oy)
              align: 'end', //postion légende axe (Oy)
              text: 'Nombre de FRC' //legende axe (Oy)
            },
            ticks: {
              min: 0, //val min axe (Oy)
              max: 100,// valeur max axe (Oy)
              callback: function (value) {
               return value + '%'; // convertir en notation pourcentage
              },
            }
        },
            {
                id: 'CA',
                position: "right",
            beginAtZero: true, //pour commencer à partir de zero en ordonnée
            title: {
                display: true, //afficher la légende axe (Oy)
                align: 'end', //postion légende axe (Oy)
                text: 'Nombre de FRC' //legende axe (Oy)
            },
            ticks: {
                min: 0, //val min axe (Oy)
                max: 500000,// valeur max axe (Oy)
                callback: function (value) {
                    return value + '€'; // convertir en notation pourcentage
                },
            }
        }],
        x: {
            title: {
            display: true, //afficher légende axe (Ox)
            align: 'end', //postion légende axe (Ox)
            text: 'semaine' //légende axe (Ox)
          }

        }
    },
    animation: {
    //  duration: 1250,
    //  easing: 'easeOutBounce' // doc animation : https://www.chartjs.org/docs/latest/configuration/animations.html#easing
    },
    responsive: true, //pour que la charte soit responsive
    maintainAspectRatio: false
}


const dataParService = {
    labels: xValues, //axe (Ox) labels
    datasets: datasetsValue
}

const chartParServiceObj = new Chart(chartParServiceElem, {
    type: 'bar', //type de graphe
    data: dataParService,
    options: optionsParService
});
