<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Login.Login" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>ログイン画面</title>
    <!-- IE 互換表示モードを抑止 -->
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>

    <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>
    <meta http-equiv='Content-Style-Type' content='text/css'>
    <meta http-equiv='Content-Script-Type' content='text/javascript'>
</head>
<body>
    <div class='contents'>

        <!-- ヘッダー部 (システム名、リンク) -->
        <div class='systemName'>
            <div class='floatLeft navigation'>
                <span>勤務システム</span>
            </div>
            <div class='clearFloat'></div>
        </div>

        <!-- ログイン画面 -->
        <div class='contentsBlock'>
            <h1 align="center">勤務システム　ログイン</h1>

            <div class="notice">
                <form id="loginForm" runat="server">
                    <p align="center">アカウント：<asp:TextBox ID="user" MaxLength="20" Style="ime-mode: disabled" runat="server" /><font size="2" color="blue">※必須</font></p>
                    <p align="center">パスワード：<asp:TextBox ID="pass" MaxLength="20" Style="ime-mode: disabled" runat="server" /><font size="2" color="blue">※必須</font></p>
                    <p align="center">
                        <asp:Button ID="submit" runat="server" OnClick="submit_Click" Text="ログイン" />
                    </p>
                </form>
                <p align="center">
                    <asp:Label ID="resultLabel" runat="server"></asp:Label>
                </p>
            </div>
        </div>
    </div>

</body>
</html>
