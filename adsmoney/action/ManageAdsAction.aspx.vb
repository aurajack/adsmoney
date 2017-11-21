Imports System.IO
Imports System.Data.SqlClient

Public Class ManageAdsAction
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

                Case "adsdata"

                    Call GenData(Request.Form("memid").ToString)

                Case "editdata"

                    Call EditData(Request.Form("memid").ToString, Request.Form("id").ToString)

                Case "save"

                    Call SaveData(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("name").ToString, Request.Form("link").ToString, Request.Form("file").ToString)

                Case "savestatus"

                    Call SaveStatus(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("status").ToString)

            End Select

        End If

    End Sub

    Private Sub GenData(ByVal _MemData As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemData
        Try

            Cookie = CheckExpire(_MemData)

            Dim sql As String = " select ROW_NUMBER() over(order by updated_date desc) rownum, id,updated_date, ads_name,ads_link,ads_path,case when status = 'ACTIVE' then 'ใช้งาน' else 'ยกเลิก' end ads_status from ads"

            If Conn.State = ConnectionState.Closed Then Conn.Open()
            Dim dtData As DataTable = GetDataTable(sql, Conn)
            If Conn.State = ConnectionState.Open Then Conn.Close()

            Dim DataInfoItems As New Generic.List(Of ItemDataAds)

            Dim item As New ItemDataAds()

            For Each mRow As DataRow In dtData.Select("")

                item = New ItemDataAds()

                item.no = mRow("rownum").ToString
                item.id = mRow("id").ToString
                item.updated_date = mRow("updated_date").ToString
                item.ads_name = mRow("ads_name").ToString
                item.ads_link = mRow("ads_link").ToString
                item.ads_path = mRow("ads_path").ToString
                item.status = mRow("ads_status").ToString

                DataInfoItems.Add(item)

            Next

            Response.Write(JSON.Serialize(Of Generic.List(Of ItemDataAds))(DataInfoItems))

        Catch ex As Exception
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
        End Try
        Response.End()

    End Sub

    Private Sub EditData(ByVal _MemData As String, ByVal _ID As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemData
        Try

            Cookie = CheckExpire(_MemData)

            Dim sql As String = " select id,ads_name,ads_link,ads_path from ads where id = " & _ID & ""

            If Conn.State = ConnectionState.Closed Then Conn.Open()
            Dim dtData As DataTable = GetDataTable(sql, Conn)
            If Conn.State = ConnectionState.Open Then Conn.Close()

            Dim DataInfoItems As New Generic.List(Of ItemDataAds)

            Dim item As New ItemDataAds()

            For Each mRow As DataRow In dtData.Select("")

                item = New ItemDataAds()

                item.id = mRow("id").ToString
                item.ads_name = mRow("ads_name").ToString
                item.ads_link = mRow("ads_link").ToString
                item.ads_path = mRow("ads_path").ToString

                DataInfoItems.Add(item)

            Next

            Response.Write(JSON.Serialize(Of Generic.List(Of ItemDataAds))(DataInfoItems))

        Catch ex As Exception
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
        End Try
        Response.End()

    End Sub

    Private Sub SaveData(ByVal _MemID As String, ByVal _ID As String, ByVal _Name As String, ByVal _Link As String, ByVal _File As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemID
        Try

            Cookie = CheckExpire(_MemID)

            Dim sql As String = ""

            If _ID = "" Then
                sql = "insert into ads(ads_name,ads_link,status,ads_path) values(@ads_name,@ads_link,'ACTIVE',@ads_path) "
            Else
                sql = "update ads set ads_name=@ads_name,ads_link=@ads_link,ads_path=@ads_path,updated_date=getdate() where id = " & _ID & " "
            End If

            Cmd = New SqlCommand(sql, Conn)

            Cmd.Parameters.Add("@ads_name", SqlDbType.VarChar).Value = _Name
            Cmd.Parameters.Add("@ads_link", SqlDbType.VarChar).Value = _Link
            Cmd.Parameters.Add("@ads_path", SqlDbType.VarChar).Value = _File

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

    Private Sub SaveStatus(ByVal _MemID As String, ByVal _ID As String, ByVal _Status As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemID
        Try

            Cookie = CheckExpire(_MemID)

            Dim sql As String = ""

            sql = "update ads set status = @status,updated_date = getdate() where id = @id "

            Cmd = New SqlCommand(sql, Conn)

            Cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = _Status
            Cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = _ID

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

Public Structure ItemDataAds

    Public no As String
    Public id As String
    Public updated_date As String
    Public ads_name As String
    Public ads_link As String
    Public ads_path As String
    Public status As String

End Structure