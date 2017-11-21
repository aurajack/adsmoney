<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="login.aspx.vb" Inherits="adsmoney.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>threesixfive.com</title>
    <script src="js/jquery-1.7.1.js"></script>
    <script src="js/jquery.js"></script>
    <script src="js/jsMaster.js?v=2"></script>
    <script src="js/jsLogin.js?v=2"></script>  
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/styleLogin.css?v=1" rel="stylesheet" />
    <%--<link href="http://hayageek.github.io/jQuery-Upload-File/4.0.10/uploadfile.css" rel="stylesheet" />--%>
    <link href="css/styleUpload.css" rel="stylesheet" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <%--<script src="http://hayageek.github.io/jQuery-Upload-File/4.0.10/jquery.uploadfile.min.js"></script>--%>
    <script src="js/jsUloadmin.js"></script>
</head>
<body style="background-color: #303030;">
    <form id="form1" runat="server">
        <div class='success' style='display: none'>บันทึกข้อมูลเรียบร้อย</div>
        <div class='error' style='display: none'></div>
        <div class="login">
            <div style="text-align: center; font-size: 22px; font-weight: bold;">เข้าสู่ระบบ</div>
            <br />
            <div id="remark" style="font-size: 12px; color: red; text-align: center;"></div>
            <input id="username" type="text" placeholder="USERNAME" style="text-align: center;" />
            <input id="password" type="password" placeholder="PASSWORD" style="text-align: center;" />

            <div style="text-align: center; font-size: 12px;">สมัครสมาชิก <span style="cursor: pointer; color: #9ecad3; text-decoration: underline;" onclick="signup();">คลิกที่นี่!</span></div>
            <div style="text-align: center; font-size: 12px;">ลืมรหัสผ่าน <span style="cursor: pointer; color: #9ecad3; text-decoration: underline;" onclick="forgot();">คลิกที่นี่!</span></div>
            <br />
            <input id="btnLogin" class="btnsubmit" type="button" value="ยืนยัน" />
            <div style="text-align: center; font-size: 12px;">
                <span id="countMember" >จำนวนสมาชิกทั้งหมด 1,000 คน</span>
            </div>            
        </div>

        <div id="divForgotTemplate" style="display: none;">
            <div class='divOver'>
            </div>
            <div id="divForgot">
                <center>
                <table id="tableForgot" align="center">
                    <tr>
                        <td style="width:400px; float:left;">
                            <table style="width: 400px; border-collapse: collapse; border-spacing: 0;">
                                <tr>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <div>
                                            <span style="font-size: 14px;">USERNAME</span>
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <input id="txtForgotUser" type="text" placeholder="USERNAME" style="text-align: center;" />
                                        </div>
                                    </td>
                                    <td style="width: 10px;"></td>
                                </tr>
                                <tr>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <div>
                                            <span style="font-size: 14px;">เลขบัตรประชาชน</span>
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <input id="txtForgotNation" type="text" placeholder="เลขบัตรประชาชน" style="text-align: center;" />
                                        </div>
                                    </td>
                                    <td style="width: 10px;"></td>
                                </tr>
                                <tr>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <div>
                                            <span style="font-size: 14px;">อีเมลล์</span>
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <input id="txtForgotEmail" type="text" placeholder="EMAIL" style="text-align: center;" />
                                        </div>
                                    </td>
                                    <td style="width: 10px;"></td>
                                </tr>
                                <tr>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <div>
                                            <span style="font-size: 14px;"></span>
                                        </div>
                                    </td>
                                    <td>
                                        <div style="width: 110px; float: left;">
                                            <input id="btnForSubmit" class="btnsubmit" type="button" value="ยืนยัน" style="text-align: center;" />
                                        </div>
                                        <div style="width: 110px; float: right;">
                                            <input id="btnForCancle" class="btncancel" type="button" value="ยกเลิก" style="text-align: center;" />
                                        </div>
                                    </td>
                                    <td style="width: 10px;"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </center>
            </div>
        </div>
     
        <div id="divRegisTemplate" style="display: none;">
            <div class='divOver'>
            </div>
            <div id="divRegis">
                <center>
                        <table id="tableRegis" align="center">
                            <tr>
                                <td style="width:800px; float:left;">
                                    <div style="text-align:left; font-size:22px; font-weight:bold; padding:10px; border-bottom: solid 1px #dddddd;">สมัครสมาชิก</div>
                                    <%--<table style="width: 600px; border-collapse: collapse; border-spacing: 0;">
                                        <tr>
                                            <td style="width: 400px; ">
                                                 <table style="width: 400px; border-collapse: collapse; border-spacing: 0;">
                                                    <tr>
                                                        <td style="width:10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size:14px;">ชื่อผู้ใช้</span>
                                                            </div> 
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_user" type="text" placeholder="USERNAME" style="text-align:center;" />
                                                            </div> 
                                                        </td>
                                                        <td style="width:10px;"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size:14px;">รหัสผ่าน</span>
                                                            </div> 
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_pass" type="password" placeholder="PASWORD" style="text-align:center;" />
                                                            </div> 
                                                        </td>
                                                        <td style="width:10px;"></td>
                                                    </tr>
                                                     <tr>
                                                        <td style="width:10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size:14px;">ยืนยันรหัสผ่าน</span>
                                                            </div> 
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_conf" type="password" placeholder="CONFIRM PASWORD" style="text-align:center;" />
                                                            </div> 
                                                        </td>
                                                        <td style="width:10px;"></td>
                                                    </tr>
                                                     <tr>
                                                        <td style="width:10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size:14px;">ชื่อ</span>
                                                            </div> 
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_fname" type="text" placeholder="FIRST NAME" style="text-align:center;" />
                                                            </div> 
                                                        </td>
                                                        <td style="width:10px;"></td>
                                                    </tr>
                                                     <tr>
                                                        <td style="width:10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size:14px;">นามสกุล</span>
                                                            </div> 
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_lname" type="text" placeholder="LAST NAME" style="text-align:center;" />
                                                            </div> 
                                                        </td>
                                                        <td style="width:10px;"></td>
                                                    </tr>
                                                     <tr>
                                                        <td style="width:10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size:14px;">เลขบัตรประชาชน</span>
                                                            </div> 
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_nation" type="text" placeholder="NATION ID" maxlength="13" style="text-align:center;" onkeypress="return event.charCode >= 48 && event.charCode <= 57" />
                                                            </div> 
                                                        </td>
                                                        <td style="width:10px;"></td>
                                                    </tr>
                                                     <tr>
                                                        <td style="width:10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size:14px;">เบอร์โทรศัพท์</span>
                                                            </div> 
                                                        </td>
                                                        <td>
                                                            <div>                                                                
                                                                <input id="regis_tel" type="text" placeholder="TEL" style="text-align:center;" maxlength="10" onkeypress="return event.charCode >= 48 && event.charCode <= 57" />
                                                            </div> 
                                                        </td>
                                                        <td style="width:10px;"></td>
                                                    </tr>
                                                     <tr>
                                                        <td style="width:10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size:14px;">อีเมล์</span>
                                                            </div> 
                                                        </td>
                                                        <td>
                                                            <div>                                                                
                                                                <input id="regis_email" type="text" placeholder="EMAIL" style="text-align:center;" />
                                                            </div> 
                                                        </td>
                                                        <td style="width:10px;"></td>
                                                    </tr>
                                                     <tr>
                                                        <td style="width:10px;"></td>
                                                        <td>
                                                            
                                                        </td>
                                                        <td>
                                                            <div style="width:110px; float:left;">
                                                                <input id="btnRegisSubmit" class="btnsubmit" type="button" value="ยืนยัน" style="text-align:center;" />
                                                            </div> 
                                                            <div style="width:110px; float:right;">
                                                                <input id="btnCancelRegis" class="btncancel" type="button" value="ยกเลิก" style="text-align:center;" />
                                                            </div> 
                                                        </td>
                                                        <td style="width:10px;"></td>
                                                    </tr>
                                                </table> 
                                            </td>
                                            <td style="width: 200px; vertical-align:text-top; ">
                                               <div>หมายเหตุ.</div>
                                            </td>
                                        </tr>
                                    </table>--%>     
                                    <table style="width: 800px; border-collapse: collapse; border-spacing: 0;">
                                        <tr>
                                            <td style="width: 400px; vertical-align:top;">
                                                <table style="width: 400px; border-collapse: collapse; border-spacing: 0;">
                                                    <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size: 14px;">ชื่อผู้ใช้</span>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_user" type="text" placeholder="USERNAME" style="text-align: center;" />
                                                            </div>
                                                        </td>
                                                        <td style="width: 10px;"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size: 14px;">รหัสผ่าน</span>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_pass" type="password" placeholder="PASWORD" style="text-align: center;" />
                                                            </div>
                                                        </td>
                                                        <td style="width: 10px;"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size: 14px;">ยืนยันรหัสผ่าน</span>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_conf" type="password" placeholder="CONFIRM PASWORD" style="text-align: center;" />
                                                            </div>
                                                        </td>
                                                        <td style="width: 10px;"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size: 14px;">ชื่อ</span>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_fname" type="text" placeholder="FIRST NAME" style="text-align: center;" />
                                                            </div>
                                                        </td>
                                                        <td style="width: 10px;"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size: 14px;">นามสกุล</span>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_lname" type="text" placeholder="LAST NAME" style="text-align: center;" />
                                                            </div>
                                                        </td>
                                                        <td style="width: 10px;"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size: 14px;">เลขบัตรประชาชน</span>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_nation" type="text" placeholder="NATION ID" maxlength="13" style="text-align: center;" onkeypress="return event.charCode >= 48 && event.charCode <= 57" />
                                                            </div>
                                                        </td>
                                                        <td style="width: 10px;"></td>
                                                    </tr>
                                                     <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size: 14px;">เบอร์โทรศัพท์</span>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_tel" type="text" placeholder="TEL" style="text-align: center;" maxlength="10" onkeypress="return event.charCode >= 48 && event.charCode <= 57" />
                                                            </div>
                                                        </td>
                                                        <td style="width: 10px;"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size: 14px;">อีเมล์</span>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_email" type="text" placeholder="EMAIL" style="text-align: center;" />
                                                            </div>
                                                        </td>
                                                        <td style="width: 10px;"></td>
                                                    </tr>   
                                                     <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size: 14px;">LINE ID</span>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <input id="regis_line" type="text" placeholder="LINE ID" style="text-align: center;" />
                                                            </div>
                                                        </td>
                                                        <td style="width: 10px;"></td>
                                                    </tr>   
                            
                                                </table>
                                            </td>
                                            <td style="width: 400px; vertical-align:top;">
                                                <table style="width: 400px; border-collapse: collapse; border-spacing: 0;">                            
                                                   
                                                    <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td>
                                                            <div>
                                                                <span style="font-size: 14px;">ข้อมูลบัญชี</span>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <select id="ddlBank">
                                                                    <option value="11111">11111</option>
                                                                </select>                                                                
                                                            </div>
                                                        </td>
                                                        <td style="width: 10px;"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td>
                                                           
                                                        </td>
                                                        <td>
                                                            <div>
                                                               <input id="regis_acc" type="text" placeholder="ข้อมูลบัญชี" style="text-align: center;" />
                                                            </div>
                                                        </td>
                                                        <td style="width: 10px;"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td style="vertical-align:top;">
                                                            <div>
                                                                <span style="font-size: 14px;">สำเนาบัตรประชาชน</span>
                                                            </div>
                                                        </td>
                                                        <td style="vertical-align:top;">
                                                            <div>
                                                                 <input id="hdfRegisPath" type="hidden" />
                                                                 <img id="imgPreview" src="ads/icon_no.png" width="170" height="170"/>
                                                                 <div id="fileuploader" style="width: 150px;">Upload</div>
                                                            </div>
                                                        </td>
                                                        <td style="width: 10px;"></td>
                                                    </tr>                           
                                                   <tr>
                                                        <td style="width: 10px;"></td>
                                                        <td></td>
                                                        <td>
                                                            <div style="width: 110px; float: left;">
                                                                <input id="btnRegisSubmit" class="btnsubmit" type="button" value="ยืนยัน" style="text-align: center;" />
                                                            </div>
                                                            <div style="width: 110px; float: right;">
                                                                <input id="btnCancelRegis" class="btncancel" type="button" value="ยกเลิก" style="text-align: center;" />
                                                            </div>
                                                        </td>
                                                        <td style="width: 10px;"></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>                             
                                     
                                </td>                                
                            </tr>
                        </table>
                                       
                    </center>
            </div>
        </div>
        <div id="divLoadingTemplate" style="display: none;">
            <div class='divOver'>
            </div>
            <div id="divLoader1">
                <center>
                        <table id="tableLoad" align="center">
                            <tr>
                                <td style="width:50px;"></td>
                                <td>
                                     <img src="image/loading.gif" />
                                       
                                </td>
                                <td><span style="color:#000; font-size:24px;">Now loading ...</span> </td>
                            </tr>
                        </table>
                                       
                    </center>
            </div>
        </div>

        <script>
            $(function () {

                getDataBank();

                $('#username').focus();

                $('#btnLogin').click(function () {

                    if ($('#username').val() == '' || $('#password').val() == '') {
                        $('#remark').html('Please enter username or password.');
                    } else {
                        login($('#username').val(), $('#password').val());
                    }

                });

                $('#username').keypress(function (event) {
                    if (event.which == 13) {
                        $('#password').focus();
                    }
                });

                $('#password').keypress(function (event) {
                    if (event.which == 13) {
                        $('#btnLogin').click();
                    }
                });

                $('#btnRegisSubmit').click(function () {
                    CheckRegister();
                });

                $('#btnCancelRegis').click(function () {
                    $('#divRegisTemplate').fadeOut(400, function () {
                        $('#username').focus();
                    });
                });

                $('#btnForSubmit').click(function () {
                    sendforgot();
                });

                $('#btnForCancle').click(function () {
                    $('#divForgotTemplate').fadeOut(400, function () {
                        $('#username').focus();
                    });
                });
                
                $("#fileuploader").uploadFile({
                    url: "../action/UploadCardAction.aspx",
                    fileName: "CardFile",
                    maxFileSize: 100 * 1024,
                    //showPreview: true,
                    //previewHeight: "150px",
                    //previewWidth: "250px",
                    onSuccess: function (files, data, xhr, pd) {
                        
                        $('#hdfRegisPath').val(files[0]);
                        $('#imgPreview').attr('src', '../card/' + files[0]);
                        //$('#imgPreview').css('display', 'block');

                        $('.ajax-file-upload-container').html('');

                    },
                    onError: function (files, status, errMsg, pd) {
                        //$("#eventsmessage").html($("#eventsmessage").html() + "<br/>Error for: " + JSON.stringify(files));
                        $('.error').html('เกิดข้อผิดพลาดในการอัพโหลดไฟล์').fadeIn(500).delay(2000).fadeOut(500);
                        $('.ajax-file-upload-container').html('');
                        //$('#txtTopup').val('');
                        //$('#txtTopup').focus();
                    },
                    onCancel: function (files, pd) {
                        //$("#eventsmessage").html($("#eventsmessage").html() + "<br/>Canceled  files: " + JSON.stringify(files));
                        $('.error').html('ยกเลิกการอัพโหลดไฟล์').fadeIn(500).delay(2000).fadeOut(500);
                        $('.ajax-file-upload-container').html('');
                    }
                });

            });
        </script>
    </form>

</body>
</html>
