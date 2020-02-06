<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="KinmuSystem.View.List" MasterPageFile="KinmuSystem.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <title>勤務一覧画面</title>
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server"><img src="./img/16_html.png">
    勤務一覧画面
</asp:Content>

<asp:Content ID="PageBody" ContentPlaceHolderID="ContentPlaceHolderBody" runat="server">
    <form id="form1" runat="server">

        <div class='contentsBlock'>
            <div class='notice'>
                <asp:Label ID="noticeLabel" runat="server"></asp:Label>
            </div>
        </div>

        <div class='contentsBlock'>
            <div class='userInfo floatLeft'>
                <asp:Label ID="employeeCodeLabel" runat="server"></asp:Label>
                <asp:Label ID="employeeNameLabel" runat="server"></asp:Label>
                <asp:DropDownList ID="employeeDropDownList" runat="server" OnSelectedIndexChanged="EmployeeDropDownList_SelectedIndexChanged" AutoPostBack="True">
                </asp:DropDownList>
                <asp:Label ID="closingDateLabel" runat="server" CssClass="fontWeak"></asp:Label>
                <asp:Label ID="nenkyuLabel" runat="server" CssClass="fontWeak"></asp:Label>
                <asp:Label ID="tokukyuLabel" runat="server" CssClass="fontWeak"></asp:Label>
                <asp:Label ID="koukyuLabel" runat="server" CssClass="fontWeak"></asp:Label>
            </div>

            <div class='userInfo fontWeak floatRight'>
                <asp:Label ID="overtimePlansLabel" runat="server"></asp:Label>
            </div>

            <div class="clearFloat"></div>

            <div class='floatList' id='selectMonth'>
                <ul class='floatLeft'>
                    <li>
                        <asp:Button ID="prevMonthButton" runat="server" Text="前月" OnClick="PrevMonthButton_Click" OnClientClick="this.disabled = true; this.value = '前月';" UseSubmitBehavior="false" CssClass="ui-button ui-widget ui-state-default ui-button-text-only ui-corner-all"/>
                    </li>
                    <li>
                        <asp:Label ID="selectedMonthLabel" runat="server" />
                    </li>
                    <li>
                        <asp:Button ID="nextMonthButton" runat="server" Text="翌月" OnClick="NextMonthButton_Click" OnClientClick="this.disabled = true; this.value = '翌月';" UseSubmitBehavior="false" CssClass="ui-button ui-widget ui-state-default ui-button-text-only ui-corner-all"/>
                    </li>
                </ul>
            </div>

            <div class='floatRight'>
                <div class="toolbar">
                    <span class="ui-buttonset">
                        <asp:Button ID="EditButton" runat="server" Text="編集" OnClick="EditButton_Click" OnClientClick="this.disabled = true; this.value = '編集';" UseSubmitBehavior="false" CssClass="ui-button ui-widget ui-state-default ui-button-text-only ui-corner-right ui-corner-left" />
                        <asp:Button ID="CopyPlansButton" runat="server" Text="予定コピー" OnClick="CopyPlansButton_Click" OnClientClick="this.disabled = true; this.value = '予定コピー';" UseSubmitBehavior="false" CssClass="ui-button ui-widget ui-state-default ui-button-text-only ui-corner-right ui-corner-left" />
                        <asp:Button ID="CopyResultsButton" runat="server" Text="実績コピー" OnClick="CopyResultsButton_Click" OnClientClick="this.disabled = true; this.value = '実績コピー';" UseSubmitBehavior="false" CssClass="ui-button ui-widget ui-state-default ui-button-text-only ui-corner-right ui-corner-left" />
                        <asp:Button ID="ClearResultsButton" runat="server" Text="実績クリア" OnClick="ClearResultsButton_Click" OnClientClick="this.disabled = true; this.value = '実績クリア';" UseSubmitBehavior="false" CssClass="ui-button ui-widget ui-state-default ui-button-text-only ui-corner-right ui-corner-left" />
                        <asp:Button ID="RunButton" runat="server" Text="実行" OnClick="RunButton_Click" OnClientClick="this.disabled = true; this.value = '実行';" UseSubmitBehavior="false" CssClass="ui-button ui-widget ui-state-default ui-button-text-only ui-corner-right ui-corner-left" />
                        &nbsp;
                        <a href="WorkPlans.aspx" target="_blank" class="ui-button ui-widget ui-state-default ui-button-text-only ui-corner-all" style="padding: .4em 1em;">勤務予定</a>
                        <a href="WorkResults.aspx" target="_blank" class="ui-button ui-widget ui-state-default ui-button-text-only ui-corner-all" style="padding: .4em 1em;">勤務実績</a>
                        <a href="WorkDiary.aspx" target="_blank" class="ui-button ui-widget ui-state-default ui-button-text-only ui-corner-all" style="padding: .4em 1em;">作業日誌</a>
                        
                    </span>
                </div>
            </div>
        </div>

        <div class="clearFloat"></div>

        <asp:Panel ID="InformationPanel" runat="server"　CssClass="ui-state-highlight ui-corner-all">
            <asp:Label ID="InfoIcon" runat="server" CssClass="ui-icon ui-icon-alert floatLeft" />
            <asp:Label ID="InfoText" runat="server" Text="ここにメッセージ情報が表示されます。"/>
        </asp:Panel>

        <div id='contentsKinmu'>
            <div class="lock_box">
                <asp:Panel ID="EditPanel" runat="server">
                    <div>
                        <asp:GridView ID="EditGridView" runat="server" AutoGenerateColumns="False" CssClass="kinmuTableStyle dataTable" OnSelectedIndexChanged="EditGridView_SelectedIndexChanged" AlternatingRowStyle-CssClass="kinmuDataRow even">
                            <HeaderStyle CssClass="kinmuHeaderRow" />
                            <RowStyle CssClass="kinmuDataRow" />
                            <AlternatingRowStyle CssClass="kinmuDataRow even" />
                            <Columns>
                                <asp:BoundField DataField="Date" />
                                <asp:BoundField DataField="Day" />
                                <asp:BoundField DataField="NinsyoSchedule" HeaderText="予定" />
                                <asp:BoundField DataField="OpeningTimeToClosingTimeSchedule" />
                                <asp:BoundField DataField="NinsyoResult" HeaderText="実績" />
                                <asp:BoundField DataField="OpeningTimeToClosingTimeResult" />
                                <asp:BoundField DataField="RestTime" HeaderStyle-CssClass="colGroupWorkTime colMinashi1 headerSmall sorting_disabled" HeaderText="休憩">
                                <HeaderStyle CssClass="colGroupWorkTime colMinashi1 headerSmall sorting_disabled" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Article" HeaderStyle-CssClass="colGroupWorkTime colMinashi1 headerSmall sorting_disabled" HeaderText="記事">
                                <HeaderStyle CssClass="colGroupWorkTime colMinashi1 headerSmall sorting_disabled" />
                                </asp:BoundField>
                                <asp:BoundField DataField="WorkTime" HeaderText="作業時間" />
                                <asp:BoundField DataField="Minashi1" HeaderStyle-CssClass="colGroupWorkTime colMinashi1 headerSmall sorting_disabled" HeaderText="みなし1">
                                <HeaderStyle CssClass="colGroupWorkTime colMinashi1 headerSmall sorting_disabled" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Minashi2" HeaderStyle-CssClass="colGroupWorkTime colMinashi1 headerSmall sorting_disabled" HeaderText="みなし2">
                                <HeaderStyle CssClass="colGroupWorkTime colMinashi1 headerSmall sorting_disabled" />
                                </asp:BoundField>
                                <asp:BoundField DataField="WorkTimeTotal" HeaderStyle-CssClass="colGroupWorkTime colMinashi1 headerSmall sorting_disabled" HeaderText="累計">
                                <HeaderStyle CssClass="colGroupWorkTime colMinashi1 headerSmall sorting_disabled" />
                                <ItemStyle CssClass="total" />
                                </asp:BoundField>
                                <asp:CommandField SelectText="編集" ShowSelectButton="True" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
                <asp:Panel ID="CopyPlansPanel" runat="server">
                    <div>
                        <asp:GridView ID="CopyPlansGridView" runat="server" AutoGenerateColumns="False" CssClass="kinmuTableStyle">
                            <Columns>
                                <asp:BoundField DataField="Date" />
                                <asp:BoundField DataField="Day" />
                                <asp:BoundField DataField="NinsyoSchedule" HeaderText="予定" />
                                <asp:BoundField DataField="OpeningTimeToClosingTimeSchedule" />
                                <asp:BoundField DataField="CopyRadioButton" HeaderText="コピー" HtmlEncode="False" ItemStyle-CssClass="colGroupYoteiCopy textCenter"/>
                                <asp:BoundField DataField="CopyCheckBox" HeaderText="（元→先）" HtmlEncode="False" ItemStyle-CssClass="colGroupYoteiCopy textCenter"/>
                                <asp:BoundField DataField="NinsyoResult" HeaderText="実績" />
                                <asp:BoundField DataField="OpeningTimeToClosingTimeResult" />
                                <asp:BoundField DataField="RestTime" HeaderText="休憩" />
                                <asp:BoundField DataField="Article" HeaderText="記事" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
                <asp:Panel ID="CopyResultsPanel" runat="server">
                    <div>
                        <asp:GridView ID="CopyResultsGridView" runat="server" AutoGenerateColumns="False" CssClass="kinmuTableStyle">
                            <Columns>
                                <asp:BoundField DataField="Date" />
                                <asp:BoundField DataField="Day" />
                                <asp:BoundField DataField="NinsyoSchedule" HeaderText="予定" />
                                <asp:BoundField DataField="OpeningTimeToClosingTimeSchedule" />
                                <asp:BoundField DataField="NinsyoResult" HeaderText="実績" />
                                <asp:BoundField DataField="OpeningTimeToClosingTimeResult" />
                                <asp:BoundField DataField="RestTime" HeaderText="休憩" />
                                <asp:BoundField DataField="Article" HeaderText="記事" />
                                <asp:BoundField DataField="CopyRadioButton" HeaderText="コピー" HtmlEncode="False" ItemStyle-CssClass="textCenter" />
                                <asp:BoundField DataField="CopyCheckBox" HeaderText="（元→先）" HtmlEncode="False" ItemStyle-CssClass="textCenter" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
                <asp:Panel ID="ClearResultsPanel" runat="server">
                    <div>
                        <asp:GridView ID="ClearResultsGridView" runat="server" AutoGenerateColumns="False" CssClass="kinmuTableStyle">
                            <Columns>
                                <asp:BoundField DataField="Date" />
                                <asp:BoundField DataField="Day" />
                                <asp:BoundField DataField="NinsyoSchedule" HeaderText="予定" />
                                <asp:BoundField DataField="OpeningTimeToClosingTimeSchedule" />
                                <asp:BoundField DataField="NinsyoResult" HeaderText="実績" />
                                <asp:BoundField DataField="OpeningTimeToClosingTimeResult" />
                                <asp:BoundField DataField="RestTime" HeaderText="休憩" />
                                <asp:BoundField DataField="Article" HeaderText="記事" />
                                <asp:BoundField DataField="ClearCheckBox" HeaderText="クリア" HtmlEncode="False" ItemStyle-CssClass="textCenter" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </form>
</asp:Content>

