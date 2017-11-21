function SaveTopup(fileName) {

    //if ($('#txtTopup').val() == '') {
    //    $('#txtTopup').focus();
    //    $('.error').html('กรุณากรอกเลขบัตรเติมเงิน').fadeIn(500).delay(2000).fadeOut(500);
    //    return false;
    //}

    $.ajax({
        url: '../action/TopupAction.aspx',
        data: {
            action: 'topup',
            memid: checkExpire(),
            topup_file: fileName,
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: อัพโหลดไฟล์ไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);           
        },
        success: function (data) {

            if (data.Param == 'success') {
                $('.success').html('อัพโหลดไฟล์เรียบร้อย').fadeIn(500).delay(2000).fadeOut(500);
                //$('.success').html('เติมเงินเรียบร้อย').fadeIn(500).delay(2000).fadeOut(500);
                //$('#txtTopup').val('');
                //$('#txtTopup').focus();
                getDataTopup();
            //} else {
            //    $('.error').html('รหัสบัตรมีการใช้งานแล้ว').fadeIn(500).delay(2000).fadeOut(500);
            //    $('#txtTopup').val('');
            //    $('#txtTopup').focus();
            }
          
        }
    });
}

function getDataTopup() {
    $('#divData').html($("#divLoadingTemplate").clone().children());
    $.ajax({
        url: '../action/TopupAction.aspx',
        data: {
            action: 'getdata',
            memid: checkExpire().split('|')[1],
            role: checkExpire().split('|')[0],
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
                    $(dataRow).find(".topup_date").html(e.created_date).attr("title", e.created_date);
                    $(dataRow).find(".name").html(e.name).attr("title", e.name);
                    $(dataRow).find(".mobile").html(e.mobile).attr("title", e.mobile);
                    //$(dataRow).find(".topup_no").html(e.topup_no).attr("title", e.topup_no);
                   
                    $(dataRow).find(".expire").html(e.expire_date).attr("title", e.expire_date);
                    var flag = 'off';
                    var color = '#E6E6E6;';
                    var margin = '0px;';
                    
                    if (e.status == 'อนุมัติ') {                      
                        flag = 'on';
                        margin = '25px;';
                        color = '#04B404;';
                    }
                   
                    if (checkExpire().split('|')[0] == 'AD') {
                        var btnSlide = '';
                        btnSlide = '<div class="wrapper" style="background-color:' + color + '">'
                        btnSlide += '<div class="inner" style="margin-left: ' + margin + ' cursor:pointer;" app="' + flag + '" onclick="choose(this,\'' + e.id + '\',\'\')"></div>'
                        btnSlide += '</div>'
                        var flag2 = 'off';
                        var color2 = '#E6E6E6;';
                        var margin2 = '0px;';

                        if (e.main_status == 'ใช้งาน') {
                            flag2 = 'on';
                            margin2 = '25px;';
                            color2 = '#04B404;';
                        }
                        var btnSlide2 = '';
                        btnSlide2 = '<div class="wrapper2" style="background-color:' + color2 + '">'
                        btnSlide2 += '<div class="inner" style="margin-left: ' + margin2 + ' cursor:pointer;" app="' + flag2 + '" onclick="choose(this,\'' + e.member_id + '\',\'M\')"></div>'
                        btnSlide2 += '</div>'
                        $(dataRow).find(".status").html(btnSlide).attr("title", e.status);
                        $(dataRow).find(".mainstatus").html(btnSlide2).attr("title", e.main_status);
                    } else {
                        $(dataRow).find(".status").html(e.status).attr("title", e.status);
                    }                   
                    
                    $(dataRow).find(".file").html('<img class="file" width="30px" src="../image/iconfile.png" style="cursor:pointer;"  onClick="ShowImg(\'' + e.path_file + '\',\'\');" ></img>&nbsp;<img class="file" width="30px" src="../image/iconcard.png" style="cursor:pointer;"  onClick="ShowImg(\'' + e.img_nation + '\',\'C\');" ></img>');
                    //$(dataRow).find(".file2").html('<img class="file" width="30px" src="../image/iconfile.png" style="cursor:pointer;"  onClick="ShowImg(\'' + e.path_file + '\');" ></img>');
                    $(dataView).find("table").children().append(dataRow);
                });           

            } else { $(dataView).html($("#divNoTranTemplate").clone().children()); }            
            $('#divData').html($(dataView).children());
            CheckNoti();
        }
    });
}

function ShowImg(file, type) {
    if (type == 'C') {
        $('#showImg').attr('src', '../card/' + file);
    } else {
        $('#showImg').attr('src', '../member/' + file);
    }    
    //$('#divImg').show();
    $("#divShowImgTemplate").fadeIn(300);
}

function HideImg() {
    $("#divShowImgTemplate").fadeOut(300);
    // $('#divImg').hide();
}

function choose(e,ref,type) {
    
    var val = $(e).attr('app');
    if (val == "off") {
        on(e, ref, type);
    }
    else {
        off(e, ref, type);
    }
}


function on(e, ref, type) {
    $(e).animate({ marginLeft: '25px' }, function () {
        $(e).attr('app', 'on');        
        $('#hdfID').val(ref);
        if (type == 'M') {           
            ApproveMain(ref, 'Y')
            $('div .wrapper2').css('background-color', '#04B404');
        } else {
            confirmbox();            
        }
        
    });
}

function off(e, ref, type) {
    $(e).animate({ marginLeft: '0px' }, function () {
        $(e).attr('app', 'off');
       
        //document.getElementById("wrapper").style.background = "#E6E6E6";
        if (type == 'M') { 
            ApproveMain(ref,'N')
            $('div .wrapper2').css('background-color', '#E6E6E6');
        } else {
            ApproveTopup(ref, 'CANCEL');
            $('div .wrapper').css('background-color', '#E6E6E6');
        }
        
    });
}

function ApproveTopup(id,status) {

    $.ajax({
        url: '../action/TopupAction.aspx',
        data: {
            action: 'approve',
            memid: checkExpire(),
            id: id,
            status: status,
            expire: $('#txtExpireDay').val(),
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: อนุมิติไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
        },
        success: function (data) {

            if (data.Param == 'success') {
                $('.success').html('อนุมัติเรียบร้อย').fadeIn(500).delay(2000).fadeOut(500);
                if (status == 'APPROVE'){
                    $('#hdfID').val('');
                    $('#txtExpireDay').val('');
                    $('#divConfirmTemplate2').fadeOut(400);
                }
                getDataTopup();             
            }

        }
    });
}

function ApproveMain(id, status) {

    $.ajax({
        url: '../action/TopupAction.aspx',
        data: {
            action: 'approvemain',
            memid: checkExpire(),
            id: id,
            status: status,
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: อนุมิติไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
        },
        success: function (data) {

            if (data.Param == 'success') {
                $('.success').html('อนุมัติเรียบร้อย').fadeIn(500).delay(2000).fadeOut(500);
                getDataTopup();
            }

        }
    });
}

function confirmbox() {   
    //$('#hdfAppID').val(id);
    //$('#hdfAppMempackid').val(mempackid);
    $("#divConfirmTemplate2").fadeIn(300);
    $("#txtExpireDay").focus();

}