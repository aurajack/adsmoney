function signup() {
    // $('<form action="signup.aspx" method="post"></form>').submit();
    $("#divRegisTemplate").fadeIn(300);
}

function forgot() {
    // $('<form action="signup.aspx" method="post"></form>').submit();
    $("#divForgotTemplate").fadeIn(300);
}

function getDataBank() {
    $.ajax({
        url: '../action/LoginAction.aspx',
        data: {
            action: 'getdatabank',
        },
        type: "POST",
        dataType: "json",
        error: function () { alert('error display data.'); },
        success: function (data) {
           
            if (data.length > 0) {
                var _option = ''
                $.each((data), function (index, e) {

                    _option += '<option value=' + e.id + '>'+ e.bank_name +'</option>'
                   
                });           

            }        
            $('#ddlBank').html(_option);

        }
    });
}

function sendforgot() {

    if ($('#txtForgotUser').val() == '') {
        $('#txtForgotUser').focus();
        $('.error').html('กรุณากรอก username').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#txtForgotNation').val() == '') {
        $('#txtForgotNation').focus();
        $('.error').html('กรุณากรอกเลขบัตรประชาชน').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#txtForgotEmail').val() == '') {
        $('#txtForgotEmail').focus();
        $('.error').html('กรุณากรอกอีเมล์').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    $.ajax({
        url: '../action/LoginAction.aspx',
        data: {
            action: 'forgot',
            user: $('#txtForgotUser').val(),
            nation: $('#txtForgotNation').val(),
            email: $('#txtForgotEmail').val(),
        },
        type: "POST",
        dataType: "json",
        error: function () { "เกิดข้อผิดพลาด: รีเซ็ตรหัสผ่านไม่สำเร็จ." },
        success: function (data) {

            if (data.Param == 'success') {
                $('.success').html('รีเซ็ตพาสเวิร์ดเรียบร้อย').fadeIn(500).delay(2000).fadeOut(500, function () {
                    $('#divForgotTemplate').fadeOut(400, function () {

                        $('#username').val($('#txtForgotUser').val());
                        $('#password').focus();
                    });
                });
            } else if (data.Param == 'error') {
                $('.error').html('ข้อมูลไม่ถูกต้อง ไม่สามารถรีเซ็ตพาสเวิร์ดได้').fadeIn(500).delay(2000).fadeOut(500);
                $('#txtForgotUser').val('');
                $('#txtForgotNation').val('');
                $('#txtForgotEmail').val('');
                $('#txtForgotUser').focus();
            } else if (data.Param == 'sendfail') {
                $('.error').html('เกิดข้อผิดพลาดในการส่งอีเมล์').fadeIn(500).delay(2000).fadeOut(500);
                $('#txtForgotUser').val('');
                $('#txtForgotNation').val('');
                $('#txtForgotEmail').val('');
                $('#txtForgotUser').focus();
            } else {
                $('.error').html(data.Param).fadeIn(500).delay(2000).fadeOut(500);

            }

        }
    });

}

function login(_user, _pass) {
    $.ajax({
        url: '../action/LoginAction.aspx',
        data: {
            action: 'login',
            user: _user,
            pass: _pass,
        },
        type: "POST",
        dataType: "json",
        error: function () { "เกิดข้อผิดพลาด: การเข้าสู่ระบบไม่สำเร็จ." },
        success: function (data) {
            
            if (data.length > 0) {
               
                //if (data[0].main_status == 'N') {                   
                //    setCookie("ckwebload", "", -1);
                //    $('#remark').html('USER ถูกระงับใช้งาน กรุณาติดต่อเจ้าหน้าที่');
                //    $('#username').val('');
                //    $('#password').val('');
                //    $('#username').focus();
                //} else {
                    setCookie("ckwebload", data[0].code + '|' + data[0].id + '|' + data[0].username + '|' + data[0].active, 1);
                    $('<form action="default.aspx" method="post"></form>').appendTo('html').submit();
                //}               

            } else {
                $('#remark').html('ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง');
                $('#username').val('');
                $('#password').val('');
                $('#username').focus();
            }

        }
    });
}

function CheckRegister() {

    if ($('#regis_user').val() == '') {
        $('#regis_user').focus();
        $('.error').html('กรุณากรอกชื่อผู้ใช้.').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#regis_pass').val() == '') {
        $('#regis_pass').focus();
        $('.error').html('กรุณากรอกรหัสผ่าน.').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#regis_conf').val() == '') {
        $('#regis_conf').focus();
        $('.error').html('กรุณายืนยันรหัสผ่าน').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#regis_fname').val() == '') {
        $('#regis_fname').focus();
        $('.error').html('กรุณากรอกชื่อ').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#regis_lname').val() == '') {
        $('#regis_lname').focus();
        $('.error').html('กรุุณากรอกนามสกุล').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#regis_nation').val() == '') {
        $('#regis_nation').focus();
        $('.error').html('กรุณากรอกรหัสบัตรประชาชน').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    } else {
        if (checkID($('#regis_nation').val()) == false) {
            $('#regis_nation').focus();
            $('.error').html('รหัสบัตรประชาชนไม่ถูกต้อง').fadeIn(500).delay(2000).fadeOut(500);
            return false;
        }
    }

    if ($('#regis_tel').val() == '') {
        $('#regis_tel').focus();
        $('.error').html('กรุณาเบอร์โทรศัพท์').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#regis_email').val() == '') {
        $('#regis_email').focus();
        $('.error').html('กรุณากรอกอีเมล์').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#regis_pass').val() != $('#regis_conf').val()) {
        $('#regis_pass').val('');
        $('#regis_conf').val('');
        $('#regis_pass').focus();
        $('.error').html('รหัสผ่านไม่ตรงกัน').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#regis_acc').val() == '') {
        $('#regis_acc').focus();
        $('.error').html('กรุณากรอกข้อมูลบัญชีธนาคาร').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#regis_line').val() == '') {
        $('#regis_line').focus();
        $('.error').html('กรุณากรอกข้อมูลไอดีไลน์').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#hdfRegisPath').val() == '') {        
        $('.error').html('กรุณากรอกอัพโหลดรูปสำเนาบัตรประชาชน').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    SaveRegister();

}

function SaveRegister() {
    $.ajax({
        url: '../action/LoginAction.aspx',
        data: {
            action: 'register',
            user: $('#regis_user').val(),
            pass: $('#regis_pass').val(),
            fname: $('#regis_fname').val(),
            lname: $('#regis_lname').val(),
            nation_id: $('#regis_nation').val(),
            tel: $('#regis_tel').val(),
            email: $('#regis_email').val(),
            acc: $('#regis_acc').val(),
            line: $('#regis_line').val(),
            file: $('#hdfRegisPath').val(),
            bank: $('#ddlBank').val(),
        },
        type: "POST",
        dataType: "json",
        error: function () { "เกิดข้อผิดพลาด: ไม่สามารถสมัครสมาชิกได้" },
        success: function (data) {

            if (data.Param == 'success') {
                $('.success').html('สมัครสมาชิกเรียบร้อย').fadeIn(500).delay(2000).fadeOut(500, function () {
                    $('#divRegisTemplate').fadeOut(400, function () {

                        $('#username').val($('#regis_user').val());
                        $('#password').focus();
                    });
                });
            } else if (data.Param == 'duplicate') {
                $('.error').html('ชื่อผู้ใช้มีอยู่ในระบบแล้ว').fadeIn(500).delay(2000).fadeOut(500);
                $('#regis_user').val('');
                $('#regis_pass').val('');
                $('#regis_conf').val('');
                $('#regis_user').focus();
            } else {
                $('.error').html(data.Param).fadeIn(500).delay(2000).fadeOut(500);
              
            }

        }
    });
}

function checkID(id) {
    if (id.length != 13) return false;
    for (i = 0, sum = 0; i < 12; i++)
        sum += parseFloat(id.charAt(i)) * (13 - i); if ((11 - sum % 11) % 10 != parseFloat(id.charAt(12)))
            return false; return true;
}




