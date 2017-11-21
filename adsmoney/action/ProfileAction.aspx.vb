Imports System.IO
Imports System.Data.SqlClient

Public Class ProfileAction
    Inherits CModule

    Public Structure JSONObject
        Public Param As Object
        Public Cookie As Object
        Public exMessage As String
    End Structure

    Public Structure JSONObjectArray
        Public Param() As Object
        Public Cookie As Object
        Public exMessage As String
    End Structure

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.Form("action") IsNot Nothing Then

            Select Case Request.Form("action").ToString

                Case "profiledata"

                    Call GenData(Request.Form("memid").ToString, Request.Form("id").ToString)

                Case "save"

                    Call SaveData(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("fname").ToString, Request.Form("lname").ToString, Request.Form("acc").ToString, Request.Form("line").ToString, Request.Form("bank_id").ToString)

                Case "saveimg"

                    Call SaveImg(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("file").ToString)

                Case "changepassword"

                    Call ChangePassword(Request.Form("id").ToString, Request.Form("pass").ToString)


            End Select

        End If

    End Sub

    Private Sub GenData(ByVal _MemData As String, ByVal _ID As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemData
        Try

            Cookie = CheckExpire(_MemData)

            Dim sql As String = " select id,username,fname,lname,account,line_id,mobile,email,nation_code,img_nation,bank_id from member where id = " & _ID & ""

            If Conn.State = ConnectionState.Closed Then Conn.Open()
            Dim dtData As DataTable = GetDataTable(Sql, Conn)
            If Conn.State = ConnectionState.Open Then Conn.Close()

            Dim DataInfoItems As New Generic.List(Of ItemDataProfile)

            Dim item As New ItemDataProfile()

            For Each mRow As DataRow In dtData.Select("")

                item = New ItemDataProfile()

                item.id = mRow("id").ToString
                item.user = mRow("username").ToString
                item.nation = mRow("nation_code").ToString
                item.mobile = mRow("mobile").ToString
                item.email = mRow("email").ToString
                item.fname = mRow("fname").ToString
                item.lname = mRow("lname").ToString
                item.acc = mRow("account").ToString
                item.line = mRow("line_id").ToString
                item.img_nation = mRow("img_nation").ToString
                item.bank_id = mRow("bank_id").ToString
                DataInfoItems.Add(item)

            Next

            Response.Write(JSON.Serialize(Of Generic.List(Of ItemDataProfile))(DataInfoItems))

        Catch ex As Exception
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
        End Try
        Response.End()

    End Sub

    Private Sub SaveData(ByVal _MemID As String, ByVal _ID As String, ByVal _Fname As String, ByVal _Lname As String, ByVal _Acc As String, ByVal _Line As String, ByVal _BankID As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemID
        Try

            Cookie = CheckExpire(_MemID)

            Dim sql As String = ""

            sql = "update member set fname=@fname,lname=@lname,account=@acc,line_id=@line,updated_date=getdate(),bank_id = @bankid where id = " & _ID & " "

            Cmd = New SqlCommand(sql, Conn)

            Cmd.Parameters.Add("@fname", SqlDbType.VarChar).Value = _Fname
            Cmd.Parameters.Add("@lname", SqlDbType.VarChar).Value = _Lname
            Cmd.Parameters.Add("@acc", SqlDbType.VarChar).Value = _Acc
            Cmd.Parameters.Add("@line", SqlDbType.VarChar).Value = _Line
            Cmd.Parameters.Add("@bankid", SqlDbType.BigInt).Value = _BankID

            Conn.Open()

            Cmd.ExecuteNonQuery()
            Conn.Close()

            JSONResponse.exMessage = JSON.FixString("")
            JSONResponse.Param = "success"
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

        Catch ex As Exception

            Conn.Close()
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

        End Try

        Response.End()

    End Sub

    Private Sub ChangePassword(ByVal _ID As String, ByVal _Pass As String)

        Dim JSONResponse As New JSONObject()
        Try

            Dim sql As String = ""

            sql = "update member set password=@pass where id = " & _ID & " "

            Cmd = New SqlCommand(sql, Conn)

            Cmd.Parameters.Add("@pass", SqlDbType.VarChar).Value = MD5(_Pass)

            Conn.Open()

            Cmd.ExecuteNonQuery()
            Conn.Close()

            JSONResponse.exMessage = JSON.FixString("")
            JSONResponse.Param = "success"
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

        Catch ex As Exception

            Conn.Close()
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

        End Try

        Response.End()

    End Sub

    Private Sub SaveImg(ByVal _MemID As String, ByVal _ID As String, ByVal _File As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemID
        Try

            Cookie = CheckExpire(_MemID)

            Dim sql As String = ""


            sql = "update member set img_nation=@img,updated_date=getdate() where id = " & _ID & " "


            Cmd = New SqlCommand(sql, Conn)

            Cmd.Parameters.Add("@img", SqlDbType.VarChar).Value = _File

            Conn.Open()

            Cmd.ExecuteNonQuery()
            Conn.Close()

            JSONResponse.exMessage = JSON.FixString("")
            JSONResponse.Param = "success"
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

        Catch ex As Exception

            Conn.Close()
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

        End Try

        Response.End()

    End Sub

End Class

Public Structure ItemDataProfile

    Public id As String
    Public user As String
    Public nation As String
    Public mobile As String
    Public email As String
    Public fname As String
    Public lname As String
    Public acc As String
    Public line As String
    Public img_nation As String
    Public bank_id As String

End Structure