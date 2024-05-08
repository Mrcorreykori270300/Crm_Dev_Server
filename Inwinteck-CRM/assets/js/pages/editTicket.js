
    @if (TempData["message"] != null) {
        <script type="text/javascript">

            window.onload = function () {
                $('#myModal').modal('show');
            GoLocation();
            };
        </script>
    }
    else {
        <script type="text/javascript">
            window.onload = function () {

                GoLocation();
            };
        </script>
    }

    <script type="text/javascript">


        function clicked(item) {
            var id = $(item).attr("id");

        $("#FE").val($('#nam-' + id).val());
        $("#FE_ID").val($('#FEI-' + id).val());

        }



        @*function getFEdetails(code, country) {

            $.get(baseurl + "Transaction/getFEdetails", { PinCode: code, Country: country, ticket:@Model.Id }, function (data) {
                if (data.res == "Success") {

            $('#FE_ID').empty();
        $.each(data.CC, function (i, val) {


            $('#FE_ID').append('<option value =' + data.CC[i]['FE_ID'] + '>' + data.CC[i]['FE_Name'] + '</option>');
                    });

        getFEInfo(code,country);
                }
        else {

            $('#FE_ID').empty();
        $('#FE_ID').append('<option value="">Select Field Engineer Name</option>');
        $('#FEerror').text('No FE Found in Master.');
        $('#FEmyModal').modal('show');
                }

            });



        }*@

        function getFEInfo(id) {
            debugger

        $.get(baseurl + "Transaction/getFEInfo", {id:id}, function (data) {
                if (data.res == "Success") {

            $('#FE_ID').empty();
        $('#FE_ID').append('<option value=' + data.CC[' FE_ID'] + '>' + data.CC['FE_Name'] + '</option>');
    $('#FE_contact').val(data.CC['Phone']);
    $('#FE_Email').val(data.CC['Email']);
    $('#FE_Origin').val(data.CC['FE_Origin']);
    $('#FE_Certification').val(data.cert);
    $('#FE_Buss_Hrs').val(data.CC['Charges_Business_Hour']);
    $('#FE_Non_Buss_Hrs').val(data.CC['Charges_Non_Business_Hour']);
    $('#FE_Minimum_Hrs').val(data.CC['Minimum_Hrs']);
    $('#FE_Fixed').val(data.CC['Charge_Job']);

}
                else {
    $('#FE_contact').val('');
    $('#FE_Email').val('');
    $('#FE_Origin').val('');
    $('#FE_Certification').val('');
    $('#FE_Buss_Hrs').val('');
    $('#FE_Non_Buss_Hrs').val('');
    $('#FE_Minimum_Hrs').val('');
    $('#FE_Fixed').val('');
    $('#FEerror').text('No FE Found in Master.');
    $('#FEmyModal').modal('show');
}

            });
        }

function getFEMail(id) {

    $.get(baseurl + "Transaction/getFEMail", { id: id, Ticket: @Model.Id
}, function (data) {
    if (data.res == "Success") {


        $('#FEerror').text('Email Sent to ' + data.fename);
        $('#FEmyModal').modal('show');

    }
    else {
        $('#FEerror').text('No FE Found in Master.');
        $('#FEmyModal').modal('show');
    }

});
        }

function clickedon(item) {
    var id = $(item).attr("id");

    $("#City").val($('#city-' + id).val());
    $("#State").val($('#state-' + id).val());
    $("#Country").val($('#Country-' + id).val());

    var country = $('#Country-' + id).val();
    var code = $('#pin-' + id).val();

    getFEdetails(code, country);

}



