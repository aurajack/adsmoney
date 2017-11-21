function testclick(){
    $('#name').html('11111111111');
    
}

function CreateMenu(_Cookie) {

    var _Selectmenu = 'home';
    var _Role = _Cookie.split('|')[0];

    if (_Cookie.split('|').length > 4) {
        _Selectmenu = _Cookie.split('|')[4];
    }
    
    var _menu = ' <div id="home" style="text-align: left;" onclick="selectMenu(\'home\');"><img src="../image/icon_menu/1.png" width="30" style="vertical-align:middle;" /><span>หน้าหลัก</span></div> ';
    _menu += '                     <div id="ads" style="text-align: left;" onclick="selectMenu(\'ads\');"><img src="../image/icon_menu/2.png" width="30" style="vertical-align:middle;" /><span style="position:relative;">คลิกโฆษณา<div id="notads" class="not" style="display:none;"><span id="spanads" class="spannotic">0</span></div></span></div> ';
    _menu += '                     <div id="topup" style="text-align: left;" onclick="selectMenu(\'topup\');"><img src="../image/icon_menu/3.png" width="30" style="vertical-align:middle;" /><span style="position:relative;">โอนเงินค่าสมาชิก<div id="nottopup" class="not" style="display:none;"><span id="spantopup" class="spannotic">0</span></div></span></div> ';
    _menu += '                     <div id="package" style="text-align: left;" onclick="selectMenu(\'package\');"><img src="../image/icon_menu/4.png" width="30" style="vertical-align:middle;" /><span style="position:relative;">ซื้อแพคเกจ<div id="notpackage" class="not" style="display:none;"><span id="spanpackage" class="spannotic">0</span></div></span></div> ';
    _menu += '                     <div id="transfer" style="text-align: left;" onclick="selectMenu(\'transfer\');"><img src="../image/icon_menu/5.png" width="30" style="vertical-align:middle;" /><span style="position:relative;">โอนเงิน-ถอนเงิน<div id="nottransfer" class="not" style="display:none;"><span id="spantransfer" class="spannotic">0</span></div></span></div> ';
    _menu += '                     <div id="profile" class="submenu" style="text-align: left;" onclick="selectMenu(\'profile\');"><img src="../image/icon_menu/6.png" width="30" style="vertical-align:middle;" /><span>ข้อมูลส่วนตัว</span></div> ';
    _menu += '                     <div style="text-align: left;" onclick="conflogout();"><span>ออกจากระบบ</span></div> ';
    // _menu += '                     <div style="text-align: left;" onclick="conflogout();"><span style="position:relative;">ออกจากระบบ</span></div> ';
  
    if (_Role == 'AD') {
        _menu = ' <div id="home" style="text-align: left;" onclick="selectMenu(\'home\');"><img src="../image/icon_menu/1.png" width="30" style="vertical-align:middle;" /><span>หน้าหลัก</span></div> ';
        _menu += '                     <div id="manageads" style="text-align: left;" onclick="selectMenu(\'manageads\');"><img src="../image/icon_menu/2.png" width="30" style="vertical-align:middle;" /><span style="position:relative;">จัดการโฆษณา</span></div> ';
        _menu += '                     <div id="managetopup" style="text-align: left;" onclick="selectMenu(\'managetopup\');"><img src="../image/icon_menu/3.png" width="30" style="vertical-align:middle;" /><span style="position:relative;">อนุมัติค่าสมัครสมาชิก<div id="nottopup" class="not" style="display:none;"><span id="spantopup" class="spannotic">0</span></div></span></div> ';
        _menu += '                     <div id="managepackage" style="text-align: left;" onclick="selectMenu(\'managepackage\');"><img src="../image/icon_menu/4.png" width="30" style="vertical-align:middle;" /><span style="position:relative;">จัดการแพคเกจ</span></div> ';
        _menu += '                     <div id="transfer" style="text-align: left;" onclick="selectMenu(\'transfer\');"><img src="../image/icon_menu/5.png" width="30" style="vertical-align:middle;" /><span style="position:relative;">รายการเบิกถอน<div id="nottransfer" class="not" style="display:none;"><span id="spantransfer" class="spannotic">0</span></div></span></div> ';
        _menu += '                     <div id="profile" class="submenu" style="text-align: left;" onclick="selectMenu(\'profile\');"><img src="../image/icon_menu/6.png" width="30" style="vertical-align:middle;" /><span style="position:relative;">ข้อมูลส่วนตัว</span></div> ';
        _menu += '                     <div style="text-align: left;" onclick="conflogout();"><span>ออกจากระบบ</span></div> ';
        $('div .menu').html(_menu);
    } else {
        $('div .menu').html(_menu);
    }

    setMenu(_Selectmenu);

}

