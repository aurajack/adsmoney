<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="manageads.aspx.vb" Inherits="adsmoney.manageads" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="js/jsManageAds.js?v=2"></script>
    <%--<link href="http://hayageek.github.io/jQuery-Upload-File/4.0.10/uploadfile.css" rel="stylesheet" />--%>
    <link href="css/styleUpload.css" rel="stylesheet" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <%--<script src="http://hayageek.github.io/jQuery-Upload-File/4.0.10/jquery.uploadfile.min.js"></script>--%>
    <script src="js/jsUloadmin.js"></script>
    <div style="width: 100%; text-align: center; vertical-align: top;">
        <div style="width:100%; background-color:#d7d7d7; border-radius:3px; height:35px; text-align:left;">
            <span style="vertical-align:middle; font-weight:bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;จัดการโฆษณา</span>
        </div>     
        <table style="width: 100%; border-collapse: collapse; border-spacing: 0;">
            <tr>
                <td style="width: 50px;"></td>
                <td align="center">
                   <div style="text-align: center; width: 420px;">
                        <input id="txtAdsname" type="text" value="" style="text-align: center; width:420px;" placeholder="กรุณากรอกชื่อโฆษณา" />
                        <input id="txtAdslink" type="text" value="" style="text-align: center; width:420px;" placeholder="กรุณากรอกลิงค์โฆษณา" />
                        <span style="font-size: 12px; color: red; text-align: center;">รูปโฆษณาขนาดจะต้องเป็นขนาด 250 x 350 pixel</span><br />
                        <input id="hdfID" type="hidden" />
                        <input id="hdfAdsPath" type="hidden" />
                        <img id="imgPreview" src="ads/icon_no.png" width="250" height="350" style="display:none;"/>
                        <div id="fileuploader" style="width: 330px;">Upload</div>
                        <input id="btnSave" class="btnsubmit" type="button" value="บันทึก" />
                    </div>
                </td>
                <td style="width: 50px;"></td>
            </tr>
        </table> 
       
        <div style="width:100%; text-align:center; height:50px;"></div>        
        <div style="width: 100%; background-color: #d7d7d7; border-radius: 3px; text-align: center;">
            <table style="width: 100%; border-collapse: collapse; border-spacing: 0; font-size: 14px; font-weight: bold; height: 35px;">
                <tr>
                    <td style="width: 2%; height: 35px;"></td>
                    <td style="width: 10%; height: 35px;">
                        <span class="column">ลำดับ</span>
                    </td>
                    <td style="width: 15%; height: 35px;">
                        <span class="column">วันที่ทำรายการ</span>
                    </td>
                    <td style="width: 20%; height: 35px;">
                        <span class="column">ชื่อโฆษณา</span>
                    </td>
                     <td style="width: 25%; height: 35px;">
                        <span class="column">ลิ้งโฆษณา</span>
                    </td>                   
                    <td style="width: 10%; height: 35px;">
                        <span class="column">สถานะ</span>
                    </td>                  
                    <td style="width: 16%; height: 35px;">
                        <span class="column">ดูตัวอย่างไฟล์</span>
                    </td>
                    <td style="width: 2%; height: 35px;"></td>
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
                    <td style="width: 2%; height: 35px;"></td>
                    <td style="width: 10%; height: 35px;">
                        <span class="no">1</span>
                    </td>
                    <td style="width: 15%; height: 35px;">
                        <span class="system_date"></span>
                    </td>
                    <td style="width: 20%; height: 35px;">
                        <span class="ads_name"></span>
                    </td>
                     <td style="width: 25%; height: 35px;">
                        <span class="ads_link"></span>
                    </td>                   
                    <td style="width: 10%; height: 35px;">
                        <span class="ads_status"></span>
                    </td>                  
                    <td style="width: 16%; height: 35px;">
                        <div class="file"></div>       
                    </td>
                    <td style="width: 2%; height: 35px;"></td>
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
        <script>
            $(function () {

                $('#btnSave').click(function () {

                    SaveData( $('#hdfAdsPath').val());

                });

                $("#fileuploader").uploadFile({
                    url: "../action/UploadAdsAction.aspx",
                    action:'ads',
                    fileName: "AdsFile",
                    maxFileSize:100*1024,
                    //showPreview: true,
                    //previewHeight: "150px",
                    //previewWidth: "250px",
                    onSuccess: function (files, data, xhr, pd) {

                       
                        $('#hdfAdsPath').val(files[0]);
                        $('#imgPreview').attr('src', '../ads/' + files[0]);
                        $('#imgPreview').css('display', 'block');
                     
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
