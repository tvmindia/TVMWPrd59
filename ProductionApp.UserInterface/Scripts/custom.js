var appAddress = window.location.protocol + "//" + window.location.host + "/";   //Retrieving browser Url 
var fileArray = [];
(function Checker() {
    var flag = false;
    $.ajax({
        url: appAddress + 'Account/AreyouAlive/',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            if (data.Result == "OK") {
                switch (data.Record) {
                    case "dead":
                        $('.modal').modal('hide');
                        $("#RedirectToLoginModel").modal('show');
  
                        flag = true;
                        break;
                    case "alive":
                        flag = false;
                        break;
                }


            }
            if (data.Result == "ERROR") {
                notyAlert('error', data.Message);
            }

        },
        complete: function () {
            // Schedule the next request when the current one's complete
            //  setTimeout(Checker, 126000);
            if (flag != true) {
                //for 15.2 minutes
                setTimeout(Checker, 912000);
               // setTimeout(Checker, 126000);
            }

        }
    });
})();

//LOADER/SPINNER
$(window).bind("load", function () {
    "use strict";
    $('.dataTables_processing').hide();
    $(".spn_hol").fadeOut(1000);
});
$(document).ready(function () {
    debugger;
    $("input.Amount").on('click',function () {
        debugger;
        $(this).select();
    });
    $('#ahrefHome').on('click',function () {
        $('.wrap,a').toggleClass('active');
        return false;
    });    
    $('.wrap').mouseleave(function () {
        $('.wrap,a').removeClass('active');
        return false;
    });
    //$('#divMainBody').attr('style',"height:"+screen.height + "px");
    var wrap = $(".EntryForms");
    wrap.on("scroll", function (e) {
        if (this.scrollTop > 147) {
            $('#CommonFigure').addClass("fix-search");
            $("#outstandingdetailsdiv").hide();
            $("#divStatus").hide();
        } else {
            $('#CommonFigure').removeClass("fix-search");
            $("#outstandingdetailsdiv").show();
            $("#divStatus").show();
        }
    });
    //GetUndepositedChequeBubbleCount();
    $('input.datepicker').datepicker({
        format: "dd-M-yyyy",//",
        maxViewMode: 0,
        todayBtn: "linked",
        clearBtn: true,
        autoclose: true,
        todayHighlight: true
    });
   
    $('input').keydown(function (e) {
        var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
        if (key == 13) {
            e.preventDefault();
            var inputs = $(this).closest('form').find(':input');
            inputs.eq(inputs.index(this) + 1).focus();
        }
    });

    $('input,textarea').attr('autocomplete', 'off');
   
    //menu submenu popup on click 3rd level menus
    $('.navbar a.dropdown-toggle').on('click', function (e) {
        var $el = $(this);
        var $parent = $(this).offsetParent(".dropdown-menu");
        $(this).parent("li").toggleClass('open');

        if (!$parent.parent().hasClass('nav')) {
            $el.next().css({ "top": $el[0].offsetTop, "left": $parent.outerWidth() - 4 });
        }

        $('.nav li.open').not($(this).parents("li")).removeClass("open");

        return false;
    });
   
    $(".dropdown, .btn-group").hover(function () {
       
        var dropdownMenu = $(this).children(".dropdown-menu");
        if (dropdownMenu.is(":visible")) {
            dropdownMenu.parent().toggleClass("open");
        }
    });

    $('.BlockEnter').keydown(function (e) {
    
        try {
            if (e.which === 13) {
                var index = $('.BlockEnter').index(this) + 1;
                $('.BlockEnter').eq(index).focus();
                e.preventDefault();
                
                return false;
            }
        } catch (e) {

        }

    });
   
});
//for showing loading while saving data
function OnMasterBegin() {
    debugger;
    $('#btnSaveMaster').prop('disabled', true);
    $('#imgMasterLoading').show()    
}
function OnMasterComplete() {
    $('#btnSaveMaster').prop('disabled', false);
    $('#imgMasterLoading').fadeOut(1000)
    
}
function OnServerCallBegin(){
    $('#divLoader').show();    
}
function OnServerCallComplete() {
    $('#divLoader').fadeOut(1000);
}
//---------------------------------------------
function MasterAlert(type,msgtxt)
{
    var iconclass = (type == "success") ? "fa fa-check" : ((type == "info") ? "fa fa-info-circle" : "fa fa-warning")
    $.notify({
        title: msgtxt,
        message: ""
    }, {
        type: 'pastel-' + type,
        allow_dismiss: false,
        placement: {
            from: 'top',
            align: 'right'
        },
        z_index: 21031,
        delay: 5000,
        template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
            '<span data-notify="title">{1}</span>' +
            '<i class="'+iconclass+'" style="font-size:48px;color:white"></i></span>' +
        '</div>'
    });
}
function notyAlert(type, msgtxt,title) {
    var t = '';
    if (title == undefined) {
        t = type;
    }
    else {
        t = title;
    }

    swal({ title: t, text: msgtxt, type: type, timer: 6000 });
    //var n = noty({
    //    text: msgtxt,
    //    type: type,//'alert','information','error','warning','notification','success'
    //    dismissQueue: true,
    //    timeout: 3000,
    //    layout: 'center',
    //    theme: 'defaultTheme',//closeWith: ['click'],
    //    maxVisible: 5
    //});
   
}
function formattToCurrency(n, currency) {
    n = parseFloat(n);
    return currency + " " + n.toFixed(2).replace(/./g, function (c, i, a) {
        return i > 0 && c !== "." && (a.length - i) % 3 === 0 ? "," + c : c;
    });
}
function SelectAllValue(e) {
    $(e).select();
}
function PostDataToServer(page, formData, callback)
{
    debugger;
   $.ajax({
        type: "POST",
        url: appAddress+page,
        async: true,
        data: formData,
        beforeSend: function () {
            showLoader();
        },
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            callback(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            notyAlert('error', errorThrown + ',' + textStatus + ',' + jqXHR.statusText);
        },
        complete:function()
        {
            hideLoader();
        }

    });
    
}


