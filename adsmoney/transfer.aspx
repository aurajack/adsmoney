<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="transfer.aspx.vb" Inherits="adsmoney.transfer" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="js/jsTransfer.js?v=2"></script>
    <%--<link href="http://hayageek.github.io/jQuery-Upload-File/4.0.10/uploadfile.css" rel="stylesheet" />--%>
    <link href="css/styleUpload.css" rel="stylesheet" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <%--<script src="http://hayageek.github.io/jQuery-Upload-File/4.0.10/jquery.uploadfile.min.js"></script>--%>
    <script src="js/jsUloadmin2.js"></script>
    <div style="width: 100%; text-align: center; vertical-align: top;">
        <div style="width: 100%; background-color: #d7d7d7; border-radius: 3px; height: 35px; text-align: left;">
            <span style="vertical-align: middle; font-weight: bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;รายการเบิก-ถอนเงิน</span>
        </div>       
        <table style="width: 100%; border-collapse: collapse; border-spacing: 0;">
            <tr>
                <td style="width: 50px;"></td>
                <td align="center">
                    <div style="text-align: center; width: 420px;">
                        <span>โอนเงินไปยังบัญชีของ</span><input id="txtName" type="text" value="" style="text-align: center;" disabled="disabled"  />
                        <span>รายละเอียดบัญชี</span><input id="txtAcc" type="text" value="" style="text-align: center;" disabled="disabled"  />
                        <span>เบอร์ติดต่อ</span><input id="txtMobile" type="text" value="" style="text-align: center;" disabled="disabled"  />
                        <input id="hdfID" type="hidden" value="" />
                        <input id="hdfIDRe" type="hidden" value="" />
                        <input id="hdfTransferPath" type="hidden" value="" />
                        <img id="imgPreview" src="ads/icon_no.png" width="250" height="350" style="display:none;" align="center" />
                        <div id="fileuploader" style="width: 330px;">Upload สลิป</div>    
                        <input id="btnSave" class="btnsubmit" type="button" value="บันทึก" />                    
                    </div>                 
                </td>
                <td style="width: 50px;"></td>
            </tr>
        </table>
        <div style="width: 100%; background-color: #d7d7d7; border-radius: 3px; text-align: center;">
            <table style="width: 100%; border-collapse: collapse; border-spacing: 0; font-size: 14px; font-weight: bold; height: 35px;">
                <tr>
                    <td style="width: 1%; height: 35px;"></td>
                    <td style="width: 4%; height: 35px;">
                        <span class="column">ลำดับ</span>
                    </td>
                    <td style="width: 15%; height: 35px;">
                        <span class="column">วันที่ทำรายการ</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="column">รายการ</span>
                    </td>
                    <td style="width: 15%; height: 35px;">
                        <span class="column">ชื่อสมาชิก</span>
                    </td>
                     <td style="width: 8%; height: 35px;">
                        <span class="column">เบอร์โทร</span>
                    </td>
                    <td style="width: 8%; height: 35px;">
                        <span class="column">จำนวนเงิน</span>
                    </td>
                    <td style="width: 8%; height: 35px;">
                        <span class="column">สถานะ</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="column">สลิป</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="column">เลือกโอน</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="column">อนุมัติ</span>
                    </td>
                    <td style="width: 1%; height: 35px;"></td>
                </tr>
            </table>
        </div>
        <div id="divShowImgTemplate" style="display: none;">
            <div class='divOver' onclick="HideImg();">
            </div>
            <div id="divImg" style=" position: fixed; width: 600px; height: 350px; z-index: 200; top: 60%; left: 50%; margin: -300px 0 0 -150px; overflow:auto;">
                <img id="showImg" src="../member/1.png" />
            </div>
        </div>
        <div id="divData" style="width: 100%; text-align: center;">
        </div>
        <div id="divDataTemplate" align="center" style="display: none;">
            <table style="width: 100%; border-collapse: collapse; border-spacing: 0; font-size: 14px; height: 35px;">
                 <tr>
                    <td style="width: 1%; height: 35px;"></td>
                    <td style="width: 4%; height: 35px;">
                        <span class="no">1</span>
                    </td>
                    <td style="width: 15%; height: 35px;">
                        <span class="system_date">1</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="job">1</span>
                    </td>
                    <td style="width: 15%; height: 35px;">
                        <span class="name">1</span>
                    </td>
                     <td style="width: 8%; height: 35px;">
                        <span class="mobile">1</span>
                    </td>
                    <td style="width: 8%; height: 35px;">
                        <span class="amount">1</span>
                    </td>
                    <td style="width: 8%; height: 35px;">
                        <span class="status">1</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <div class="file"></div> 
                    </td>
                      <td style="width: 10%; height: 35px;">
                        <div class="file2"></div> 
                    </td>
                     <td style="width: 10%; height: 35px;">
                        <div class="file3"></div> 
                    </td>
                    <td style="width: 1%; height: 35px;"></td>
                </tr>               
            </table>
        </div>
        <div id="divNoTranTemplate" style="display: none;">
            <table class="info-table" style="border-collapse: collapse; border-spacing: 0; margin-bottom: 10px; width: 100%">
                <tbody>
                    <tr class="info-line">
                        <td style="width: 100%; text-align: center; padding: 7px 0px;">
                            <span>ไม่พบข้อมูล</span>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="divLoadingTemplate" style="display: none;">
            <table style="border-collapse: collapse; width: 100%;">
                <tr>
                    <td style="text-align: right; width: 40%;">
                        <img style="margin: 50px -35px 45px 0px;" src="/image/loading.gif" width="150px" />
                    </td>
                    <td style="text-align: left; width: 60%; vertical-align: middle;">
                        <span style="font-size: medium; font-weight: bold; color: #808080;">กำลังค้นหาข้อมูล...</span>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divConfirmTemplate2" style="display: none;">
            <div class='divOver2'>
            </div>
            <div id="divConfirm2">
                <center>
                        <table id="tableConfirm2" align="center">
                            <tr>                                
                                <td style="float:left; width:400px;">                                     
                                   <div style="text-align:left; font-size:22px; font-weight:bold; padding:10px; border-bottom: solid 1px #dddddd;">ยืนยันการอนุมัติ</div>    
                                    <table style="width: 400px; border-collapse: collapse; border-spacing: 0;">
                                        <tr>
                                            <td style="height:20px;"></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align:center;"><span>คุณต้องการยืนยันการรับเงินหรือไม่</span></td>
                                        </tr>
                                         <tr>
                                            <td style="height:20px;"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table style="width: 400px; border-collapse: collapse; border-spacing: 0;">
                                                    <tr>
                                                        <td style="width:90px;"></td>
                                                        <td>
                                                            <div id="boxConfirm" style="width:100px; float:left;">
                                                                <input id="btnAppConfirm" class="btnsubmit" type="button" value="ยืนยัน" style="text-align:center;" />
                                                            </div> 
                                                            <div id="boxCancel" style="width:100px; float:right;">
                                                                <input id="btnAppCancel" class="btncancel" type="button" value="ยกเลิก" style="text-align:center;" />
                                                            </div> 
                                                        </td>
                                                        <td style="width:90px;"></td>
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
        <input id="hdfAppID" type="hidden" value="" />
        <input id="hdfAppMempackid" type="hidden" value="" />
    <script>
        $(function () {

            $('#btnSave').click(function () {

                SaveData($('#hdfTransferPath').val());

            });

            $('#btnAppConfirm').click(function () {

                ApproveData($('#hdfAppID').val(), $('#hdfAppMempackid').val());
                $('#divConfirmTemplate2').fadeOut(400);
            });

            $('#btnAppCancel').click(function () {

                $('#hdfAppID').val('');
                $('#hdfAppMempackid').val('');
                $('#divConfirmTemplate2').fadeOut(400);

            });

            $("#fileuploader").uploadFile({
                url: "../action/UploadTransferAction.aspx",
                fileName: "TransferFile",
                //showPreview: true,
                //previewHeight: "150px",
                //previewWidth: "250px",
                onSuccess: function (files, data, xhr, pd) {

                    //$("#eventsmessage").html($("#eventsmessage").html() + "<br/>Success for: " + JSON.stringify(data));
                                
                    $('#hdfTransferPath').val(files[0]);
                    $('#imgPreview').attr('src', '../transfer/' + files[0]);
                    $('#imgPreview').css('display', 'block');

                    // $('.success').html('อัพโหลดไฟล์การชำระเงินเรียบร้อย').fadeIn(500).delay(2000).fadeOut(500);
                    //$('#txtTopup').val('');
                    //$('#txtTopup').focus();
                    //getDataTopup();
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

