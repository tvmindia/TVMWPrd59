
$(document).ready(function () {
    try{
        debugger;
        $('#ProductionTrackingSearch_Value,#EmployeeID,#ProductID,#SubComponentID').select2();
        RemoveAddOn();
    } catch (ex) {
        console.log(ex.message)
    }
});

function RemoveAddOn() {
    try{
        debugger;
        $('.input-group-addon').each(function () {
            $(this).parent().css("width", "100%");
            $(this).remove();
        });
    } catch (ex) {
        console.log(ex.message)
    }
}