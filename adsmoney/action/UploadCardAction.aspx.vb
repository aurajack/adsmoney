Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Graphics

Public Class UploadCardAction
    Inherits CModule

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call UploadFile(Request.Files("CardFile"))

    End Sub

    Private Sub UploadFile(ByVal _File As HttpPostedFile)

        Dim UploadDirectory As String = Request.PhysicalApplicationPath & "card\"
        Dim UploadFile As HttpPostedFile = _File

        Dim FilePath As String = UploadDirectory & Path.GetFileName(UploadFile.FileName())
        Dim FileName As String = Path.GetFileName(UploadFile.FileName())
        If (Not Directory.Exists(UploadDirectory)) Then Directory.CreateDirectory(UploadDirectory)
        If (File.Exists(FilePath)) Then
            FilePath = UploadDirectory & Path.GetFileNameWithoutExtension(UploadFile.FileName()) & "_" & _
                        Now.ToString("yyyyMMddHHmmss") & Path.GetExtension(UploadFile.FileName())
        End If
        UploadFile.SaveAs(FilePath)

        'Dim UploadDirectory As String = Request.PhysicalApplicationPath & "card\"
        'Dim UploadFile As HttpPostedFile = _File

        'Dim bm As System.Drawing.Image = System.Drawing.Image.FromStream(UploadFile.InputStream)
        'bm = ResizeBitmap(bm, 100, 100) '' new width, height

        'Dim FilePath As String = UploadDirectory & Path.GetFileName(UploadFile.FileName())
        'Dim FileName As String = Path.GetFileName(UploadFile.FileName())
        'If (Not Directory.Exists(UploadDirectory)) Then Directory.CreateDirectory(UploadDirectory)
        'If (File.Exists(FilePath)) Then
        '    FilePath = UploadDirectory & Path.GetFileNameWithoutExtension(UploadFile.FileName()) & "_" & _
        '                Now.ToString("yyyyMMddHHmmss") & Path.GetExtension(UploadFile.FileName())
        'End If
        ''UploadFile.SaveAs(FilePath)
        'bm.Save(Path.Combine(FilePath, FilePath))

    End Sub


    'Function ResizeBitmap(ByVal b As Bitmap, ByVal nWidth As Integer, ByVal nHeight As Integer)

    '    Dim result As Bitmap = New Bitmap(nWidth, nHeight)
    '    Using g As New Graphics(Graphics.FromImage(result))
    '        g.DrawImage(b, 0, 0, nWidth, nHeight)
    '    End Using
    '    Return result

    'End Function

End Class