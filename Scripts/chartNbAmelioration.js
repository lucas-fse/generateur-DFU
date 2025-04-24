/*
  Script pour créer une charte qui affiche le nombre de FRC par service
*/
var chartObj;
var data;
var option;
var chartElem = document.getElementById('Graphique').getContext('2d');


function GraphSecteur(val1) {

    console.log(val1)

    document.getElementById("Graphique").remove();
    var content1 = val1.split(";");
    var contenta = content1[0].split(":");
    var contentb = content1[1].split(":");


    var SecteurElem = document.createElement('canvas');
    document.getElementById('canvas1').append(SecteurElem);
    SecteurElem.id = 'Graphique';
    var chartSecteurElem = SecteurElem.getContext('2d');

    option = {
        plugins: {
            title: {
                display: true,
                text: 'Tendances par secteurs',
                font: {
                    size: 30
                }
            },
            legend: {
                display: false
            }
        },

        scales: {
            y: {
                beginAtZero: true, //pour commencer à partir de zero en ordonnée
                title: {
                    display: true,
                    align: 'end',
                    text: 'Nombre de Propositions'
                },
                stacked: true //on veut empiler les barres
            },
            x: {
                stacked: true //on veut empiler les barres
                , title: {
                    display: true,
                    align: 'end',
                    text: 'secteurs'
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

    data = {
        labels: contentb, //axe (Ox) labels
        datasets: [{
            label: "Production", //y label
            data: contenta, //donnes pour chaque barre
            backgroundColor: [ //couleurs de fond des barres
                '#FF6600', '#E10000', '#00BF2B', '#A900DF', '#FFCF0D', '#318ce7',
            ],
            borderColor: [ //couleurs des bordures des barres
                'rgba(255, 99, 132, 1)',
            ],
            borderWidth: 0 //épaisseur bordure des barres
        }

        ]
    }

    chartObj = new Chart(chartSecteurElem, {
        type: 'bar', //type de graphe
        data: data,
        options: option
    });
}

function GraphService(val1) {

    console.log(val1);
    document.getElementById("Graphique").remove();
    var ServiceElem = document.createElement('canvas');
    document.getElementById('canvas1').append(ServiceElem);
    ServiceElem.id = 'Graphique';
    var chartServiceElem = ServiceElem.getContext('2d');
    var content1 = val1.split(";");
    var contenta = content1[0].split(":");
    var contentb = content1[1].split(":");

    option = {
        plugins: {
            title: {
                display: true,
                text: 'Tendances par services',
                font: {
                    size: 30
                }
            },
            legend: {
                display: false
            }
        },

        scales: {
            y: {
                beginAtZero: true, //pour commencer à partir de zero en ordonnée
                title: {
                    display: true,
                    align: 'end',
                    text: 'Nombre de Propositions'
                },
                stacked: true //on veut empiler les barres
            },
            x: {
                stacked: true //on veut empiler les barres
                , title: {
                    display: true,
                    align: 'end',
                    text: 'services'
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

    data = {
        labels: contentb, //axe (Ox) labels
        datasets: [{
            label: "Production", //y label
            data: contenta, //donnes pour chaque barre
            backgroundColor: [ //couleurs de fond des barres
                '#FF6600', '#E10000', '#00BF2B', '#A900DF', '#FFCF0D', '#0050E4', '#02DDC2',
            ],
            borderColor: [ //couleurs des bordures des barres
                'rgba(255, 99, 132, 1)',
            ],
            borderWidth: 0 //épaisseur bordure des barres
        }

        ]
    }

    chartObj = new Chart(chartServiceElem, {
        type: 'bar', //type de graphe
        data: data,
        options: option
    });
}

function GraphStatus(val1) {

    document.getElementById("Graphique").remove();
    var StatusElem = document.createElement('canvas');
    document.getElementById('canvas1').append(StatusElem);
    StatusElem.id = 'Graphique';
    var chartStatusElem = StatusElem.getContext('2d');
    var content1 = val1.split(";");
    var contenta = content1[0].split(":");
    var contentb = content1[1].split(":");

    option = {
        plugins: {
            title: {
                display: true,
                text: 'Tendances par status',
                font: {
                    size: 30
                }
            },
            legend: {
                display: false
            }
        },

        scales: {
            y: {
                beginAtZero: true, //pour commencer à partir de zero en ordonnée
                title: {
                    display: true,
                    align: 'end',
                    text: 'Nombre de Propositions'
                },
                stacked: true //on veut empiler les barres
            },
            x: {
                stacked: true //on veut empiler les barres
                , title: {
                    display: true,
                    align: 'end',
                    text: 'status'
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

    data = {
        labels: contentb, //axe (Ox) labels
        datasets: [{
            label: "Production", //y label
            data: contenta, //donnes pour chaque barre
            backgroundColor: [ //couleurs de fond des barres
                '#FF6600', '#E10000', '#00BF2B', '#A900DF', '#FFCF0D',
            ],
            borderColor: [ //couleurs des bordures des barres
                'rgba(255, 99, 132, 1)',
            ],
            borderWidth: 0 //épaisseur bordure des barres
        }

        ]
    }

    chartObj = new Chart(chartStatusElem, {
        type: 'bar', //type de graphe
        data: data,
        options: option
    });
}





option = {
    plugins: {
        title: {
            display: true,
            text: ''
        },
        legend: {
            display: false
        }
    },

    scales: {
        y: {
            beginAtZero: true, //pour commencer à partir de zero en ordonnée
            title: {
                display: true,
                align: 'end',
                text: ''
            },
            stacked: true //on veut empiler les barres
        },
        x: {
            stacked: true //on veut empiler les barres
            , title: {
                display: true,
                align: 'end',
                text: ''
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

data = {
    labels: [' ', ' ', ' ', ' ', ' '], //axe (Ox) labels
    datasets: [{
        label: "Production", //y label
        data: [0, 0, 0, 0, 0], //donnes pour chaque barre
        backgroundColor: [ //couleurs de fond des barres
            '#FF6600',
        ],
        borderColor: [ //couleurs des bordures des barres
            'rgba(255, 99, 132, 1)',
        ],
        borderWidth: 0 //épaisseur bordure des barres
    }

    ]
}

chartObj = new Chart(chartElem, {
    type: 'bar', //type de graphe
    data: data,
    options: option
});


