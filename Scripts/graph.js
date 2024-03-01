
google.charts.load('current', { packages: ['corechart','geochart','gauge'] });
    google.charts.setOnLoadCallback(drawChart);

function drawChart(container) {
    // Define the chart to be drawn.
    var list = document.getElementsByClassName("ColumnChart")
    for (var y = 0; y < list.length; y++) {
        var data = new google.visualization.DataTable();
        var container = list[y];
        //var container = document.getElementById("myPieChart");
        var dnl = container.getElementsByTagName("li");
        var abscisse = "";
        var ordonnee = "";
        var titre = "";
        for (j = 0; j < container.attributes.length; j++) {
            if (container.attributes[j].name == "abscisse") {
                abscisse = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "ordonne") {
                ordonnee = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "titre") {
                titre = container.attributes[j].value;
            }
        }
        data.addColumn('string', abscisse);
        data.addColumn('number', ordonnee);
        for (var i = 0; i < dnl.length; i++) {
            var item = dnl.item(i);
            var value = item.innerHTML;
            var content = value.split(":");
            var color = item.style.background;

            data.addRows([[content[1], parseInt(content[0])]]);
        }
        //data.addRows([
        //    ['Nitrogen', 0.78],
        //    ['Oxygen', 0.21],
        //    ['Other', 0.01]
        //]);
        var options = {
            legend: { position: 'top', maxLines: 3 },
            bar: { groupWidth: '75%' },
            isStacked: true,
        };

        // Instantiate and draw the chart.

        var chart = new google.visualization.ColumnChart(container);
        chart.draw(data, options);
    }



    var list = document.getElementsByClassName("graphPieChart")
    for (var y = 0; y < list.length; y++) {
        var data = new google.visualization.DataTable();
        var container = list[y];
        //var container = document.getElementById("myPieChart");
        var dnl = container.getElementsByTagName("li");
        var abscisse = "";
        var ordonnee = "";
        var titre = "";
        for (j = 0; j < container.attributes.length; j++) {
            if (container.attributes[j].name == "abscisse") {
                abscisse = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "ordonne") {
                ordonnee = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "titre") {
                titre = container.attributes[j].value;
            }
        }
        data.addColumn('string', abscisse);
        data.addColumn('number', ordonnee);
        for (var i = 0; i < dnl.length; i++) {
            var item = dnl.item(i);
            var value = item.innerHTML;
            var content = value.split(":");
            var color = item.style.background;

            data.addRows([[content[1], parseInt(content[0])]]);
        }
        //data.addRows([
        //    ['Nitrogen', 0.78],
        //    ['Oxygen', 0.21],
        //    ['Other', 0.01]
        //]);
        var options = {
            title: titre,
            is3D: true,
        };

        // Instantiate and draw the chart.
        //var chart = new google.visualization.PieChart(document.getElementById('myPieChart'));
        var chart = new google.visualization.PieChart(container);
        chart.draw(data, options);
    }

    var list = document.getElementsByClassName("graphArrayChart")
    for (var y = 0; y < list.length; y++) {
        var data = new google.visualization.DataTable();
        var container = list[y];
        var abscisse = "";
        var ordonnee = "";
        var titre = "";
        var vAxisminValue = "";
        for (j = 0; j < container.attributes.length; j++) {
            if (container.attributes[j].name == "abscisse") {
                abscisse = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "ordonne") {
                ordonnee = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "titre") {
                titre = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "vaxisminvalue") {
                vAxisminValue = container.attributes[j].value;
            }
        }
        var dnl = container.getElementsByTagName("li");
        data.addColumn('string', abscisse);
        data.addColumn('number', ordonnee);
        for (var i = 0; i < dnl.length; i++) {
            var item = dnl.item(i);
            var value = item.innerHTML;
            var content = value.split(":");
            var color = item.style.background;
            data.addRows([[content[1], parseInt(content[0])]]);
        }
        //data.addRows([
        //    ['Nitrogen', 0.78],
        //    ['Oxygen', 0.21],
        //    ['Other', 0.01]
        //]);
        //Set chart options
        var options = {
            title: titre,
            vAxis: { minValue: vAxisminValue }
        }
        // Instantiate and draw the chart.
        //var chart = new google.visualization.PieChart(document.getElementById('myPieChart'));
        var chart = new google.visualization.ColumnChart(container);
        chart.draw(data, options);
    }

    var list = document.getElementsByClassName("graphArrayChart2")
    for (var y = 0; y < list.length; y++) {
        var data = new google.visualization.DataTable();
        var container = list[y];
        var abscisse = "";
        var ordonnee = "";
        var titre = "";
        var vAxisminValue = "";
        for (j = 0; j < container.attributes.length; j++) {
            if (container.attributes[j].name == "abscisse") {
                abscisse = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "ordonne1") {
                ordonnee1 = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "ordonne2") {
                ordonnee2 = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "ordonne3") {
                ordonnee3 = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "ordonne4") {
                ordonnee4 = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "titre") {
                titre = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "vaxisminvalue") {
                vAxisminValue = container.attributes[j].value;
            }
        }
        var dnl = container.getElementsByTagName("li");
        data.addColumn('string', abscisse);
        data.addColumn('number', ordonnee1);
        data.addColumn({ type: 'string', role: 'annotation' });
        data.addColumn({ type: 'string', role: 'annotationText' });
        data.addColumn('number', ordonnee2);
        data.addColumn({ type: 'string', role: 'annotation' });
        data.addColumn({ type: 'string', role: 'annotationText' });
        data.addColumn('number', ordonnee3);
        data.addColumn({ type: 'string', role: 'annotation' });
        data.addColumn({ type: 'string', role: 'annotationText' });
        data.addColumn('number', ordonnee4);
        data.addColumn({ type: 'string', role: 'annotation' });
        data.addColumn({ type: 'string', role: 'annotationText' });

        for (var i = 0; i < dnl.length; i++) {
            var item = dnl.item(i);
            var value = item.innerHTML;
            var content = value.split(":");
            var color = item.style.background;
            if (content[3] == "")
            {
                content[2] = null;
            }
            if (content[6] == "") {
                content[5] = null;
            }
            if (content[9] == "") {
                content[8] = null;
            }
            if (content[12] == "") {
                content[11] = null;
            }
            data.addRows([[content[0], parseInt(content[1]), content[2], content[3], parseInt(content[4]), content[5], content[6], parseInt(content[7]), content[8], content[9], parseInt(content[10]), content[11], content[12]]]);
        }
        //data.addRows([
        //    ['Nitrogen', 0.78],
        //    ['Oxygen', 0.21],
        //    ['Other', 0.01]
        //]);
        //Set chart options
        var options = {
            title: titre,
            vAxis: { minValue: vAxisminValue },
            bar: {
                groupWidth: "100%"
            },
            colors:['blue','green','orange','red']
        }
        // Instantiate and draw the chart.
        //var chart = new google.visualization.PieChart(document.getElementById('myPieChart'));
        var chart = new google.visualization.ColumnChart(container);
        chart.draw(data, options);
    }

    var list = document.getElementsByClassName("ComboChart")
    for (var y = 0; y < list.length; y++) {
        var data = new google.visualization.DataTable();
        var container = list[y];
        var abscisse = "";
        var ordonnee1 = "";
        var ordonnee2 = "";
        var titre = "";
        for (j = 0; j < container.attributes.length; j++) {
            if (container.attributes[j].name == "abscisse") {
                abscisse = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "ordonne1") {
                ordonnee1 = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "ordonne2") {
                ordonnee2 = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "titre") {
                titre = container.attributes[j].value;
            }
        }
        var dnl = container.getElementsByTagName("li");
        data.addColumn('string', abscisse);
        data.addColumn('number', ordonnee1);
        data.addColumn('number', ordonnee2);
        data.addColumn('number', 'objectif');
        data.addColumn('number', 'Nb Art');
        for (var i = 0; i < dnl.length; i++) {
            var item = dnl.item(i);
            var value = item.innerHTML;
            var content = value.split(":");
            var color = item.style.background;
            data.addRows([[content[0], parseInt(content[1]), parseInt(content[2]), parseInt(content[3]), parseInt(content[4])]]);
        }

        var options = {
            title: titre,

            series: {
                0: { type: 'bars', axis: 'Temps' },
                1: { type: 'bars', axis: 'Daylight' },
                2: { type: 'line' },
                3: { type: 'line', targetAxisIndex: 1 }
            }
        };

        var chart = new google.visualization.ComboChart(container);
        chart.draw(data, options);
    }


    var list = document.getElementsByClassName("GaugeChart")
    for (var y = 0; y < list.length; y++) {
        var data = new google.visualization.DataTable();
        var container = list[y];
        var abscisse = "";
        var ordonnee1 = "";
        var ordonnee2 = "";
        var titre = "";
        var width = "";
        var height = "";
        for (j = 0; j < container.attributes.length; j++) {
            if (container.attributes[j].name == "abscisse") {
                abscisse = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "ordonne1") {
                ordonnee1 = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "titre") {
                titre = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "style") {
                width = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "height") {
                height = container.attributes[j].value;
            }
        }
        var dnl = container.getElementsByTagName("li");
        data.addColumn('string', 'Label');
        data.addColumn('number', 'Value');
        var finred = 0;
        var finyellow = 0;
        var fingreen = 0;
        for (var i = 0; i < dnl.length; i++) {
            var item = dnl.item(i);
            var value = item.innerHTML;
            var content = value.split(":");
            var color = item.style.background;
            data.addRows([[content[0], parseInt(content[1])]]);
            finred = content[2];
            finyellow = content[3];
            fingreen = content[4];
        }

        var options = {
            redFrom: 0, redTo: 80,
            yellowFrom: 80, yellowTo: 98,
            greenFrom:98, greenTo:101,
            minorTicks: 5,
            width: width, height: height,
            max:100
        };
        options.redFrom = 0; options.redTo = finred;
       // options.yellowFrom = finred; options.yellowTo = finyellow;
        options.greenFrom = finyellow; options.greenTo = fingreen;
        var chart = new google.visualization.Gauge(container);
        chart.draw(data, options);
    }


    var list = document.getElementsByClassName("chartGeoCharts")
    for (var y = 0; y < list.length; y++) {
        var container = list[y];
        var dnl = container.getElementsByTagName("li");
        var abscisse = "";
        var ordonnee = "";
        var titre = "";
        for (j = 0; j < container.attributes.length; j++) {
            if (container.attributes[j].name == "abscisse") {
                abscisse = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "ordonne") {
                ordonnee = container.attributes[j].value;
            }
            else if (container.attributes[j].name == "titre") {
                titre = container.attributes[j].value;
            }
        }
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Country');
        data.addColumn('number', 'Nbr Cmd');

        for (var i = 0; i < dnl.length; i++) {
            var item = dnl.item(i);
            var value = item.innerHTML;
            var content = value.split(":");
            var color = item.style.background;

            data.addRows([[content[0], parseInt(content[1])]]);
        }
        var options = {};

        var chart = new google.visualization.GeoChart(container);

        chart.draw(data, options);
    }
}

