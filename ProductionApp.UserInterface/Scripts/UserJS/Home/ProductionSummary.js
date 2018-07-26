function CreateProductionSummary() {
    var chrt = document.getElementById("Productionchart").getContext("2d");
    debugger;
    var data = {
        labels: lblProduction, //x-axis
        datasets: [
        {
            stack:"stanck2",
            legendText: "Materials Used",
            label: "Matrls Usd", //optional
            fillColor: "rgba(211, 218, 7,0.8)",
            strokeColor: "rgba(211, 218, 7,0.8)",
            highlightFill: "rgba(211, 218, 7,0.75)",
            highlightStroke: "rgba(211, 218, 7,1)",
            data: dta1, // y-axis
            backgroundColor: "rgba(211, 218, 7,0.8)"
        },
         {
             stack: "stanck1",
             legendText: "FG Prodcd",
             label: "FG Produced", //optional
             fillColor: "rgba(50, 115, 25,0.8)",
             strokeColor: "rgba(50, 115, 25,0.8)",
             highlightFill: "rgba(50, 115, 25,0.75)",
             highlightStroke: "rgba(50, 115, 25,1)",
             data: dta2,// y-axis
             backgroundColor: "rgba(50, 115, 25,0.8)"
         },
          {
              stack: "stanck1",
              legendText: "In Progress",
              label: "In Progr.", //optional
              fillColor: "rgba(120, 240, 120,0.8)",
              strokeColor: "rgba(120, 240, 120,0.8)",
              highlightFill: "rgba(120, 240, 120,0.75)",
              highlightStroke: "rgba(120, 240, 120,1)",
              data: dta3,// y-axis
              backgroundColor: "rgba(120, 240, 120,0.8)"
          },
           {
               stack: "stanck1",
               legendText: "Damage&Wastage",
               label: "Damg & Wstg", //optional
               fillColor: "rgba(211, 130, 30,0.8)",
               strokeColor: "rgba(211, 130, 30,0.8)",
               highlightFill: "rgba(211, 130, 30,0.75)",
               highlightStroke: "rgba(211, 130, 30,1)",
               data: dta4, // y-axis
               backgroundColor: "rgba(211, 130, 30,1)"
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
    //    animationEasing: "easeOutCirc",

    //    //Function - Fires when the animation is complete
    //    onAnimationComplete: null,

    //    tooltipTemplate: '<%=label%> : ₹ <%=value %> Kg',
    //    scales: {
    //        xAxes: [{
    //            stacked: true,
    //        }],
    //        yAxes: [{
    //            stacked: true
    //        }]
    //    }

    //}


    // var productionchart = new Chart(chrt).Bar(data, options);

    options = {

        tooltips: {
            callbacks: {
                label: function (tooltipItem, data) {
                    var label = data.datasets[tooltipItem.datasetIndex].label || '';

                    if (label) {
                        label += ': ';
                    }
                    label += tooltipItem.yLabel + 'Kgs';
                    return label;
                }
            }
        },
        scales: {
                    xAxes: [{
                        stacked: true,                       
                        barPercentage: 0.4,
                        maxBarThickness: 10
                    }],
                    yAxes: [{
                        stacked: true
                    }]
                }
    }

    var myBarChart = new Chart(chrt, {
        type: 'bar',
        data: data,
        options: options
    });

}

$(function () {
    CreateProductionSummary();
});