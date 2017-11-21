Public Class manageads
    Inherits CModule

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.Form("menu") IsNot Nothing Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ReCookie('" & Request.Form("menu").ToString & "'); setMenu('" & Request.Form("menu").ToString & "'); getDataAds();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ReCookie('manageads');setMenu('manageads'); getDataAds();", True)
        End If

    End Sub

End Class