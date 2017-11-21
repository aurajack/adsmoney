Imports System.IO
Imports System.Data.SqlClient

Public Class TransferAction
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

                Case "transferdata"

                    Call GenData(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("role").ToString)

                Case "editdata"

                    Call EditData(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("idre").ToString)

                Case "save"

                    Call SaveData(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("idre").ToString, Request.Form("file").ToString)

                Case "saveapprove"

                    Call SaveApprove(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("mempackid").ToString)

                Case "saveextract"

                    Call SaveExtract(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("mempackid").ToString)

            End Select

        End If

    End Sub

    Private Sub GenData(ByVal _MemData As String, ByVal _MemID As String, ByVal _Role As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemData
        Try

            Cookie = CheckExpire(_MemData)
            Dim sql As String = ""
            If Cookie <> "" Then
                If (Cookie.Split("|")(3) = "Y") Then

                    If _Role = "AD" Then
                        sql = "select ROW_NUMBER() over(order by show desc,system_date desc) rownum,* from (  select r.id,r.system_date" & _
                        " ,r.mem_pack_id,r.amount,m.fname + ' '+ m.lname dep_name,m.id dep_id,m.account dep_acc,m.mobile dep_mobile" & _
                        " ,m2.fname + ' '+ m2.lname ex_name,m2.id ex_id,m2.account ex_acc,m2.mobile ex_mobile,r.status" & _
                        " ,case when r.mem_deposit_id is NULL then 'โอนให้' when r.mem_deposit_id = " & _MemID & "  and r.mem_extract_id is not null then 'โอนให้' else 'ได้รับจาก' end job, r.img_slip, 'Y' show " & _
                        " from relation r" & _
                        " left join member m on r.mem_deposit_id = m.id" & _
                        " left join member m2 on r.mem_extract_id = m2.id" & _
                        " where r.status = 'N' and ((r.mem_deposit_id is NULL or r.mem_extract_id is NULL) or (r.mem_deposit_id = " & _MemID & " or r.mem_extract_id = " & _MemID & ")) union all " & _
                        " select r.id,r.system_date" & _
                        " ,r.mem_pack_id,r.amount,m.fname + ' '+ m.lname dep_name,m.id dep_id,m.account dep_acc,m.mobile dep_mobile" & _
                        " ,m2.fname + ' '+ m2.lname ex_name,m2.id ex_id,m2.account ex_acc,m2.mobile ex_mobile,r.status" & _
                        " ,case when r.mem_deposit_id is NULL then 'โอนให้' when r.mem_deposit_id = " & _MemID & "  and r.mem_extract_id is not null then 'โอนให้' else 'ได้รับจาก' end job, r.img_slip, 'N' show " & _
                        " from relation r" & _
                        " left join member m on r.mem_deposit_id = m.id" & _
                        " left join member m2 on r.mem_extract_id = m2.id" & _
                        " where r.status = 'Y' and ((r.mem_deposit_id is NULL or r.mem_extract_id is NULL) or (r.mem_deposit_id = " & _MemID & " or r.mem_extract_id = " & _MemID & "))  and r.updated_date >= DATEADD(DAY,-7,getdate()) )xx"
                    Else
                        sql = " select ROW_NUMBER() over(order by show desc,system_date desc) rownum,* from ( select r.id,r.system_date" & _
                        " ,r.mem_pack_id,r.amount,m.fname + ' '+ m.lname dep_name,m.id dep_id,m.account dep_acc,m.mobile dep_mobile" & _
                        " ,m2.fname + ' '+ m2.lname ex_name,m2.id ex_id,m2.account ex_acc,m2.mobile ex_mobile,r.status" & _
                        " ,case when r.mem_deposit_id = " & _MemID & " then 'โอนให้'  else 'ได้รับจาก' end job, r.img_slip, 'Y' show " & _
                        " from relation r" & _
                        " left join member m on r.mem_deposit_id = m.id" & _
                        " left join member m2 on r.mem_extract_id = m2.id" & _
                        " where r.status = 'N' and (r.mem_deposit_id = " & _MemID & " or r.mem_extract_id = " & _MemID & ") union all" & _
                        " select r.id,r.system_date,r.mem_pack_id,r.amount,m.fname + ' '+ m.lname dep_name,m.id dep_id,m.account dep_acc,m.mobile dep_mobile" & _
                        " ,m2.fname + ' '+ m2.lname ex_name,m2.id ex_id,m2.account ex_acc,m2.mobile ex_mobile,r.status" & _
                        " ,case when r.mem_deposit_id = " & _MemID & " then 'โอนให้'  else 'ได้รับจาก' end job, r.img_slip , 'N' show " & _
                        " from relation r" & _
                        " left join member m on r.mem_deposit_id = m.id" & _
                        " left join member m2 on r.mem_extract_id = m2.id" & _
                        " where r.status = 'Y' and (r.mem_deposit_id = " & _MemID & " or r.mem_extract_id = " & _MemID & ") and r.updated_date >= DATEADD(DAY,-7,getdate()) ) xx"
                    End If

                    If Conn.State = ConnectionState.Closed Then Conn.Open()
                    Dim dtData As DataTable = GetDataTable(sql, Conn)
                    If Conn.State = ConnectionState.Open Then Conn.Close()

                    Dim DataInfoItems As New Generic.List(Of ItemDataTransfer)

                    Dim item As New ItemDataTransfer()

                    For Each mRow As DataRow In dtData.Select("")

                        item = New ItemDataTransfer()

                        item.no = mRow("rownum").ToString
                        item.id = mRow("id").ToString
                        item.system_date = mRow("system_date").ToString
                        item.mem_pack_id = mRow("mem_pack_id").ToString
                        item.amount = mRow("amount").ToString
                        item.dep_name = mRow("dep_name").ToString
                        item.dep_id = mRow("dep_id").ToString
                        item.dep_acc = mRow("dep_acc").ToString
                        item.dep_mobile = mRow("dep_mobile").ToString
                        item.ex_name = mRow("ex_name").ToString
                        item.ex_id = mRow("ex_id").ToString
                        item.ex_acc = mRow("ex_acc").ToString
                        item.ex_mobile = mRow("ex_mobile").ToString
                        item.status = mRow("status").ToString
                        item.job = mRow("job").ToString
                        item.img_slip = mRow("img_slip").ToString
                        item.show = mRow("show").ToString

                        DataInfoItems.Add(item)

                    Next

                    Response.Write(JSON.Serialize(Of Generic.List(Of ItemDataTransfer))(DataInfoItems))
                Else
                    JSONResponse.exMessage = JSONResponse.exMessage = ""
                    JSONResponse.Param = "expire"
                    JSONResponse.Cookie = Cookie
                    Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
                End If

            Else

                JSONResponse.exMessage = JSONResponse.exMessage = ""
                JSONResponse.Param = "expire"
                JSONResponse.Cookie = Cookie
                Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
            End If

        Catch ex As Exception
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
        End Try
        Response.End()

    End Sub

    Private Sub EditData(ByVal _MemData As String, ByVal _ID As String, ByVal _ReID As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemData
        Try

            Cookie = CheckExpire(_MemData)

            If Cookie <> "" Then
                If (Cookie.Split("|")(3) = "Y") Then

                    Dim sql As String = " select id,img_slip from relation where id = " & _ReID & ""

                    If Conn.State = ConnectionState.Closed Then Conn.Open()
                    Dim dtDataRe As DataTable = GetDataTable(sql, Conn)
                    If Conn.State = ConnectionState.Open Then Conn.Close()
                    Dim imgSlip As String = ""

                    For Each mRow As DataRow In dtDataRe.Select("")

                        imgSlip = mRow("img_slip").ToString

                    Next

                    sql = "  select m.id,m.fname + ' ' + m.lname name,b.bank_name +' '+ m.account account,m.mobile from member m inner join bank b on m.bank_id = b.id where m.id = " & _ID & ""

                    If Conn.State = ConnectionState.Closed Then Conn.Open()
                    Dim dtData As DataTable = GetDataTable(sql, Conn)
                    If Conn.State = ConnectionState.Open Then Conn.Close()

                    Dim DataInfoItems As New Generic.List(Of ItemDataTransferEdit)

                    Dim item As New ItemDataTransferEdit()

                    For Each mRow As DataRow In dtData.Select("")

                        item = New ItemDataTransferEdit()

                        item.id = mRow("id").ToString
                        item.name = mRow("name").ToString
                        item.acc = mRow("account").ToString
                        item.mobile = mRow("mobile").ToString
                        item.img_slip = imgSlip
                        item.idre = _ReID
                        DataInfoItems.Add(item)

                    Next

                    Response.Write(JSON.Serialize(Of Generic.List(Of ItemDataTransferEdit))(DataInfoItems))

                Else
                    JSONResponse.exMessage = JSONResponse.exMessage = ""
                    JSONResponse.Param = "expire"
                    JSONResponse.Cookie = Cookie
                    Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
                End If
            Else
                JSONResponse.exMessage = JSONResponse.exMessage = ""
                JSONResponse.Param = "expire"
                JSONResponse.Cookie = Cookie
                Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
            End If

        Catch ex As Exception
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
        End Try
        Response.End()

    End Sub

    Private Sub SaveData(ByVal _MemID As String, ByVal _ID As String, ByVal _IDre As String, ByVal _File As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemID
        Try

            Cookie = CheckExpire(_MemID)
            If Cookie <> "" Then
                If (Cookie.Split("|")(3) = "Y") Then

                    Dim sql As String = ""
                    If Cookie.Split("|")(0) = "AD" Then
                        sql = "update relation set img_slip=@img_slip,updated_date=getdate(),mem_deposit_id='" & Cookie.Split("|")(1) & "' where id = @id "
                    Else
                        sql = "update relation set img_slip=@img_slip,updated_date=getdate() where id = @id "
                    End If

                    Cmd = New SqlCommand(sql, Conn)
                    Cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = _IDre
                    Cmd.Parameters.Add("@img_slip", SqlDbType.VarChar).Value = _File

                    Conn.Open()
                    Cmd.ExecuteNonQuery()
                    Conn.Close()

                    JSONResponse.exMessage = JSON.FixString("")
                    JSONResponse.Param = "success"
                    JSONResponse.Cookie = Cookie
                    Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

                Else
                    JSONResponse.exMessage = JSONResponse.exMessage = ""
                    JSONResponse.Param = "expire"
                    JSONResponse.Cookie = Cookie
                    Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
                End If

            Else
                JSONResponse.exMessage = JSONResponse.exMessage = ""
                JSONResponse.Param = "expire"
                JSONResponse.Cookie = Cookie
                Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
            End If

        Catch ex As Exception

            Conn.Close()
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

        End Try

        Response.End()

    End Sub

    Private Sub SaveApprove(ByVal _MemID As String, ByVal _ID As String, ByVal _MemPackID As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemID
        Try

            Cookie = CheckExpire(_MemID)

            Dim sql As String = ""
            If Cookie <> "" Then
                If (Cookie.Split("|")(3) = "Y") Then

                    sql = " select pack_amount,package_id,expire_day from member_package where id = " & _MemPackID & ""
                    If Conn.State = ConnectionState.Closed Then Conn.Open()
                    Dim dtDataMemPack As DataTable = GetDataTable(sql, Conn)
                    If Conn.State = ConnectionState.Open Then Conn.Close()

                    Dim packAmount As Decimal = dtDataMemPack.Rows(0)("pack_amount")
                    Dim packageID As String = dtDataMemPack.Rows(0)("package_id").ToString
                    Dim ExpirePackage As String = dtDataMemPack.Rows(0)("expire_day").ToString

                    Try
                        Conn.Open()
                        tx = Conn.BeginTransaction

                        If (Cookie.Split("|")(0) = "AD") Then
                            sql = "update relation set status = 'Y',updated_date = getdate() where id = " & _ID & " and mem_extract_id = " & Cookie.Split("|")(1) & ""
                        Else
                            sql = "update relation set status = 'Y',updated_date = getdate() where id = " & _ID & ""
                        End If

                        Cmd = New SqlCommand(sql, Conn, tx)
                        Cmd.ExecuteNonQuery()

                        sql = " select mem_deposit_id from relation where id = '" & _ID & "'"

                        Dim dtCheckMemID As DataTable = GetDataTableTx(sql, Conn, tx)
                        Dim _MemDepID As String = ""
                        If dtCheckMemID.Rows.Count <> 0 Then
                            _MemDepID = dtCheckMemID.Rows(0)("mem_deposit_id").ToString
                        End If

                        sql = "insert into book_transection(description,amount,member_id,package_id) values('โอนเงิน','1000',@memid,@packid) "
                        Cmd = New SqlCommand(sql, Conn, tx)
                        Cmd.Parameters.Add("@memid", SqlDbType.BigInt).Value = _MemDepID
                        Cmd.Parameters.Add("@packid", SqlDbType.BigInt).Value = packageID
                        Cmd.ExecuteNonQuery()

                        sql = " select sum(amount) amount from relation where mem_pack_id = " & _MemPackID & " and status = 'Y'"
                        'If Conn.State = ConnectionState.Closed Then Conn.Open()
                        Dim dtDataAmount As DataTable = GetDataTableTx(sql, Conn, tx)
                        'If Conn.State = ConnectionState.Open Then Conn.Close()

                        Dim approveAmount As Decimal = dtDataAmount.Rows(0)("amount")

                        If approveAmount = packAmount Then
                            sql = "update member_package set status = 'Y',expire_date =  DATEADD(day," & ExpirePackage & ",getdate()) where id = " & _MemPackID & ""
                            Cmd = New SqlCommand(sql, Conn, tx)
                            Cmd.ExecuteNonQuery()
                        End If

                        tx.Commit()
                        Conn.Close()

                        JSONResponse.exMessage = JSON.FixString("")
                        JSONResponse.Param = "success"
                        JSONResponse.Cookie = Cookie
                        Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

                    Catch ex As Exception
                        tx.Rollback()
                        Conn.Close()
                        JSONResponse.exMessage = JSON.FixString(ex.Message)
                        JSONResponse.Param = ""
                        JSONResponse.Cookie = Cookie
                        Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
                    End Try

                Else
                    JSONResponse.exMessage = ""
                    JSONResponse.Param = "expire"
                    JSONResponse.Cookie = Cookie
                    Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
                End If

            Else
                JSONResponse.exMessage = ""
                JSONResponse.Param = "expire"
                JSONResponse.Cookie = Cookie
                Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
            End If

        Catch ex As Exception

            Conn.Close()
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

        End Try

        Response.End()

    End Sub

    Private Sub SaveExtract(ByVal _MemID As String, ByVal _ID As String, ByVal _MemPackID As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemID
        Try

            Cookie = CheckExpire(_MemID)

            Dim sql As String = ""

            sql = "update relation set mem_extract_id = '" & Cookie.Split("|")(1) & "',updated_date = getdate() where id = " & _ID & ""

            Cmd = New SqlCommand(sql, Conn)
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

Public Structure ItemDataTransfer

    Public no As String
    Public id As String
    Public system_date As String
    Public mem_pack_id As String
    Public amount As String
    Public dep_name As String
    Public dep_id As String
    Public dep_acc As String
    Public dep_mobile As String
    Public ex_name As String
    Public ex_id As String
    Public ex_acc As String
    Public ex_mobile As String
    Public status As String
    Public job As String
    Public img_slip As String
    Public show As String

End Structure

Public Structure ItemDataTransferEdit

    Public id As String
    Public name As String
    Public acc As String
    Public mobile As String
    Public img_slip As String
    Public idre As String

End Structure