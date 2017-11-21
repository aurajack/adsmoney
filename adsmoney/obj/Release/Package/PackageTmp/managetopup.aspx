<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="managetopup.aspx.vb" Inherits="adsmoney.managetopup" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="js/jsTopup.js?v=2"></script>
    <div style="width: 100%; text-align: center; vertical-align: top;">
        <div style="width: 100%; background-color: #d7d7d7; border-radius: 3px; height: 35px; text-align: left;">
            <span style="vertical-align: middle; font-weight: bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;รายการเติมเงินสมาชิก</span>
        </div>
        <table style="width: 100%; border-collapse: collapse; border-spacing: 0;">
            <tr>
                <td style="width: 50px;"></td>
                <td align="center">                  
                 
                </td>
                <td style="width: 50px;"></td>
            </tr>
        </table>
        <div style="width: 100%; background-color: #d7d7d7; border-radius: 3px; text-align: center;">
            <table style="width: 100%; border-collapse: collapse; border-spacing: 0; font-size: 14px; font-weight: bold; height: 35px;">
                <tr>
                    <td style="width: 2%; height: 35px;"></td>
                    <td style="width: 5%; height: 35px;">
                        <span class="column">ลำดับ</span>
                    </td>
                    <td style="width: 15%; height: 35px;">
                        <span class="column">วันที่เติมเงิน</span>
                    </td>
                    <td style="width: 20%; height: 35px;">
                        <span class="column">ชื่อ-สกุล</span>
                    </td>
                     <td style="width: 10%; height: 35px;">
                        <span class="column">เบอร์โทร</span>
                    </td>
                    <td style="width: 15%; height: 35px;">
                        <span class="column">วันหมดอายุ</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="column">สถานะเติมเงิน</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="column">สถานะสมาชิก</span>
                    </td>
                    <td style="width: 11%; height: 35px;">
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
                    <td style="width: 5%; height: 35px;">
                        <span class="no">1</span>
                    </td>
                    <td style="width: 15%; height: 35px;">
                        <span class="topup_date"></span>
                    </td>
                     <td style="width: 20%; height: 35px;">
                        <span class="name">ชื่อ-สกุล</span>
                    </td>
                     <td style="width: 10%; height: 35px;">
                        <span class="mobile">เบอร์โทร</span>
                    </td>
                    <td style="width: 15%; height: 35px;">
                        <span class="expire"></span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="status">รอดำเนินการ</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="mainstatus">ใช้งาน</span>
                    </td>
                    <td style="width: 11%; height: 35px;">
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
                                            <td style="text-align:center;"><span>สมาชิกมีอายุ </span><input id="txtExpireDay" type="text" value="5" style="text-align: center;" placeholder="จำนวน" /><span> วัน</span></td>
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
    <input id="hdfID" type="hidden" />
    <script>

        $(function () {
            $('#btnAppConfirm').click(function () {

                if ($('#txtExpireDay').val() == '' || $('#txtExpireDay').val() == '0') {
                    $('.error').html('กรุณากรองจำนวนวันหมดอายุ').fadeIn(500).delay(2000).fadeOut(500);
                    return false
                }
                //ApproveData($('#hdfAppID').val(), $('#hdfAppMempackid').val());
                ApproveTopup($('#hdfID').val(), 'APPROVE');
                $('div .wrapper').css('background-color', '#04B404')
                $('#divConfirmTemplate2').fadeOut(400);
               
            });

            $('#btnAppCancel').click(function () {

                $('#hdfID').val('');
                $('#txtExpireDay').val('');
                $('#divConfirmTemplate2').fadeOut(400);

            });
        });        

    </script>

</asp:Content>

