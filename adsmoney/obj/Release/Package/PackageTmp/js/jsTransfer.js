function getDataTransfer() {
    $('#divData').html($("#divLoadingTemplate").clone().children());
    $.ajax({
        url: '../action/TransferAction.aspx',
        data: {
            action: 'transferdata',
            memid: checkExpire(),
            id: checkExpire().split('|')[1],
            role: checkExpire().split('|')[0],
        },
        type: "POST",
        dataType: "json",
        error: function () { alert('error display data.'); $('#divData').html($("#divNoTranTemplate").clone().children()); },
        success: function (data) {
            var dataView = $("#divDataTemplate").clone();
            $(dataView).find("table").children().html("");
            console.log('data=', data);
            if (data.length > 0) {

                $.each((data), function (index, e) {

                    var dataRow = $("#divDataTemplate tr").clone();
                    //$(dataRow).attr('id', e.id);
                    $(dataRow).find(".no").html(e.no).attr("title", e.no);
                    $(dataRow).find(".system_date").html(e.system_date).attr("title", e.system_date);
                    $(dataRow).find(".job").html(e.job).attr("title", e.job);
                    if (e.job == 'โอนให้') {                       
                        $(dataRow).find(".job").css('color', 'red');
                        $(dataRow).find(".name").html(e.ex_name).attr("title", e.ex_name);
                        $(dataRow).find(".mobile").html(e.ex_mobile).attr("title", e.ex_mobile);
                        if (e.ex_id == '') {
                            $(dataRow).find(".file").html('<img class="file" width="30px" src="../image/iconfile.png" style="cursor:pointer;"  onClick="ShowImg(\'' + e.img_slip + '\');" ></img>');
                            $(dataRow).find(".file2").html('รอจับคู่');
                        } else {
                            $(dataRow).find(".file").html('<img class="file" width="30px" src="../image/iconfile.png" style="cursor:pointer;"  onClick="ShowImg(\'' + e.img_slip + '\');" ></img>');
                            if (e.show == 'Y') {
                                $(dataRow).find(".file2").html('<img class="file" width="30px" src="../image/iconedit.png" style="cursor:pointer;"  onClick="EditData(\'' + e.id + '\',\'' + e.ex_id + '\');" ></img>');
                            }                            
                        }
                        
                    } else {                      
                        $(dataRow).find(".job").css('color', '#05d71d');
                        $(dataRow).find(".name").html(e.dep_name).attr("title", e.dep_name);
                        $(dataRow).find(".mobile").html(e.dep_mobile).attr("title", e.dep_mobile);                 
                       
                        if (checkExpire().split('|')[0] == 'AD') {
                            if (e.ex_id == '') {
                                if (e.show == 'Y') {
                                    $(dataRow).find(".file3").html('<img class="file" width="30px" src="../image/add.png" style="cursor:pointer;"  onClick="AddExtract(\'' + e.id + '\',\'' + e.mem_pack_id + '\');" ></img>');
                                }
                                $(dataRow).find(".file").html('<img class="file" width="30px" src="../image/iconfile.png" style="cursor:pointer;"  onClick="ShowImg(\'' + e.img_slip + '\');" ></img>');
                                
                            } else {
                                $(dataRow).find(".file").html('<img class="file" width="30px" src="../image/iconfile.png" style="cursor:pointer;"  onClick="ShowImg(\'' + e.img_slip + '\');" ></img>');
                                if (e.img_slip != '' && e.show == 'Y') {
                                    $(dataRow).css('background-color', '#ffdfe4');
                                    
                                    $(dataRow).find(".file3").html('<img class="file" width="30px" src="../image/approve.png" style="cursor:pointer;"  onClick="confirmbox(\'' + e.id + '\',\'' + e.mem_pack_id + '\');" ></img>');
                                    //$(dataRow).find(".file3").html('<img class="file" width="30px" src="../image/approve.png" style="cursor:pointer;"  onClick="ApproveData(\'' + e.id + '\',\'' + e.mem_pack_id + '\');" ></img>');
                                }
                                
                            }
                            
                        } else {
                            $(dataRow).find(".file").html('<img class="file" width="30px" src="../image/iconfile.png" style="cursor:pointer;"  onClick="ShowImg(\'' + e.img_slip + '\');" ></img>');
                            if (e.img_slip != '' && e.show == 'Y') {
                                $(dataRow).css('background-color', '#ffdfe4');
                                
                                $(dataRow).find(".file3").html('<img class="file" width="30px" src="../image/approve.png" style="cursor:pointer;"  onClick="confirmbox(\'' + e.id + '\',\'' + e.mem_pack_id + '\');" ></img>');
                                //$(dataRow).find(".file3").html('<img class="file" width="30px" src="../image/approve.png" style="cursor:pointer;"  onClick="ApproveData(\'' + e.id + '\',\'' + e.mem_pack_id + '\');" ></img>');
                            }                           
                        }
                        
                    }
                    $(dataRow).find(".amount").html(e.amount).attr("title", e.amount);
                    $(dataRow).find(".status").html(e.status).attr("title", e.status);                    
                
                    $(dataView).find("table").children().append(dataRow);
                });

            } else { $(dataView).html($("#divNoTranTemplate").clone().children()); }
            $('#divData').html($(dataView).children());
            CheckNoti();
            
        }

    });
}