$(document).ready(function () {

    $("#OEM").val("@Model.OEM");
    $("#Country").val("@Model.Country");
    $("#Status").val("@Model.Status");

    $("#Other_Charge").val("@Model.Other_Charge");
    $("#Pregame").val("@Model.Pregame");
    $("#Part_Management").val("@Model.Part_Management");
    $("#Part_Handover").val("@Model.Part_Handover");
    $("#Ticket_Mode").val("@Model.Ticket_Mode");
    $("#Ticket_Type").val("@Model.Ticket_Type");
    $("#Ticket_Priority").val("@Model.Ticket_Priority");
    $("#SLA").val("@Model.SLA");

    $("#Certification_Need").val("@Model.Certification_Need");

    $("#CT_Buss_Hrs").val("@ViewBag.Cust_Business");
    $("#CT_Non_Buss_Hrs").val("@ViewBag.Cust_Business_Non");
    $("#CT_Minimum_Hrs").val("@ViewBag.Cust_Minimum");
    $("#CT_Fixed").val("@ViewBag.Cust_Charges_job");

    $("#FE_Pay_Type").val("@Model.FE_Pay_Type");
    $("#FE_Payment_Mode").val("@Model.FE_Payment_Mode");
    $("#CT_Pay_Type").val("@Model.CT_Pay_Type");
    $("#CT_Payment_Mode").val("@Model.CT_Payment_Mode");

    $("#Return_Label").val("@Model.Return_Label");
    $("#FE_Part_Storage_Charge").val("@Model.FE_Part_Storage_Charge");
    $("#Other_Charge").val("@Model.Other_Charge");
    $("#Customer_Email_Send").val("@Model.Customer_Email");
    $("#FE_Email_Send").val("@Model.FE_Email");

    if (@Model.Part_Management == 22) {
    $(".pickup").css('display', 'block');
    $(".handover").css('display', 'none');

    if (@Model.Return_Label == 1) {
        $(".pickup-av").css('display', 'block');
        $(".pickup-nav").css('display', 'none');
    }
                else if (@Model.Return_Label == 0) {
        $(".pickup-av").css('display', 'block');
        $(".pickup-nav").css('display', 'block');
    }
                else
    {
        $("#Return_Label option:selected").val("");
        $(".pickup-av").css('display', 'none');
        $(".pickup-nav").css('display', 'none');
    }

    if (@Model.FE_Part_Storage_Charge == 1) {
        $(".fe-charge").css('display', 'block');
    }
                else if (@Model.FE_Part_Storage_Charge == 0) {
        $(".fe-charge").css('display', 'none');
    }
                else
    {
        $("#FE_Part_Storage_Charge option:selected").val("");
        $(".fe-charge").css('display', 'none');
    }
}
            else if (@Model.Part_Management == 23) {
    $(".pickup").css('display', 'none');
    $(".handover").css('display', 'block');
    $(".pickup-av").css('display', 'none');
    $(".pickup-nav").css('display', 'none');
}
            else
{
    $(".pickup").css('display', 'none');
    $(".handover").css('display', 'none');
    $(".pickup-av").css('display', 'none');
    $(".pickup-nav").css('display', 'none');
}

if (@Model.Certification_Need == 1) {

    $(".cert").css('display', 'grid');
}
            else {
    $(".cert").css('display', 'none');
}

if (@Model.Other_Charge == 1) {
    $(".other-charge").css('display', 'block');
}
            else if (@Model.Other_Charge  == 0) {
    $(".other-charge").css('display', 'none');
}
            else
{
    $("#Other_Charge option:selected").val("");
    $(".other-charge").css('display', 'none');
}


if (@Model.Customer_Email == 1) {
    $(".customer-email").css('display', 'block');
}
            else if (@Model.Customer_Email == 0) {
    $(".customer-email").css('display', 'none');
}
            else
{
    $("#Customer_Email_Send option:selected").val("");
    $(".customer-email").css('display', 'none');
}


if (@Model.FE_Email == 1) {
    $(".fe-email").css('display', 'block');

}
            else if (@Model.FE_Email  == 0) {
    $(".fe-email").css('display', 'none');
    $("#FE_Email_Name").val('');
}
            else
{
    $("#FE_Email option:selected").val('');
    $(".fe-email").css('display', 'none');
    $("#FE_Email_Name").val('');
}


if ('@Model.Pregame' == '') {


}
else if ('@Model.Pregame' == '1') {
    $(".pg").css('display', 'block');
}
else {
    $(".pg").css('display', 'none');
}


if (@Model.Status == 19) {
    $(".sc").css('display', 'none');
    $(".cl").css('display', 'none');
    $(".RDT").css('display', 'block');
    $(".TDR").css('display', 'none');
    $(".TCR").css('display', 'none');
    $(".chin").css('display', 'none');
    $("#Reschedule_DT").prop("required", true);
    $("#Decline_Reason").prop("required", false);
    $("#Cancel_Reason").prop("required", false);
}
            else if (@Model.Status == 21) {
    $(".sc").css('display', 'none');
    $(".cl").css('display', 'none');
    $(".RDT").css('display', 'none');
    $(".TDR").css('display', 'none');
    $(".TCR").css('display', 'block');
    $(".chin").css('display', 'none');
    $("#Reschedule_DT").prop("required", false);
    $("#Decline_Reason").prop("required", false);
    $("#Cancel_Reason").prop("required", true);
}
            else if (@Model.Status == 1362)
{
    $(".sc").css('display', 'none');
    $(".cl").css('display', 'none');
    $(".RDT").css('display', 'none');
    $(".TDR").css('display', 'block');
    $(".TCR").css('display', 'none');
    $(".chin").css('display', 'none');
    $("#Reschedule_DT").prop("required", false);
    $("#Decline_Reason").prop("required", true);
    $("#Cancel_Reason").prop("required", false);
}
            else if (@Model.Status == 1372)
{
    $(".sc").css('display', 'block');
    $(".cl").css('display', 'none');
    $(".RDT").css('display', 'none');
    $(".TDR").css('display', 'none');
    $(".TCR").css('display', 'none');
    $(".chin").css('display', 'none');
    $("#Reschedule_DT").prop("required", false);
    $("#Decline_Reason").prop("required", false);
    $("#Cancel_Reason").prop("required", false);
}
            else if (@Model.Status == 20)
{
    $(".sc").css('display', 'block');
    $(".cl").css('display', 'block');
    $(".RDT").css('display', 'none');
    $(".TDR").css('display', 'none');
    $(".TCR").css('display', 'none');
    $(".chin").css('display', 'block');
    $("#Reschedule_DT").prop("required", false);
    $("#Decline_Reason").prop("required", false);
    $("#Cancel_Reason").prop("required", false);
}
            else if (@Model.Status == 1415)
{
    $(".chin").css('display', 'block');
    $(".sc").css('display', 'none');
    $(".cl").css('display', 'none');
    $(".RDT").css('display', 'none');
    $(".TDR").css('display', 'none');
    $(".TCR").css('display', 'none');
    $(".pg").css('display', 'none');
    $("#Reschedule_DT").prop("required", false);
    $("#Decline_Reason").prop("required", false);
    $("#Cancel_Reason").prop("required", false);

}
            else {
    $(".sc").css('display', 'none');
    $(".cl").css('display', 'none');
    $(".RDT").css('display', 'none');
    $(".TDR").css('display', 'none');
    $(".TCR").css('display', 'none');
    $(".chin").css('display', 'none');
    $("#Reschedule_DT").prop("required", false);
    $("#Decline_Reason").prop("required", false);
    $("#Cancel_Reason").prop("required", false);
}


$.get(baseurl + "/Transaction/GetContact", { Cnt: @Model.EU_Office }, function (data) {

    if (data.res == "Success") {
        $('#Office_contact tr').has('td').empty();

        $.each(data.CC, function (i, val) {

            $('#Office_contact tr:last').after('<tr><td> <input type="checkbox" name="customCheck_' + data.CC[i]['Id'] + '" class="form-control" id="customCheck_' + data.CC[i]['Id'] + '"  value="' + data.CC[i]['Email'] + '" style="width: 20px; height: 20px;"></td><td><span class="badge" style="font-size:100%;">'
                + data.CC[i]['Person'] + " , " + data.CC[i]['Designation'] + '</span></td><td> <button type="button" title="edit" class="btn btn-link has-ripple" id=' + data.CC[i]['Id'] +
                ' onclick="Contactfill(this.id)"><i class="fa fa-edit"></i></button> <button type="button" title="delete" class="btn btn-link has-ripple" id=' + data.CC[i]['Id'] +
                ' onclick="Contactdelete(this.id)" style="margin-left:10%;"><i class="fa fa-trash"></i></button></td><tr>');
        });

        $("#Contact_ID").val(0);
        $("#Person").val('');
        $("#Designation").val('');
        $("#Department").val('');
        $("#CNumber").val('');
        $("#CEmail").val('');

    } else {

    }
});


if (@Model.Is_Reschedule == 1)
{
    $(".RDT").css('display', 'block');
    $(".TDR").css('display', 'none');
    $(".TCR").css('display', 'none');
    $("#Reschedule_DT").prop("required", true);
    $("#Decline_Reason").prop("required", false);
    $("#Cancel_Reason").prop("required", false);

}



$('#Certification').select2({
    multiple: true,
    placeholder: 'Add Certification Name...'
});

$('#Certification').val([@Model.Certification_Name]).trigger("change");

$.get(baseurl + "Transaction/GetTicketHistory", { tn: @Model.Id }, function (data) {
    if (data.tv == "success") {
        if (Object.keys(data.hist).length > 0) {


            $.each(data.hist, function (i, val) {
                $("#th_TicketNo").val(data.hist[i]['Ticket_No']);
                $("#th_CreatedBy").val(data.hist[i]['CreatedBy']);
                $("#th_CreatedOn").val(data.hist[i]['Createdon']);
                $("#th_Comments").val(data.hist[i]['Remark']);
                $('#Show tr:last').after('<tr><td>' + data.hist[i]['CreatedBy'] + '</td><td>' + data.hist[i]['Createdon'] + '</td><td>' + data.hist[i]['Remark'] + '</td></tr>');
            });
            $('#Show tr:last').remove();
        }

    }
    else {
    }

});




if (@Model.FE_ID > 0)
{

    $.get(baseurl + "Transaction/getFEInfo", { id:@Model.FE_ID
}, function (data) {
    if (data.res == "Success") {


        $('#FE_ID').empty();
        $('#FE_ID').append('<option value =' + data.CC['FE_ID'] + '>' + data.CC['FE_Name'] + '</option>');
        $('#FE_contact').val(data.CC['Phone']);
        $('#FE_Email').val(data.CC['Email']);
        $('#FE_Origin').val(data.CC['FE_Origin']);
        $('#FE_Certification').val(data.cert);
        $('#FE_Buss_Hrs').val(data.CC['Charges_Business_Hour']);
        $('#FE_Non_Buss_Hrs').val(data.CC['Charges_Non_Business_Hour']);
        $('#FE_Minimum_Hrs').val(data.CC['Minimum_Hrs']);
        $('#FE_Fixed').val(data.CC['Charge_Job']);

    }
    else {
    }

});

            }
            else
{
    @* getFEdetails(@Model.Zip_Pin_Code, '@Model.Country');*@
            }



$(document).on("click", ".classAdd", function () { //
    var rowCount = $('.data-contact-person').length + 1;
    var i = rowCount - 1;
    var contactdiv = '<tr class="data-contact-person">' +
        '<td><input type="text" name="IV[' + i + '].Part_type" class="form-control f-name01" /></td>' +
        '<td><input type="text" name="IV[' + i + '].Serial_No" class="form-control l-name01" /></td>' +
        '<td><input type="text" name="IV[' + i + '].Make_Model" class="form-control email01" /></td>' +
        '<td><input type="text" name="IV[' + i + '].Part_Description" class="form-control email02" /></td>' +
        '<td><button type="button" id="btnAdd" class="btn btn-xs btn-primary classAdd"><i class="fas fa-plus-circle"></i></button>' +
        '<button type="button" id="btnDelete" class="deleteContact btn btn btn-danger btn-xs"><i class="fas fa-minus-circle"></i></button></td>' +
        '</tr>';
    $('#maintable').append(contactdiv); // Adding these controls to Main table class
});

$(document).on("click", ".deleteContact", function () {
    $(this).closest("tr").remove(); // closest used to remove the respective 'tr' in which I have my controls
});


        });