function ReCookie(_menu) {
    var user = getCookie("ckwebload");
    var data = user.split('|');
  
    if (user != "") {
        setCookie("ckwebload", data[0] + '|' + data[1] + '|' + data[2] + '|' + data[3] + '|' + _menu, 1);
    } else {
        $('.error').html('Login expires.').fadeIn(500).delay(2000).fadeOut(500, function () {
           
            $('<form action="login.aspx" method="post"></form>').appendTo('html').submit();
            //$('form').Attr("action","").submit();
        });
    }
   
}

function setCookie(cname, cvalue, exHour) {
    var d = new Date();
    d.setTime(d.getTime() + (exHour * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function checkCookie() {
    var user = getCookie("ckwebload");
    if (user != "") {
        $('#name').html(user.split('|')[2]);
        if (user.split('|')[3] == 'Y' || user.split('|')[0] == 'AD') {
            $('#member_status').css('color', '#05d71d');
            $('#member_status').html('พร้อมใช้งาน');
        } else {
            $('#member_status').css('color', 'red');
            $('#member_status').html('หมดอายุ');
        }
    } else {
        $('.error').html('Login expires.').fadeIn(500).delay(2000).fadeOut(500, function () {
            $('<form action="login.aspx" method="post"></form>').appendTo('html').appendTo('html').submit();
        });

    }
}

function checkExpire() {
    var user = getCookie("ckwebload");
    if (user != "") {
        return user;
    } else {
        return "";
    }
}

function selectMenu(_menu) {

    //$("div .menu div").each(function (index) {
    //    $(this).css("background-color", "");
    //});
    if (_menu == 'home') {
        _menu = 'default'
    }
    $('<form action="' + _menu + '.aspx" method="post"><input name="menu" value="' + _menu + '" /></form>').appendTo('html').submit();
    //$('#' + _menu).css("background-color", "#2d2e2f");
}

function setMenu(menu) {
   
    $("div .menu div").each(function (index) {
        $(this).css("background-color", "");
    });

    $('#' + menu).css("background-color", "#2d2e2f");

}

function conflogout() {    
    $("#divConfirmTemplate").fadeIn(300);
}

function logout() {
    setCookie("ckwebload", "", -1);
    $('<form action="login.aspx" method="post"></form>').appendTo('html').submit();
}

function CheckNoti() {    
    $.ajax({
        url: '../action/NotiAction.aspx',
        data: {
            action: 'checknoti',
            memid: checkExpire(),
            id: checkExpire().split('|')[1],
            role : checkExpire().split('|')[0],
        },
        type: "POST",
        dataType: "json",
        error: function () { },
        success: function (data) {
            
            var arrNoti = data.Param.split('|');

            $.each((arrNoti), function (index, e) {

                if (checkExpire().split('|')[0] == 'AD') {
                    if (index == 1) {
                        if (e > 0) {
                            $('#nottopup').css('display', 'block');
                            $('#spantopup').html(e);
                        } else {
                            $('#nottopup').css('display', 'none');
                        }
                    }                  

                    if (index == 2) {
                        if (e > 0) {
                            $('#nottransfer').css('display', 'block');
                            $('#spantransfer').html(e);
                        } else {
                            $('#nottransfer').css('display', 'none');
                        }

                    }
                } else {
                    if (index == 0) {
                        if (e > 0) {
                            $('#nottopup').css('display', 'block');
                            $('#spantopup').html(e);
                        } else {
                            $('#nottopup').css('display', 'none');
                        }
                    }

                    if (index == 1) {
                        if (e > 0) {
                            $('#notpackage').css('display', 'block');
                            $('#spanpackage').html(e);
                        } else {
                            $('#notpackage').css('display', 'none');
                        }
                    }

                    if (index == 2) {
                        if (e > 0) {
                            $('#nottransfer').css('display', 'block');
                            $('#spantransfer').html(e);
                        } else {
                            $('#nottransfer').css('display', 'none');
                        }

                    }
                }
                
            });           
          
        }
    });
}