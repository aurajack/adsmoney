﻿Public Class managepackage
    Inherits CModule

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.Form("menu") IsNot Nothing Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ReCookie('" & Request.Form("menu").ToString & "'); setMenu('" & Request.Form("menu").ToString & "'); getDataPackage();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ReCookie('managepackage');setMenu('managepackage'); getDataPackage();", True)
        End If

    End Sub

End Class