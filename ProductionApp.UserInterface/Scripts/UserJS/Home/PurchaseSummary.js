function CreatePurchaseSummary() {
    var chrt = document.getElementById("purchasechart").getContext("2d");
    var myarr = [65, 59, 80, 81, 56, 55, 40];
    var data = {
        labels: purchaseSummarylbls, //x-axis
        datasets: [
        {
            legendText: "Sales Months",
            label: "Purchase", //optional
            fillColor: "rgba(105,240,174,0.8)",
            strokeColor: "rgba(105,240,174,0.8)",
            highlightFill: "rgba(105,240,174,0.75)",
            highlightStroke: "rgba(105,240,174,1)",
            data: purchaseSummarydta, // y-axis
            backgroundColor: "rgba(105,240,174,0.5)"
        }
        ]

    };

    //options = {

    //    //Boolean - If we show the scale above the chart data			
    //    scaleOverlay: false,

    //    //Boolean - If we want to override with a hard coded scale
    //    scaleOverride: false,

    //    //** Required if scaleOverride is true **
    //    //Number - The number of steps in a hard coded scale
    //    scaleSteps: null,
    //    //Number - The value jump in the hard coded scale
    //    scaleStepWidth: null,
    //    //Number - The scale starting value
    //    scaleStartValue: null,

    //    //String - Colour of the scale line	
    //    scaleLineColor: "rgba(0,0,0,.1)",

    //    //Number - Pixel width of the scale line	
    //    scaleLineWidth: 1,

    //    //Boolean - Whether to show labels on the scale	
    //    scaleShowLabels: true,

    //    //Interpolated JS string - can access value
    //    scaleLabel: "<%=value%>",

    //    //String - Scale label font declaration for the scale label
    //    scaleFontFamily: "'Arial'",

    //    //Number - Scale label font size in pixels	
    //    scaleFontSize: 12,

    //    //String - Scale label font weight style	
    //    scaleFontStyle: "normal",

    //    //String - Scale label font colour	
    //    scaleFontColor: "#666",

    //    ///Boolean - Whether grid lines are shown across the chart
    //    scaleShowGridLines: true,

    //    //String - Colour of the grid lines
    //    scaleGridLineColor: "rgba(0,0,0,.05)",

    //    //Number - Width of the grid lines
    //    scaleGridLineWidth: 1,

    //    //Boolean - If there is a stroke on each bar	
    //    barShowStroke: true,

    //    //Number - Pixel width of the bar stroke	
    //    barStrokeWidth: 2,

    //    //Number - Spacing between each of the X value sets
    //    barValueSpacing: 5,

    //    //Number - Spacing between data sets within X values
    //    barDatasetSpacing: 1,

    //    //Boolean - Whether to animate the chart
    //    animation: true,

    //    //Number - Number of animation steps
    //    animationSteps: 60,

    //    //String - Animation easing effect
    //    animationEasing: "easeInCirc",

    //    //Function - Fires when the animation is complete
    //    onAnimationComplete: null,

    //    tooltipTemplate: '<%=label%> : ₹ <%=value %> Lakhs'

    //}


    //var purchaseChart = new Chart(chrt).Bar(data, options);

    options = {
        scales: {
            xAxes: [{
                gridLines: {
                    offsetGridLines: true
                }
            }]
        },
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

    var myBarChart = new Chart(chrt, {
        type: 'bar',
        data: data,
        options: options
    });



}

$(function () {
    CreatePurchaseSummary();
});