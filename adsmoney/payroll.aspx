<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="payroll.aspx.vb" Inherits="adsmoney.payroll" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
    <script>

        function testpayroll() {

            var data = {
                action: 'payroll',
                year: $('#txtYear').val(),
                month: $('#txtMonth').val(),
                list: [
                  { shopid: $('#txtShop').val(), pay: $('#txtTotal').val() }
                ]
            };

            $.ajax({
                url: 'http://202.129.206.254:9101/securestock/api/payrollData/payrollData',
                data : JSON.stringify(data),
                contentType: 'application/json ',
                type: "POST",
                dataType: "json",
                error: function () { alert('error send data.'); },
                success: function (data) {

                    alert('send data sucess.');

                }
            });
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
        <tr>
            <td style="width:200px; "><span>ปี</span></td>
            <td style="width:200px; "><span>เดือน</span></td>
            <td style="width:200px; "><span>สาขา</span></td>
            <td style="width:200px; "><span>เงินเดือนรวม</span></td>
        </tr>
        <tr>
            <td style="width:200px; "><input id="txtYear" type="text" value=""  /></td>
            <td style="width:200px; "><input id="txtMonth" type="text" value=""  /></td>
            <td style="width:200px; "><input id="txtShop" type="text" value=""  /></td>
            <td style="width:200px; "><input id="txtTotal" type="text" value=""  /></td>
        </tr>
        <tr>
            <td style="width:100px; " colspan="4"><span id="status"></span></td>
        </tr>
        <tr>
            <td style="width:100px; " colspan="4"><input id="btnSubmit" type="button" value="Send Request"  /></td>
        </tr>
    </table>
    </div>

         <script>
             $(function () {               

                 $('#btnSubmit').click(function () {

                     testpayroll();

                 });

             });
          </script>
    </form>
</body>
</html>
