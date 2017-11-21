Public Class topup
    Inherits CModule

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.Form("menu") IsNot Nothing Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ReCookie('" & Request.Form("menu").ToString & "'); setMenu('" & Request.Form("menu").ToString & "'); getDataTopup();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ReCookie('topup');setMenu('topup'); getDataTopup();", True)
        End If

    End Sub

End Class