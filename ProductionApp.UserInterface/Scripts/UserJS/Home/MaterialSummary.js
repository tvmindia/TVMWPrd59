$(function () {

    MaterialSummarySummaryGraph();

});


function MaterialSummarySummaryGraph() {
    'use strict';

    var list = MaterialModel.MaterialSummaryList;
    var jsonData = [];
    //for (i = 0; i < list.length; i++) {
    //    debugger;
    //    jsonData.push({ value: list[i].Value, color: list[i].Color, highlight: list[i].Color, label: list[i].MaterialType })
    //}

    var data=[];
    var lbl = [];
    var colrs = [];

    for (i = 0; i < list.length; i++) {
        debugger;
        data.push(list[i].Value);
        lbl.push(list[i].MaterialType);
        colrs.push(list[i].Color);


     //   jsonData.push({ data: list[i].Value, color: list[i].Color, highlight: list[i].Color, labels: list[i].MaterialType })
    }

    var pieData = {
        datasets: [{ data: data, backgroundColor: colrs }],
        labels: lbl
        
    };


    // -------------
    // - PIE CHART -
    // -------------
    // Get context with jQuery - using jQuery's .get() method.
    var pieChartCanvas = $('#MaterialChart').get(0).getContext('2d');
    var pieChart = new Chart(pieChartCanvas);


    //var pieOptions = {
    //    // Boolean - Whether we should show a stroke on each segment
    //    segmentShowStroke: true,
    //    // String - The colour of each segment stroke
    //    segmentStrokeColor: '#fff',
    //    // Number - The width of each segment stroke
    //    segmentStrokeWidth: 1,
    //    // Number - The percentage of the chart that we cut out of the middle
    //    percentageInnerCutout: 50, // This is 0 for Pie charts
    //    // Number - Amount of animation steps
    //    animationSteps: 100,
    //    // String - Animation easing effect
    //    animationEasing: 'easeOutBounce',
    //    // Boolean - Whether we animate the rotation of the Doughnut
    //    animateRotate: true,
    //    // Boolean - Whether we animate scaling the Doughnut from the centre
    //    animateScale: false,
    //    // Boolean - whether to make the chart responsive to window resizing
    //    responsive: true,
    //    // Boolean - whether to maintain the starting aspect ratio or not when responsive, if set to false, will take up entire container
    //    maintainAspectRatio: false,
    //    // String - A legend template
    //    legendTemplate: '<ul class=\'<%=name.toLowerCase() %>-legend\'><% for (var i=0; i<segments.length; i++){%><li><span style=\'background-color:<%=segments[i].fillColor%>\'></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>',
    //    // String - A tooltip template
    //    tooltipTemplate: '<%=label%> ₹ <%=value %>'
    //};
    // Create pie or douhnut chart
    // You can switch between pie and douhnut using the method below.
    //  pieChart.Pie(  jsonData, pieOptions);

    options = {

       
        legend: {
            display: false,
            labels: {
                fontColor: 'rgb(255, 99, 132)'
            }
        }
    }

    var myBarChart = new Chart(pieChart, {
        type: 'pie',
        data: pieData,
        options: options
    });
  
    // -----------------
    // - END PIE CHART -
    // -----------------

}
