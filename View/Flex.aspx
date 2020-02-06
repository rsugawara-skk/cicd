<%@ Page Title="" Language="C#" MasterPageFile="~/KinmuSystem.master" AutoEventWireup="true" CodeBehind="Flex.aspx.cs" Inherits="KinmuSystem.View.Flex" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <title>フレックス確認画面</title>

    <link href="css/kinmuConfirmList.css" rel="stylesheet" type="text/css">
    <style type="text/css">
        td, th {
            border: 1px solid;
            border-collapse: collapse;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
    フレックス確認画面
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderBody" runat="server">
    <form id="form1" runat="server">
        <div class="contents">
            <!-- 月選択・確認ボタン -->
            <div class="contentsBlock">

                <!-- 月選択 -->
                <div class="floatList" id="selectMonth">
                    <ul class="floatLeft">
                        <li>
                            <asp:Button ID="Goto1MonthBefore" runat="server" Text="前月" OnClick="Goto1MonthBefore_Click" OnClientClick="this.disabled = true; this.value = '前月';" UseSubmitBehavior="false" />
                            <div class="clearFloat"></div>
                        </li>
                        <li>
                            <a title="表示月" id="targetMonth">
                                <asp:Label runat="server" ID="DateLabel" />

                            </a>
                        </li>
                        <li>
                            <asp:Button ID="Goto1MonthAfter" runat="server" Text="翌月" OnClick="Goto1MonthAfter_Click" OnClientClick="this.disabled = true; this.value = '翌月';" UseSubmitBehavior="false" />
                            <div class="clearFloat"></div>
                        </li>
                    </ul>


                </div>

                <!-- 確認ボタン -->
                <div class="floatRight">
                    <asp:Button ID="BtnCodeUpdate" runat="server" Text="確　認" OnClick="BtnCodeUpdate_Click" OnClientClick="this.disabled = true; this.value = '確　認';" UseSubmitBehavior="false" />
                </div>
                <div class="clearFloat"></div>
            </div>

            <!-- システムメッセージ表示領域 -->
            <div class="contentsBlock">
                <asp:Panel ID="InformationPanel" runat="server" CssClass="ui-state-highlight ui-corner-all">
                    <asp:Label ID="InfoIcon" runat="server" CssClass="ui-icon ui-icon-alert floatLeft" />
                    <asp:Label ID="InfoText" runat="server" Text="ここにメッセージ情報が表示されます。" />
                </asp:Panel>
            </div>

            <!-- 凡例 -->
            <div class="floatRight">
                <asp:Table ID="LegendTable" runat="server" CssClass="kinmuConfirmTableStyle lockTableWidth" Width="300px">
                    <asp:TableRow runat="server">
                        <asp:TableCell BorderStyle="Solid" runat="server" CssClass="jisseki" Width="33%" Style="text-align: center">実績</asp:TableCell>
                        <asp:TableCell BorderStyle="Solid" runat="server" CssClass="notConfirmYotei" Width="33%" Style="text-align: center">計画(未確認)</asp:TableCell>
                        <asp:TableCell BorderStyle="Solid" RowSpan="2" runat="server" CssClass="confirmYotei" Width="33%" Style="text-align: center">計画(確認済)</asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>

            <br />

            <div class="contentsKinmuConfirm">
                <!-- 外枠 -->
                <div class="x_data_area">
                    <!-- ロック部分 -->
                    <div class="lock_box">
                        <asp:Table ID="kinmuConfirmTableCalender" runat="server" CssClass="kinmuConfirmTableStyle lockTableWidth">
                        </asp:Table>
                    </div>
                    <!-- /ロック部分 -->
                    <!-- 横スクロール部分 -->
                    <div class="x_scroll_box">
                        <asp:Table ID="kinmuConfirmTableData" runat="server" CssClass="scrollTableWidth data kinmuConfirmTableStyle">
                        </asp:Table>
                    </div>
                    <!-- /横スクロール部分 -->
                </div>
                <!-- /外枠 -->
            </div>
        </div>
    </form>
</asp:Content>






