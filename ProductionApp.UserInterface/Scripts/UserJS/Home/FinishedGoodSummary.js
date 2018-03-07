$(function () {

    FinishedGoodSummaryGraph();

});


function FinishedGoodSummaryGraph() {
    'use strict';

    var list = FGModel.FinishedGoodsSummaryList;
    var jsonData = [];
    //for (i = 0; i < list.length; i++) {
    //    debugger;
    //    jsonData.push({ value: list[i].Value, color: list[i].Color, highlight: list[i].Color, label: list[i].Category })
    //}


    var data = [];
    var lbl = [];
    var colrs = [];

    for (i = 0; i < list.length; i++) {
        debugger;
        data.push(list[i].Value);
        lbl.push(list[i].Category);
        colrs.push(list[i].Color);


       
    }

    var pieData = {
        datasets: [{ data: data, backgroundColor: colrs }],
        labels: lbl

    };



    // -------------
    // - PIE CHART -
    // -------------
    // Get context with jQuery - using jQuery's .get() method.
    var pieChartCanvas = $('#FGChart').get(0).getContext('2d');
    var pieChart = new Chart(pieChartCanvas);
   

    


    options = {

        tooltips: {
            callbacks: {
                label: function(tooltipItem, data) {
                    //get the concerned dataset
                    var dataset = data.datasets[tooltipItem.datasetIndex];
                    var lbl = data.labels[tooltipItem.index];
                    //calculate the total of this data set
                    var total = dataset.data.reduce(function(previousValue, currentValue, currentIndex, array) {
                        return previousValue + currentValue;
                    });
                    //get the current items value
                    var currentValue = dataset.data[tooltipItem.index];
                    //calculate the precentage based on the total and current item, also this does a rough rounding to give a whole number
                    var precentage = Math.floor(((currentValue/total) * 100)+0.5);

                    return lbl + ': ₹' + currentValue + ' (' + precentage + "%)";
                }
            }
        } ,
        legend: {
            display: false,
            labels: {
                fontColor: 'rgb(255, 99, 132)'
            }
        }
    }

    var myBarChart = new Chart(pieChart, {
        type: 'doughnut',
        data: pieData,
        options: options
    });


}
