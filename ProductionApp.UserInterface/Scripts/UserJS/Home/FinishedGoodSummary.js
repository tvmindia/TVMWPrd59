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
