function getDataPackage() {
    $('#divData').html($("#divLoadingTemplate").clone().children());
    $.ajax({
        url: '../action/ManagePackageAction.aspx',
        data: {
            action: 'packagedata',
            memid: checkExpire(),
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
                    //$(dataRow).attr('id', e.id);
                    $(dataRow).find(".no").html(e.no).attr("title", e.no);
                    $(dataRow).find(".updated_date").html(e.updated_date).attr("title", e.updated_date);
                    $(dataRow).find(".package_name").html(e.package_name).attr("title", e.package_name);
                    $(dataRow).find(".pack_amount").html(e.pack_amount).attr("title", e.pack_amount);
                    $(dataRow).find(".click_amount").html(e.click_amount).attr("title", e.click_amount);
                    $(dataRow).find(".result_amount").html(e.result_amount).attr("title", e.result_amount);
                    $(dataRow).find(".expire_day").html(e.expire_day).attr("title", e.expire_day);
                    
                    var flag = 'off';
                    var color = '#E6E6E6;';
                    var margin = '0px;';

                    if (e.status == 'ใช้งาน') {
                        flag = 'on';
                        margin = '25px;';
                        color = '#04B404;';
                    }

                    var btnSlide = '';
                    btnSlide = '<div class="wrapper" style="background-color:' + color + '">'
                    btnSlide += '<div class="inner" style="margin-left: ' + margin + ' cursor:pointer;" app="' + flag + '" onclick="choose(this,\'' + e.id + '\')"></div>'
                    btnSlide += '</div>'

                    $(dataRow).find(".file").html('<img class="file" width="30px" src="../image/iconedit.png" style="cursor:pointer;"  onClick="EditData(\'' + e.id + '\');" ></img>');
                    $(dataRow).find(".status").html(btnSlide).attr("title", e.status);
                    $(dataView).find("table").children().append(dataRow);
                });

            } else { $(dataView).html($("#divNoTranTemplate").clone().children()); }
            $('#divData').html($(dataView).children());
            CheckNoti();
        }

    });
}

function choose(e, ref) {

    var val = $(e).attr('app');
    if (val == "off") {
        on(e, ref);
    }
    else {
        off(e, ref);
    }
}


function on(e, ref) {
    $(e).animate({ marginLeft: '25px' }, function () {
        $(e).attr('app', 'on');
        EditStatus(ref, 'ACTIVE');
        $('div .wrapper').css('background-color', '#04B404');

    });
}

function off(e, ref, type) {
    $(e).animate({ marginLeft: '0px' }, function () {
        $(e).attr('app', 'off');
        EditStatus(ref, 'INACTIVE');
        $('div .wrapper').css('background-color', '#E6E6E6');

    });
}

function EditStatus(id, status) {

    $.ajax({
        url: '../action/ManagePackageAction.aspx',
        data: {
            action: 'savestatus',
            memid: checkExpire(),
            id: id,
            status: status,
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: บันทึกไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
        },
        success: function (data) {

            if (data.Param == 'success') {
                $('.success').html('บันทึกข้อมูลสำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
                getDataPackage();
            }

        }
    });
}

function EditData(id) {
    
    $.ajax({
        url: '../action/ManagePackageAction.aspx',
        data: {
            action: 'editdata',
            memid: checkExpire(),
            id: id,
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: ไม่สามารถแก้ไขได้').fadeIn(500).delay(2000).fadeOut(500);
        },
        success: function (data) {

            if (data.length > 0) {
            
                $.each((data), function (index, e) {

                    $('#hdfID').val(e.id);               
                    $('#txtPackname').val(e.pack_name);
                    $('#txtPackamount').val(e.pack_amount);
                    $('#txtPackclick').val(e.click_amount);
                    $('#txtPackresult').val(e.result_amount);
                    $('#txtPackExpire').val(e.expire_day);
                 
                });
            
            }

        }
    });
}

function SaveData(_File) {

    if ($('#txtPackname').val() == '') {
        $('#txtPackname').focus();
        $('.error').html('กรุณากรอกชื่อแพคเกจ').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#txtPackamount').val() == '') {
        $('#txtPackamount').focus();
        $('.error').html('กรุณากรอกราคาแพคเกจ').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#txtPackclick').val() == '') {
        $('#txtPackclick').focus();
        $('.error').html('กรุณากรอกจำนวนคลิก').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#txtPackresult').val() == '') {
        $('#txtPackresult').focus();
        $('.error').html('กรุณากรอกจำนวนเงินที่ได้รับ').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#txtPackExpire').val() == '' || $('#txtPackExpire').val() == '0') {
        $('#txtPackExpire').focus();
        $('.error').html('กรุณากรอกจำนวนวันหมดอายุ').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    $.ajax({
        url: '../action/ManagePackageAction.aspx',
        data: {
            action: 'save',
            memid: checkExpire(),
            id: $('#hdfID').val(),
            name: $('#txtPackname').val(),
            amount: $('#txtPackamount').val(),
            click: $('#txtPackclick').val(),
            result: $('#txtPackresult').val(),
            expire: $('#txtPackExpire').val(),
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: บันทึกไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
        },
        success: function (data) {

            if (data.Param == 'success') {
                $('.success').html('บันทึกข้อมูลสำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
                $('#txtPackname').val('');
                $('#txtPackamount').val('');
                $('#txtPackclick').val('');
                $('#txtPackresult').val('');
                $('#txtPackExpire').val('');
                $('#hdfID').val('');                   
                getDataPackage();
            } else {
                $('.error').html('บันทึกข้อมูลไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);               
            }

        }
    });
}