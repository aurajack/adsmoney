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

                    _option += '<option value=' + e.id + '>' + e.bank_name + '</option>'

                });

            }
            $('#ddlBank').html(_option);
            getDataProfile()
        }
    });
}

function getDataProfile() {
    $.ajax({
        url: '../action/ProfileAction.aspx',
        data: {
            action: 'profiledata',
            memid: checkExpire(),
            id: checkExpire().split('|')[1],
        },
        type: "POST",
        dataType: "json",
        error: function () { alert('error display data.'); },
        success: function (data) {

            if (data.length > 0) {

                $.each((data), function (index, e) {

                    $('#hdfID').val(e.id);
                    $('#txtUser').val(e.user);
                    $('#txtNation').val(e.nation);
                    $('#txtMobile').val(e.mobile);
                    $('#txtEmail').val(e.email);
                    $('#txtFname').val(e.fname);
                    $('#txtLname').val(e.lname);
                    $('#txtAcc').val(e.acc);
                    $('#txtLine').val(e.line);
                    var _imgCard = 'icon_no.png';
                    if (e.img_nation != '') {
                        _imgCard = e.img_nation;
                    }
                    $('#imgPreview').attr('src', '../card/' + _imgCard);
                    $('#ddlBank').val(e.bank_id);
                });

            }
            CheckNoti();
        }

    });
}


function ChangePassword() {


    if ($('#txtPass').val() == '') {
        $('#txtPass').focus();
        $('.error').html('กรุณากรอกรหัสผ่านเดิม').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#txtNewpass').val() == '') {
        $('#txtNewpass').focus();
        $('.error').html('กรุณากรอกรหัสผ่านใหม่').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#txtConnewpass').val() == '') {
        $('#txtConnewpass').focus();
        $('.error').html('กรุณายืนยันรหัสผ่านใหม่').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#txtNewpass').val() != $('#txtConnewpass').val() ) {
        $('#txtNewpass').val('');
        $('#txtConnewpass').val('');
        $('#txtNewpass').focus();
        $('#txtConnewpass').focus();
        $('.error').html('ยืนยันรหัสผ่านไม่ถูกต้อง').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    } 
    
    $.ajax({
        url: '../action/ProfileAction.aspx',
        data: {
            action: 'changepassword',
            memid: checkExpire(),
            id: $('#hdfID').val(),
            pass: $('#txtNewpass').val(),         
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: บันทึกไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
        },
        success: function (data) {

            if (data.Param == 'success') {
                $('.success').html('แก้ไขรหัสผ่านเรียบร้อย').fadeIn(500).delay(2000).fadeOut(500, function () {
                    getDataBank();
                });               
            } else {
                $('.error').html('แก้ไขรหัสผ่านไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);                
            }
            $('#txtNewpass').val('');
            $('#txtConnewpass').val('');
            $('#txtPass').val('');
            $('#txtPass').focus();
        }
    });
}

function SaveData() {

    if ($('#txtFname').val() == '') {
        $('#txtFname').focus();
        $('.error').html('กรุณากรอกชื่อ').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#txtLname').val() == '') {
        $('#txtLname').focus();
        $('.error').html('กรุณากรอกนามสกุล').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#txtAcc').val() == '') {
        $('#txtAcc').focus();
        $('.error').html('กรุณากรอกข้อมูลบัญชีธนาคาร').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }


    $.ajax({
        url: '../action/ProfileAction.aspx',
        data: {
            action: 'save',
            memid: checkExpire(),
            id: $('#hdfID').val(),
            fname: $('#txtFname').val(),
            lname: $('#txtLname').val(),
            acc: $('#txtAcc').val(),
            line: $('#txtLine').val(),
            bank_id: $('#ddlBank').val(),
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: บันทึกไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
        },
        success: function (data) {

            if (data.Param == 'success') {                    
                $('.success').html('บันทึกข้อมูลเรียบร้อย').fadeIn(500).delay(2000).fadeOut(500, function () {
                    getDataBank();
                });
            } else {
                $('.error').html('บันทึกข้อมูลไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);               
            }

        }
    });
}

function SaveImgCard(_File) {

   $.ajax({
       url: '../action/ProfileAction.aspx',
        data: {
            action: 'saveimg',
            memid: checkExpire(),
            id: $('#hdfID').val(),
            file : _File,
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: อัพโหลดรูปไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
        },
        success: function (data) {

            if (data.Param == 'success') {
                $('#imgPreview').attr('src', '../card/' + _File);
            } else {
                $('.error').html('อัพโหลดรูปไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
            }

        }
    });
}