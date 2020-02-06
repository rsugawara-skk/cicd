/**
 * 個人設定画面用JavaScript
 * @version 2013/02/12 SKK三品 新規作成
 * @version 2013/02/14 SKK上原 全体的に修正
 * @version 2013/03/21 SKK上原 不具合対応(ゼロパディング用関数で引数が0の場合にゼロパディングされない)、コメント修正
 */

/*==============================================
 * グローバル定数宣言 START
 *==============================================*/

KINMU_END_FLG_TODAY = '0';		// 終業翌日フラグ：当日
KINMU_END_FLG_NEXT_DAY = '1';	// 終業翌日フラグ：翌日

/*==============================================
 * グローバル定数宣言 END
 *==============================================*/

/*==============================================
 * 汎用関数宣言 START
 *==============================================*/

/**
 * 処理中ダイアログ初期化
 */
function initProcessingDialog() {
    $('#processingDialog').dialog({

        resizable: false,
        draggable: false,
        height: 'auto',

        modal: true,
        autoOpen: false,
        closeOnEscape: false,

        open: function (event, ui) {
            $(".ui-dialog-titlebar-close").hide();	// ダイアログの右上の閉じるボタンを消す
        },
        show: {
            effect: "drop",
            direction: "up",
            duration: 500
        },
        hide: {
            effect: "drop",
            direction: "down",
            duration: 500
        }


    });
}

/**
 * 処理中ダイアログ表示
 */
function openProcessingDialog() {
    $("#processingDialog").dialog("open");
}

/**
 * 処理中ダイアログ消去
 */
function closeProcessingDialog() {
    $("#processingDialog").dialog("close");
}

/*==============================================
 * 汎用関数宣言 END
 *==============================================*/


/*==============================================
 * ページ読み込み完了時に実行するメソッド START
 *==============================================*/
$(document).ready(
    function () {
        // タブを設定
        var tabGroup = $('#tabGroup').tabs();

        // 初期表示タブを設定
        // ID決め打ちだけど、2019年度回収案件でここらへんはきれいさっぱり消し去りたいので暫定対応
        var defaultTab = $('input#ContentPlaceHolderBody_defaultTab').val();
        if (defaultTab != '') {
            tabGroup.tabs('select', '#' + defaultTab);
        }

        // 処理中ダイアログ初期化
        initProcessingDialog();

        // 時間計算
        calcDefaultTime();
    }
);

/*==============================================
 * ページ読み込み完了時に実行するメソッド END
 *==============================================*/

/*==============================================
 * 勤務時間の計算 START
 *==============================================*/

/**
 * 入力初期値の拘束時間・休憩時間(合計)・作業時間を計算する。
 */
function calcDefaultTime() {
    var kousokuTimeAsMin = 0;
    var restTimeTotalAsMin = 0;
    var sagyoTimeAsMin = 0;

    kousokuTimeAsMin = calcDefaultKousokuTimeAsMin();						// 拘束時間(エラーの場合、負の無限大)
    restTimeTotalAsMin = calcDefaultRestTimeTotalAsMin();					// 休憩時間合計(エラーの場合、負の無限大)
    sagyoTimeAsMin = calcSagyoTime(kousokuTimeAsMin, restTimeTotalAsMin);	// 作業時間(エラーの場合、負の無限大)

    setValidaterTimeMsg('span#kousokuTimeMsg', kousokuTimeAsMin);
    setValidaterTimeMsg('span#restTimeTotalMsg', restTimeTotalAsMin);
    setValidaterTimeMsg('span#sagyoTimeMsg', sagyoTimeAsMin);
}


/**
 * 時分のFrom・Toがすべて空欄か判定する。
 * @param startHour		時From
 * @param startMin		分From
 * @param endHour		時To
 * @param endMin		分To
 * @returns {Boolean}	全て空欄の場合True
 */
function isBlankTime(startHour, startMin, endHour, endMin) {
    // 全て空欄
    if (startHour == '' &&
        startMin == '' &&
        endHour == '' &&
        endMin == '') {
        return true;
    } else {
        return false;
    }
}

/**
 * 時分のFrom・Toが正当な値かチェックを行う。
 * (数値チェック、範囲チェック)
 * @param startHour		時From
 * @param startMin		分From
 * @param endHour		時To
 * @param endMin		分To
 * @returns {Boolean}	チェックOKの場合True
 */
function isInvalidTime(startHour, startMin, endHour, endMin) {
    if (
        $.isNumeric(startHour) == false ||
        $.isNumeric(startMin) == false ||
        $.isNumeric(endHour) == false ||
        $.isNumeric(endMin) == false) {
        return true;
    }
    // 範囲外の数値
    else if (
        isRangeOfHour(startHour) == false ||
        isRangeOfMinute(startMin) == false ||
        isRangeOfHour(endHour) == false ||
        isRangeOfMinute(endMin) == false) {
        return true;
    }
    else {
        return false;
    }
}


/**
 * 入力初期値のテキストボックスに入力された値を取得し、拘束時間(分)を算出する
 * エラーの場合、負の無限大を返却する
 */
