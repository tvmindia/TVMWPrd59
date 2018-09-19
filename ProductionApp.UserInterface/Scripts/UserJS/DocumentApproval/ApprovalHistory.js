
function GetApprovalHistory() {
    try {
        debugger;
        var DocumentID = $("#DocumentID").val();
        var DocumentTypeCode = $("#DocumentType").val();
        var data = { "DocumentID": DocumentID, "DocumentTypeCode": DocumentTypeCode };
        var ds = {};
        ds = GetDataFromServer("DocumentApproval/GetApprovalHistory/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            return ds.Records;
        }
        if (ds.Result == "ERROR") {
            alert(ds.Message);
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}

//USED TO BIND APPROVAL HISTORY TABLE BY DATATABLES
//function BindApprovalHistoryTable() {
//    try {
//        debugger;
//        DataTables.ApprovalHistoryTable = $('#tblApprovalHistory').DataTable({
//            dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
//            "scrollY": "150px",
//            "scrollCollapse": true,
//            ordering: false,
//            searching: false,
//            paging: false,
//            bInfo: false,
//            data: GetApprovalHistory(),//ApprovalHistory.js
//            autoWidth: false,
//            columns: [
//            { "data": "ApproverName", "defaultContent": "<i>-</i>", "width": "20%" },
//            { "data": "ApproverLevel", "defaultContent": "<i>-</i>", "width": "5%" },
//            { "data": "ApprovalDate", "defaultContent": "<i>-</i>", "width": "20%" },
//            { "data": "Remarks", "defaultContent": "<i>-</i>", "width": "35%" },
//            { "data": "ApprovalStatus", "defaultContent": "<i>-</i>", "width": "20%" },
//            ],
//            columnDefs: [
//                { className: "text-center", "targets": [2] },
//                { className: "text-right", "targets": [] },
//                { className: "text-left", "targets": [0, 1, 3] }
//            ]
//        });
//    }
//    catch (e) {
//        console.log(e.message);
//    }
//}

function ShowApprovalHistory() {
    try {
        debugger;
        $('#modelContextLabel').text("Approval History");
        $('#ApprovalHistoryModal').modal('show');
        //TableOverflowResize();
        //BindApprovalHistoryTable();
    }
    catch (e) {
        console.log(e.message);
    }
}

////USED TO RESIZE TBODY LIKE THAT OF THEAD ON OVERFLOW-Y
//function TableOverflowResize(){
//    try{
//        // Change the selector if needed
//        var $table = $('table.scroll'),
//            $bodyCells = $table.find('tbody tr:first').children(),
//            colWidth;

//        // Adjust the width of thead cells when window resizes
//        $(window).resize(function() {
//            // Get the tbody columns width array
//            colWidth = $bodyCells.map(function() {
//                return $(this).width();
//            }).get();
    
//            // Set the width of thead columns
//            $table.find('thead tr').children().each(function(i, v) {
//                $(v).width(colWidth[i]);
//            });    
//        }).resize(); // Trigger resize handler
//    }
//    catch (e) {
//        console.log(e.message);
//    }
//}