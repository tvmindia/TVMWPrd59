$(document).ready(function () {
    $('#Print').click(function () {
        window.open("../PDFGenerator/RecieptPrint");
    });
});
function CheckandView()
{
    debugger;
    if($('.custom').is(":visible"))
    {
        $('#customtbl').hide();
        $('#richbox').show();
        $('#editor').easyEditor({
            buttons: ['bold', 'italic', 'h2', 'h3', 'h4', 'alignleft', 'aligncenter', 'alignright']

        });
    }
    if($('.default').is(":visible"))
    {
        $('#customtbl').show();
        $('#richbox').hide();
        DrawTable({
            Action: "Report/GetCustomerPaymentLedger/",
            data: { "FromDate": "20-Jun-2017", "ToDate": "21-Oct-2017", "CustomerIDs": "ALL" },
            Exclude_column: ["CustomerID", "customerList", "CustomerCode", "CustomerName"],
            Header_column_style: { "Date": "width:110px;font-size:12px;border-bottom:2px solid grey;font-weight: 600;", "Type": "font-size:12px;border-bottom:2px solid grey;width:110px;font-weight: 600;", "Ref": "font-size:12px;border-bottom:2px solid grey;width:150px;font-weight: 600;", "Debit": "width:150px;text-align: center;font-size:12px;border-bottom:2px solid grey;font-weight: 600;", "Credit": "width:150px;text-align: center;font-size:12px;border-bottom:2px solid grey;font-weight: 600;", "Balance": "width:150px;text-align: center;font-size:12px;border-bottom:2px solid grey;font-weight: 600;" },
            Row_color: { "Odd": "White", "Even": "white" },
            Body_Column_style: { "Date": "font-size:11px;font-weight: 100;width:110px;", "Type": "font-size:11px;font-weight: 100;width:150px;", "Ref": "font-size:11px;font-weight: 100;", "Debit": "text-align:right;font-size:11px;font-weight: 100;width:150px;", "Credit": "text-align:right;font-size:11px;font-weight: 100;width:150px;", "Balance": "text-align:right;font-size:11px;font-weight: 100;width:150px;" }
            
        });
    }
}
function GetHtmlValue()
{
    debugger;
    if ($('.custom').is(":visible"))
    {
        var HtmlData = '<p>' + $("#editor").html() + '</p>';
        SendToGenerate(HtmlData);
    }
    if ($('.default').is(":visible"))
    {
        SendToGenerate($("#customtbl").html())
    }
    

}

function DrawTable(options) {
    var Records = GetItemsSummary(options);
    if (Records != null) {
        for (var i = 0; i < Records.length; i++) {
            for (var j = 0; j < options.Exclude_column.length; j++)
            {
                delete Records[i][options.Exclude_column[j]];
            }            
        }
        $("#customtbl").empty();
        $("#customtbl").append('<table id="tblTechnicianPerformanceList" style="margin-top:30px;" class="table compact" cellspacing="0" width="100%">'
                            + '<thead><tr id="trTechPerform"></tr></thead>'
                            + '<tbody id="tbodyPerform"></tbody>'
                        + '</table>')
        var Header = [];
        debugger;
        $.each(Records, function (index, Records) {
            debugger;
            if (Header.length == 0) {
                $.each(Records, function (key, value) {
                    Header.push(key);
                });

                for (var i = 0; i < Header.length; i++) {
                    Performed = 0;
                        $.each(options.Header_column_style, function (key, value) {
                            debugger;
                            if (key == Header[i])
                            {
                                $("#trTechPerform").append('<th style="' + value + '">' + Header[i] + '</th>')
                                Performed = 1;
                            }
                            
                        });
                        if (Performed == 0)
                        {
                            $("#trTechPerform").append('<th>' + Header[i] + '</th>')
                        }
                    
                }
            }
            var html = "";
            $.each(Records, function (key, value) {
                PerformedCol = 0;
                $.each(options.Body_Column_style, function (keyCol, valueCol) {
                    debugger;
                    if (keyCol == key) {
                        if (value === null)
                        {
                            html = html + '<td></td>'
                        }
                        else
                        {
                            html = html + '<td style="' + valueCol + '">' + (($.isNumeric(value)) ? roundoff(value) : value) + '</td>'
                        }                        
                        PerformedCol = 1;
                    }

                });
                if (PerformedCol == 0)
                {
                    if (value === null) {
                        html = html + '<td></td>'
                    }
                    else
                    {
                        html = html + '<td>' + ($.isNumeric(value) ? roundoff(value) : value) + '</td>'
                    }
                        
                   
                }                   

            });
            if (index % 2 === 0)
            {
                $("#tbodyPerform").append('<tr style="padding-top:1px;padding-botton:1px;min-height:14px;background-color:' + options.Row_color.Even + '">' + html + '</tr>')
            }
            else
            {
                $("#tbodyPerform").append('<tr style="padding-top:1px;padding-botton:1px;min-height:14px;background-color:' + options.Row_color.Odd + '">' + html + '</tr>')
            }
            

        });
    }

}
function SaveSuccess(data, status) {
    debugger;
    var JsonResult = JSON.parse(data)
    switch (JsonResult.Result) {
        case "OK":
            debugger;
            w = window.open(JsonResult.URL);
            w.print();
            break;
        case "ERROR":
            notyAlert('error', JsonResult.Message);
            break;
        default:
            notyAlert('error', JsonResult.Message);
            break;
    }
}
function GetItemsSummary(options) {
    debugger;
    try {
        var data = options.data;
        var ds = {};
        ds = GetDataFromServerTraditional(options.Action, data);
            if (ds != '') {
                ds = JSON.parse(ds);                 
            }
            if (ds.Result == "OK") {
                debugger;
                //var result = ds.Records.reduce(function (res, obj) {
                //    if (!(obj.CustomerName in res))
                //        res.__array.push(res[obj.CustomerName] = obj);
                //    else {
                //        //res[obj.CustomerName].hits += obj.hits;
                //        //res[obj.CustomerName].bytes += obj.bytes;
                //    }
                //    return res;
                //}, { __array: [] }).__array
                //.sort();
                return ds.Records;
            }
            if (ds.Result == "ERROR") {
                notyAlert('error', ds.Message);
            }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function SendToGenerate(BodyContent) {
    debugger;
    try {
        //var data = { "MailBody": BodyContent };
        //var ds = {};
        //ds = GetDataFromServer("PDFGenerator/SendPDFDoc/", data);
        //if (ds != '') {
        //    ds = JSON.parse(ds);
        //}
        //if (ds.Result == "OK") {
        //    return ds.Records;
        //}
        //if (ds.Result == "ERROR") {
        //    notyAlert('error', ds.Message);
        //}
        var data = "{'MailBody':" + JSON.stringify(BodyContent) + "}";
        PostDataToServer("PDFGenerator/SendPDFDoc/", data, function (JsonResult) {
            if (JsonResult != '') {
                switch (JsonResult.Result) {
                    case "OK":
                        notyAlert('success', JsonResult.Message);
                        List();
                        break;
                    case "ERROR":
                        notyAlert('error', JsonResult.Message);
                        break;
                    default:
                        break;
                }
            }
        });
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
