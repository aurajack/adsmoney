Imports System.IO
Imports System.Data.SqlClient

Public Class PackageAction
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

                    Call DataPackage(Request.Form("memid").ToString)

                Case "getdata"

                    Call GetData(Request.Form("memid").ToString, Request.Form("id").ToString)

                Case "savepackage"

                    Call SavePackage(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("pack_id").ToString)

                Case "delete"

                    Call DeletePackage(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("packid").ToString)

            End Select

        End If

    End Sub

    Private Sub SavePackage(ByVal _MemData As String, ByVal _MemID As String, ByVal _PackID As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemData
        Dim sql As String = ""
        Try

            Cookie = CheckExpire(_MemData)
            If Cookie <> "" Then
                If (Cookie.Split("|")(3) = "Y") Then

                    sql = "select id,pack_amount,click_amount,result_amount,expire_day,(result_amount/expire_day)/click_amount amount_per_click from package where id='" & _PackID & "'"
                    Conn.Open()
                    Dim dtCheck As DataTable = GetDataTable(sql, Conn)
                    Conn.Close()

                    If dtCheck.Rows.Count <> 0 Then

                        sql = "select count(id) id from member_package where member_id='" & _MemID & "' and status = 'Y' and getdate() < expire_date and archive = 'N'"
                        Conn.Open()
                        Dim dtCheckDup As DataTable = GetDataTable(sql, Conn)
                        Conn.Close()

                        If dtCheckDup.Rows(0)("ID") = 0 Then
                            Try
                                Conn.Open()
                                tx = Conn.BeginTransaction

                                sql = "update member_package set archive = 'Y' where member_id='" & _MemID & "' and package_id='" & _PackID & "' and status='Y' and archive='N'"
                                Cmd = New SqlCommand(sql, Conn, tx)
                                Cmd.ExecuteNonQuery()

                                sql = "insert into member_package(package_id,member_id,pack_amount,pack_click,pack_result,amount_per_click,expire_day) " & _
                                                    " values(@packid,@memid,@pack_amount,@pack_click,@pack_result,@amount_per_click,@expire_day) select @@IDENTITY id  from member_package"

                                Cmd = New SqlCommand(sql, Conn, tx)

                                Cmd.Parameters.Add("@memid", SqlDbType.BigInt).Value = _MemID
                                Cmd.Parameters.Add("@packid", SqlDbType.BigInt).Value = _PackID
                                Cmd.Parameters.Add("@pack_amount", SqlDbType.Decimal).Value = dtCheck.Rows(0)("pack_amount")
                                Cmd.Parameters.Add("@pack_click", SqlDbType.Int).Value = dtCheck.Rows(0)("click_amount")
                                Cmd.Parameters.Add("@pack_result", SqlDbType.Decimal).Value = dtCheck.Rows(0)("result_amount")
                                Cmd.Parameters.Add("@amount_per_click", SqlDbType.Decimal).Value = dtCheck.Rows(0)("amount_per_click")
                                Cmd.Parameters.Add("@expire_day", SqlDbType.Decimal).Value = dtCheck.Rows(0)("expire_day")

                                'Conn.Open()
                                Dim _MemPackID As String = Cmd.ExecuteScalar()
                                'Conn.Close()

                                sql = " select top 1 id from relation where mem_extract_id is not NULL and status = 'N' " & _
                                      " and mem_deposit_id is NULL order by system_date asc"

                                Dim dtCheckRelation As DataTable = GetDataTableTx(sql, Conn, tx)

                                If dtCheckRelation.Rows.Count <> 0 AndAlso dtCheckRelation.Rows(0)("id") <> 0 Then
                                    sql = "update relation set mem_deposit_id = '" & _MemID & "', updated_date=getdate(),mem_pack_id = '" & _MemPackID & "' where id = " & dtCheckRelation.Rows(0)("id") & ""
                                    Cmd = New SqlCommand(sql, Conn, tx)
                                    Cmd.ExecuteNonQuery()
                                Else

                                    For i As Integer = 0 To (dtCheck.Rows(0)("pack_amount") / 1000) - 1
                                        sql = "insert into relation(mem_deposit_id,amount,mem_pack_id) values('" & _MemID & "','1000','" & _MemPackID & "')"
                                        Cmd = New SqlCommand(sql, Conn, tx)
                                        Cmd.ExecuteNonQuery()
                                    Next

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

    Private Sub Approve(ByVal _MemID As String, ByVal _ID As String, ByVal _Status As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemID
        Try

            Cookie = CheckExpire(_MemID)

            Dim sql As String = ""

            If _Status = "APPROVE" Then
                sql = "update member_topup set status = @status,expire_date = DATEADD(day,5,getdate()), active='Y', approved_date=getdate() where id = @id "
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

    Private Sub DataPackage(ByVal _MemID As String)

        Dim JSONResponse As New JSONObject()

        Try

            Dim sql As String = "  select id,pack_name,pack_amount,click_amount,result_amount ,expire_day" & _
                                " ,convert(decimal(18,2),(result_amount / expire_day) / click_amount) per_click_amout" & _
                                " from package where status = 'ACTIVE' order by pack_amount asc"

            If Conn.State = ConnectionState.Closed Then Conn.Open()
            Dim dtData As DataTable = GetDataTable(sql, Conn)
            If Conn.State = ConnectionState.Open Then Conn.Close()

            Dim DataInfoItems As New Generic.List(Of ItemDataFontPackage)

            Dim item As New ItemDataFontPackage()

            For Each mRow As DataRow In dtData.Select("")

                item = New ItemDataFontPackage()

                item.id = mRow("id").ToString
                item.pack_name = mRow("pack_name").ToString + " (" & mRow("pack_amount").ToString & "/" & mRow("result_amount").ToString & ") จำนวนคลิกสูงสุด " & mRow("click_amount") & " ครั้ง/วัน"
                item.pack_amount = mRow("pack_amount")
                item.click_amount = mRow("click_amount")
                item.result_amount = mRow("result_amount")
                item.expire_day = mRow("expire_day")
                item.per_click_amout = mRow("per_click_amout")
              
                DataInfoItems.Add(item)

            Next

            Response.Write(JSON.Serialize(Of Generic.List(Of ItemDataFontPackage))(DataInfoItems))

        Catch ex As Exception
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = ""
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
        End Try
        Response.End()

    End Sub

    Private Sub GetData(ByVal _MemID As String, ByVal _ID As String)

        Dim JSONResponse As New JSONObject()

        Try

            Dim sql As String = " select ROW_NUMBER() over(order by mp.id desc) rownum, mp.id,mp.system_date,mp.expire_date,p.pack_name,p.pack_amount,case when mp.status = 'Y'  and mp.archive = 'N' then 'ใช้งาน' when mp.status ='Y' and mp.archive = 'Y' then 'หมดอายุ' else 'รอการชำระเงิน' end pack_status" & _
                                  " ,(amount_per_click * current_click ) total_amount" & _
                                  " from member_package mp inner join package p on p.id = mp.package_id" & _
                                  " where member_id = " & _ID & ""

            If Conn.State = ConnectionState.Closed Then Conn.Open()
            Dim dtData As DataTable = GetDataTable(sql, Conn)
            If Conn.State = ConnectionState.Open Then Conn.Close()

            Dim DataInfoItems As New Generic.List(Of ItemDataListPackage)

            Dim item As New ItemDataListPackage()

            For Each mRow As DataRow In dtData.Select("")

                item = New ItemDataListPackage()

                item.no = mRow("rownum").ToString
                item.id = mRow("id").ToString
                item.pack_name = mRow("pack_name").ToString
                item.pack_amount = mRow("pack_amount")
                item.system_date = mRow("system_date").ToString
                item.expire_date = mRow("expire_date").ToString
                item.pack_status = mRow("pack_status").ToString
                item.total_amount = mRow("total_amount")

                DataInfoItems.Add(item)

            Next

            Response.Write(JSON.Serialize(Of Generic.List(Of ItemDataListPackage))(DataInfoItems))

        Catch ex As Exception
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = ""
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
        End Try
        Response.End()

    End Sub

    Private Sub DeletePackage(ByVal _MemData As String, ByVal _MemID As String, ByVal _PackID As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemData
        Dim sql As String = ""
        Try

            Cookie = CheckExpire(_MemData)
            If Cookie <> "" Then
                If (Cookie.Split("|")(3) = "Y") Then

                    sql = "select id from member_package where member_id = '" & _MemID & "' and id='" & _PackID & "' and status='N' and archive = 'N'"
                    Conn.Open()
                    Dim dtCheck As DataTable = GetDataTable(sql, Conn)
                    Conn.Close()

                    If dtCheck.Rows.Count <> 0 Then
                        Conn.Open()
                        tx = Conn.BeginTransaction
                        sql = "delete from relation where mem_pack_id = '" & dtCheck.Rows(0)("id") & "' and mem_deposit_id = '" & _MemID & "' and mem_extract_id is null "
                        Cmd = New SqlCommand(sql, Conn, tx)
                        Cmd.ExecuteNonQuery()

                        sql = "delete from member_package where id = '" & _PackID & "' and member_id = '" & _MemID & "' and status='N' and archive = 'N' "
                        Cmd = New SqlCommand(sql, Conn, tx)
                        Cmd.ExecuteNonQuery()


                        tx.Commit()
                        Conn.Close()
                        JSONResponse.exMessage = JSON.FixString("")
                        JSONResponse.Param = "success"
                        JSONResponse.Cookie = Cookie
                        Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))

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

End Class

Public Structure ItemDataFontPackage

    Public id As String
    Public pack_name As String
    Public pack_amount As Decimal
    Public click_amount As Decimal
    Public result_amount As Decimal
    Public expire_day As Decimal
    Public per_click_amout As Decimal

End Structure

Public Structure ItemDataListPackage

    Public no As String
    Public id As String
    Public pack_name As String
    Public pack_amount As Decimal
    Public system_date As String
    Public pack_status As String
    Public total_amount As Decimal
    Public expire_date As String

End Structure