<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkPlans.aspx.cs" Inherits="KinmuSystem.View.WorkPlans" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="/css/WorkPlans.css" />
    <title>勤務予定表</title>
</head>
<body>
    <form id="form1" runat="server">

        <div class="Title">
            勤務予定表
        </div>
        <div class="WorkPlansSubTitleContainer">
            <div class="WorkPlansSubTitle">
                対象年月：<asp:Label ID="YearLabel" runat="server"></asp:Label>年<asp:Label ID="MonthLabel" runat="server"></asp:Label>月
            </div>
            <div class="WorkPlansSubTitle">
                業務機関：<asp:Label ID="CompanyNameLabel" runat="server"></asp:Label>
            </div>
            <div class="WorkPlansSubTitle">
                社員番号：<asp:Label ID="EmployeeCodeLabel" runat="server"></asp:Label>
            </div>
            <div class="WorkPlansSubTitle">
                氏名：<asp:Label ID="NameLabel" runat="server"></asp:Label>
            </div>
        </div>
        <asp:Panel ID="WorkPlansPanel" runat="server">

            <asp:Table ID="WorkPlansTable" runat="server">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Text="日付" RowSpan="2" />
                    <asp:TableHeaderCell Text="曜日" RowSpan="2" />
                    <asp:TableHeaderCell Text="予定" RowSpan="2" />
                    <asp:TableHeaderCell Text="確定" RowSpan="2" />
                    <asp:TableHeaderCell Text="予定" />
                    <asp:TableHeaderCell Text="実績" ColumnSpan="6" />
                    <asp:TableHeaderCell Text="記事欄" RowSpan="2" />
                </asp:TableHeaderRow>
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Text="始業時間～終業時間" />
                    <asp:TableHeaderCell Text="始業時間～終業時間" />
                    <asp:TableHeaderCell Text="翌日" />
                    <asp:TableHeaderCell Text="休憩時間" />
                    <asp:TableHeaderCell Text="勤務時間" />
                    <asp:TableHeaderCell Text="みなし" />
                    <asp:TableHeaderCell Text="累計" />
                </asp:TableHeaderRow>
            </asp:Table>

            <asp:Table ID="TotalTable" runat="server">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server" CssClass="TotalTableHeader">合計</asp:TableCell>
                    <asp:TableCell runat="server" CssClass="TotalTableItem"></asp:TableCell>
                    <asp:TableCell runat="server" CssClass="TotalTableItem"></asp:TableCell>
                    <asp:TableCell runat="server" CssClass="TotalTableItem"></asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <asp:Table ID="KoKyuWorkTable" runat="server">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server" CssClass="KoKyuWorkTableHeader">公休労働回数</asp:TableCell>
                    <asp:TableCell runat="server" CssClass="KoKyuWorkTableItem"></asp:TableCell>
                </asp:TableRow>
            </asp:Table>


            <div class="CalculateTableContainer">

                <div class="Check36Table1">
                    <asp:GridView ID="OverTimeGridView" runat="server" AutoGenerateColumns="False" OnRowDataBound="OverTimeGridView_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="RuleHeader" HeaderText="時間/日" ItemStyle-CssClass="Check36Header1" />
                            <asp:BoundField DataField="OvertimeHeader" HeaderText="超過勤務時間" ItemStyle-CssClass="Check36Header1" />
                            <asp:BoundField DataField="ResultTime" HeaderText="実績" ItemStyle-CssClass="Check36Item1" />
                            <asp:BoundField DataField="PredictionTime" HeaderText="予測" ItemStyle-CssClass="Check36Item1" />
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="Check36Table2">
                    <asp:GridView ID="Check36GridView" runat="server" AutoGenerateColumns="False" OnRowCreated="Check36GridView_RowCreated">
                        <Columns>
                            <asp:BoundField DataField="ResultTime" HeaderText="実績" ItemStyle-CssClass="Check36Item2" />
                            <asp:BoundField DataField="PredictionTime" HeaderText="予測" ItemStyle-CssClass="Check36Item2" />
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="CalculateTableMargin">
                    <!--　テーブル間の余白　-->
                </div>

                <div class="NenkyuTable">
                    <asp:Table ID="NenkyuTable" runat="server">
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" ColumnSpan="2" CssClass="NenkyuTableHeader">年休等確認欄</asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" CssClass="NenkyuTableHeader">年休</asp:TableCell>
                            <asp:TableCell runat="server" CssClass="NenkyuTableItem"></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" CssClass="NenkyuTableHeader">AM休</asp:TableCell>
                            <asp:TableCell runat="server" CssClass="NenkyuTableItem"></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" CssClass="NenkyuTableHeader">PM休</asp:TableCell>
                            <asp:TableCell runat="server" CssClass="NenkyuTableItem"></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:Table ID="TokukyuAndKokyuTable" runat="server">
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" CssClass="TokukyuAndKokyuTableHeader"></asp:TableCell>
                            <asp:TableCell runat="server" CssClass="TokukyuAndKokyuTableHeader">予定</asp:TableCell>
                            <asp:TableCell runat="server" CssClass="TokukyuAndKokyuTableHeader">確定</asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" CssClass="TokukyuAndKokyuTableHeader">特休日数</asp:TableCell>
                            <asp:TableCell runat="server" CssClass="TokukyuAndKokyuTableItem"></asp:TableCell>
                            <asp:TableCell runat="server" CssClass="TokukyuAndKokyuTableItem"></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" CssClass="TokukyuAndKokyuTableHeader">公休日数</asp:TableCell>
                            <asp:TableCell runat="server" CssClass="TokukyuAndKokyuTableItem"></asp:TableCell>
                            <asp:TableCell runat="server" CssClass="TokukyuAndKokyuTableItem"></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>

                <div class="WorkDaysAndTimeTable">
                    <asp:Table ID="WorkDaysAndTimeTable" runat="server" BorderStyle="Dotted">
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" CssClass="WorkDaysAndTimeTableHeader">所定労働日数・時間</asp:TableCell>
                            <asp:TableCell runat="server" CssClass="WorkDaysAndTimeTableItem"></asp:TableCell>
                            <asp:TableCell runat="server" CssClass="WorkDaysAndTimeTableItem"></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" CssClass="WorkDaysAndTimeTableHeader">法定労働日数・時間</asp:TableCell>
                            <asp:TableCell runat="server" CssClass="WorkDaysAndTimeTableItem"></asp:TableCell>
                            <asp:TableCell runat="server" CssClass="WorkDaysAndTimeTableItem"></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>

            </div>

            <asp:Label ID="HoteiRoudoCheckMessageLabel" runat="server"></asp:Label>

        </asp:Panel>
        <asp:Panel ID="ErrorPanel" runat="server" Visible="False">
            <asp:Label ID="ErrorMessageLabel" runat="server"></asp:Label>
        </asp:Panel>
    </form>
</body>
</html>
