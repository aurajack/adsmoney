Imports System.IO
Imports System.Data.SqlClient

Public Class AdsAction
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

                    Call GenData(Request.Form("memid").ToString, Request.Form("id").ToString)

                Case "clicklink"

                    Call SaveClick(Request.Form("memid").ToString, Request.Form("id").ToString, Request.Form("pack_id").ToString)

            End Select

        End If

    End Sub

    Private Sub GenData(ByVal _MemData As String, ByVal _MemID As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemData
        Try

            Cookie = CheckExpire(_MemData)
            If Cookie <> "" Then
                If (Cookie.Split("|")(3) = "Y") Then

                    'Dim sql As String = " select a.ads_name,a.ads_link,a.ads_path,mp.pack_click,mp.current_click,mp.package_id from ads a" & _
                    '                    " left join member_package mp on 1 = 1 " & _
                    '                    " where a.status = 'ACTIVE' and mp.member_id = '" & _MemID & "' and mp.status='Y'" & _
                    '                    " and getdate() < mp.expire_date"
                    Dim sql As String = " select a.ads_name,a.ads_link,a.ads_path,mp.pack_click,c.xx current_click,mp.package_id from ads a " & _
                    " left join member_package mp on 1 = 1 " & _
                    " left join ( " & _
                    "      select count(b.id) xx from book_transection b " & _
                    "     left join member_package mp2 on mp2.package_id = b.package_id " & _
                    "         and mp2.status = 'Y' and mp2.archive = 'N' " & _
                    "     where b.member_id = '" & _MemID & "' " & _
                    "     and b.description = 'รับเงินค่ากดลิ้งค์' and convert(varchar(10),b.system_date,121) = convert(varchar(10),getdate(),121) " & _
                    " )c on 1 = 1 " & _
                    " where a.status = 'ACTIVE' and mp.member_id = '" & _MemID & "' and mp.status='Y' " & _
                    " and getdate() < mp.expire_date"

                    If Conn.State = ConnectionState.Closed Then Conn.Open()
                    Dim dtData As DataTable = GetDataTable(sql, Conn)
                    If Conn.State = ConnectionState.Open Then Conn.Close()

                    Dim DataInfoItems As New Generic.List(Of ItemDataFontAds)

                    Dim item As New ItemDataFontAds()

                    For Each mRow As DataRow In dtData.Select("")

                        item = New ItemDataFontAds()

                        item.ads_name = mRow("ads_name").ToString
                        item.ads_link = mRow("ads_link").ToString
                        item.ads_path = mRow("ads_path").ToString
                        item.pack_id = mRow("package_id").ToString
                        If mRow("current_click") = 0 Then
                            item.current_click = mRow("current_click").ToString
                        Else
                            item.current_click = mRow("current_click").ToString
                        End If

                        item.pack_click = mRow("pack_click").ToString

                        DataInfoItems.Add(item)

                    Next

                    Response.Write(JSON.Serialize(Of Generic.List(Of ItemDataFontAds))(DataInfoItems))

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
            JSONResponse.exMessage = JSON.FixString(ex.Message)
            JSONResponse.Param = ""
            JSONResponse.Cookie = Cookie
            Response.Write(JSON.Serialize(Of JSONObject)(JSONResponse))
        End Try
        Response.End()

    End Sub

    Private Sub SaveClick(ByVal _MemData As String, ByVal _ID As String, ByVal _PackID As String)

        Dim JSONResponse As New JSONObject()
        Dim Cookie As String = _MemData
        Try

            Cookie = CheckExpire(_MemData)

            If Cookie <> "" Then
                If (Cookie.Split("|")(3) = "Y") Then
                    Dim sql As String = ""

                    sql = "select id,amount_per_click,pack_click from member_package where member_id='" & _ID & "' and package_id = '" & _PackID & "' and status = 'Y' and getdate() < expire_date and archive = 'N'"
                    Conn.Open()
                    Dim dtCheck As DataTable = GetDataTable(sql, Conn)
                    Conn.Close()

                    If dtCheck.Rows.Count <> 0 Then

                        sql = "select count(id) id from book_transection where member_id='" & _ID & "' and description='รับเงินค่ากดลิ้งค์' and package_id = '" & _PackID & "' and convert(varchar(10),system_date,121) = convert(varchar(10),getdate(),121)"
                        Conn.Open()
                        Dim dtCheckClickDay As DataTable = GetDataTable(sql, Conn)
                        Conn.Close()

                        If dtCheckClickDay.Rows(0)("id") < dtCheck.Rows(0)("pack_click") Then

                            Try
                                sql = "update member_package set current_click = current_click + 1  where id = @id "
                                Conn.Open()
                                tx = Conn.BeginTransaction
                                Cmd = New SqlCommand(sql, Conn, tx)
                                Cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = dtCheck.Rows(0)("id")
                                Cmd.ExecuteNonQuery()

                                sql = "insert into book_transection(description,amount,member_id,package_id) values('รับเงินค่ากดลิ้งค์',@perclick,@memid,@packid) "
                                Cmd = New SqlCommand(sql, Conn, tx)
                                Cmd.Parameters.Add("@perclick", SqlDbType.Decimal).Value = dtCheck.Rows(0)("amount_per_click")
                                Cmd.Parameters.Add("@memid", SqlDbType.BigInt).Value = _ID
                                Cmd.Parameters.Add("@packid", SqlDbType.BigInt).Value = _PackID
                                Cmd.ExecuteNonQuery()

                                sql = "update member set total_amount = total_amount + @perclick, balance = balance + @perclick,check_amount_relation = check_amount_relation + @perclick where id = @id "
                                Cmd = New SqlCommand(sql, Conn, tx)
                                Cmd.Parameters.Add("@perclick", SqlDbType.Decimal).Value = dtCheck.Rows(0)("amount_per_click")
                                Cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = _ID
                                Cmd.ExecuteNonQuery()

                                sql = " select check_amount_relation from member where id = '" & _ID & "'"
                                Dim dtCheckBalance As DataTable = GetDataTableTx(sql, Conn, tx)

                                If dtCheckBalance.Rows(0)("check_amount_relation") > 1000 Then

                                    sql = " select top 1 id from relation where mem_deposit_id is not NULL and status = 'N' " & _
                                   " and mem_extract_id is NULL order by system_date asc"

                                    Dim dtCheckRelation As DataTable = GetDataTableTx(sql, Conn, tx)

                                    If dtCheckRelation.Rows.Count <> 0 AndAlso dtCheckRelation.Rows(0)("id") <> 0 Then
                                        sql = "update relation set mem_mem_extract_id_id = '" & _ID & "', updated_date=getdate(),mem_pack_id = '" & dtCheck.Rows(0)("id") & "' where id = " & dtCheckRelation.Rows(0)("id") & ""
                                        Cmd = New SqlCommand(sql, Conn, tx)
                                        Cmd.ExecuteNonQuery()
                                    Else
                                        sql = "insert into relation(mem_extract_id,amount,mem_pack_id) values('" & _ID & "','1000','" & dtCheck.Rows(0)("id") & "')"
                                        Cmd = New SqlCommand(sql, Conn, tx)
                                        Cmd.ExecuteNonQuery()
                                    End If

                                    sql = "update member set check_amount_relation = check_amount_relation - 1000 where id = @id "
                                    Cmd = New SqlCommand(sql, Conn, tx)
                                    Cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = _ID
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
                                JSONResponse.exMessage = ""
                                JSONResponse.Param = "expire"
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

End Class

Public Structure ItemDataFontAds

    Public no As String
    Public ads_name As String
    Public ads_link As String
    Public ads_path As String
    Public pack_click As String
    Public current_click As String
    Public pack_id As String

End Structure