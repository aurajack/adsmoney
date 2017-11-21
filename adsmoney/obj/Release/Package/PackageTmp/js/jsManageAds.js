function getDataAds() {
    $('#divData').html($("#divLoadingTemplate").clone().children());
    $.ajax({
        url: '../action/ManageAdsAction.aspx',
        data: {
            action: 'adsdata',
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
                    $(dataRow).find(".system_date").html(e.updated_date).attr("title", e.updated_date);
                    $(dataRow).find(".ads_name").html(e.ads_name).attr("title", e.ads_name);
                    $(dataRow).find(".ads_link").html(e.ads_link).attr("title", e.ads_link);
                                     
                    var flag = 'off';
                    var color = '#E6E6E6;';
                    var margin = '0px;';

                    if (e.ads_status == 'ใช้งาน') {
                        flag = 'on';
                        margin = '25px;';
                        color = '#04B404;';
                    }

                    var btnSlide = '';
                    btnSlide = '<div class="wrapper" style="background-color:' + color + '">'
                    btnSlide += '<div class="inner" style="margin-left: ' + margin + ' cursor:pointer;" app="' + flag + '" onclick="choose(this,\'' + e.id + '\')"></div>'
                    btnSlide += '</div>'

                    $(dataRow).find(".file").html('<img class="file" width="30px" src="../image/iconfile.png" style="cursor:pointer;"  onClick="ShowImg(\'' + e.ads_path + '\');" ></img>&nbsp;<img class="file" width="30px" src="../image/iconedit.png" style="cursor:pointer;"  onClick="EditData(\'' + e.id + '\');" ></img>');
                    $(dataRow).find(".status").html(btnSlide).attr("title", e.ads_status);
                    $(dataView).find("table").children().append(dataRow);
                });

            } else { $(dataView).html($("#divNoTranTemplate").clone().children()); }
            $('#divData').html($(dataView).children());
            CheckNoti();
        }

    });
}

function ShowImg(file) {
  
    $('#showImg').attr('src', 'ads/' + file);
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

function EditStatus(id, status) {

    $.ajax({
        url: '../action/ManageAdsAction.aspx',
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
                getDataAds();
                CheckNoti();
            }

        }
    });
}

function EditData(id) {

    $.ajax({
        url: '../action/ManageAdsAction.aspx',
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
                    $('#hdfAdsPath').val(e.ads_path);
                    $('#txtAdsname').val(e.ads_name);
                    $('#txtAdslink').val(e.ads_link);
                    $('#imgPreview').attr('src', '../ads/' + e.ads_path);
                    $('#imgPreview').css('display', 'block');

                });
            
            }

        }
    });
}

function SaveData(_File) {

    if ($('#txtAdsname').val() == '') {
        $('#txtAdsname').focus();
        $('.error').html('กรุณากรอกชื่อโฆษณา').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    if ($('#txtAdslink').val() == '') {
        $('#txtAdslink').focus();
        $('.error').html('กรุณากรอกลิ้งโฆษณา').fadeIn(500).delay(2000).fadeOut(500);
        return false;
    }

    $.ajax({
        url: '../action/ManageAdsAction.aspx',
        data: {
            action: 'save',
            memid: checkExpire(),
            id: $('#hdfID').val(),
            name: $('#txtAdsname').val(),
            link: $('#txtAdslink').val(),
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
                $('#txtAdsname').val('');
                $('#txtAdslink').val('');
                $('#hdfID').val('');
                $('#hdfAdsPath').val('');
                $('#imgPreview').attr('src', '../ads/icon_no.png');
                $('#imgPreview').css('display', 'none');                
                getDataAds();
                CheckNoti();
            } else {
                $('.error').html('บันทึกข้อมูลไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);               
            }

        }
    });
}