Imports System.IO
Imports System.Data.SqlClient

Public Class LoginAction
    Inherits CModule

    Public Structure JSONObject
        Public Param As Object
        Public exMessage As String
    End Structure

    Public Structure JSONObjectArray
        Public Param() As Object
        Public exMessage As String
    End Structure

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.Form("action") IsNot Nothing Then

            Select Case Request.Form("action").ToString

                Case "login"

                    Call Login(Request.Form("user").ToString, Request.Form("pass").ToString)

                Case "register"

                    Call Register(Request.Form("user").ToString, Request.Form("pass").ToString, Request.Form("fname").ToString _
                                  , Request.Form("lname").ToString, Request.Form("nation_id").ToString, Request.Form("tel").ToString _
                                  , Request.Form("email").ToString, Request.Form("acc").ToString, Request.Form("line").ToString _
                                  , Request.Form("file").ToString, Request.Form("bank").ToString)

                Case "forgot"

                    Call ForgotPassword(Request.Form("user").ToString, Request.Form("nation").ToString, Request.Form("email").ToString)

                Case "getdatabank"

                    Call GetDataBank()

            End Select

        End If

    End Sub

    Private Sub GetDataBank()

        Dim JSONResponse As New JSONObject()

        Try

            Dim sql As String = " select id,code,bank_name from bank where status='ACTIVE' order by bank_name asc"
 
            If Conn.State = ConnectionState.Closed Then Conn.Open()
            Dim dtData As DataTable = GetDataTable(sql, Conn)
            If Conn.State = ConnectionState.Open Then Conn.Close()

            Dim DataInfoItems As New Generic.List(Of ItemBank)

            Dim item As New ItemBank()

            For Each mRow As DataRow In dtData.Select("")

                item = New ItemBank()

                item.id = mRow("id").ToString
                item.code = mRow("code").ToString
                item.bank_name = mRow("bank_name").ToString
             
                DataInfoItems.Add(item)

            Next

            Response.Write(JSON.Serialize(Of Generic.List(Of ItemBank))(DataInfoItems))

        Catch ex As Exception
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
        End Try
        Response.End()

    End Sub

    Private Sub ForgotPassword(ByVal _User As String, ByVal _Nation As String, ByVal _Email As String)

        Dim JSONResponse As New JSONObject()

        Try
            Dim sql As String = "select id from member where username='" & _User & "' and nation_code = '" & _Nation & "'"
            Dim dtCheck As DataTable = GetDataTable(sql, Conn)
            Dim _NewPass As String = ""
            If dtCheck.Rows.Count <> 0 Then
                _NewPass = GeneratePassword()
                sql = "update member set password=@pass where id = @id and username = @user and nation_code = @nation"

                Cmd = New SqlCommand(sql, Conn)

                Cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = dtCheck.Rows(0)("id").ToString
                Cmd.Parameters.Add("@user", SqlDbType.VarChar).Value = _User
                Cmd.Parameters.Add("@pass", SqlDbType.VarChar).Value = MD5(_NewPass)
                Cmd.Parameters.Add("@nation", SqlDbType.VarChar).Value = _Nation

                If Conn.State = ConnectionState.Open Then Conn.Close()
                Conn.Open()

                Cmd.ExecuteNonQuery()
                Conn.Close()

                SelectEmail("Your new password is " & _NewPass, _Email)

                JSONResponse.exMessage = JSON.FixString("")
                JSONResponse.Param = "success"
                Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

            Else

                JSONResponse.exMessage = JSON.FixString("")
                JSONResponse.Param = "error"
                Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

            End If

        Catch ex As Exception

            Conn.Close()
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ex.Message
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

        End Try

        Response.End()

    End Sub

    Private Sub Login(ByVal _User As String, ByVal _Pass As String)

        Dim JSONResponse As New JSONObject()

        Try

            Dim sql As String = " select m.code,m.id,m.username,isnull(xx.active,'N') active,m.status main_status from member m " & _
                                " left join ( " & _
                                " select m2.member_id mem_id, m2.active from member_topup m2 inner join member m1 on m1.id = m2.member_id  " & _
                                " where m1.username = @user and m1.password = @pass and approved_date is not NULL and active='Y' and getdate() < expire_date" & _
                                " ) xx on xx.mem_id = m.id " & _
                                " where m.username = @user and m.password = @pass;"

            Cmd = New SqlCommand(sql, Conn)

            Cmd.Parameters.Add("@user", SqlDbType.VarChar).Value = _User
            Cmd.Parameters.Add("@pass", SqlDbType.VarChar).Value = MD5(_Pass)
            If Conn.State = ConnectionState.Open Then Conn.Close()
            Conn.Open()
            Dim dt As New DataTable
            Dim da As New SqlDataAdapter
            da.SelectCommand = Cmd
            da.Fill(dt)
            da.Dispose()
            Conn.Close()

            Dim LoginItems As New Generic.List(Of ItemLogin)

            Dim item As New ItemLogin()

            For Each mRow As DataRow In dt.Select("")

                item = New ItemLogin()

                item.code = mRow("code").ToString
                item.id = mRow("id").ToString
                item.username = mRow("username").ToString
                item.active = mRow("active").ToString
                item.main_status = mRow("main_status").ToString
                LoginItems.Add(item)

            Next

            Response.Write(JSON.Serialize(Of Generic.List(Of ItemLogin))(LoginItems))

        Catch ex As Exception

            Conn.Close()
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

        End Try

        Response.End()

    End Sub

    Private Sub Register(ByVal _User As String, ByVal _Pass As String, ByVal _Fname As String _
                         , ByVal _Lname As String, ByVal _NationID As String, ByVal _Tel As String _
                         , ByVal _Email As String, ByVal _Acc As String, ByVal _line As String, ByVal _File As String, ByVal _Bank As String)

        Dim JSONResponse As New JSONObject()

        Try
            Dim sql As String = "select id from member where username='" & _User & "'"
            Dim dtCheck As DataTable = GetDataTable(sql, Conn)

            If dtCheck.Rows.Count = 0 Then
                sql = "insert into member(code,username,password,tname,fname,lname,account,line_id,tel,mobile,email,nation_code,img_nation,bank_id) " & _
                            " values('M',@user,@pass,'',@fname,@lname,@account,@line,'',@mobile,@email,@nation,@file,@bank)"

                Cmd = New SqlCommand(sql, Conn)

                Cmd.Parameters.Add("@user", SqlDbType.VarChar).Value = _User
                Cmd.Parameters.Add("@pass", SqlDbType.VarChar).Value = MD5(_Pass)
                Cmd.Parameters.Add("@fname", SqlDbType.VarChar).Value = _Fname
                Cmd.Parameters.Add("@lname", SqlDbType.VarChar).Value = _Lname
                Cmd.Parameters.Add("@mobile", SqlDbType.VarChar).Value = _Tel
                Cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = _Email
                Cmd.Parameters.Add("@nation", SqlDbType.VarChar).Value = _NationID
                Cmd.Parameters.Add("@account", SqlDbType.VarChar).Value = _Acc
                Cmd.Parameters.Add("@line", SqlDbType.VarChar).Value = _line
                Cmd.Parameters.Add("@file", SqlDbType.VarChar).Value = _File
                Cmd.Parameters.Add("@bank", SqlDbType.BigInt).Value = _Bank
                If Conn.State = ConnectionState.Open Then Conn.Close()
                Conn.Open()

                Cmd.ExecuteNonQuery()
                Conn.Close()

                JSONResponse.exMessage = JSON.FixString("")
                JSONResponse.Param = "success"
                Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

            Else

                JSONResponse.exMessage = JSON.FixString("")
                JSONResponse.Param = "duplicate"
                Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

            End If

        Catch ex As Exception

            Conn.Close()
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ex.Message
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

        End Try

        Response.End()

    End Sub

End Class

Public Structure ItemLogin

    Public code As String
    Public id As String
    Public username As String
    Public active As String
    Public main_status As String

End Structure

Public Structure ItemBank

    Public code As String
    Public id As String
    Public bank_name As String

End Structure