function GetDataFromServer(page, formData) {
    var jsonResult = {};
    $.ajax({
        
        type: "GET",
        url: appAddress + page,
        data: formData,
        beforeSend: function () {
            showLoader();
        },
        async: false,
        cache: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
         jsonResult = data;
        },
        error: function (jqXHR, textStatus, errorThrown) {
          notyAlert('error',errorThrown + ',' + textStatus + ',' + jqXHR.statusText);
        },
        complete: function () {
            hideLoader();
        }

    });
    return jsonResult;
}

function GetDataFromServerTraditional(page, formData) {
    var jsonResult = {};
    $.ajax({

        type: "GET",
        url: appAddress + page,
        data: formData,
        beforeSend: function () {
            showLoader();
        },
        async: false,
        cache: false,
        traditional: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            jsonResult = data;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            notyAlert('error', errorThrown + ',' + textStatus + ',' + jqXHR.statusText);
        },
        complete: function () {
            hideLoader();
        }

    });
    return jsonResult;
}

function ChangeButtonPatchView(Controller, Dom, Action) {
    debugger
    var data = { actionType: Action };
    var ds = {};
    ds = GetDataFromServer(Controller + "/ChangeButtonStyle/", data);
    if (ds == "Nochange")
    {
        return;0
    }
    $("#" + Dom).empty();
    $("#" + Dom).html(ds);
}
function GetMasterPartial(Controller, MasterCode) {
    try
    {
        debugger
        var data = { "masterCode": MasterCode };
        var ds = {};
        ds = GetDataFromServer(Controller + "/MasterPartial/", data);
        if (ds == "Nochange") {
            return; 0
        }
        $("#divMasterBody").empty();
        $("#divMasterBody").html(ds);
    }
    catch(e)
    {
        console.log(e.message);
    }
    
}
function NetworkFailure(data, status, xhr) {
    console.log(e.message);
}