$("#Status").change(function () {

    if ($("#Status option:selected").val() == 19) {
        $(".sc").css('display', 'none');
        $(".cl").css('display', 'none');
        $(".RDT").css('display', 'block');
        $(".TDR").css('display', 'none');
        $(".TCR").css('display', 'none');
        $(".chin").css('display', 'none');
        $(".pg").css('display', 'none');
        $("#Reschedule_DT").prop("required", true);
        $("#Decline_Reason").prop("required", false);
        $("#Cancel_Reason").prop("required", false);
    }
    else if ($("#Status option:selected").val() == 21) {
        $(".sc").css('display', 'none');
        $(".cl").css('display', 'none');
        $(".RDT").css('display', 'none');
        $(".TDR").css('display', 'none');
        $(".TCR").css('display', 'block');
        $(".chin").css('display', 'none');
        $("#Reschedule_DT").prop("required", false);
        $("#Decline_Reason").prop("required", false);
        $("#Cancel_Reason").prop("required", true);
    }
    else if ($("#Status option:selected").val() == 1362) {
        $(".sc").css('display', 'none');
        $(".cl").css('display', 'none');
        $(".RDT").css('display', 'none');
        $(".TDR").css('display', 'block');
        $(".TCR").css('display', 'none');
        $(".chin").css('display', 'none');
        $(".pg").css('display', 'none');
        $("#Reschedule_DT").prop("required", false);
        $("#Decline_Reason").prop("required", true);
        $("#Cancel_Reason").prop("required", false);
    }
    else if ($("#Status option:selected").val() == 1372) {
        $(".sc").css('display', 'block');
        $(".cl").css('display', 'none');
        $(".RDT").css('display', 'none');
        $(".TDR").css('display', 'none');
        $(".TCR").css('display', 'none');
        $(".chin").css('display', 'none');
        $("#Reschedule_DT").prop("required", false);
        $("#Decline_Reason").prop("required", false);
        $("#Cancel_Reason").prop("required", false);
    }
    else if ($("#Status option:selected").val() == 20) {
        $(".sc").css('display', 'none');
        $(".cl").css('display', 'block');
        $(".RDT").css('display', 'none');
        $(".TDR").css('display', 'none');
        $(".TCR").css('display', 'none');
        $(".chin").css('display', 'block');
        $(".pg").css('display', 'none');
        $("#Reschedule_DT").prop("required", false);
        $("#Decline_Reason").prop("required", false);
        $("#Cancel_Reason").prop("required", false);
    }
    else if ($("#Status option:selected").val() == 1415) {
        $(".chin").css('display', 'block');
        $(".sc").css('display', 'none');
        $(".cl").css('display', 'none');
        $(".RDT").css('display', 'none');
        $(".TDR").css('display', 'none');
        $(".TCR").css('display', 'none');
        $(".pg").css('display', 'none');
        $("#Reschedule_DT").prop("required", false);
        $("#Decline_Reason").prop("required", false);
        $("#Cancel_Reason").prop("required", false);

    }
    else if ($("#Status option:selected").val() == 1414) {
        $(".chin").css('display', 'none');
        $(".sc").css('display', 'none');
        $(".cl").css('display', 'none');
        $(".RDT").css('display', 'none');
        $(".TDR").css('display', 'none');
        $(".TCR").css('display', 'none');
        $(".pg").css('display', 'none');
        $("#Reschedule_DT").prop("required", false);
        $("#Decline_Reason").prop("required", false);
        $("#Cancel_Reason").prop("required", false);

    }
    else {
        $(".sc").css('display', 'none');
        $(".cl").css('display', 'none');
        $(".RDT").css('display', 'none');
        $(".TDR").css('display', 'none');
        $(".TCR").css('display', 'none');
        $(".chin").css('display', 'none');
        $("#Reschedule_DT").prop("required", false);
        $("#Decline_Reason").prop("required", false);
        $("#Cancel_Reason").prop("required", false);
    }

});





