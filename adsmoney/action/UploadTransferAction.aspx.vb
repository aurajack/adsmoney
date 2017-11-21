Imports System.IO

Public Class UploadTransferAction
    Inherits CModule

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call UploadFile(Request.Files("TransferFile"))

    End Sub

    Private Sub UploadFile(ByVal _File As HttpPostedFile)

        Dim UploadDirectory As String = Request.PhysicalApplicationPath & "transfer\"
        Dim UploadFile As HttpPostedFile = _File
        Dim FilePath As String = UploadDirectory & Path.GetFileName(UploadFile.FileName())
        Dim FileName As String = Path.GetFileName(UploadFile.FileName())
        If (Not Directory.Exists(UploadDirectory)) Then Directory.CreateDirectory(UploadDirectory)
        If (File.Exists(FilePath)) Then
            FilePath = UploadDirectory & Path.GetFileNameWithoutExtension(UploadFile.FileName()) & "_" & _
                        Now.ToString("yyyyMMddHHmmss") & Path.GetExtension(UploadFile.FileName())
        End If
        UploadFile.SaveAs(FilePath)

    End Sub

End Class