//Common function for clearing input fields
function ClearFields() {
    $(':input').each(function () {

        if (this.type == 'text' || this.type == 'textarea' || this.type == 'file'|| this.type == 'search') {
            this.value = '';
        }
        else if (this.type == 'checkbox') {
            this.checked = false;
        }
        else if (this.type == 'select-one' || this.type == 'select-multiple') {
            this.value = '-1';
        }
    });

}


//only number validation
function isNumber(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8 && unicode!=46) { //if the key isn't the backspace key (which we should allow)
        if (unicode < 48 || unicode > 57) //if not a number
            return false //disable key press
    }
}


function notyConfirm(msg, functionIfSuccess,msg2,btnText,value) {
    var m = 'You will not be able to recover this action!'
    if (msg2 != undefined) {
        m = msg2 + '  ' + m;
    }
    if (value == 1)
    {
        m = '';
    }
    if (btnText == undefined)
    {
        btnText = "Yes, delete it!";
    }
    swal({
        title: msg,
        text: m,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: btnText,
        closeOnConfirm: false
    },
function () {
    //swal("Deleted!", "Your imaginary file has been deleted.", "success");
    eval(functionIfSuccess );
});


    //var text = '<div class="confirmbox"><span class="confirmboxHead">Delete Alert !</span><br/><br/><span class="confirmboxMsg">' + msg + '</span><br/><br/><span class="confirmboxFooter">You cannot reverse this action</span><div>'
    //var n = noty({
    //    text: text,
    //    type: 'confirm',
    //    dismissQueue: false,
    //    layout: 'center',
    //    modal: true,
    //    theme: 'defaultTheme',
    //    buttons: [
    //        {
    //            addClass: 'btn btn-primary', text: '&nbsp&nbsp;&nbsp;&nbsp;Ok&nbsp;&nbsp;&nbsp;&nbsp', onClick: function ($noty) {
    //                $noty.close();
    //                eval(functionIfSuccess + '()');

    //            }
    //        },
    //    {
    //        addClass: 'btn btn-danger', text: 'Cancel', onClick: function ($noty) {
    //            $noty.close();
    //            return false;
    //        }
    //    }
    //    ]
    //})

}

function goHome() {
    window.location = appAddress + '/SAMPanel';
}
function Logout() {
    window.location = appAddress;
}

var loadStatus = 0;
 
function showLoader() {
    try {
        $(".preloader").show();
    } catch (e) {

    }   
}
 
function hideLoader() {
    try {
        $('.preloader').fadeOut();
    } catch (e) {

    }
}

function roundoff(num) {
    return (Math.round(num * 100) / 100).toFixed(2);
}
function roundoff(num, opt) {
    if (num == '' && opt != undefined) { return ''; }
    return (Math.round(num * 100) / 100).toFixed(2);
}


