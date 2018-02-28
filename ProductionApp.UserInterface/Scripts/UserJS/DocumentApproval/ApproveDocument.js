
var DataTables = {};
var EmptyGuid = "00000000-0000-0000-0000-000000000000";
$(document).ready(function () {
    debugger;
    try {
        DataTables.ApprovalHistoryTable = $('#tblApprovalHistory').DataTable(
{
    dom: '<"pull-right"f>rt<"bottom"ip><"clear">',
    ordering: false,
    searching: false,
    paging: false,
    bInfo : false,
    data: GetApprovalHistory(),//ApprovalHistory.js
    autoWidth: false,
    columns: [
    { "data": "ApproverName", "defaultContent": "<i>-</i>" },
    { "data": "ApproverLevel", "defaultContent": "<i>-</i>" },
    { "data": "ApprovalDate", "defaultContent": "<i>-</i>" },
    { "data": "ApprovalStatus", "defaultContent": "<i>-</i>" },
    ],
    columnDefs: [
        { className: "text-center", "targets": [2] },
        { className: "text-right", "targets": [] },
        { className: "text-left", "targets": [0, 1, 3] }
    ]
});
    }
    catch (e) {
        console.log(e.message);
    }
});


function ApproveDocument() {
    debugger;
    alert('1')
}
function RejectDocument() {
    debugger;
    alert('2')

}
