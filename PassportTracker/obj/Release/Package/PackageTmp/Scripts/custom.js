$(document).ready(function () {
    $(function () {

        var options = {
            "appIconUrl": 'https://dev-sccm/njcc/Images/njccEventPic.JPG',
            "appTitle": "Passport Tracker",
            "appHelpPageUrl": "Help.html?"
                + document.URL.split("?")[1],
            "settingsLinks": [
                {
                    "linkUrl": "Account.html?"
                        + document.URL.split("?")[1],
                    "displayName": "Account settings"
                },
                {
                    "linkUrl": "Contact.html?"
                        + document.URL.split("?")[1],
                    "displayName": "Contact us"
                }
            ]
        };


        var nav = new SP.UI.Controls.Navigation("chrome_ctrl_container", options);
        nav.setVisible(true);
    });

    $(".datepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true,
        dateFormat: "dd-MM-yy",
        altField: '#date_due',
        altFormat: 'yy-mm-dd',
        firstDay: 1 // rows starts on Monday
    })
        .change(function () {
            if($(this).attr("name")== "PF_Next_Hearing_Date")
              $("#PF_Next_Hearing_Date").val($(this).val());
            else
                if ($(this).attr("name")==  "PF_Date_Of_Birth")
                  $("#PF_Next_Hearing_Date").html($(this).val());
    });
    


    $('#PF_Current_Location1').change(function (event)
    {
        $("#PF_OriginalTransferedLoc").val($("#PF_Current_Location").val());
        $("#PF_Current_Location").html($(this).val());
        $("#PF_Current_Location").val($(this).val());
        $.getJSON("/PassportForm/DoTransfer", { id: $("#PF_Id").val(), CL: $("#PF_Current_Location").val(), OL: $("#PF_OriginalTransferedLoc").val() }, function () {
    
        });
    });

    $("form").submit(function () {
        var option = $("#CS_Id").find("option:selected").text();
        $("#PF_Country").val(option);
        $('input[type=submit]', this).attr('disabled', 'disabled');
    });


    TableTools.BUTTONS.Back = $.extend({}, TableTools.buttonBase, {
        "sAction":"div",
        "sTag":"default",
        "sToolTip":"Back to main List",
        "sNewLine": " ",
        "sButtonText": "Back to List",
        "fnClick": function( nButton, oConfig ) {
            document.location.href = "/?SPHostUrl=" + $("#SPHostUrl").val();
        }
    });
  

    // ************* Datatables Server Side ********************/
    var oTable = $('#example').dataTable({
 
        "sDom": 'T<"clear">lfrtip',
        "oTableTools": {           
            "aButtons": ["Back"]
        },

        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).on('click', function () {
               // var temp = "/PassportForm/Edit/" + aData[8] + "?SPHostUrl=" + $("#SPHostUrl").val();
                document.location.href = "/PassportForm/Edit/" + aData[8] + "?SPHostUrl=" + $("#SPHostUrl").val();
            })
        },
        "bLengthChange": false,
        "bPaginate": false,
        "bServerSide": true,  
        "sAjaxSource":"../Home/GetAjaxData",   
        "bProcessing": true,
        "bFilter": true,      
        "aoColumn": [  
       
        {"sName":"ViewColumn"},
        {"sName":"Initial Location"},
        {"sName":"Passport Number" },
        {"sName":"Name" },
        {"sName":"Date of Birth"},
        {"sName":"Case Id"},
        {"sName":"Date Handed In"},
        {"sName":"Date Returned" },
        {"sName:":"Id", "bSearchable": false, "bVisible": false }]
       
         
    })
      .rowGrouping({
          iGroupingColumnIndex2: 1
          //  bExpandableGrouping: true
          //  iExpandGroupOffset: 1,
          //  asExpandedGroups: [""],
          // bExpandableGrouping2: true
          //  iExpandGroupOffset: 2,
          //  asExpandedGroups2: [""]
      });


   // $('#example_filter input').unbind();

    $('#example_filter input').bind('keyup', function (e) {
        if ($(this).val().length > 2) {
            oTable.fnFilter(this.value);
        }
    });


    $('#locationDatatable').dataTable({
        "bLengthChange": false,
        "bPaginate": false
    }).rowGrouping(
        {
            iGroupingColumnIndex: 0,
            bExpandableGrouping: true,          
            asExpandedGroups: [""]
         
        });

  
    //Moved to server side
    //$('#PF_Country1').change(function (event) {
       
    //    var myVal = $(this).find("option:selected").text();
    //    $("#PF_Country").val(myVal.split("--")[0]);
    //    $("#PF_Country_Location").val(myVal.substr(myVal.indexOf('--') + 2));
    
    //});

   
    $('#accessgroups').dataTable
        ({
            "bLengthChange": false,
            "bPaginate": true,
            "sScrollY": 400,
            "bJQueryUI": true,
            "sPaginationType": "full_numbers",

        })
            .rowGrouping({
                iGroupingColumnIndex: 0,
                iGroupingOrderByColumnIndex: 0,
                //iGroupingColumnIndex2: 1,
                iGroupingOrderByColumnIndex: 0,
                bExpandableGrouping: true,
                iExpandGroupOffset: 1,
                bExpandableGrouping2: true,
                iExpandGroupOffset: 2,
            });

    $("#UsersDataList").click(function () {

        $.getJSON($('#UserName').attr("data-autocompleteme"), function (data) {
            var items;
            $.each(data, function (i, alrs) {
                items += "<option>" + data[i] + "</option>";
            });
            $('#UserName').html(items);
        });

    });

    

     $('#example tbody').on('click', 'td.subgroup', function () {
         oSettings = oTable.fnSettings();

         value2 = $(this).html().toLowerCase();
         test5 = value2.replace(/ /g, "").replace(/-/g, "").replace(/'/g, "");
         test1 = $(this).attr("class");
         test3 = test1.replace(/(subgroup|-)/g, "");
         test4 = test3.replace(test5, "");
         value1 = test4.replace(/ /g, "").replace(/-/g, "");

       
        if (oSettings != null) {
            oSettings.aoServerParams.push({
                "sName": "user",
                "fn": function (aoData) {
                    aoData.push(
                        { "name": "first_data", "value": value1 },
                        { "name": "second_data",  "value": value2 });
                }
            });

            oTable.fnDraw();
        }
    });

 

});

