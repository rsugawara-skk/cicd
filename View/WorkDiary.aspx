<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkDiary.aspx.cs" Inherits="KinmuSystem.View.WorkDiary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="/css/WorkDiary.css" />
    <title>作業日誌</title>
</head>
<body>
    <form id="form1" runat="server">
        <section class="sheet">
            <div class="WorkDiaryTitle">
                作業日誌
            </div>
            <div class="WorkDiarySubTitleContainer">
                <div class="WorkDiarySubTitle">
                    対象年月：<asp:Label ID="YearLabel" runat="server"></asp:Label>年<asp:Label ID="MonthLabel" runat="server"></asp:Label>月
                    

                </div>
                <div class="WorkDiarySubTitle">
                    <asp:Label ID="CompanyNameLabel" runat="server"></asp:Label>


                </div>
                <div class="WorkDiarySubTitle">
                    氏名：<asp:Label ID="NameLabel" runat="server"></asp:Label>


                </div>
            </div>

            <br />

            <asp:Panel ID="WorkDiaryPanel" runat="server">
                <asp:GridView ID="WorkDiaryGridView" runat="server" AutoGenerateColumns="False" CssClass="WorkDiaryGridView" GridLines="None" Font-Bold="False" CellPadding="0">
                    <HeaderStyle CssClass="WorkDiaryGridViewHeader" />
                    <Columns>
                        <asp:BoundField DataField="Day" HeaderText="日付" ItemStyle-CssClass="Date" HeaderStyle-CssClass="Date" />
                        <asp:BoundField DataField="ProjectName" HeaderText="プロジェクト名" ControlStyle-CssClass="ProjectName" ItemStyle-CssClass="ProjectName" HeaderStyle-CssClass="ProjectName" />
                        <asp:BoundField DataField="WorkName" HeaderText="作業内容" ControlStyle-CssClass="Work" ItemStyle-CssClass="Work" HeaderStyle-CssClass="Work" />
                        <asp:BoundField DataField="WorkTime" HeaderText="作業時間" ControlStyle-CssClass="WorkTime" ItemStyle-CssClass="WorkTime" HeaderStyle-CssClass="WorkTime" />
                    </Columns>
                </asp:GridView>

                <br />

                <div class="TotalWorkTime">
                    <asp:Label ID="TotalWorkTimeLabel" runat="server"></asp:Label>
                </div>




            </asp:Panel>
            <asp:Panel ID="ErrorPanel" runat="server" Visible="False" BorderStyle="Double">
                <asp:Label ID="ErrorMessageLabel" runat="server"></asp:Label>
            </asp:Panel>
        </section>
    </form>
</body>
</html>