//---* Order Status Notification * ---//
var Messages = {
     
}
//DATE FORMAT
function IsVaildDateFormat(date) {
    var regExp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]|(?:Jan|Mar|May|Jul|Aug|Oct|Dec)))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2]|(?:Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)(?:0?2|(?:Feb))\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9]|(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep))|(?:1[0-2]|(?:Oct|Nov|Dec)))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
    return regExp.test(date);
}
function openNav() {
   
    var left = $(".main-sidebar").width();
    var total = $(document).width();
    //var windowheight = $(document).height();
    //var topheight = $("#myNav").height();
    //var headerheight = $("#OverlayHeader").height();
    //var resultHeight = (topheight - headerheight-200) + 'px!important'
    //$('.EntryForms').attr('style', 'height: ' + resultHeight + '');
  
    $('.main').fadeOut();
    document.getElementById("myNav").style.left = "3%";
    $('#main').fadeOut();

    if ($("body").hasClass("sidebar-collapse")) {
       
        }
    else {
        $(".sidebar-toggle").trigger("click");
    }

    
}
function closeNav() {
    document.getElementById("myNav").style.left = "100%";
    $('#main').fadeIn();
}
$('.EntryForms').scroll(function () {
    var scroll = $('.EntryForms').scrollTop();

    
    if (scroll >= 50) {
        //clearHeader, not clearheader - caps H
        $("#OverlayHeader").addClass("OverlayHeader");
    }
    else {
        $("#OverlayHeader").removeClass("OverlayHeader");
    }
});
function UploadFile(FileObject)
{
    debugger;
   // $('#btnUpload').click(function () {

        // Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {
            debugger;
            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;
            if (files.length > 0)
            {
                // Create FormData object  
                var fileData = new FormData();

                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    debugger;
                    fileData.append(files[i].name, files[i]);
                    var filesize = (parseInt($('#hdnFileSizebytes').val())) + files[i].size;
                    $('#hdnFileSizebytes').val(filesize);
                    if(parseInt($('#hdnFileSizebytes').val())>10485760)
                    {
                        notyAlert('error', "File size exceeds the limit 10 MB");
                        return false;
                    }
                }

                // Adding one more key to FormData object  
                fileData.append('ParentID', FileObject.ParentID);
                fileData.append('ParentType', FileObject.ParentType);
                $.ajax({
                    url: '/' + FileObject.Controller + '/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: fileData,
                    success: function (result) {
                        if (result.Result == "OK")
                        {
                            $('#hdnFileDupID').val(result.Records.ParentID);
                            notyAlert('success', result.Message);
                            PaintImages(result.Records.ParentID);
                        }
                        else if(result.Result=="ERROR")
                        {
                            notyAlert('error', result.Message);
                        }

                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            }
            
        } else {
            notyAlert('error', 'FormData is not supported.');
        }
   // });
}

function DeleteFile(this_Obj)
{
    debugger;
    try
    {

        notyConfirm('Are you sure to delete?', 'DeleteNow("' + this_Obj.attributes.token.value + '")', '', "Yes, delete it!");
        
    }
    catch(e)
    {

    }
}
function DeleteNow(this_Obj)
{
    try
    {
        var data = { "id": this_Obj };
        var ds = {};
        ds = GetDataFromServer("FileUpload/DeleteFile/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            notyAlert('success', ds.Message);
            PaintImages($('#ID').val());
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message);
        }
    }
    catch(e)
    {

    }    
}
function PaintImages(ID)
{
    try {
        debugger;
        var data = { "ID": ID };
        var ds = {};
        ds = GetDataFromServer("FileUpload/GetAttachments/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            //ds.Records
            debugger;
            if (ds.Records != null)
            {
                $('#ExistingPreview').empty();
                var filesize = 0;
                for (var i = 0; i < ds.Records.length; i++) {
                    var html = "";
                    html = '<div class="file-preview-thumbnails">'
                                        + '  <div class="file-preview-frame krajee-default  kv-preview-thumb">'
                                             + ' <div class="kv-file-content">'
                                             + '<a href="/FileUpload/DownloadFile?token=' + ds.Records[i].AttachmentURL + '" style="position: absolute;left: 7%;cursor:pointer;z-index: 900;color: #26a026;"><i class="fa fa-download" aria-hidden="true" ></i></a>'
                                             + '<a style="position: absolute;right: 0%;cursor:pointer;z-index: 900;color: #dc3939;" ><i class="fa fa-trash-o" aria-hidden="true" onclick="DeleteFile(this);" token="' + ds.Records[i].ID + '"></i></a>'
                                                 + ' <div class="kv-preview-data file-preview-other-frame">'
                                                     + ' <div class="file-preview-other">'
                                                        + '  <span class="file-other-icon">' + validateType(ds.Records[i].FileName) + '</span>'
                                                     + ' </div>'
                                                  + '</div>'
                                              + '</div>'
                                            + '  <div class="file-thumbnail-footer">'
                                                 + ' <div class="file-footer-caption" title="' + ds.Records[i].FileName + '">' + ds.Records[i].FileName + '<br> <samp>(' + bytesToSize(ds.Records[i].FileSize) + ')</samp></div>'

                                           + '   </div>'
                                       + '   </div>'
                                      + '</div>'
                    $('#ExistingPreview').append(html);
                    filesize = filesize + parseInt(ds.Records[i].FileSize);
                }
                $('#hdnFileSizebytes').val(filesize);
            }
            if(ds.Records === null)
            {
                $('#ExistingPreview').empty();
                $('#hdnFileSizebytes').val(0);
            }
            
        }
        if (ds.Result == "ERROR") {
            notyAlert('error', ds.Message); 
        }
    }
    catch (e) {
        notyAlert('error', e.message);
    }
}
function clearUploadControl()
{
    $('#UploadPreview').empty();
    $('#UploadPreview').append('New attachments');
    var file = document.getElementById('FileUpload1');
    file.value = '';
    $('#hdnFileSizebytes').val(0);
    $('#hdnFileDupID').val('00000000-0000-0000-0000-000000000000');
    $('#ExistingPreview').empty();
}
function validateType(ext) {
    debugger;
    if (ext.match(/(doc|docx)$/i)) {
        //doc
        return '<i class="fa fa-file-word-o text-primary"></i>';
    }
    if (ext.match(/(xls|xlsx)$/i)) {
        //xls
        return '<i class="fa fa-file-excel-o text-success"></i>';
    }
    if (ext.match(/(ppt|pptx)$/i)) {
        //ppt
        return '<i class="fa fa-file-powerpoint-o text-danger"></i>';
    }
    if (ext.match(/(zip|rar|tar|gzip|gz|7z)$/i)) {
        //zip
        return '<i class="fa fa-file-archive-o text-muted"></i>';
    }
    if (ext.match(/(php|js|css|htm|html)$/i)) {
        //htm
        return '<i class="fa fa-file-code-o text-info"></i>';
    }
    if (ext.match(/(txt|ini|md)$/i)) {
        //txt
        return '<i class="fa fa-file-text-o text-info"></i>';
    }
    if (ext.match(/(avi|mpg|mkv|mov|mp4|3gp|webm|wmv)$/i)) {
        //mov
        return '<i class="fa fa-file-movie-o text-warning"></i>';
    }
    if (ext.match(/(mp3|wav)$/i)) {
        //mp3
        return '<i class="fa fa-file-audio-o text-warning"></i>';
    }
    if (ext.match(/(jpg|png|jpeg)$/i)) {
        return '<i class="fa fa-file-photo-o text-warning"></i>';
    }
    if (ext.match(/(pdf)$/i)) {
        return '<i class="fa fa-file-pdf-o text-danger"></i>';
    }
    else
    {
        return '<i class="fa fa-file-text-o text-danger"></i>';
    }
}
function bytesToSize(bytes) {
    debugger;
    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (bytes == 0) return '0 Byte';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];
}
function ShowAttachmentsTable() {
    var table = document.getElementById("filelist");
    while (table.firstChild) table.removeChild(table.firstChild);

    AppendToFileList(fileArray);
}
function AppendToFileList(list) {
    var table = document.getElementById("FileUpload1");

    for (var i = 0; i < list.length; i++) {
        var item = list[i];
        var row = table.insertRow(-1);
        row.setAttribute("fileguid", item.FileID);
        row.setAttribute("filename", item.FileName);
        var td1 = row.insertCell(-1);
        td1.innerHTML = "<img src='/Content/circle.png' border='0'/>";
        var td2 = row.insertCell(-1);
        td2.innerHTML = item.FileName;
        var td4 = row.insertCell(-1);
        td4.innerHTML = "[<a href='javascript:void(0)' onclick='Attachment_Remove(this)'>remove</a>]";
    }
}
function Attachment_FindRow(element) {
    debugger;
    while (true) {
        if (element.nodeName == "A")
            return element;
        element = element.parentNode;
    }
}

