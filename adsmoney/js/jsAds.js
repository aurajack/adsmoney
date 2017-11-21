function getDataAds() {
    $('#divData').html($("#divLoadingTemplate").clone().children());
    $.ajax({
        url: '../action/AdsAction.aspx',
        data: {
            action: 'adsdata',
            memid: checkExpire(),
            id: checkExpire().split('|')[1],
        },
        type: "POST",
        dataType: "json",
        error: function () { alert('error display data.'); $('#divData').html($("#divNoTranTemplate").clone().children()); },
        success: function (data) {

            var _str = ' <table style="width: 100%; border-collapse: collapse; border-spacing: 0; font-size:14px; height:35px;"> ';
           
            if (data.length > 0) {
                
                for (var i = 0; i < data.length; i++) {
                    if (i == 0) {
                        _str += '<tr>';
                    }

                    _str += '       <td style="text-align:center;">';
                    _str += '            <img class="ads" src="../ads/'+ data[i].ads_path +'" width="250" style="cursor:pointer" /><br />';
                    _str += '            <a href="' + data[i].ads_link + '" target="_blank,_top" onClick="SaveClick(\'' + data[i].pack_id + '\');">คลิกที่นี่ (จำนวนครั้ง ' + data[i].current_click + '/' + data[i].pack_click + ')</a>';
                    _str += '       </td>';

                    if ((i + 1) % 3 == 0) {
                        _str += '</tr>';
                        if ((i + 1) < data.length) {
                            _str += '<tr>'
                        }
                    } else {
                        if ((i + 1) == data.length) {
                            _str += '</tr>'
                        }
                    }
                }
                _str += '</table>';
                $('#divRemark').css('display', 'none');
            } else { $('#divData').html($("#divNoTranTemplate").clone().children()); $('#divRemark').css('display', 'block'); }
            $('#divData').html(_str);
            CheckNoti();
        }
    });
}

function SaveClick(Packid) {

    $.ajax({
        url: '../action/AdsAction.aspx',
        data: {
            action: 'clicklink',
            memid: checkExpire(),
            id: checkExpire().split('|')[1],
            pack_id: Packid,
        },
        type: "POST",
        dataType: "json",
        error: function () {
            $('.error').html('เกิดข้อผิดพลาด: บันทึกข้อมูลไม่สำเร็จ').fadeIn(500).delay(2000).fadeOut(500);
        },
        success: function (data) {

            if (data.Param == 'success') {
                             
                getDataAds();
                CheckNoti();
             
            }

        }
    });
   
}