$("#Part_Management").change(function () {

    if ($("#Part_Management option:selected").val() == 22) {
        $(".pickup").css('display', 'block');
        $(".handover").css('display', 'none');
    }
    else if ($("#Part_Management option:selected").val() == 23) {
        $(".pickup").css('display', 'none');
        $(".handover").css('display', 'block');
        $(".pickup-av").css('display', 'none');
        $(".pickup-nav").css('display', 'none');
    }
    else {
        $(".pickup").css('display', 'none');
        $(".handover").css('display', 'none');
        $(".pickup-av").css('display', 'none');
        $(".pickup-nav").css('display', 'none');
    }
});

$("#Other_Charge").change(function () {

    if ($("#Other_Charge option:selected").val() == 1) {
        $(".other-charge").css('display', 'block');
    }
    else if ($("#Other_Charge option:selected").val() == 0) {
        $(".other-charge").css('display', 'none');
    }
    else {
        $("#Other_Charge option:selected").val("");
        $(".other-charge").css('display', 'none');
    }
});

$("#Customer_Email_Send").change(function () {

    if ($("#Customer_Email_Send option:selected").val() == 1) {
        $(".customer-email").css('display', 'block');
    }
    else if ($("#Customer_Email_Send option:selected").val() == 0) {
        $(".customer-email").css('display', 'none');
    }
    else {
        $("#Customer_Email_Send option:selected").val("");
        $(".customer-email").css('display', 'none');
    }
});

$("#FE_Email_Send").change(function () {

    if ($("#FE_Email_Send option:selected").val() == 1) {
        $(".fe-email").css('display', 'block');
        $("#FE_Email_Name").val($("#FE_Email").val());

    }
    else if ($("#FE_Email option:selected").val() == 0) {
        $(".fe-email").css('display', 'none');
        $("#FE_Email_Name").val('');
    }
    else {
        $("#FE_Email option:selected").val('');
        $(".fe-email").css('display', 'none');
        $("#FE_Email_Name").val('');
    }
});

$("#Return_Label").change(function () {

    if ($("#Return_Label option:selected").val() == 1) {
        $(".pickup-av").css('display', 'block');
        $(".pickup-nav").css('display', 'none');
    }
    else if ($("#Return_Label option:selected").val() == 0) {
        $(".pickup-av").css('display', 'block');
        $(".pickup-nav").css('display', 'block');
    }
    else {
        $("#Return_Label option:selected").val("");
        $(".pickup-av").css('display', 'none');
        $(".pickup-nav").css('display', 'none');
    }
});

$("#FE_Part_Storage_Charge").change(function () {

    if ($("#FE_Part_Storage_Charge option:selected").val() == 1) {
        $(".fe-charge").css('display', 'block');
    }
    else if ($("#FE_Part_Storage_Charge option:selected").val() == 0) {
        $(".fe-charge").css('display', 'none');
    }
    else {
        $("#FE_Part_Storage_Charge option:selected").val("");
        $(".fe-charge").css('display', 'none');
    }
});

$("#Certification_Need").change(function () {

    if ($("#Certification_Need option:selected").val() == 1) {

        $(".cert").css('display', 'grid');
    }
    else {
        $(".cert").css('display', 'none');
    }
});
$("#Street_Address").blur(function () {
    geocode()
});

$("#Pregame").change(function () {

    if ($("#Pregame option:selected").val() == 1) {

        $(".pg").css('display', 'grid');
    }
    else {
        $(".pg").css('display', 'none');
    }
});


@* function getAllEmpData() {
    var data = [];
    $('tr.data-contact-person').each(function () {
        var Part_Type = $(this).find('.f-name01').val();//Bind to the first name with class f-name01
        var Serial_No = $(this).find('.l-name01').val();//Bind to the last name with class l-name01
        var Make_Model = $(this).find('.email01').val();//Bind to the emailId with class email01
        var Part_description = $(this).find('.email02').val();//Bind to the emailId with class email01
        var alldata = {
            Part_Type, //FName as per Employee class name in .cs
            Serial_No, //LName as per Employee class name in .cs
            Make_Model, //EmailId as per Employee class name in .cs
            Part_description, //EmailId as per Employee class name in .cs
                 @Model.Id,
    }
                data.push(alldata);
});
console.log(data);//
return data;
        }
$("#btnSubmit").click(function () {
    debugger
    var data = JSON.stringify(getAllEmpData());
    //console.log(data);
    $.ajax({
        url: baseurl + 'Transaction/PartSaveData',//Home.aspx is the page
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.l({ 'empdata': data }),
        success: function () {
            //  alert("Data Added Successfully");
        },
        error: function () {
            //   alert("Data Added Successfully");
        }
    });
});*@

function printData() {
    var divToPrint = document.getElementById("printTable");
    newWin = window.open("");
    newWin.document.write(divToPrint.outerHTML);
    newWin.print();
    newWin.close();
}
$('.btn-print-invoice').on('click', function () {
    printData();
})

function Contactfill(id) {

    $.get(baseurl + "/Master/GetContact", { Id: id }, function (data) {

        if (data.res == "Success") {
            $("#Contact_ID").val(data.CC['Id']);
            $("#Person").val(data.CC['Person']);
            $("#Designation").val(data.CC['Designation']);
            $("#Department").val(data.CC['Department']);
            $("#CNumber").val(data.CC['Number']);
            $("#CEmail").val(data.CC['Email']);
            $("#Contact_btn").val('Update');
            $("#Contact_add").css('display', 'unset');
        }
        else {

        }
    });
}

