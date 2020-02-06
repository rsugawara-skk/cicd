<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="KinmuSystem.View.Update" MasterPageFile="KinmuSystem.master" MaintainScrollPositionOnPostback="true" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <title>勤務情報編集画面</title>


    <script type="text/javascript">
        function KijiCnt() {
            __doPostBack('<%= JissekiKiji.UniqueID %>', '');
        };
    </script>

    <!-- スタイルシートは後ほど転機 -->
    <style>
        ul.YoteiNinshoCdContainer {
            margin: 0;
            padding: 0;
            display: inline;
        }

        .YoteiNinshoCdContainer li {
            display: inline;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderPageTitle" runat="server">
    勤務情報編集画面
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderBody" runat="server">
    <form id="UpdateForm" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server" />
        <div>
            <asp:Button runat="server" ID="GotoYesterday" Text="前日" OnClick="OnClickGotoYesterday" OnClientClick="this.disabled = true; this.value = '前日';" UseSubmitBehavior="false" />
            [<asp:Label runat="server" ID="ViewShainCD" Text="9999999" />:<asp:Label runat="server" ID="ViewShainName" Text="社員名" />]
            <asp:Label runat="server" ID="KinmuYear" Text="9999" />年
            <asp:Label runat="server" ID="KinmuMonth" Text="99" />月
            <asp:Label runat="server" ID="KinmuDay" Text="99" />日
            <asp:Label runat="server" ID="KinmuDayOfWeek" Text="（日）" />
            <asp:Label runat="server" ID="CalendarNinsyoCD" Text="[出勤]" />の勤務実績　
            <asp:Button runat="server" ID="GotoTomorrow" Text="翌日" OnClick="OnClickGotoTomorrow" OnClientClick="this.disabled = true; this.value = '翌日';" UseSubmitBehavior="false" />
        </div>

        <asp:Panel runat="server" ID="ContentsBlock" CssClass="ui-state-highlight ui-corner-all">
            <asp:Label runat="server" ID="ContentsBlockIcon" CssClass="ui-icon ui-icon-alert floatLeft" />
            <asp:Label runat="server" ID="ContentsBlockLabel" Text="ここに情報が表示されます。" />
        </asp:Panel>

        <div class="fontSmall contentsBlock ui-tabs ui-widget ui-widget-content ui-corner-all">
            予定勤務時間
            <asp:TextBox ID="YoteiStartHour" runat="server" CssClass="time checkYoteiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
            ：
            <asp:TextBox ID="YoteiStartMinute" runat="server" CssClass="time checkYoteiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
            ～
		    <asp:TextBox ID="YoteiEndHour" runat="server" CssClass="time checkYoteiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
            ：
            <asp:TextBox ID="YoteiEndMinute" runat="server" CssClass="time checkYoteiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
            <label>
                翌日
                <asp:CheckBox ID="YoteiEndNextDayFlg" runat="server" CssClass="checkYoteiTime" OnCheckedChanged="OnChangedTimeValue" />
            </label>

            <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional" style="display: inline;">
                <ContentTemplate>
                    <asp:Label ID="yoteiKousokuTimeMsg" runat="server" CssClass="validaterInfo" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="YoteiStartHour" />
                    <asp:AsyncPostBackTrigger ControlID="YoteiStartMinute" />
                    <asp:AsyncPostBackTrigger ControlID="YoteiEndHour" />
                    <asp:AsyncPostBackTrigger ControlID="YoteiEndMinute" />
                    <asp:AsyncPostBackTrigger ControlID="YoteiEndNextDayFlg" />
                </Triggers>
            </asp:UpdatePanel>

            <asp:RadioButtonList runat="server" RepeatLayout="UnorderedList" ID="YoteiNinshoCd" CssClass="YoteiNinshoCdContainer" OnSelectedIndexChanged="OnChangedYoteiNinshoCd" AutoPostBack="true">
                <asp:ListItem Text="通常勤務" Value="01" />
                <asp:ListItem Text="年休" Value="19" />
                <asp:ListItem Text="AM半休" Value="20" />
                <asp:ListItem Text="PM半休" Value="21" />
            </asp:RadioButtonList>
            <br />
            <asp:Button ID="YoteiUpdateButton" runat="server" Text="予定を更新する" OnClick="OnClickYoteiUpdateButton" OnClientClick="this.disabled = true; this.value = '予定を更新する';" UseSubmitBehavior="false" /><br />
        </div>
        <hr />
        <div class="fontSmall contentsBlock ui-tabs ui-widget ui-widget-content ui-corner-all">
            <asp:Panel ID="JissekiPanel" runat="server">
                <table>
                    <tbody>
                        <tr>
                            <td>実績</td>
                            <td>勤務認証:</td>
                            <td>
                                <asp:DropDownList ID="JissekiNinshoCd" runat="server" OnSelectedIndexChanged="OnChangedJissekiNinshoCd" AutoPostBack="true" />
                                <asp:Label ID="jissekiNinshoMsg" runat="server" CssClass="validaterInfo" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>勤務時間:</td>
                            <td>
                                <asp:TextBox ID="JissekiStartHour" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                ：
                            <asp:TextBox ID="JissekiStartMinute" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                ～
						    <asp:TextBox ID="JissekiEndHour" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                ：
                            <asp:TextBox ID="JissekiEndMinute" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                <label>
                                    翌日
                            <asp:CheckBox ID="JissekiEndNextDayFlg" runat="server" AutoPostBack="True" OnCheckedChanged="OnChangedTimeValue" />
                                </label>

                                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional" style="display: inline;">
                                    <ContentTemplate>
                                        <asp:Label ID="jissekiKousokuTimeMsg" runat="server" CssClass="validaterInfo" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="JissekiStartHour" EventName="TextChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiStartMinute" EventName="TextChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiEndHour" EventName="TextChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiEndMinute" EventName="TextChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiEndNextDayFlg" EventName="CheckedChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>記事:</td>
                            <td>
                                <asp:TextBox ID="JissekiKiji" runat="server" CssClass="kiji checkJissekiKiji" OnTextChanged="OnTextChangedKiji" AutoPostBack="true" onkeyup="KijiCnt();" MaxLength="20" />
                                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional" style="display: inline;">
                                    <ContentTemplate>
                                        <asp:Label ID="jissekiKijiMsg" runat="server" CssClass="validaterInfo" Text="0/20" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="JissekiKiji" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>休憩時間:</td>
                            <td>[1]
                        <asp:TextBox ID="JissekiRestStartHour_1" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                ：
                        <asp:TextBox ID="JissekiRestStartMinute_1" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                ～
						<asp:TextBox ID="JissekiRestEndHour_1" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                ：
                        <asp:TextBox ID="JissekiRestEndMinute_1" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                <br />
                                [2]
                        <asp:TextBox ID="JissekiRestStartHour_2" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                ：
                        <asp:TextBox ID="JissekiRestStartMinute_2" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                ～
						<asp:TextBox ID="JissekiRestEndHour_2" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                ：
                        <asp:TextBox ID="JissekiRestEndMinute_2" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />

                                <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional" style="display: inline;">
                                    <ContentTemplate>
                                        <asp:Label ID="jissekiRestTimeTotalMsg" runat="server" CssClass="validaterInfo" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="JissekiStartHour" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiStartMinute" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiEndHour" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiEndMinute" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiEndNextDayFlg" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestStartHour_1" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestStartMinute_1" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestEndHour_1" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestEndMinute_1" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestStartHour_2" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestStartMinute_2" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestEndHour_2" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestEndMinute_2" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestStartHour_3" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestStartMinute_3" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestEndHour_3" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestEndMinute_3" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <br />
                                [3]
                        <asp:TextBox ID="JissekiRestStartHour_3" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                ：
                        <asp:TextBox ID="JissekiRestStartMinute_3" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                ～
						<asp:TextBox ID="JissekiRestEndHour_3" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                ：
                        <asp:TextBox ID="JissekiRestEndMinute_3" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />

                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>作業内容:</td>
                            <td>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        [作業時間]
                                    <asp:Label ID="sagyoTime" runat="server" CssClass="validaterInfo" />
                                        [未入力時間]
                                    <asp:Label ID="nisshiSagyoTimeRemain" runat="server" CssClass="validaterInfo" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="JissekiStartHour" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiStartMinute" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiEndHour" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiEndMinute" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiEndNextDayFlg" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestStartHour_1" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestStartMinute_1" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestEndHour_1" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestEndMinute_1" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestStartHour_2" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestStartMinute_2" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestEndHour_2" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestEndMinute_2" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestStartHour_3" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestStartMinute_3" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestEndHour_3" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiRestEndMinute_3" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkHour_0" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkMinute_0" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkHour_1" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkMinute_1" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkHour_2" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkMinute_2" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkHour_3" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkMinute_3" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkHour_4" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkMinute_4" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkHour_5" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkMinute_5" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkHour_6" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkMinute_6" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkHour_7" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkMinute_7" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkHour_8" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkMinute_8" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkHour_9" />
                                        <asp:AsyncPostBackTrigger ControlID="JissekiWorkMinute_9" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="JissekiProjCd_0" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="JissekiWorkCd_0" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="JissekiWorkHour_0" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                                ：
                                        <asp:TextBox ID="JissekiWorkMinute_0" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                            </td>
                                            <td>
                                                <asp:Button ID="Nokorizenbu_0" runat="server" Text="残り全部" OnClick="OnClickNokoriButton" OnClientClick="this.disabled = true; this.value = '残り全部';" UseSubmitBehavior="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="JissekiProjCd_1" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="JissekiWorkCd_1" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="JissekiWorkHour_1" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                                ：
                                        <asp:TextBox ID="JissekiWorkMinute_1" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                            </td>
                                            <td>
                                                <asp:Button ID="Nokorizenbu_1" runat="server" Text="残り全部" OnClick="OnClickNokoriButton" OnClientClick="this.disabled = true; this.value = '残り全部';" UseSubmitBehavior="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="JissekiProjCd_2" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="JissekiWorkCd_2" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="JissekiWorkHour_2" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                                ：
                                        <asp:TextBox ID="JissekiWorkMinute_2" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                            </td>
                                            <td>
                                                <asp:Button ID="Nokorizenbu_2" runat="server" Text="残り全部" OnClick="OnClickNokoriButton" OnClientClick="this.disabled = true; this.value = '残り全部';" UseSubmitBehavior="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="JissekiProjCd_3" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="JissekiWorkCd_3" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="JissekiWorkHour_3" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                                ：
                                        <asp:TextBox ID="JissekiWorkMinute_3" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                            </td>
                                            <td>
                                                <asp:Button ID="Nokorizenbu_3" runat="server" Text="残り全部" OnClick="OnClickNokoriButton" OnClientClick="this.disabled = true; this.value = '残り全部';" UseSubmitBehavior="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="JissekiProjCd_4" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="JissekiWorkCd_4" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="JissekiWorkHour_4" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                                ：
                                        <asp:TextBox ID="JissekiWorkMinute_4" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                            </td>
                                            <td>
                                                <asp:Button ID="Nokorizenbu_4" runat="server" Text="残り全部" OnClick="OnClickNokoriButton" OnClientClick="this.disabled = true; this.value = '残り全部';" UseSubmitBehavior="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="JissekiProjCd_5" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="JissekiWorkCd_5" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="JissekiWorkHour_5" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                                ：
                                        <asp:TextBox ID="JissekiWorkMinute_5" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                            </td>
                                            <td>
                                                <asp:Button ID="Nokorizenbu_5" runat="server" Text="残り全部" OnClick="OnClickNokoriButton" OnClientClick="this.disabled = true; this.value = '残り全部';" UseSubmitBehavior="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="JissekiProjCd_6" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="JissekiWorkCd_6" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="JissekiWorkHour_6" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                                ：
                                        <asp:TextBox ID="JissekiWorkMinute_6" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                            </td>
                                            <td>
                                                <asp:Button ID="Nokorizenbu_6" runat="server" Text="残り全部" OnClick="OnClickNokoriButton" OnClientClick="this.disabled = true; this.value = '残り全部';" UseSubmitBehavior="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="JissekiProjCd_7" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="JissekiWorkCd_7" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="JissekiWorkHour_7" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                                ：
                                        <asp:TextBox ID="JissekiWorkMinute_7" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                            </td>
                                            <td>
                                                <asp:Button ID="Nokorizenbu_7" runat="server" Text="残り全部" OnClick="OnClickNokoriButton" OnClientClick="this.disabled = true; this.value = '残り全部';" UseSubmitBehavior="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="JissekiProjCd_8" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="JissekiWorkCd_8" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="JissekiWorkHour_8" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                                ：
                                        <asp:TextBox ID="JissekiWorkMinute_8" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                            </td>
                                            <td>
                                                <asp:Button ID="Nokorizenbu_8" runat="server" Text="残り全部" OnClick="OnClickNokoriButton" OnClientClick="this.disabled = true; this.value = '残り全部';" UseSubmitBehavior="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="JissekiProjCd_9" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="JissekiWorkCd_9" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="JissekiWorkHour_9" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                                ：
                                        <asp:TextBox ID="JissekiWorkMinute_9" runat="server" CssClass="time checkJissekiTime" AutoPostBack="True" OnTextChanged="OnChangedTimeValue" />
                                            </td>
                                            <td>
                                                <asp:Button ID="Nokorizenbu_9" runat="server" Text="残り全部" OnClick="OnClickNokoriButton" OnClientClick="this.disabled = true; this.value = '残り全部';" UseSubmitBehavior="false" />
                                            </td>
                                        </tr>

                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="2">
                                <asp:Button ID="JissekiUpdateButton" runat="server" Text="実績を更新する" OnClick="OnClickJissekiUpdateButton" OnClientClick="this.disabled = true; this.value = '実績を更新する';" UseSubmitBehavior="false" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </div>
    </form>
</asp:Content>
