﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="package.aspx.vb" Inherits="adsmoney.package" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="js/jsPackage.js?v=3"></script>
    <div style="width: 100%; text-align: center; vertical-align: top;">
        <div style="width: 100%; background-color: #d7d7d7; border-radius: 3px; height: 35px; text-align: left;">
            <span style="vertical-align: middle; font-weight: bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ซื้อแพคเกจ</span>
        </div>
        <table style="width: 100%; border-collapse: collapse; border-spacing: 0;">
            <tr>
                <td style="width: 50px;"></td>
                <td align="center">
                    <div id="dataPackage">
                        
                    </div>
                    <div style="text-align: center; width: 350px;"> 
                                          
                        <input id="hdfPackID" type="hidden" />
                        <input id="btnSave" type="button" value="บันทึก"  class="btnsubmit" />
                    </div>
                 
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
                        <span class="column">วันที่ซื้อแพคเกจ</span>
                    </td>
                    <td style="width: 20%; height: 35px;">
                        <span class="column">ชื่อแพคเกจ</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="column">ราคาแพคเกจ</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="column">ยอดเงินสะสม</span>
                    </td>
                    <td style="width: 15%; height: 35px;">
                        <span class="column">วันหมดอายุ</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="column">สถานะ</span>
                    </td>
                    <td style="width: 11%; height: 35px;">
                        <span class="column">ยกเลิกรายการ</span>
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
                        <span class="system_date">1</span>
                    </td>
                    <td style="width: 20%; height: 35px;">
                        <span class="pack_name">1</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="pack_amount">1</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="total_amount">1</span>
                    </td>
                    <td style="width: 15%; height: 35px;">
                        <span class="expire_date">1</span>
                    </td>
                    <td style="width: 10%; height: 35px;">
                        <span class="pack_status">1</span>
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
    <script>
        $(function () {

            $('#btnSave').click(function () {

                SavePackage();

            });
            
        });
    </script>
</asp:Content>