function Contactdelete(id) {

    var code = $("#Office").val();
    $("#loadermodel").css('display', 'unset');
    $.get(baseurl + "/Transaction/deleteContact", { OC: id }, function (data) { });


    $.get(baseurl + "/Transaction/GetContact", { Cnt: code }, function (data) {

        if (data.res == "Success") {
            $('#Office_contact tr').has('td').empty();

            $.each(data.CC, function (i, val) {

                $('#Office_contact tr:last').after('<tr><td> <input type="checkbox" name="customCheck_' + data.CC[i]['Id'] + '" class="form-control" id="customCheck_' + data.CC[i]['Id'] + '"  value="' + data.CC[i]['Email'] + '" style="width: 20px; height: 20px;"></td><td><span class="badge" style="font-size:100%;">'
                    + data.CC[i]['Person'] + " , " + data.CC[i]['Designation'] + '</span></td><td> <button type="button" title="edit" class="btn btn-link has-ripple" id=' + data.CC[i]['Id'] +
                    ' onclick="Contactfill(this.id)"><i class="fa fa-edit"></i></button> <button type="button" title="delete" class="btn btn-link has-ripple" id=' + data.CC[i]['Id'] +
                    ' onclick="Contactdelete(this.id)" style="margin-left:10%;"><i class="fa fa-trash"></i></button></td><tr>');
            });

            $("#Contact_ID").val(0);
            $("#Person").val('');
            $("#Designation").val('');
            $("#Department").val('');
            $("#CNumber").val('');
            $("#CEmail").val('');

        } else {

        }
    });



    $("#loadermodel").css('display', 'none');
}


$("#Contact_btn").click(function () {
    $("#loadermodel").css('display', 'unset');
    var code = $("#Office").val();

    $.get(baseurl + "/Transaction/ContactEUMaster", {
        Office_ID: $("#Office").val(), Person: $("#Person").val(),
        Designation: $("#Designation").val(), Department: $("#Department").val(), Number: $("#CNumber").val(), Email: $("#CEmail").val(),
        Contact_ID: $("#Contact_ID").val()
    }, function (data) { });

    $.get(baseurl + "/Transaction/GetContact", { Cnt: code }, function (data) {

        if (data.res == "Success") {
            $('#Office_contact tr').has('td').empty();

            $.each(data.CC, function (i, val) {

                $('#Office_contact tr:last').after('<tr><td> <input type="checkbox" name="customCheck_' + data.CC[i]['Id'] + '" class="form-control" id="customCheck_' + data.CC[i]['Id'] + '"  value="' + data.CC[i]['Email'] + '" style="width: 20px; height: 20px;"></td><td><span class="badge" style="font-size:100%;">'
                    + data.CC[i]['Person'] + " , " + data.CC[i]['Designation'] + '</span></td><td> <button type="button" title="edit" class="btn btn-link has-ripple" id=' + data.CC[i]['Id'] +
                    ' onclick="Contactfill(this.id)"><i class="fa fa-edit"></i></button> <button type="button" title="delete" class="btn btn-link has-ripple" id=' + data.CC[i]['Id'] +
                    ' onclick="Contactdelete(this.id)" style="margin-left:10%;"><i class="fa fa-trash"></i></button></td><tr>');
            });
            $("#Contact_ID").val(0);
            $("#Person").val('');
            $("#Designation").val('');
            $("#Department").val('');
            $("#CNumber").val('');
            $("#CEmail").val('');

        } else {

        }
    });
    $("#loadermodel").css('display', 'none');
});


$("#Contact_add").click(function () {
    $("#Contact_ID").val(0);
    $("#Person").val('');
    $("#Designation").val('');
    $("#Department").val('');
    $("#CNumber").val('');
    $("#CEmail").val('');
    $("#Contact_btn").val('Add');
    $("#Contact_add").css('display', 'none');
});

$('#save_value').click(function () {

    var val = [];
    $(':checkbox:checked').each(function (i) {
        val[i] = $(this).val();
    });
    $('#email_office').val(val);

});


