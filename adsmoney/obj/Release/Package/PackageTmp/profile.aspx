<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="profile.aspx.vb" Inherits="adsmoney.profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="js/jsManageProfile.js?v=2"></script>   
    <%--<link href="http://hayageek.github.io/jQuery-Upload-File/4.0.10/uploadfile.css" rel="stylesheet" />--%>
    <link href="css/styleUpload.css" rel="stylesheet" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <%--<script src="http://hayageek.github.io/jQuery-Upload-File/4.0.10/jquery.uploadfile.min.js"></script>--%>
    <script src="js/jsUloadmin.js"></script>
    <div style="width: 100%; text-align: center; vertical-align: top;">
        <div style="width:100%; background-color:#d7d7d7; border-radius:3px; height:35px; text-align:left;">
            <span style="vertical-align:middle; font-weight:bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ข้อมูลส่วนตัว</span>
        </div>     
        <table style="width: 100%; border-collapse: collapse; border-spacing: 0;">
            <tr>
                <td style="width: 50px;"></td>
                <td align="center">
                   <div style="text-align: center; width: 420px;">
                        <input id="txtUser" type="text" value="" style="text-align: center; width:420px;" disabled="disabled" />
                        <input id="txtNation" type="text" value="" style="text-align: center; width:420px;" disabled="disabled" />
                        <input id="txtMobile" type="text" value="" style="text-align: center; width:420px;" disabled="disabled" />
                        <input id="txtEmail" type="text" value="" style="text-align: center; width:420px;" disabled="disabled" />
                        <input id="txtFname" type="text" value="" style="text-align: center; width:420px;" placeholder="กรุณากรอกชื่อ" />
                        <input id="txtLname" type="text" value="" style="text-align: center; width:420px;" placeholder="กรุณากรอกนามสกุล" />
                        <select id="ddlBank" style="text-align-last:center;"></select>
                        <input id="txtAcc" type="text" value="" style="text-align: center; width:420px;" placeholder="กรุณากรอกข้อมูลบัญชีธนาคาร" />
                        <input id="txtLine" type="text" value="" style="text-align: center; width:420px;" placeholder="กรุณากรอกไลน์ไอดี" />
                        <img id="imgPreview" src="ads/icon_no.png" width="420" />
                        <span style="font-size: 12px; color: red; text-align: center;">กรุณาอัพโหลดรูปหน้าบัตรประชาชน</span><br />
                        <div id="fileuploader" style="width: 330px;">Upload</div>                        
                        <input id="hdfID" type="hidden" />                         
                        <input id="btnSave" class="btnsubmit" type="button" value="บันทึกข้อมูล" />
                        <br />
                        <input id="txtPass" type="password" value="" style="text-align: center; width:420px;" placeholder="กรุณากรอกพาสเวิดเดิม" />
                        <input id="txtNewpass" type="password" value="" style="text-align: center; width:420px;" placeholder="กรุณากรอกพาสเวิดใหม่" />
                        <input id="txtConnewpass" type="password" value="" style="text-align: center; width:420px;" placeholder="กรุณากรอกพาสเวิดใหม่อีกครั้ง" />
                        <input id="btnReset" class="btnsubmit" type="button" value="บันทึกรหัสผ่าน" />
                    </div>
                </td>
                <td style="width: 50px;"></td>
            </tr>
        </table>
       
        <div style="width:100%; text-align:center; height:50px;"></div>       
        
        <div id="divShowImgTemplate" style="display: none;">
            <div class='divOver' onclick="HideImg();">
            </div>
            <div id="divImg" style=" position: fixed; width: 600px; height: 350px; z-index: 200; top: 60%; left: 50%; margin: -300px 0 0 -150px; overflow:auto;">
                <img id="showImg" src="../member/1.png" />
            </div>
        </div>

    </div>
        <script>
            $(function () {

                $('#btnSave').click(function () {

                    SaveData($('#hdfID').val());

                });

                $('#btnReset').click(function () {

                    ChangePassword($('#hdfID').val());

                });

                $("#fileuploader").uploadFile({
                    url: "../action/UploadCardAction.aspx",
                    fileName: "CardFile",
                    maxFileSize: 100 * 1024,
                    onSuccess: function (files, data, xhr, pd) {
                        
                        //$('#imgPreview').attr('src', '../card/' + files[0]);         
                        SaveImgCard(files[0]);
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
</asp:Content>