function calcDefaultKousokuTimeAsMin() {
    var startHour = $('input[name="ctl00$ContentPlaceHolderBody$workStartHour"]').val();
    var startMin = $('input[name="ctl00$ContentPlaceHolderBody$workStartMinute"]').val();
    var endHour = $('input[name="ctl00$ContentPlaceHolderBody$workEndHour"]').val();
    var endMin = $('input[name="ctl00$ContentPlaceHolderBody$workEndMinute"]').val();
    var endNextDayFlg = '';

    if ($('input[name="ctl00$ContentPlaceHolderBody$workEndNextDayFlg"]').is(':checked') == true) {
        endNextDayFlg = KINMU_END_FLG_NEXT_DAY;
    } else {
        endNextDayFlg = KINMU_END_FLG_TODAY;
    }

    return calcKousokuTimeAsMin(startHour, startMin, endHour, endMin, endNextDayFlg);
}

/**
 * 引数の時分From・Toおよび翌日フラグから、拘束時間(分)を算出する
 * エラーの場合、負の無限大を返却する
 * @param startHour		時From
 * @param startMin		分From
 * @param endHour		時To
 * @param endMin		分To
 * @param endNextDayFlg	翌日フラグ
 */
function calcKousokuTimeAsMin(startHour, startMin, endHour, endMin, endNextDayFlg) {

    if (isBlankTime(startHour, startMin, endHour, endMin) == true) {
        return 0;
    } else if (isInvalidTime(startHour, startMin, endHour, endMin) == true) {
        return Number.NEGATIVE_INFINITY;
    } else {
        var startTimeAsMin = parseInt(startHour) * 60 + parseInt(startMin);
        var endTimeAsMin = parseInt(endHour) * 60 + parseInt(endMin);

        // 勤務時間の場合、翌日フラグが設定されていれば終了時間に24時間プラス
        if (endNextDayFlg == KINMU_END_FLG_NEXT_DAY) {
            endTimeAsMin += (24 * 60);
        }

        return endTimeAsMin - startTimeAsMin;
    }
}


/**
 * 休憩時間(分)の合計を算出する
 * エラーの場合、負の無限大を返却する
 */
function calcDefaultRestTimeTotalAsMin() {
    var restTimeTotalAsMin = 0;

    // 実績:休憩時間
    for (var i = 0; i < 3; i++) {

        var suffix = i + 1;

        var restStartHour = $('input[name="ctl00$ContentPlaceHolderBody$restStartHour_' + suffix + '"]').val();
        var restStartMin = $('input[name="ctl00$ContentPlaceHolderBody$restStartMinute_' + suffix + '"]').val();
        var restEndHour = $('input[name="ctl00$ContentPlaceHolderBody$restEndHour_' + suffix + '"]').val();
        var restEndMin = $('input[name="ctl00$ContentPlaceHolderBody$restEndMinute_' + suffix + '"]').val();

        var workEndNextDayFlg;
        if ($('input[name="ctl00$ContentPlaceHolderBody$workEndNextDayFlg"]').is(':checked') == true) {
            workEndNextDayFlg = KINMU_END_FLG_NEXT_DAY;
        } else {
            workEndNextDayFlg = KINMU_END_FLG_TODAY;
        }


        // 実績:休憩時間【計算】
        var restTimeAsMin = calcSettingRestTimeAsMin(restStartHour, restStartMin, restEndHour, restEndMin, workEndNextDayFlg);

        if (restTimeAsMin < 0) {
            return Number.NEGATIVE_INFINITY;
        } else {
            restTimeTotalAsMin += restTimeAsMin;
        }
    }

    return restTimeTotalAsMin;
}

/**
 * 1行毎の休憩時間(分)を算出する
 * エラーの場合、負の無限大を返却する
 * @param restStartHour		休憩時間_時From
 * @param restStartMin		休憩時間_分From
 * @param restEndHour		休憩時間_時To
 * @param restEndMin		休憩時間_分To
 * @param workEndNextDayFlg	翌日フラグ
 * @returns	休憩時間(分)
 */
function calcSettingRestTimeAsMin(restStartHour, restStartMin, restEndHour, restEndMin, workEndNextDayFlg) {

    if (isBlankTime(restStartHour, restStartMin, restEndHour, restEndMin) == true) {
        return 0;
    } else if (isInvalidTime(restStartHour, restStartMin, restEndHour, restEndMin) == true) {
        return Number.NEGATIVE_INFINITY;
    } else {
        var startTimeAsMin = parseInt(restStartHour) * 60 + parseInt(restStartMin);
        var endTimeAsMin = parseInt(restEndHour) * 60 + parseInt(restEndMin);

        // 休憩時間の場合、翌日フラグが設定されていて、かつ開始終了の差がマイナスなら終了時間に24時間プラス(日跨り休憩)
        if (workEndNextDayFlg == KINMU_END_FLG_NEXT_DAY && startTimeAsMin > endTimeAsMin) {
            endTimeAsMin += 24 * 60;
        }

        return endTimeAsMin - startTimeAsMin;
    }
}

