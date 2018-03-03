function CreateSalesSummary() {
    var chrt = document.getElementById("myChart").getContext("2d");
    var myarr = [65, 59, 80, 81, 56, 55, 40];
    var data = {
        labels: lbls, //x-axis
        datasets: [
        {
            label: "My First dataset", //optional
            fillColor: "rgba(220,220,220,0.8)",
            strokeColor: "rgba(220,220,220,0.8)",
            highlightFill: "rgba(220,220,220,0.75)",
            highlightStroke: "rgba(220,220,220,1)",
            data: dta // y-axis
        } 
        ]
    };

    var myFirstChart = new Chart(chrt).Bar(data);
}
 

$(function() {
    CreateSalesSummary();
});