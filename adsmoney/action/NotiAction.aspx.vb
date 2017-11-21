Imports System.IO
Imports System.Data.SqlClient

Public Class NotiAction
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

                Case "checknoti"

                    Call GenData(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("role").ToString)

            End Select

        End If

    End Sub

    Private Sub GenData(ByVal _MemData As String, ByVal _MemID As String, ByVal _Role As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemData
        Try

            Cookie = CheckExpire(_MemData)
            Dim _Remark As String = "0"
            Dim sql As String = ""
            If _Role = "AD" Then
                sql = " select count(mt.id) id from member_topup mt  inner join member m on m.id = mt.member_id " & _
                       " right join ( select max(id) max_id from member_topup group by member_id )xx on mt.id = xx.max_id  " & _
                       " where m.code <> 'AD' and mt.status = 'WAITING'"

                If Conn.State = ConnectionState.Closed Then Conn.Open()
                Dim dtData As DataTable = GetDataTable(sql, Conn)
                If Conn.State = ConnectionState.Open Then Conn.Close()
                If dtData.Rows.Count <> 0 Then
                    _Remark += "|" & dtData.Rows(0)("id") & ""
                End If

                sql = "  select count(id) id from relation where status = 'N' and (mem_extract_id = '" & _MemID & "' or mem_deposit_id='" & _MemID & "') "

                If Conn.State = ConnectionState.Closed Then Conn.Open()
                Dim dtData2 As DataTable = GetDataTable(sql, Conn)
                If Conn.State = ConnectionState.Open Then Conn.Close()

                If dtData2.Rows.Count <> 0 Then
                    _Remark += "|" & dtData2.Rows(0)("id") & ""
                End If

            Else
                If Cookie <> "" Then
                    If (Cookie.Split("|")(3) = "N") Then
                        _Remark = "1"
                    End If
                Else
                    _Remark = "1"
                End If

                sql = " select id from member_package where member_id = '" & _MemID & "' and archive ='N'"

                If Conn.State = ConnectionState.Closed Then Conn.Open()
                Dim dtData As DataTable = GetDataTable(sql, Conn)
                If Conn.State = ConnectionState.Open Then Conn.Close()

                If dtData.Rows.Count = 0 Then
                    _Remark += "|1"
                Else
                    sql = " select count(id) id from member_package where member_id = '" & _MemID & "' and (status = 'N' or getdate() > expire_date) and archive ='N'"

                    If Conn.State = ConnectionState.Closed Then Conn.Open()
                    Dim dtData_2 As DataTable = GetDataTable(sql, Conn)
                    If Conn.State = ConnectionState.Open Then Conn.Close()

                    If dtData_2.Rows.Count <> 0 Then
                        _Remark += "|" & dtData_2.Rows(0)("id") & ""
                    End If
                End If

                'sql = "  select count(id) id from relation where (mem_deposit_id is NULL and mem_extract_id = '" & _MemID & "') or (mem_extract_id is NULL and mem_deposit_id='" & _MemID & "') and status = 'N' "
                sql = "  select count(id) id from relation where status = 'N' and (mem_extract_id = '" & _MemID & "' or mem_deposit_id='" & _MemID & "') "

                If Conn.State = ConnectionState.Closed Then Conn.Open()
                Dim dtData2 As DataTable = GetDataTable(sql, Conn)
                If Conn.State = ConnectionState.Open Then Conn.Close()

                If dtData2.Rows.Count <> 0 Then
                    _Remark += "|" & dtData2.Rows(0)("id") & ""
                End If
            End If

            JSONResponse.exMessage = ""
            JSONResponse.Param = _Remark
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
        Catch ex As Exception
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
        End Try
        Response.End()

    End Sub

End Class
