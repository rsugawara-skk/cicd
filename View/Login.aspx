<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="KinmuSystem.View.Login" MasterPageFile="KinmuSystem.master" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <title>ログイン画面</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
    ログイン画面
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderBody" runat="server">
    <div class='contentsBlock textCenter'>
        <h1>勤務システム</h1>

        <div class="notice">
            <form id="loginForm" runat="server">
                <p>
                    <asp:Label ID="resultLabel" runat="server" />
                </p>
                <p>
                    <asp:Button ID="LogoutButton" runat="server" OnClick="OnClickLogoutButton" Text="ログアウト" OnClientClick="this.disabled = true; this.value = 'ログアウト';" UseSubmitBehavior="false" />
                </p>
            </form>
        </div>
    </div>
</asp:Content>