// use
//<li><a id="tab1" href="/PassportForm/TransferTo/@Model.PF_Id" class="modalDlg" style="font-size:10px; text-align:left">Transfer to...</a></li>

//$('.modalDlg').click(function (event) {
//    loadDialog(this, event, '#PF_Initial_Location1');
//});



//function loadDialog(tag, event, target) {
//    event.preventDefault();
//    var $loading = $('<div>Please Select from drop down</div>');
//    var $url = $(tag).attr('href');
//    var $title = $(tag).attr('title');
//    var $dialog = $('<td align="left"><select data-val="true"  id="PF_Country" name="PF_Country"> <option>Australia</option> <option>Uruguay</option></select></td>');
   
//    $dialog
//	.append($loading)
//	.load($url)
//	.dialog({
//	    autoOpen: false
//		, title: $title
//		, width: 500
//        	, modal: true
//		, minHeight: 200
//        	, show: 'fade'
//        	, hide: 'fade'
//	});

//    $dialog.dialog("option", "buttons", {
//        "Cancel": function () {
//            $(this).dialog("close");
//            $(this).empty();
//        },
//        "Submit": function () {
//            var dlg = $(this);
//            var selected = $("#PF_Country").val();
//            $.ajax({
//                url: $url,
//                type: 'POST',
//                data: $("#target").serialize(),
//                success: function (response) {
//                    dlg.dialog('close');
//                    $(target).html(response);
//                    dlg.empty();
//                    $("#ajaxResult").hide().html('Record saved.').fadeIn(300, function () {
//                        var e = this;
//                        setTimeout(function () { $(e).fadeOut(400); }, 2500);
//                    });
//                },
//                error: function (xhr) {
//                    if (xhr.status == 400)
//                        dlg.html(xhr.responseText, xhr.status);     /* display validation errors in this dialog */
//                    else
//                        displayError(xhr.responseText, xhr.status); /* display other errors in separate dialog */
//                }
//            });
//        }
//    });

//    $dialog.dialog('open');
//};
