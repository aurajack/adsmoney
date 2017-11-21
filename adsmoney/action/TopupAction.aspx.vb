Imports System.IO
Imports System.Data.SqlClient

Public Class TopupAction
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

                Case "topup"

                    Call Topup(Request.Form("memid").ToString, Request.Form("topup_file").ToString)


                Case "getdata"

                    Call GenData(Request.Form("memid").ToString, Request.Form("role").ToString)

                Case "approve"

                    Call Approve(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("status").ToString, Request.Form("expire").ToString)

                Case "approvemain"

                    Call ApproveMain(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("status").ToString)

            End Select

        End If

    End Sub

    Private Sub Topup(ByVal _MemID As String, ByVal _Topupfile As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemID
        Dim sql As String = ""
        Try

            Cookie = CheckExpire(_MemID)
            'If Cookie <> "" Then
            '    If (Cookie.Split("|")(3) = "Y") Then
            'sql  = "select id from member_topup where true_no='" & _Topupfile & "'"
            'Conn.Open()
            'Dim dtCheck As DataTable = GetDataTable(sql, Conn)
            'Conn.Close()

            'If dtCheck.Rows.Count = 0 Then
            sql = "insert into member_topup(member_id,path_file,true_no) " & _
                        " values(@memid,@path_file,'')"

            Cmd = New SqlCommand(sql, Conn)

            Cmd.Parameters.Add("@memid", SqlDbType.BigInt).Value = _MemID.Split("|")(1)
            Cmd.Parameters.Add("@path_file", SqlDbType.VarChar).Value = _Topupfile

            Conn.Open()

            Cmd.ExecuteNonQuery()
            Conn.Close()

            JSONResponse.exMessage = JSON.FixString("")
            JSONResponse.Param = "success"
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
            '    Else
            'JSONResponse.exMessage = ""
            'JSONResponse.Param = "expire"
            'JSONResponse.Cookie = Cookie
            'Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
            '    End If
            'Else
            'JSONResponse.exMessage = ""
            'JSONResponse.Param = "expire"
            'JSONResponse.Cookie = Cookie
            'Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
            'End If

            'Else

            'JSONResponse.exMessage = JSON.FixString("")
            'JSONResponse.Param = "duplicate"
            'JSONResponse.Cookie = Cookie
            'Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

            'End If

        Catch ex As Exception

            Conn.Close()
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

        End Try

        Response.End()

    End Sub

    Private Sub Approve(ByVal _MemID As String, ByVal _ID As String, ByVal _Status As String, ByVal _Expire As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemID
        Try

            Cookie = CheckExpire(_MemID)

            Dim sql As String = ""

            If _Status = "APPROVE" Then
                sql = "update member_topup set status = @status,expire_date = DATEADD(day," & _Expire & ",getdate()), active='Y', approved_date=getdate() where id = @id "
            Else
                sql = "update member_topup set status = @status,expire_date = NULL, active='N',approved_date=NULL where id = @id "
            End If


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

    Private Sub ApproveMain(ByVal _MemID As String, ByVal _ID As String, ByVal _Status As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemID
        Try

            Cookie = CheckExpire(_MemID)

            Dim sql As String = ""

            sql = "update member set status = @status where id = @id "

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

    Private Sub GenData(ByVal _MemID As String, ByVal _Role As String)

        Dim JSONResponse As New JSONObject()

        Try

            Dim sql As String = " select ROW_NUMBER() over(order by mt.id desc) rownum,mt.id,mt.member_id,m.fname + ' ' + m.lname name, m.mobile,convert(varchar(19),mt.created_date,121) created_date,mt.true_no " & _
                         " ,case when mt.status = 'WAITING' then 'รอดำเนินการ' when mt.status = 'APPROVE' then 'อนุมัติ' else 'ยกเลิก' end status " & _
                         " ,case when mt.expire_date is NULL then '-' else convert(varchar(19),mt.expire_date,121) end expire_date, case when isnull(mt.path_file,'') = '' then 'icon_no.png' else mt.path_file end path_file, case when isnull(m.img_nation,'') = '' then 'icon_no.png' else m.img_nation end img_nation,case when m.status = 'Y' then 'ใช้งาน' else 'ยกเลิก' end main_status  " & _
                         " from member_topup mt  " & _
                         " inner join member m on m.id = mt.member_id " & _
                         " where m.id = '" & _MemID & "'"

            If _Role = "AD" Then
                sql = " select ROW_NUMBER() over(order by mt.id desc) rownum,mt.id,mt.member_id,m.fname + ' ' + m.lname name, m.mobile,convert(varchar(19),mt.created_date,121) created_date,mt.true_no " & _
                         " ,case when mt.status = 'WAITING' then 'รอดำเนินการ' when mt.status = 'APPROVE' then 'อนุมัติ' else 'ยกเลิก' end status " & _
                         " ,case when mt.expire_date is NULL then '-' else convert(varchar(19),mt.expire_date,121) end expire_date, case when isnull(mt.path_file,'') = '' then 'icon_no.png' else mt.path_file end path_file, case when isnull(m.img_nation,'') = '' then 'icon_no.png' else m.img_nation end img_nation,case when m.status = 'Y' then 'ใช้งาน' else 'ยกเลิก' end main_status " & _
                         " from member_topup mt  " & _
                         " inner join member m on m.id = mt.member_id " & _
                         " right join ( " & _
                         "    select max(id) max_id from member_topup group by member_id " & _
                         " )xx on mt.id = xx.max_id " & _
                         " where m.code <> 'AD'"

            End If

            If Conn.State = ConnectionState.Closed Then Conn.Open()
            Dim dtData As DataTable = GetDataTable(sql, Conn)
            If Conn.State = ConnectionState.Open Then Conn.Close()

            Dim DataInfoItems As New Generic.List(Of ItemData)

            Dim item As New ItemData()

            For Each mRow As DataRow In dtData.Select("")

                item = New ItemData()

                item.no = mRow("rownum").ToString
                item.id = mRow("id").ToString
                item.member_id = mRow("member_id").ToString
                item.name = mRow("name").ToString
                item.mobile = mRow("mobile").ToString
                item.created_date = mRow("created_date").ToString
                item.topup_no = mRow("true_no").ToString
                item.status = mRow("status").ToString
                item.expire_date = mRow("expire_date").ToString
                item.path_file = mRow("path_file").ToString
                item.img_nation = mRow("img_nation").ToString
                item.main_status = mRow("main_status").ToString
                DataInfoItems.Add(item)

            Next

            Response.Write(JSON.Serialize(Of Generic.List(Of ItemData))(DataInfoItems))

        Catch ex As Exception
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = ""
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
        End Try
        Response.End()

    End Sub

End Class

Public Structure ItemData

    Public no As String
    Public id As String
    Public name As String
    Public mobile As String
    Public created_date As String
    Public topup_no As String
    Public status As String
    Public expire_date As String
    Public path_file As String
    Public img_nation As String
    Public main_status As String
    Public member_id As String

End Structure