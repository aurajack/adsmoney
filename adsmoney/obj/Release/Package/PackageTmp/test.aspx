<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="test.aspx.vb" Inherits="adsmoney.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width: 800px; border-collapse: collapse; border-spacing: 0;">
                <tr>
                    <td style="width: 400px;">
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
                            
                        </table>
                    </td>
                    <td style="width: 400px; vertical-align:top;">
                        <table style="width: 400px; border-collapse: collapse; border-spacing: 0;">                            
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
                            <tr>
                                <td style="width: 10px;"></td>
                                <td>
                                    <div>
                                        <span style="font-size: 14px;">ข้อมูลบัญชี</span>
                                    </div>
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
                                <td>
                                    <div>
                                        <span style="font-size: 14px;">สำเนาบัตรประชาชน</span>
                                    </div>
                                </td>
                                <td>
                                    <div>
                                         <img id="imgPreview" src="ads/icon_no.png" width="250" height="250" style="display:none;"/>
                                         <div id="fileuploader" style="width: 250px;">Upload</div>
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
        </div>
    </form>
</body>
</html>