/**
 * 拘束時間・休憩時間合計から作業時間を算出する。
 * エラーの場合、負の無限大を返却する
 * @param kousokuTimeAsMin		拘束時間(分)
 * @param restTimeTotalAsMin	休憩時間合計(分)
 */
function calcSagyoTime(kousokuTimeAsMin, restTimeTotalAsMin) {
    // 作業時間 (拘束時間 - 休憩時間合計)
    if (kousokuTimeAsMin >= 0 && restTimeTotalAsMin >= 0) {
        return kousokuTimeAsMin - restTimeTotalAsMin;
    } else {
        return Number.NEGATIVE_INFINITY;
    }
}

/**
 * 入力初期値のテキストボックスに設定された値に合わせて、バリデーターメッセージを画面に設定する。
 * 引数の時間(分)がマイナス値(エラー(負の無限大)の場合を含む)の場合、文字色を警告色(赤)にする。
 * @param controlName	メッセージ設定対象コントロール
 * @param timeAsMin		時間(分)
 */
function setValidaterTimeMsg(controlName, timeAsMin) {
    var control = $(controlName);
    var timeMsg = convertMinToHM(timeAsMin);

    control.text(timeMsg);

    // 時間(分)の値が負の場合、文字色を警告色(赤)にする。
    if (timeAsMin < 0) {
        control.removeClass('validaterInfo');
        control.addClass('validaterError');
    } else {
        control.removeClass('validaterError');
        control.addClass('validaterInfo');
    }
}

/**
 * 分を「99:99」形式の文字列に変換します。
 * 負の値の場合は「-99:99」形式となります。
 * 引数が数値でない場合、または負の無限大の場合、「--:--」とします
 * @param min			分
 * @returns {String}	「99:99」形式の文字列
 */
function convertMinToHM(min) {
    if ($.isNumeric(min) == false || min == Number.NEGATIVE_INFINITY) {
        return '--:--';
    } else if (min < 0) {
        var tempMin = parseInt(min) * -1;
        return '-' + ZeroPadding2(parseInt(tempMin / 60)) + ':' + ZeroPadding2(tempMin % 60);
    } else {
        var tempMin = parseInt(min);
        return ZeroPadding2(parseInt(tempMin / 60)) + ':' + ZeroPadding2(tempMin % 60);
    }

}

/**
 * hourが0～23の範囲かどうかをチェックして結果を返却する関数。
 * @param hour	時
 * @returns {Boolean}	true:範囲内　false:範囲外
 */
function isRangeOfHour(hour) {
    if (hour >= 0 && hour < 24) {
        return true;
    } else {
        return false;
    }
}

/**
 * minuteが0～59の範囲かどうかをチェックして結果を返却する関数。
 * @param minute	分
 * @returns {Boolean}	true:範囲内　false:範囲外
 */
function isRangeOfMinute(minute) {
    if (minute >= 0 && minute < 60) {
        return true;
    } else {
        return false;
    }
}

/**
 * 引数の数値に対してゼロパディングを行う
 * 返す値は2文字
 * @param number	数値
 */
function ZeroPadding2(number) {
    return ('00' + String(number)).slice(-2);
}

/**
 * 引数の数値に対して、数値の場合、ZeroPadding2関数を適用する
 * 数値でない場合は元の値を返す
 * @param value		数値または文字列
 */
function ZeroPadding2IfNumeric(value) {
    if ($.isNumeric(value)) {
        return ZeroPadding2(value);
    } else {
        return value;
    }
}

/*==============================================
 * 勤務時間の計算 END
 *==============================================*/


/*==============================================
 * サーブレット呼び出し処理 START
 *==============================================*/

/**
 * 入力初期値の更新処理呼び出し
 */
function submitInitValueUpdate() {
    openProcessingDialog();
    var formObj = $('form[name=initValueUpdateForm]');
    formObj.submit();
}

/**
 * 利用コードの更新処理呼び出し
 */
function submitCodeUpdate() {
    openProcessingDialog();
    var formObj = $('form[name=codeUpdateForm]');
    formObj.submit();
}

/**
 * 利用コード：プロジェクトコード一覧の編集処理(↑、↓、追加、削除)呼び出し
 * @param action	編集処理種別
 */
function submitProjCodeEdit(action) {
    openProcessingDialog();
    var formObj = $('form[name=projCodeEditForm]');

    var input = document.createElement('input');
    input.setAttribute('type', 'hidden');
    input.setAttribute('name', 'action');
    input.setAttribute('value', action);
    formObj.append(input);

    formObj.submit();
}

/**
 * 利用コード：作業コード一覧の編集処理(↑、↓、追加、削除)呼び出し
 * @param action	編集処理種別
 */
function submitSagyoCodeEdit(action) {
    openProcessingDialog();
    var formObj = $('form[name=sagyoCodeEditForm]');

    var input = document.createElement('input');
    input.setAttribute('type', 'hidden');
    input.setAttribute('name', 'action');
    input.setAttribute('value', action);
    formObj.append(input);

    formObj.submit();
}

/*==============================================
 * サーブレット呼び出し処理 END
 *==============================================*/

//画面遷移スクリプト
function jumpUrl(url) {
    openProcessingDialog();
    location.href = url;
}

