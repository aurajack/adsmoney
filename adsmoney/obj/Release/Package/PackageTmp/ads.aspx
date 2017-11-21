<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ads.aspx.vb" Inherits="adsmoney.ads" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="js/jsAds.js?v=1"></script>
    <link href="http://hayageek.github.io/jQuery-Upload-File/4.0.10/uploadfile.css" rel="stylesheet">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script src="http://hayageek.github.io/jQuery-Upload-File/4.0.10/jquery.uploadfile.min.js"></script>
    <div style="width: 100%; text-align: center; vertical-align: top;">
        <div style="width:100%; background-color:#d7d7d7; border-radius:3px; height:35px; text-align:left;">
            <span style="vertical-align:middle; font-weight:bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;คลิกโฆษณา</span>
        </div>
         <div id="divRemark" style="width:100%; background-color:#e56d6d; border-radius:3px; height:35px; text-align:center; display:none;">
            <span style="vertical-align:middle; font-weight:bold;">กรุณาตรวจสอบสถานะการชำระเงินค่าแพคเกจของท่าน</span>
        </div>      
        <table style="width: 100%; border-collapse: collapse; border-spacing: 0;">
            <tr>
                <td style="width: 50px;"></td>
                <td align="center">
                   <div style="text-align: center; width: 420px;">
                       
                    </div>
                </td>
                <td style="width: 50px;"></td>
            </tr>
        </table> 
       
        <div style="width:100%; text-align:center; height:50px;"></div>        
        <div id="divData" style="width:100%; text-align:center;">  
           
        </div>      
        <div id="divDataTemplate" align="center" style="display: none;">
            <table style="width: 100%; border-collapse: collapse; border-spacing: 0; font-size:14px; height:35px;">
                <tr>
                    <td style="text-align:center;">
                        <img class="ads" src="image/logo.png" width="250" style="cursor:pointer" /><br />
                        <span style="cursor:pointer" >คลิกที่นี่ (จำนวนครั้ง 0/3)</span>
                    </td>
                     <td style="text-align:center;">
                        <img class="ads" src="image/logo.png" width="250" style="cursor:pointer" /><br />
                        <span style="cursor:pointer" >คลิกที่นี่ (จำนวนครั้ง 0/3)</span>
                    </td>
                     <td style="text-align:center;">
                        <img class="ads" src="image/logo.png" width="250" style="cursor:pointer" /><br />
                        <span style="cursor:pointer" >คลิกที่นี่ (จำนวนครั้ง 0/3)</span>
                    </td>
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
                        <img style="margin:50px -35px 45px 0px;" src="/image/loading.gif" width="150px" />
                    </td>
                    <td style="text-align: left; width: 60%; vertical-align: middle;">
                        <span style="font-size: medium; font-weight:bold; color: #808080;">กำลังค้นหาข้อมูล...</span>
                    </td>
                </tr>
            </table>
        </div>
    </div>
        <script>
            $(function () {

                //$('#btnTopup').click(function () {

                //    SaveTopup();

                //});

                $("#fileuploader").uploadFile({
                    url: "../action/UploadAction.aspx",
                    fileName: "AdsFile",
                    formData: { "adsname": $('#txtAdsname').val(), "adslink": $('#txtAdslink').val() },
                    //showPreview: true,
                    //previewHeight: "150px",
                    //previewWidth: "250px",
                    onSuccess: function (files, data, xhr, pd) {

                        //$("#eventsmessage").html($("#eventsmessage").html() + "<br/>Success for: " + JSON.stringify(data));
                        //console.log(files[0]);               
                        SaveTopup(files[0]);

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
