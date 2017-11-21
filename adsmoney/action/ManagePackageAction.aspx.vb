Imports System.IO
Imports System.Data.SqlClient

Public Class ManagePackageAction
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

                Case "packagedata"

                    Call GenData(Request.Form("memid").ToString)

                Case "editdata"

                    Call EditData(Request.Form("memid").ToString, Request.Form("id").ToString)

                Case "save"

                    Call SaveData(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("name").ToString, Request.Form("amount"), Request.Form("click"), Request.Form("result"), Request.Form("expire"))

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

            Dim sql As String = " select ROW_NUMBER() over(order by updated_date desc) rownum, id,updated_date, pack_name,pack_amount,click_amount,result_amount,case when status = 'ACTIVE' then 'ใช้งาน' else 'ยกเลิก' end pack_status,expire_day from package"

            If Conn.State = ConnectionState.Closed Then Conn.Open()
            Dim dtData As DataTable = GetDataTable(sql, Conn)
            If Conn.State = ConnectionState.Open Then Conn.Close()

            Dim DataInfoItems As New Generic.List(Of ItemDataPackage)

            Dim item As New ItemDataPackage()

            For Each mRow As DataRow In dtData.Select("")

                item = New ItemDataPackage()

                item.no = mRow("rownum").ToString
                item.id = mRow("id").ToString
                item.updated_date = mRow("updated_date").ToString
                item.pack_name = mRow("pack_name").ToString
                item.pack_amount = mRow("pack_amount")
                item.click_amount = mRow("click_amount")
                item.result_amount = mRow("result_amount")
                item.status = mRow("pack_status").ToString
                item.expire_day = mRow("expire_day").ToString

                DataInfoItems.Add(item)

            Next

            Response.Write(JSON.Serialize(Of Generic.List(Of ItemDataPackage))(DataInfoItems))

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

            Dim sql As String = " select id,pack_name,pack_amount,click_amount,result_amount,expire_day from package where id = " & _ID & ""

            If Conn.State = ConnectionState.Closed Then Conn.Open()
            Dim dtData As DataTable = GetDataTable(sql, Conn)
            If Conn.State = ConnectionState.Open Then Conn.Close()

            Dim DataInfoItems As New Generic.List(Of ItemDataPackage)

            Dim item As New ItemDataPackage()

            For Each mRow As DataRow In dtData.Select("")

                item = New ItemDataPackage()

                item.id = mRow("id").ToString
                item.pack_name = mRow("pack_name").ToString
                item.pack_amount = mRow("pack_amount")
                item.click_amount = mRow("click_amount")
                item.result_amount = mRow("result_amount")
                item.expire_day = mRow("expire_day")
               
                DataInfoItems.Add(item)

            Next

            Response.Write(JSON.Serialize(Of Generic.List(Of ItemDataPackage))(DataInfoItems))

        Catch ex As Exception
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
        End Try
        Response.End()

    End Sub

    Private Sub SaveData(ByVal _MemID As String, ByVal _ID As String, ByVal _Name As String, ByVal _Amount As Decimal, ByVal _Click As Integer, ByVal _Result As Decimal, ByVal _Expire As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemID
        Try

            Cookie = CheckExpire(_MemID)

            Dim sql As String = ""

            If _ID = "" Then
                sql = "insert into package(pack_name,pack_amount,click_amount,result_amount,status,expire_day) values(@pack_name,@pack_amount,@click_amount,@result_amount,'ACTIVE',@expire) "
            Else
                sql = "update package set pack_name=@pack_name,pack_amount=@pack_amount,click_amount=@click_amount,result_amount=@result_amount,updated_date=getdate(),expire_day=@expire where id = " & _ID & " "
            End If

            Cmd = New SqlCommand(sql, Conn)

            Cmd.Parameters.Add("@pack_name", SqlDbType.VarChar).Value = _Name
            Cmd.Parameters.Add("@pack_amount", SqlDbType.Decimal).Value = _Amount
            Cmd.Parameters.Add("@click_amount", SqlDbType.Int).Value = _Click
            Cmd.Parameters.Add("@result_amount", SqlDbType.Decimal).Value = _Result
            Cmd.Parameters.Add("@expire", SqlDbType.VarChar).Value = _Expire

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

            sql = "update package set status = @status,updated_date = getdate() where id = @id "

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

Public Structure ItemDataPackage

    Public no As String
    Public id As String
    Public updated_date As String
    Public pack_name As String
    Public pack_amount As Decimal
    Public click_amount As Integer
    Public result_amount As Decimal
    Public status As String
    Public expire_day As String

End Structure