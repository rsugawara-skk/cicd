<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Setting.aspx.cs" Inherits="KinmuSystem.View.Setting" EnableEventValidation="false" MasterPageFile="KinmuSystem.master" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <title>個人設定画面</title>
    <script type='text/javascript' src='./lib/jquery-ui-1.9.2.custom/js/jquery-1.8.3.js'></script>
    <script type='text/javascript' src='./lib/jquery-ui-1.9.2.custom/js/jquery-ui-1.9.2.custom.min.js'></script>
    <link rel='stylesheet' type='text/css' href='css/common.css'>
    <link rel='stylesheet' type='text/css' href='css/kojinSettei.css'>
    <script type='text/javascript' src='js/kojinSettei.js'></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
    個人設定画面
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderBody" runat="server">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <div class='contents'>
            <div class='contentsBlock'>
                <asp:Panel ID="ErrorMessagePanel" CssClass="ui-state-error ui-corner-all" runat="server" Visible="False">
                    <span class="ui-icon ui-icon-alert floatLeft"></span>
                    <asp:Label ID="ErrorMessageLabel" runat="server"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="MessagePanel" CssClass="ui-state-highlight ui-corner-all" runat="server" Visible="False">
                    <span class="ui-icon ui-icon-info floatLeft"></span>
                    <asp:Label ID="MessageLabel" runat="server"></asp:Label>
                </asp:Panel>
            </div>
            <div id="tabGroup" class='contentsBlock'>
                <asp:HiddenField ID="defaultTab" runat="server" />

                <ul>
                    <li><a href="#tabInitValue">入力初期値</a></li>
                    <li><a href="#tabCode">利用コード</a></li>
                </ul>
                <div id='tabInitValue'>
                    <div class='contentsEditGroup'>
                        <table border="0" class='fontSmall'>
                            <tr>
                                <td>勤務認証:</td>
                                <td>
                                    <asp:DropDownList ID="ninshoCDDropDownList" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>勤務時間:</td>
                                <td>
                                    <asp:TextBox ID="workStartHour" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />
                                    :<asp:TextBox ID="workStartMinute" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />
                                    ~<asp:TextBox ID="workEndHour" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />
                                    :<asp:TextBox ID="workEndMinute" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />
                                    <label>翌日<asp:CheckBox ID="workEndNextDayFlg" runat="server" onclick="calcDefaultTime();" /></label>
                                </td>
                                <td>
                                    <span id="kousokuTimeMsg"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>休憩時間:</td>
                                <td>[1]<asp:TextBox ID="restStartHour_1" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />:<asp:TextBox ID="restStartMinute_1" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />
                                    ~
								<asp:TextBox ID="restEndHour_1" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />:<asp:TextBox ID="restEndMinute_1" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />
                                    <br>
                                    [2]<asp:TextBox ID="restStartHour_2" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />:<asp:TextBox ID="restStartMinute_2" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />
                                    ~
								<asp:TextBox ID="restEndHour_2" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />:<asp:TextBox ID="restEndMinute_2" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />
                                    <br>
                                    [3]<asp:TextBox ID="restStartHour_3" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />:<asp:TextBox ID="restStartMinute_3" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />
                                    ~
								<asp:TextBox ID="restEndHour_3" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />:<asp:TextBox ID="restEndMinute_3" runat="server" CssClass="time" MaxLength="2" onblur="calcDefaultTime();" />
                                </td>
                                <td>
                                    <span id="restTimeTotalMsg"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>作業時間:</td>
                                <td>
                                    <span id="sagyoTimeMsg"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>プロジェクトコード:</td>
                                <td>
                                    <asp:DropDownList ID="projCDDropDownList" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>作業コード:</td>
                                <td>
                                    <asp:DropDownList ID="sagyoCDDropDownList" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Button ID="InitValueUpdateButton" runat="server" Text="更新" OnClick="InitValueUpdateButton_Click" OnClientClick="this.disabled = true; this.value = '更新';" UseSubmitBehavior="false" />
                </div>
                <div id='tabCode'>
                    <span class='fontWeak'>(注) 並び替え、追加操作後に更新ボタンを押下するのを忘れないでください。</span>
                    <div class='contentsEditGroup'>
                        <span>プロジェクトコード</span>
                        <table border="0" class='fontSmall'>
                            <tr>
                                <td class="auto-style1">
                                    <asp:UpdatePanel ID="UpdatePanelPJCD" runat="server">
                                        <ContentTemplate>
                                            <asp:ListBox ID="projCDListListBox" runat="server" CssClass='selectWidth' Rows="10" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="projCDAddButton" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="projCDUpButton" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="projCDDowmButton" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="projCDDeleteButton" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <div class='textCenter'>
                                        <asp:Button ID="projCDUpButton" runat="server" Text="↑" OnClick="ProjCDUpButton_Click" />
                                        <br />
                                        <asp:Button ID="projCDDowmButton" runat="server" Text="↓" OnClick="ProjCDDowmButton_Click" />
                                        <br />
                                        <br />
                                        <asp:Button ID="projCDDeleteButton" runat="server" Text="削除" OnClick="ProjCDDeleteButton_Click" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1">

                                    <asp:UpdatePanel ID="UpdatePanelDropDownPJCD" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="additionalProjCDDropDownList" CssClass='selectWidth' runat="server" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="projCDAddButton" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="projCDAddButton" runat="server" Text="追加" OnClick="ProjCDAddButton_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class='contentsEditGroup'>
                        <span>作業コード</span>
                        <table border="0" class='fontSmall'>
                            <tr>
                                <td class="auto-style1">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:ListBox ID="sagyoCDListBox" name="sagyoCDList" runat="server" CssClass='selectWidth' Rows="10" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="sagyoCDUpButton" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="sagyoCDDownButton" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="sagyoCDDeleteButton" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="sagyoCDAddButton" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <div class='textCenter'>
                                        <span>
                                            <asp:Button ID="sagyoCDUpButton" runat="server" Text="↑" OnClick="SagyoCDUpButton_Click" />
                                        </span>
                                        <br />
                                        <span>
                                            <asp:Button ID="sagyoCDDownButton" runat="server" Text="↓" OnClick="SagyoCDDownButton_Click" />
                                        </span>
                                        <br />
                                        <br />
                                        <span>
                                            <asp:Button ID="sagyoCDDeleteButton" runat="server" Text="削除" OnClick="SagyoCDDeleteButton_Click" />
                                        </span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="additionalSagyoCDDropDownList" CssClass='selectWidth' runat="server" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="sagyoCDAddButton" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <span>
                                        <asp:Button ID="sagyoCDAddButton" runat="server" Text="追加" OnClick="SagyoCDAddButton_Click" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Button ID="CodeUpdateButton" runat="server" Text="更新" OnClick="CodeUpdateButton_Click" OnClientClick="this.disabled = true; this.value = '更新';" UseSubmitBehavior="false" />
                </div>
            </div>
        </div>
    </form>
</asp:Content>