function Attachment_Remove(link) {
    debugger;
    return;
    var row = Attachment_FindRow(link);
    if (!confirm("Are you sure you want to delete '" + row.getAttribute("filename") + "'?"))
        return;
    var guid = row.getAttribute("fileguid");
    var table = document.getElementById("FileUpload1");
    table.deleteRow(row.rowIndex);
    //for (var i = 0; i < fileArray.length; i++) {
    //    if (fileArray[i].FileID == guid) {
    //        fileArray.splice(i, 1);
    //        break;
    //    }
    //}
   // CheckFileCount();
}

function CuteWebUI_AjaxUploader_OnPostback() {
    //var uploader = document.getElementById("myuploader");
    //var guidlist = uploader.value;

    //var xh = CreateAjaxRequest();
    //xh.send("guidlist=" + guidlist + "&limitcount=3&hascount=" + fileArray.length);

    ////call uploader to clear the client state
    //uploader.reset();

    //if (xh.status != 200) {
    //    alert("http error " + xh.status);
    //    setTimeout(function () { document.write(xh.responseText); }, 10);
    //    return;
    //}

    //var list = eval(xh.responseText); //get JSON objects

    //fileArray = fileArray.concat(list);
    CheckFileCount();
    AppendToFileList(list);
}

function CheckFileCount() {
    var uploadbutton = document.getElementById("FileUpload1");
    if (fileArray.length >= 3) {
        uploadbutton.disabled = true;
    }
    else
        uploadbutton.disabled = false;
    ShowFiles();
}

