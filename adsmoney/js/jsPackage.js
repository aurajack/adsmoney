function getPackage() {
    //$('#divData').html($("#divLoadingTemplate").clone().children());
    $.ajax({
        url: '../action/PackageAction.aspx',
        data: {
            action: 'packagedata',
            memid: checkExpire(),
        },
        type: "POST",
        dataType: "json",
        error: function () { alert('error display data.');},
        success: function (data) {

            var _str = ' <table style="width: 350px; border-collapse: collapse; border-spacing: 0; font-size: 14px; font-weight: bold; height: 35px;"> ';

            if (data.length > 0) {

                for (var i = 0; i < data.length; i++) {
                  
                    var _check = '';

                    if (i == 0) {
                        _check = 'checked="checked"';
                        $('#hdfPackID').val(data[i].id);
                    }

                    _str += '<tr> ';
                    _str += '   <td style="text-align:left;"><input type="radio" name="package" ' + _check + ' value="' + data[i].id + '" onClick="selectPack(\'' + data[i].id + '\');" /><span class="spanradio">' + data[i].pack_name + '</span></td> ';
                    _str += '</tr> ';                
                 
                }
                _str += '</table>';

            }
            $('#dataPackage').html(_str);
            getDataPackage();           
        }
    });
}

function selectPack(id) {
    $('#hdfPackID').val(id);
}

function SavePackage() {

    if ($('#hdfPackID').val() == '') {        
        $('.error').html('กรุณากรอกเลือกแพคเกจ').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    $.ajax({
        url: '../action/PackageAction.aspx',
        data: {
            action: 'savepackage',
            memid: checkExpire(),
            id: checkExpire().split('|')[1],
            pack_id: $('#hdfPackID').val(),
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: บันทึกข้อมูลไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);           
        },
        success: function (data) {

            if (data.Param == 'success') {
                $('.success').html('บันทึกข้อมูลเรียบร้อย').fadeIn(500).delay(2000).fadeOut(500);
                $('#hdfPackID').val('')
                getPackage();           
            }
          
        }
    });
}

function getDataPackage() {
    $('#divData').html($("#divLoadingTemplate").clone().children());
    $.ajax({
        url: '../action/PackageAction.aspx',
        data: {
            action: 'getdata',
            memid: checkExpire(),
            id: checkExpire().split('|')[1],
        },
        type: "POST",
        dataType: "json",
        error: function () { alert('error display data.'); $('#divData').html($("#divNoTranTemplate").clone().children()); },
        success: function (data) {
            var dataView = $("#divDataTemplate").clone();
            $(dataView).find("table").children().html("");
            
            if (data.length > 0) {
                
                $.each((data), function (index, e) {

                    var dataRow = $("#divDataTemplate tr").clone();
                    //$(dataRow).attr('saveid', e.paymentoptionID);
                    $(dataRow).find(".no").html(e.no).attr("title", e.no);
                    $(dataRow).find(".system_date").html(e.system_date).attr("title", e.system_date);
                    $(dataRow).find(".pack_name").html(e.pack_name).attr("title", e.pack_name);
                    $(dataRow).find(".pack_amount").html(e.pack_amount).attr("title", e.pack_amount);
                    $(dataRow).find(".total_amount").html(e.total_amount).attr("title", e.total_amount);
                    $(dataRow).find(".expire_date").html(e.expire_date).attr("title", e.expire_date);                    
                   
                                  
                    
                    if (e.pack_status == 'รอการชำระเงิน') {                       
                        $(dataRow).find(".pack_status").css('color', 'red');
                        $(dataRow).find(".file").html('<img class="file" width="30px" src="../image/icondelete.png" style="cursor:pointer;"  onClick="DeleteData(\'' + e.id + '\');" ></img>');
                    } else if (e.pack_status =='หมดอายุ') {
                        $(dataRow).find(".pack_status").css('color', 'red');
                        $(dataRow).find(".file").html('');
                    } else {
                        $(dataRow).find(".pack_status").css('color', '#05d71d');
                        $(dataRow).find(".file").html('');
                    }
          
                    
                    $(dataRow).find(".pack_status").html(e.pack_status).attr("title", e.pack_status);
                                                   
                                        
                   
                    //$(dataRow).find(".file2").html('<img class="file" width="30px" src="../image/iconfile.png" style="cursor:pointer;"  onClick="ShowImg(\'' + e.path_file + '\');" ></img>');
                    $(dataView).find("table").children().append(dataRow);
                });           

            } else { $(dataView).html($("#divNoTranTemplate").clone().children()); }            
            $('#divData').html($(dataView).children());
            CheckNoti();
        }
    });
}

function DeleteData(packid) {

    $.ajax({
        url: '../action/PackageAction.aspx',
        data: {
            action: 'delete',
            memid: checkExpire(),
            id: checkExpire().split('|')[1],
            packid: packid,
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: ลบข้อมูลไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
        },
        success: function (data) {

            if (data.Param == 'success') {
                $('.success').html('ลบข้อมูลเรียบร้อย').fadeIn(500).delay(2000).fadeOut(500);              
                getPackage();
            }

        }
    });
}