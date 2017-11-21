Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.IO
Imports System.Security.Cryptography
Imports System.Net.Mail
Imports System.Text

Public Class CModule
    Inherits System.Web.UI.Page

    Public Conn As SqlConnection
    Public Cmd As SqlCommand
    Public tx As SqlTransaction
    Public _MailMessage As MailMessage

    Sub New()

        Dim strConnString As String

        strConnString = String.Format(" data source={0}; initial catalog=adsmoney; ;Persist Security Info=True; user id=travox; {1} connect timeout=30;", "10.0.1.91", "password=NipSystem55!;")
        'strConnString = String.Format(" data source={0}; initial catalog=adsmoney; ;Persist Security Info=True; user id=omic; {1} connect timeout=30;", "localhost\MSSQLSERVER2016", "password=Fv,,b8;")

        Conn = New SqlConnection(strConnString)

    End Sub

    Public Overloads Function GetDataTable(ByVal Query As String, ByVal Con As SqlConnection) As DataTable
        Dim dt As New DataTable
        Try
            If Conn.State = ConnectionState.Open Then Conn.Close()

            Dim da As New SqlDataAdapter(Query, Conn)
            da.SelectCommand.CommandTimeout = 100
            da.Fill(dt)
            da.Dispose()
            Conn.Close()

        Catch ex As Exception
            Conn.Close()
        End Try
       
        Return dt

    End Function

    Public Overloads Function GetDataTableTx(ByVal Query As String, ByVal Con As SqlConnection, ByVal Tx As SqlTransaction) As DataTable
        Dim dt As New DataTable
        Try

            Dim da As New SqlDataAdapter(Query, Conn)
            da.SelectCommand.CommandTimeout = 100
            da.SelectCommand.Transaction = Tx
            da.Fill(dt)
            da.Dispose()
            'Conn.Close()

        Catch ex As Exception

        End Try

        Return dt

    End Function

    Public Function CheckExpire(ByVal _UserData As String) As String

        Dim arrData() As String = _UserData.Split("|")
        Dim MemID As String = arrData(1)

        Dim sql As String = " select m.code,m.id,m.username,isnull(xx.active,'N') active from member m " & _
                               " left join ( " & _
                               " select top 1 member_id mem_id, active from member_topup where member_id ='" & MemID & "' and approved_date is not NULL and active='Y' and getdate() < expire_date order by id desc " & _
                               " ) xx on xx.mem_id = m.id " & _
                               " where m.id = '" & MemID & "' and m.status = 'Y'"

        'Dim sql As String = "select top 1 m.id,m.username, from member_topup where member_id = '" & MemID & "' and approved_date is not NULL and active='Y' and getdate() < expire_date order by id desc"

        Dim dt As DataTable = GetDataTable(sql, Conn)
        If dt.Rows.Count <> 0 Then
            Call CheckExpirePackage(MemID)
            Return dt.Rows(0)("code").ToString & "|" & dt.Rows(0)("id").ToString & "|" & dt.Rows(0)("username").ToString & "|" & dt.Rows(0)("active").ToString
        Else
            Return ""
        End If

    End Function

    Public Sub CheckExpirePackage(ByVal _MemID As String)

        Dim Sql As String = "update member_package set archive = 'Y' where member_id = @memid and expire_date is not NULL and expire_date <= getdate() and archive = 'N'  "

        Cmd = New SqlCommand(Sql, Conn)
        Cmd.Parameters.Add("@memid", SqlDbType.VarChar).Value = _MemID
    
        Conn.Open()

        Cmd.ExecuteNonQuery()
        Conn.Close()
    End Sub

    Public Function MD5(ByVal strString As String) As String

        Dim ASCIIenc As New ASCIIEncoding

        Dim strReturn As String

        Dim ByteSourceText() As Byte = ASCIIenc.GetBytes(strString)

        Dim Md5Hash As New MD5CryptoServiceProvider

        Dim ByteHash() As Byte = Md5Hash.ComputeHash(ByteSourceText)

        For Each b As Byte In ByteHash
            strReturn = strReturn & b.ToString("x2")
        Next

        Return strReturn
    End Function

    Public Sub SelectEmail(ByVal _Content As String, ByVal _Email As String)

        'If cnGlobal.State = ConnectionState.Closed Then cnGlobal.Open()

        _MailMessage = New MailMessage
        _MailMessage.From = New MailAddress("info@THREESIXFIVE.com", "THREESIXFIVE")

        _MailMessage.To.Add(_Email)
        '_MailMessage.Bcc.Add("jack@ns.co.th,jo@ns.co.th,nut@ns.co.th,sah@ns.co.th,mim@ns.co.th")

        _MailMessage.Subject = "THREESIXFIVE.com (Remember password on user)"
        _MailMessage.IsBodyHtml = True
        _MailMessage.Body = _Content

        Call SMTPSend(_MailMessage)

    End Sub

    Public Sub SMTPSend(ByRef _MailMessage As MailMessage)

        Dim _SmtpClient As New SmtpClient

        Dim _Host As String = "petahost52.ns.co.th"

        Try
            _SmtpClient.Host = _Host
            _SmtpClient.Send(_MailMessage)
        Catch ex As Exception

        End Try

    End Sub

    Function GeneratePassword()
        Dim s As String = "abcdefghijklmnopqrstuvwxyz0123456789"
        Dim r As New Random
        Dim sb As New StringBuilder
        For i As Integer = 1 To 8
            Dim idx As Integer = r.Next(0, 35)
            sb.Append(s.Substring(idx, 1))
        Next
        Return sb.ToString()
    End Function

End Class

Public Class JSON
    Public Shared Function Serialize(Of T)(ByVal v As T) As String
        Dim memStream As New IO.MemoryStream()
        Dim dataSerializer As New DataContractJsonSerializer(v.[GetType]())
        dataSerializer.WriteObject(memStream, v)
        Dim jsonString As String = Encoding.UTF8.GetString(memStream.ToArray())
        memStream.Close()
        Return jsonString
    End Function
    Public Shared Function Deserialize(Of T)(ByVal v As String) As T
        Dim dataSerializer As New DataContractJsonSerializer(GetType(T))
        Dim ms As New IO.MemoryStream(Encoding.UTF8.GetBytes(v))
        Return DirectCast(dataSerializer.ReadObject(ms), T)
    End Function
    Public Shared Function FixString(ByVal plaintext As String) As String
        plaintext = plaintext.Replace("\\r\\n", "<br>").Replace("\", "\\").Replace("""", "\""").Replace("'", "\""")
        Return Regex.Replace(plaintext, "\s+", " ")
    End Function

End Class
