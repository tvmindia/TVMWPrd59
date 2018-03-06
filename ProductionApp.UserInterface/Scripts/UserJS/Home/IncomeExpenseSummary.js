﻿$(function () {
    debugger;
    IncomeExpenseChart();

});


 

function IncomeExpenseChart() {

    var list = IncomeExpense;
    var labelList = [];
    var IncomeList = [];
    var ExpenseList = [];
    for (i = 0; i < list.length; i++) {
        labelList.push(list[i].Month);
        IncomeList.push(parseFloat(list[i].InAmount) );
        ExpenseList.push(parseFloat(list[i].ExAmount) );

    }
    createIncomeExpenseChart(labelList, IncomeList, ExpenseList);

}
function createIncomeExpenseChart(labelList, IncomeList, ExpenseList) {

    'use strict';

    /* ChartJS
     * -------
     * Here we will create a few charts using ChartJS
     */

    // -----------------------
    // - MONTHLY SALES CHART -
    // -----------------------

    // Get context with jQuery - using jQuery's .get() method.
    var ieChartCanvas = $('#IEChart').get(0).getContext('2d');
    // This will get the first returned node in the jQuery collection.
    var ieChart = new Chart(ieChartCanvas);

    var ieChartData = {
        labels: labelList,
        datasets: [
          {
              label: 'Income',
              fillColor: 'rgba(81, 97, 225, 0.5)',
              strokeColor: 'rgb(81, 97, 225)',
              pointColor: 'rgb(81, 97, 225)',
              pointStrokeColor: '#c1c7d1',
              pointHighlightFill: '#fff',
              pointHighlightStroke: 'rgb(81, 97, 225)',
              data: IncomeList,
              backgroundColor: "rgba(81, 97, 225, 0.5)"
          },
          {
              label: 'Expense',
              fillColor: 'rgba(260,141,188,0.5)',
              strokeColor: 'rgba(260,141,188,0.8)',
              pointColor: '#3b8bba',
              pointStrokeColor: 'rgba(260,141,188,1)',
              pointHighlightFill: '#fff',
              pointHighlightStroke: 'rgba(260,141,188,1)',
              data: ExpenseList,
              backgroundColor: "rgba(260,141,188,0.5)"
          }
        ]
    };

    //var ieChartOptions = {
    //    // Boolean - If we should show the scale at all
    //    showScale: true,
    //    // Boolean - Whether grid lines are shown across the chart
    //    scaleShowGridLines: false,
    //    // String - Colour of the grid lines
    //    scaleGridLineColor: 'rgba(0,0,0,.05)',
    //    // Number - Width of the grid lines
    //    scaleGridLineWidth: 1,
    //    // Boolean - Whether to show horizontal lines (except X axis)
    //    scaleShowHorizontalLines: true,
    //    // Boolean - Whether to show vertical lines (except Y axis)
    //    scaleShowVerticalLines: true,
    //    // Boolean - Whether the line is curved between points
    //    bezierCurve: true,
    //    // Number - Tension of the bezier curve between points
    //    bezierCurveTension: 0.3,
    //    // Boolean - Whether to show a dot for each point
    //    pointDot: false,
    //    // Number - Radius of each point dot in pixels
    //    pointDotRadius: 4,
    //    // Number - Pixel width of point dot stroke
    //    pointDotStrokeWidth: 1,
    //    // Number - amount extra to add to the radius to cater for hit detection outside the drawn point
    //    pointHitDetectionRadius: 20,
    //    // Boolean - Whether to show a stroke for datasets
    //    datasetStroke: true,
    //    // Number - Pixel width of dataset stroke
    //    datasetStrokeWidth: 2,
    //    // Boolean - Whether to fill the dataset with a color
    //    datasetFill: true,
    //    // String - A legend template
    //    legendTemplate: '<ul class=\'<%=name.toLowerCase()%>-legend\'><% for (var i=0; i<datasets.length; i++){%><li><span style=\'background-color:<%=datasets[i].lineColor%>\'></span><%=datasets[i].label%></li><%}%></ul>',
    //    // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
    //    maintainAspectRatio: true,
    //    // Boolean - whether to make the chart responsive to window resizing
    //    responsive: true
    //};

    // Create the line chart
    //ieChart.Line(ieChartData, ieChartOptions);

    options = {
       
        tooltips: {
            callbacks: {
                label: function (tooltipItem, data) {
                    var label = data.datasets[tooltipItem.datasetIndex].label || '';

                    if (label) {
                        label += ': ';
                    }
                    label += tooltipItem.yLabel + 'Lakhs';
                    return label;
                }
            }
        }
    }

    var myBarChart = new Chart(ieChart, {
        type: 'line',
        data: ieChartData,
        options: options
    });


    // ---------------------------
    // - END MONTHLY SALES CHART -
    // ---------------------------


}