function ShowFiles() {
    var msgs = [];
    for (var i = 0; i < fileArray.length; i++) {
        msgs.push(fileArray[i].FileName + ", " + fileArray[i].FileSize + "Kb");
    }
    document.getElementById("text_info").value = msgs.join("\r\n");
}
function GetUndepositedChequeBubbleCount() {
    try {
        var data = {};
        var ds = {};
        ds = GetDataFromServer("DepositAndWithdrawals/GetUndepositedChequeCount/", data);
        if (ds != '') {
            ds = JSON.parse(ds);
        }
        if (ds.Result == "OK") {
            $('#undepositedCount').text(ds.Records);
            $('#undepositedCount').attr('title', ds.Records + ' Cheque(s) to be deposited Today');
            $('#undepositedCount1').attr('title', ds.Records + ' Cheque(s) to be deposited Today');
        }
        if (ds.Result == "ERROR") {
            $('#undepositedCount').text("0");
        }    
    }
    catch (e) {

    }
}

function formatCurrency(amount) {
    try{
        var x = amount;
        x = x.toString();
        var afterPoint = '';
        if (x.indexOf('.') > 0)
            afterPoint = x.substring(x.indexOf('.'), x.length);
        x = Math.floor(x);
        x = x.toString();
        var lastThree = x.substring(x.length - 3);
        var otherNumbers = x.substring(0, x.length - 3);
        if (otherNumbers != '')
            lastThree = ',' + lastThree;
        var res = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree + afterPoint;
        return res
    }catch(x){
        return amount;
    }
 

}



function OpenCustExpeditionReport(Filter) {

    var url = '../Report/CustomerPaymentExpeditingDetails/__id__';
    window.location.href = url.replace('__id__', Filter);
}


function OpenSuppExpeditionReport(Filter) {

    var url = '../Report/SupplierPaymentExpeditingDetails/__id__';
    window.location.href = url.replace('__id__', Filter);
}



function setCookie(cname, cvalue, exdays) {
    try {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    } catch (e) {

    }
  
}

function getCookie(cname) {
    try {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    } catch (e) {

    }
   
}

function clearCookie(cname) {
    try {
        setCookie(cname, "", -1)
    } catch (e) {

    }
}