$('#FEStatus').click(function () {
    debugger

    var code = @Model.Id
    $.get(baseurl + "/Transaction/GetFETicketStatus", { Ticket: code }, function (data) {

        if (data.res == "Success") {

            $('#FieldEngineer_Tic tr').has('td').remove();
            $.each(data.PZ, function (i, val) {

                $('#FieldEngineer_Tic tr:last').after('<tr><td><button class="btn-fepop btn-info has-ripple" data-dismiss="modal" aria-label="Close"  onClick="getFEInfo(' + data.PZ[i]['FE_ID'] + ')" style="padding:none;"><i class="fas fa-check"></i></button></td><td>' + data.PZ[i]['Name'] + '</td> <td>' +
                    data.PZ[i]['Status'] + '</td><td>' + data.PZ[i]['Remark'] + '</td><td>' + data.PZ[i]['Sent'] + '</td><td>' + data.PZ[i]['Request'] + '</td><tr>');
            });

            $('#modal_FieldEngineer').modal('show');




        } else {

        }
    });

});
    </script >

    <script>
        var map;

        function GoLocation() {

            map = new google.maps.Map(document.getElementById('map_canvas'), {
                zoom: 14,
                mapTypeId: 'roadmap',
                center: { lat: @Model.latitude,lng:@Model.longitude}
            });
        $.get("@Url.Action("getFEdetailsMap", "Transaction", new {id = Model.Id})", function (data, status) {


                var iconName ;
        var marker=[];
        var contentString= [];
        var infowindow = [];
        for (var i=0; i < data.length; i++){


                   // iconName = data[i].Type == 'FE' ? "/icon/icon-fe.png" : 'FE_Rej' ? "/icon/icon-fe-rej.png" : 'FE_Cert' ? "/icon/icon-fe-cert.png" : "/icon/icon-eu.png"

                    if (data[i].Type == 'FE')
        {
            latLng = new google.maps.LatLng(data[i].latitude, data[i].longitude);
        iconName = "/icon/icon-fe.png";
        contentString[i] = '<div id="content_' + data[i].FE_ID +'" style="width:350px;">' +

            '<h3 id="FEHeading_'+data[i].FE_ID+'" style="text-align: center;color: #505050;">' + data[i].FE_Name + '</h1>' +
            '<div id="bodyContent_' + data[i].FE_ID + '">' +
                '<label id="Status_' + data[i].FE_ID + '"> <b> FE Status : </b> ' + data[i].Status + '</label><br />' +
                '<label id="Address_'+data[i].FE_ID+'"> <b> FE Origin : </b> ' + data[i].FE_Origin + '</label><br />' +
                '<label id="Phone_'+data[i].FE_ID+'"> <b> FE Phone : </b> ' + data[i].Phone + '</label><br />' +
                '<label id="Email_'+data[i].FE_ID+'"> <b> FE Email : </b> ' + data[i].Email + '</label><br /> '+
                '<label id="CBH_'+data[i].FE_ID+'"> <b> FE Charges Per Business Hour : </b> ' + data[i].Charges_Business_Hour + '</label><br /> '+
                '<label id="CNBH_'+data[i].FE_ID+'"> <b> FE Charges Per Non Business Hour : </b> ' + data[i].Charges_Non_Business_Hour + '</label><br /> '+
                '<label id="CJ_'+data[i].FE_ID+'"> <b> FE Charges Per Job : </b> ' + data[i].Charge_Job + '</label><br /> '+
                '<input type=button class="btn btn-info has-ripple" value="Select This FE" onClick="getFEInfo('+data[i].FE_ID+')"><input type=button class="btn btn-primary  has-ripple" value="Send Details" onClick="getFEMail('+data[i].FE_ID+')" style="margin-left:10%;"></div>';
                    }
                    else if (data[i].Type == 'FE_Rej') {

                        latLng = new google.maps.LatLng(data[i].latitude, data[i].longitude);
                    iconName = "/icon/icon-fe-rej.png";
                    contentString[i] = '<div id="content_' + data[i].FE_ID + '" style="width:350px;">' +

                        '<h3 id="FEHeading_' + data[i].FE_ID + '" style="text-align: center;color: #505050;">' + data[i].FE_Name + '</h1>' +
                        '<div id="bodyContent_' + data[i].FE_ID + '">' +
                            '<label id="Status_' + data[i].FE_ID + '"> <b> FE Status : </b> ' + data[i].Status + '</label><br />' +
                            '<label id="Address_' + data[i].FE_ID + '"> <b> FE Origin : </b> ' + data[i].FE_Origin + '</label><br />' +
                            '<label id="Phone_' + data[i].FE_ID + '"> <b> FE Phone : </b> ' + data[i].Phone + '</label><br />' +
                            '<label id="Email_' + data[i].FE_ID + '"> <b> FE Email : </b> ' + data[i].Email + '</label><br /> ' +
                            '<label id="CBH_' + data[i].FE_ID + '"> <b> FE Charges Per Business Hour : </b> ' + data[i].Charges_Business_Hour + '</label><br /> ' +
                            '<label id="CNBH_' + data[i].FE_ID + '"> <b> FE Charges Per Non Business Hour : </b> ' + data[i].Charges_Non_Business_Hour + '</label><br /> ' +
                            '<label id="CJ_' + data[i].FE_ID + '"> <b> FE Charges Per Job : </b> ' + data[i].Charge_Job + '</label><br /> ' +
                            '<label id="Remark_' + data[i].FE_ID + '"> <b> Rejection Remark : </b> ' + data[i].Remark + '</label><br /> ' +
                            '<input type=button class="btn btn-info has-ripple" value="Select This FE" onClick="getFEInfo(' + data[i].FE_ID + ')"><input type=button class="btn btn-primary  has-ripple" value="Send Details" onClick="getFEMail(' + data[i].FE_ID + ')" style="margin-left:10%;"></div>';
                    }
                                else if (data[i].Type == 'FE_Cert') {

                                    latLng = new google.maps.LatLng(data[i].latitude, data[i].longitude);
                                iconName = "/icon/icon-fe-cert.png";
                                contentString[i] = '<div id="content_' + data[i].FE_ID + '" style="width:350px;">' +

                                    '<h3 id="FEHeading_' + data[i].FE_ID + '" style="text-align: center;color: #505050;">' + data[i].FE_Name + '</h1>' +
                                    '<div id="bodyContent_' + data[i].FE_ID + '">' +
                                        '<label id="Status_' + data[i].FE_ID + '"> <b> FE Status : </b> ' + data[i].Status + '</label><br />' +
                                        '<label id="Address_' + data[i].FE_ID + '"> <b> FE Origin : </b> ' + data[i].FE_Origin + '</label><br />' +
                                        '<label id="Phone_' + data[i].FE_ID + '"> <b> FE Phone : </b> ' + data[i].Phone + '</label><br />' +
                                        '<label id="Email_' + data[i].FE_ID + '"> <b> FE Email : </b> ' + data[i].Email + '</label><br /> ' +
                                        '<label id="CBH_' + data[i].FE_ID + '"> <b> FE Charges Per Business Hour : </b> ' + data[i].Charges_Business_Hour + '</label><br /> ' +
                                        '<label id="CNBH_' + data[i].FE_ID + '"> <b> FE Charges Per Non Business Hour : </b> ' + data[i].Charges_Non_Business_Hour + '</label><br /> ' +
                                        '<label id="CJ_' + data[i].FE_ID + '"> <b> FE Charges Per Job : </b> ' + data[i].Charge_Job + '</label><br /> ' +
                                        '<input type=button class="btn btn-info has-ripple" value="Select This FE" onClick="getFEInfo(' + data[i].FE_ID + ')"><input type=button class="btn btn-primary  has-ripple" value="Send Details" onClick="getFEMail(' + data[i].FE_ID + ')" style="margin-left:10%;"></div>';
                    }
                                            else if (data[i].Type == 'FE_Deactive') {

                                                latLng = new google.maps.LatLng(data[i].latitude, data[i].longitude);
                                            iconName = "/icon/icon-fe-rej.png";
                                            contentString[i] = '<div id="content_' + data[i].FE_ID + '" style="width:350px;">' +

                                                '<h3 id="FEHeading_' + data[i].FE_ID + '" style="text-align: center;color: #505050;">' + data[i].FE_Name + '</h1>' +
                                                '<div id="bodyContent_' + data[i].FE_ID + '">' +
                                                    '<label id="Status_' + data[i].FE_ID + '"> <b> FE Status : </b> ' + data[i].Status + '</label><br />' +
                                                    '<label id="Address_' + data[i].FE_ID + '"> <b> FE Origin : </b> ' + data[i].FE_Origin + '</label><br />' +
                                                    '<label id="Phone_' + data[i].FE_ID + '"> <b> FE Phone : </b> ' + data[i].Phone + '</label><br />' +
                                                    '<label id="Email_' + data[i].FE_ID + '"> <b> FE Email : </b> ' + data[i].Email + '</label><br /> ' +
                                                    '<label id="CBH_' + data[i].FE_ID + '"> <b> FE Charges Per Business Hour : </b> ' + data[i].Charges_Business_Hour + '</label><br /> ' +
                                                    '<label id="CNBH_' + data[i].FE_ID + '"> <b> FE Charges Per Non Business Hour : </b> ' + data[i].Charges_Non_Business_Hour + '</label><br /> ' +
                                                    '<label id="CJ_' + data[i].FE_ID + '"> <b> FE Charges Per Job : </b> ' + data[i].Charge_Job + '</label><br /> ' +
                                                    '<input type=button class="btn btn-primary  has-ripple" value="Send Details" onClick="getFEMail(' + data[i].FE_ID + ')" style="margin-left:10%;"></div>';
                    }
                                                    //rohit's addition for FE-BlackList
                                                    else if (data[i].Type == 'FE_Blacklist') {
                        debugger;
                                                    latLng = new google.maps.LatLng(data[i].latitude, data[i].longitude);
                                                    iconName = "/icon/icon-fe-blacklist.png";
                                                    contentString[i] = '<div id="content_' + data[i].FE_ID + '" style="width:350px;">' +

                                                        '<h3 id="FEHeading_' + data[i].FE_ID + '" style="text-align: center;color: #505050;">' + data[i].FE_Name + '</h1>' +
                                                        '<div id="bodyContent_' + data[i].FE_ID + '">' +
                                                            '<label id="Status_' + data[i].FE_ID + '"> <b> FE Status : </b> ' + data[i].Status + '</label><br />' +
                                                            '<label id="Address_' + data[i].FE_ID + '"> <b> FE Origin : </b> ' + data[i].FE_Origin + '</label><br />' +
                                                            '<label id="Phone_' + data[i].FE_ID + '"> <b> FE Phone : </b> ' + data[i].Phone + '</label><br />' +
                                                            '<label id="Email_' + data[i].FE_ID + '"> <b> FE Email : </b> ' + data[i].Email + '</label><br /> ' +
                                                            '<label id="CBH_' + data[i].FE_ID + '"> <b> FE Charges Per Business Hour : </b> ' + data[i].Charges_Business_Hour + '</label><br /> ' +
                                                            '<label id="CNBH_' + data[i].FE_ID + '"> <b> FE Charges Per Non Business Hour : </b> ' + data[i].Charges_Non_Business_Hour + '</label><br /> ' +
                                                            '<label id="CJ_' + data[i].FE_ID + '"> <b> FE Charges Per Job : </b> ' + data[i].Charge_Job + '</label><br /> ' +
                                                            '<input type=button class="btn btn-info has-ripple" value="Select This FE" onClick="getFEInfo(' + data[i].FE_ID + ')"><input type=button class="btn btn-primary  has-ripple" value="Send Details" onClick="getFEMail(' + data[i].FE_ID + ')" style="margin-left:10%;"></div>';
                    }
                    //else if (data[i].Type == 'FE_BlacklistDe') {
                    //    debugger;
                    //    latLng = new google.maps.LatLng(data[i].latitude, data[i].longitude);
                    //    iconName = "/icon/icon-fe-blacklist.png";
                    //    contentString[i] = '<div id="content_' + data[i].FE_ID + '" style="width:350px;">' +

                    //        '<h3 id="FEHeading_' + data[i].FE_ID + '" style="text-align: center;color: #505050;">' + data[i].FE_Name + '</h1>' +
                    //        '<div id="bodyContent_' + data[i].FE_ID + '">' +
                    //        '<label id="Status_' + data[i].FE_ID + '"> <b> FE Status : </b> ' + data[i].Status + '</label><br />' +
                    //        '<label id="Address_' + data[i].FE_ID + '"> <b> FE Origin : </b> ' + data[i].FE_Origin + '</label><br />' +
                    //        '<label id="Phone_' + data[i].FE_ID + '"> <b> FE Phone : </b> ' + data[i].Phone + '</label><br />' +
                    //        '<label id="Email_' + data[i].FE_ID + '"> <b> FE Email : </b> ' + data[i].Email + '</label><br /> ' +
                    //        '<label id="CBH_' + data[i].FE_ID + '"> <b> FE Charges Per Business Hour : </b> ' + data[i].Charges_Business_Hour + '</label><br /> ' +
                    //        '<label id="CNBH_' + data[i].FE_ID + '"> <b> FE Charges Per Non Business Hour : </b> ' + data[i].Charges_Non_Business_Hour + '</label><br /> ' +
                    //        '<label id="CJ_' + data[i].FE_ID + '"> <b> FE Charges Per Job : </b> ' + data[i].Charge_Job + '</label><br /> ' +
                    //        '<input type=button class="btn btn-info has-ripple" value="Select This FE" onClick="getFEInfo(' + data[i].FE_ID + ')"><input type=button class="btn btn-primary  has-ripple" value="Send Details" onClick="getFEMail(' + data[i].FE_ID + ')" style="margin-left:10%;"></div>';
                    //}
                    else if (data[i].Type == 'FE_Deactive_NoLan') {

                        var location = data[i].latitude + ' ' + data[i].longitude;
                                                                            var lat = "";
                                                                            var lng = "";
                                                                            axios.get('https://maps.googleapis.com/maps/api/geocode/json', {
                                                                                params: {
                                                                                address: location,
                                                                            key: 'AIzaSyBvhQbrULBXqk9wPb9RjTeacVk3A3g-ci8'
                            }
                        })
                                                                            .then(function (response) {
                                                                                // Log full response
                                                                                console.log(response);

                                                                            // Formatted Address
                                                                            var formattedAddress = response.data.results[0].formatted_address;


                                                                            // Address Components
                                                                            var addressComponents = response.data.results[0].address_components;

                                                                            // Geometry
                                                                            lat = response.data.results[0].geometry.location.lat;
                                                                            lng = response.data.results[0].geometry.location.lng;



                            })
                                                                            .catch(function (error) {
                                                                                console.log(error);
                            });

                                                                            latLng = new google.maps.LatLng(lat, lng);
                                                                            iconName = "/icon/icon-fe-rej.png";
                                                                            contentString[i] = '<div id="content_' + data[i].FE_ID + '" style="width:350px;">' +

                                                                                '<h3 id="FEHeading_' + data[i].FE_ID + '" style="text-align: center;color: #505050;">' + data[i].FE_Name + '</h1>' +
                                                                                '<div id="bodyContent_' + data[i].FE_ID + '">' +
                                                                                    '<label id="Status_' + data[i].FE_ID + '"> <b> FE Status : </b> ' + data[i].Status + '</label><br />' +
                                                                                    '<label id="Address_' + data[i].FE_ID + '"> <b> FE Origin : </b> ' + data[i].FE_Origin + '</label><br />' +
                                                                                    '<label id="Phone_' + data[i].FE_ID + '"> <b> FE Phone : </b> ' + data[i].Phone + '</label><br />' +
                                                                                    '<label id="Email_' + data[i].FE_ID + '"> <b> FE Email : </b> ' + data[i].Email + '</label><br /> ' +
                                                                                    '<label id="CBH_' + data[i].FE_ID + '"> <b> FE Charges Per Business Hour : </b> ' + data[i].Charges_Business_Hour + '</label><br /> ' +
                                                                                    '<label id="CNBH_' + data[i].FE_ID + '"> <b> FE Charges Per Non Business Hour : </b> ' + data[i].Charges_Non_Business_Hour + '</label><br /> ' +
                                                                                    '<label id="CJ_' + data[i].FE_ID + '"> <b> FE Charges Per Job : </b> ' + data[i].Charge_Job + '</label><br /> ' +
                                                                                    '<input type=button class="btn btn-primary  has-ripple" value="Send Details" onClick="getFEMail(' + data[i].FE_ID + ')" style="margin-left:10%;"></div>';
                    }
                                                                                    else
                                                                                    {

                                                                                        latLng = new google.maps.LatLng(data[i].latitude, data[i].longitude);
                                                                                    iconName = "/icon/icon-eu.png";
                                                                                    contentString[i] = '<div id="content">' +
                                                                                        '<h3 id="EUHeading" style="text-align: center;color: #505050;">' + data[i].FE_Name + '</h1>' +
                                                                                        '<div id="bodyContent">' +
                                                                                            '<label><b>Site Origin : </b> ' + data[i].FE_Origin + '</label></div>';
                    }

                                                                                        marker[i] = new google.maps.Marker({
                                                                                            position: latLng,
                                                                                        map: map,
                                                                                        icon: iconName
                    });

                                                                                        infowindow[i] = new google.maps.InfoWindow({
                                                                                            content: contentString[i]
                    });

                                                                                        google.maps.event.addListener(marker[i], 'click', (function (marker, i) {
                        return function () {
                                                                                            infowindow[i].open(map, marker[i]);
                        }
                    })(marker, i));
                }

            });

        }
                                                                                        function geocode() {

            var location = $('#Street_Address').val();

                                                                                        axios.get('https://maps.googleapis.com/maps/api/geocode/json', {
                                                                                            params: {
                                                                                            address: location,
                                                                                        key: 'AIzaSyBvhQbrULBXqk9wPb9RjTeacVk3A3g-ci8'
                }
            })
                                                                                        .then(function (response) {
                                                                                            // Log full response
                                                                                            console.log(response);

                                                                                        // Formatted Address
                                                                                        var formattedAddress = response.data.results[0].formatted_address;


                                                                                        // Address Components
                                                                                        var addressComponents = response.data.results[0].address_components;


                                                                                        // Geometry
                                                                                        var lat = response.data.results[0].geometry.location.lat;
                                                                                        var lng = response.data.results[0].geometry.location.lng;


                                                                                        // Output to app
                                                                                        $('#latitude').val(lat);
                                                                                        $('#longitude').val(lng);

                                                                                        //map
                                                                                        var myLatlng = new google.maps.LatLng(lat, lng);
                                                                                        var geocoder = new google.maps.Geocoder();
                                                                                        var infowindow = new google.maps.InfoWindow();
                                                                                        var mapOptions = {
                                                                                            zoom: 18,
                                                                                        center: myLatlng,
                                                                                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    };

                                                                                        map = new google.maps.Map(document.getElementById("myMap"), mapOptions);

                                                                                        marker = new google.maps.Marker({
                                                                                            map: map,
                                                                                        position: myLatlng,
                                                                                        draggable: true
                    });

                                                                                        geocoder.geocode({'latLng': myLatlng }, function (results, status) {
                        if (status == google.maps.GeocoderStatus.OK) {
                            if (results[0]) {
                                                                                            $('#latitude,#longitude').show();
                                                                                        //   $('#Street_Address').val(results[0].formatted_address);
                                                                                        $('#latitude').val(marker.getPosition().lat());
                                                                                        $('#longitude').val(marker.getPosition().lng());
                                                                                        var address = results[0].address_components;
                                                                                        $('#ZipCode_Pincode').val(address[address.length - 1].long_name);
                                                                                        $('#City').val(address[address.length - 4].long_name);
                                                                                        $('#State').val(address[address.length - 3].long_name);
                                                                                        $('#Country').val(address[address.length - 2].long_name);
                                                                                        infowindow.setContent(results[0].formatted_address);
                                                                                        infowindow.open(map, marker);
                            }
                        }
                    });

                                                                                        google.maps.event.addListener(marker, 'dragend', function () {

                                                                                            geocoder.geocode({ 'latLng': marker.getPosition() }, function (results, status) {
                                                                                                if (status == google.maps.GeocoderStatus.OK) {
                                                                                                    if (results[0]) {
                                                                                                        //   $('#Street_Address').val(results[0].formatted_address);
                                                                                                        $('#latitude').val(marker.getPosition().lat());
                                                                                                        $('#longitude').val(marker.getPosition().lng());
                                                                                                        var address = results[0].address_components;
                                                                                                        $('#ZipCode_Pincode').val(address[address.length - 1].long_name);
                                                                                                        $('#City').val(address[address.length - 4].long_name);
                                                                                                        $('#State').val(address[address.length - 3].long_name);
                                                                                                        $('#Country').val(address[address.length - 2].long_name);
                                                                                                        infowindow.setContent(results[0].formatted_address);
                                                                                                        infowindow.open(map, marker);
                                                                                                    }
                                                                                                }
                                                                                            });
                    });


                })
                                                                                        .catch(function (error) {
                                                                                            console.log(error);
                });
        }


                                                                                        $(document).ready(function () {
            
                var selectedText = $("#Status").find("option:selected").text();
                                                                                        $("#ticketStatus").text("Ticket Status: " + selectedText);
            });

                                                                               