function ShowImg(file) {
  
    $('#showImg').attr('src', 'transfer/' + file);
    //$('#divImg').show();
    $("#divShowImgTemplate").fadeIn(300);
}

function HideImg() {
    $("#divShowImgTemplate").fadeOut(300);
    // $('#divImg').hide();
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

function ApproveData(id,mempackid) {

    $.ajax({
        url: '../action/TransferAction.aspx',
        data: {
            action: 'saveapprove',
            memid: checkExpire(),
            id: id,
            mempackid: mempackid,
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: บันทึกไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
        },
        success: function (data) {
           
            if (data.Param == 'success') {               
                $('.success').html('บันทึกข้อมูลสำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
                getDataTransfer();
            }

        }
    });
}

function AddExtract(id, mempackid) {

    $.ajax({
        url: '../action/TransferAction.aspx',
        data: {
            action: 'saveextract',
            memid: checkExpire(),
            id: id,
            mempackid: mempackid,
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: บันทึกไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
        },
        success: function (data) {

            if (data.Param == 'success') {
                $('.success').html('บันทึกข้อมูลสำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
                getDataTransfer();
            }

        }
    });
}

function EditData(id,ex_id) {

    $.ajax({
        url: '../action/TransferAction.aspx',
        data: {
            action: 'editdata',
            memid: checkExpire(),
            id: ex_id,
            idre: id,
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
                    $('#hdfIDRe').val(e.idre);
                    $('#txtName').val(e.name);
                    $('#txtAcc').val(e.acc);
                    $('#txtMobile').val(e.mobile);
                    $('#hdfTransferPath').val(e.img_slip);
                    if (e.img_slip == '') {
                        $('#imgPreview').attr('src', '../transfer/icon_no.png');
                    } else {
                        $('#imgPreview').attr('src', '../transfer/' + e.img_slip);
                    }
                   
                    $('#imgPreview').css('display', 'block');

                });
            
            }

        }
    });
}

function SaveData(_File) {

    if (_File == '') {      
        $('.error').html('กรุณาอัพโหลดไฟล์').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    $.ajax({
        url: '../action/TransferAction.aspx',
        data: {
            action: 'save',
            memid: checkExpire(),
            id: $('#hdfID').val(),
            idre: $('#hdfIDRe').val(),           
            file: _File,
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: บันทึกไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
        },
        success: function (data) {

            if (data.Param == 'success') {
                $('.success').html('บันทึกข้อมูลสำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
                $('#hdfID').val('');
                $('#hdfIDRe').val('');
                $('#txtName').val('');
                $('#txtAcc').val('');
                $('#txtMobile').val('');
                $('#imgPreview').attr('src', '../transfer/icon_no.png');
                $('#imgPreview').css('display', 'none');
                getDataTransfer();
            } else {
                $('.error').html('บันทึกข้อมูลไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);               
            }

        }
    });
}

function confirmbox(id, mempackid) {
    $('#hdfAppID').val(id);
    $('#hdfAppMempackid').val(mempackid);
    $("#divConfirmTemplate2").fadeIn(300);
}