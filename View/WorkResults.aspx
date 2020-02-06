<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkResults.aspx.cs" Inherits="KinmuSystem.View.WorkResults" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="/css/WorkResults.css" />
    <title>勤務実績整理簿</title>
    <!-- スタイルシートは後ほど転機 -->
    <style>
        ul.table-row {
            font-size: 0;
            margin: 0;
            padding: 0;
        }

        li.table-cell {
            display: table-cell;
            table-layout:fixed;
            white-space: nowrap;
            font-size: 8pt;
            border-bottom: 1px solid;
            border-right: 1px solid;
            border-color: #000000;
            margin: 0;
            padding: 1px 1mm 1px 1mm
        }

        li.key-cell {
            width: 42mm;
            background-color: #bbbbbb;
        }

        li.value-cell {
            width: 12mm;
            text-align: right;
        }

        li.header-cell {
            text-align: center;
            font-weight: 700;
            background-color: #bbbbbb;
        }

        div.table-2-col {
            display: inline-block;
            border-top: 1px solid;
            border-left: 1px solid;
            border-color: #000000;
        }

        div.table-3-col {
            display: inline-block;
            border-top: 1px solid;
            border-left: 1px solid;
            border-color: #000000;
        }

        div.table-contaner {
            display: flex;
            flex-direction: row;
            justify-content: space-between;
            min-width: 267mm;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <section class="sheet">
            <!-- 帳表タイトル -->
            <div class="Title">
                勤務実績整理簿（フレックス）
            </div>

            <!-- ヘッダー情報 -->
            <div class="WorkResultsSubTitleContainer">
                <div class="WorkResultsSubTitleLong">
                    対象年月：<asp:Label ID="DateTimeStringLabel" runat="server" Text="9999年99月" />
                </div>
                <div class="WorkResultsSubTitleLong">
                    業務機関：<asp:Label ID="CompanyNameLabel" runat="server" Text="XXXXXXXXXX" />
                </div>
                <div class="WorkResultsSubTitleLong">
                    社員番号：<asp:Label ID="EmployeeCodeLabel" runat="server" Text="9999999" />
                </div>
                <div class="WorkResultsSubTitleLong">
                    氏名：<asp:Label ID="NameLabel" runat="server" Text="XXXX XXXX" />
                </div>
                <div class="WorkResultsSubTitleShort">
                    本人　印
                </div>
                <div class="WorkResultsSubTitleShort">
                    箇所長　印
                </div>
            </div>

            <asp:Panel ID="WorkResultsPanel" runat="server">
                <!-- 実績テーブル -->
                <asp:GridView ID="WorkResultsGridView" runat="server" AutoGenerateColumns="False" CssClass="WorkResultsGridView">
                    <Columns>
                        <asp:BoundField DataField="Date" HeaderText="日付" ItemStyle-CssClass="Date">
                            <ItemStyle CssClass="Date" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Day" HeaderText="曜日" ItemStyle-CssClass="Day">
                            <ItemStyle CssClass="Day" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NinsyoSchedule" HeaderText="予定" ItemStyle-CssClass="Plan">
                            <ItemStyle CssClass="Plan" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NinsyoResult" HeaderText="確定" ItemStyle-CssClass="Result">
                            <ItemStyle CssClass="Result" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OpeningTimeToClosingTimeResult" HeaderText="始業時刻　～　終業時刻" ItemStyle-CssClass="OpeningTimeToClosingTime">
                            <ItemStyle CssClass="OpeningTimeToClosingTime" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NextDayFlag" HeaderText="翌日" ItemStyle-CssClass="NextDay">
                            <ItemStyle CssClass="NextDay" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RestTime" HeaderText="休憩時間" ItemStyle-CssClass="BreakTime">
                            <ItemStyle CssClass="BreakTime" />
                        </asp:BoundField>
                        <asp:BoundField DataField="WorkTime" HeaderText="勤務時間" ItemStyle-CssClass="WorkTime">
                            <ItemStyle CssClass="WorkTime" />
                        </asp:BoundField>
                        <asp:BoundField DataField="WorkTimeTotal" HeaderText="勤務累計" ItemStyle-CssClass="WorkTimeTotal">
                            <ItemStyle CssClass="WorkTimeTotal" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Article" HeaderText="記事欄" ItemStyle-CssClass="Article">
                            <ItemStyle CssClass="Article" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>

                <!-- 各種集計テーブル -->
                <div class="table-contaner">

                    <!-- 1つ目 -->
                    <div class="table-2-col">
                        <ul class="table-row">
                            <li class="table-cell key-cell header-cell">項目</li>
                            <li class="table-cell value-cell header-cell">日数</li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">A1 所定日数</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="A1Label" Text="00" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">A2 出勤日数（休労を除く）</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="A2Label" Text="00" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">A3 休労日数</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="A3Label" Text="00" />

                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">A4 －日数</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="A4Label" Text="00" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">A5 代休日数</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="A5Label" Text="00" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">A6 非番日数</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="A6Label" Text="00" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">A7 有給日数</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="A7Label" Text="00" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">A8 無給日数</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="A8Label" Text="00" />
                            </li>
                        </ul>
                    </div>

                    <!-- 2つ目 -->
                    <div class="table-2-col">
                        <ul class="table-row">
                            <li class="table-cell key-cell header-cell">項目</li>
                            <li class="table-cell value-cell header-cell">日数</li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">B1 組休日数</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="B1Label" Text="00" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">B2 公休日労働回数</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="B2Label" Text="00" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">B3 所定労働日数</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="B3Label" Text="00" />

                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">B4 労働日数</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="B4Label" Text="00" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">B5 特休日数 予定</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="B5Label" Text="00" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">B6 特休日数 確定</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="B6Label" Text="00" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">B7 公休日数 予定</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="B7Label" Text="00" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">B8 公休日数 確定</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="B8Label" Text="00" />
                            </li>
                        </ul>
                    </div>

                    <!-- 3つ目 -->
                    <div class="table-3-col">
                        <ul class="table-row">
                            <li class="table-cell key-cell header-cell">項目</li>
                            <li class="table-cell value-cell header-cell">時：分</li>
                            <li class="table-cell value-cell header-cell">分換算</li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">C1 実総労働時間</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C1Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C1Label2" Text="00000" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">C2 みなし２（年休等）</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C2Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C2Label2" Text="00000" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">C3 超勤A</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C3Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C3Label2" Text="00000" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">C4 超勤B</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C4Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C4Label2" Text="00000" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">C5 超勤D（特休）</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C5Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C5Label2" Text="00000" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">C6 超勤D（公休）</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C6Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C6Label2" Text="00000" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">C7 夜勤C</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C7Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C7Label2" Text="00000" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">C8 控除A</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C8Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="C8Label2" Text="00000" />
                            </li>
                        </ul>
                    </div>

                    <!-- 4つ目 -->
                    <div class="table-3-col">
                        <ul class="table-row">
                            <li class="table-cell key-cell header-cell">項目</li>
                            <li class="table-cell value-cell header-cell">時：分</li>
                            <li class="table-cell value-cell header-cell">分換算</li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">D1 減額A（賃金減額時間）</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D1Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D1Label2" Text="00000" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">D2 時間外労働時間</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D2Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D2Label2" Text="00000" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">D3 経営 公休日労働時間</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D3Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D3Label2" Text="00000" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">D4 法定労働時間</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D4Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D4Label2" Text="00000" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">D5 所定労働時間</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D5Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D5Label2" Text="00000" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">D6 みなし１（出張等）</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D6Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D6Label2" Text="00000" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">D7 超勤E</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D7Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D7Label2" Text="00000" />
                            </li>
                        </ul>
                        <ul class="table-row">
                            <li class="table-cell key-cell">D8 祝日C</li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D8Label1" Text="000:00" />
                            </li>
                            <li class="table-cell value-cell">
                                <asp:Label runat="server" ID="D8Label2" Text="00000" />
                            </li>
                        </ul>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="ErrorPanel" runat="server" Visible="False">
                <asp:Label ID="ErrorMessageLabel" runat="server"></asp:Label>
            </asp:Panel>
        </section>
    </form>
</body>
</html>
