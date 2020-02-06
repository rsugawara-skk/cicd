/**
 * 勤務一覧画面用JavaScript
 * @version 2012/10/25 SKK上原 新規作成
 * @version 2012/11/22 SKK西島 コピー・クリア関連追加
 * @version 2012/02/07 SKK西島 PDF出力関連追加
 * @version 2013/03/21 SKK上原 不具合対応(ゼロパディング用関数で引数が0の場合にゼロパディングされない)、コメント修正
 * @version 2013/04/09 SKK西島 不具合対応(半角スペースの文字化け”?”対応)
 * @version 2013/08/23 SKK田代 勤務予定コード追加, 同項目changeイベント実装、同項目関連の関数を修正
 */

/*==============================================
 * グローバル定数宣言 START
 *==============================================*/

// IE関連
// オブジェクトのreadyState 
READYSTATE_COMPLETE = 'complete';		// 全データ読込完了

// 勤務関連
KAKUNIN_FLG_REGISTERED = '1';	// 確認フラグ：登録済

KINMU_END_FLG_TODAY = '0';		// 終業翌日フラグ：当日
KINMU_END_FLG_NEXT_DAY = '1';	// 終業翌日フラグ：翌日

KINMU_NINSHO_SHUKKIN = '01';
KINMU_NINSHO_KENSHU = '03';
KINMU_NINSHO_SHOGAI = '25';

KINMU_CALC_PTN_SHUKKIN = '10';
KINMU_CALC_PTN_KENSHU = '11';

KINMU_CALC_PTN_SHUTCHO = '12';	//出張類

KINMU_CALC_PTN_AM_HALF = '31';	//AM半休
KINMU_CALC_PTN_PM_HALF = '32';	//PM半休

KINMU_CALC_PTN_NENKYU = '30';	// 年休類（時間・記事欄入力不可）
KINMU_CALC_PTN_MUKYU = '50';	// 無給（時間入力不可）

KINMU_CALC_PTN_YUKYU_KIJI = '40';	// 有給（記事欄のみ入力可能）
KINMU_CALC_PTN_MUKYU_KIJI = '52';	// 無給（記事欄のみ入力可能）

KINMU_CALC_PTN_YUKYU_MINASHI_CALC = '41';	//有給（時間入力可能、みなし減算処理対象）

KINMU_MINASHI_CD_SHUKKIN = '01';	//勤務予定コード　出勤
KINMU_MINASHI_CD_NENKYU = '19';		//勤務予定コード　年休
KINMU_MINASHI_CD_AM_KYU = '20';		//勤務予定コード　AM半休
KINMU_MINASHI_CD_PM_KYU = '21';		//勤務予定コード　PM半休

KINMU_EVENT_PTN_INIT = '01';		//初期表示
KINMU_EVENT_PTN_MINASHI_CD = '02';	//勤務予定コード変更

/*==============================================
 * グローバル定数宣言 END
 *==============================================*/

/*==============================================
 * グローバル変数宣言 START
 *==============================================*/

/* コピー元日付保持変数 */
updateSourceDate = '';

/*==============================================
 * グローバル変数宣言 END
 *==============================================*/

/*==============================================
 * 汎用関数宣言 START
 *==============================================*/

/**
 * 処理中ダイアログ初期化
 */
function initProcessingDialog(){
	$('#processingDialog').dialog({
		
		resizable: false,
		draggable: false,
		height: 'auto',
		
		modal: true,
		autoOpen: false,
		closeOnEscape: false,
		
		open: function(event, ui){
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
function openProcessingDialog(){
	$("#processingDialog").dialog("open");
}

/**
 * 処理中ダイアログ消去
 */
function closeProcessingDialog(){
	$("#processingDialog").dialog("close");
}

/**
 * 処理中ダイアログ消去(処理完了後、PDF要求向け)
 */
function closeProcessingDialogAfterComplete() {
	
	if (document.readyState == READYSTATE_COMPLETE) {
		// 読み込み完了したら処理中ダイアログを閉じて、メソッド終了
		closeProcessingDialog();
	} else {
		// 読み込み完了してなかったら、次のメソッド呼び出しの実行予約(500ミリ秒後)
		setTimeout(function(){
			closeProcessingDialogAfterComplete();
		}, 500);
	}
}

/**
 * 引数の数値に対してゼロパディングを行う
 * 返す値は2文字
 * @param number	数値
 */
function ZeroPadding2(number){
	return ('00' + String(number)).slice(-2);
}

/**
 * 引数の数値に対して、数値の場合、ZeroPadding2関数を適用する
 * 数値でない場合は元の値を返す
 * @param value		数値または文字列
 */
function ZeroPadding2IfNumeric(value){
	if ($.isNumeric(value)) {
		// 数値
		return ZeroPadding2(value);
	} else {
		return value;
	}
}

/*==============================================
 * 汎用関数宣言 END
 *==============================================*/

/*==============================================
 * ページ読み込み完了時に実行するメソッド START
 *==============================================*/
$(document).ready(
	function() {
		// ボタンの見た目を設定
		$('#editModeList').buttonset();
		$('#pdfList').buttonset();
		
		// 年月選択ボタン活性状態設定
		initTargetYMButton();

		// #kinmuTableが存在する場合
		if ($('#kinmuTable').size() > 0) {
			initKinmuTable();	// 勤務テーブル初期化
			changeEditMode();	// 編集モード(カラム表示)を初期設定
		}
		
		// 処理中ダイアログ初期化
		initProcessingDialog();
	}
);

/*==============================================
 * ページ読み込み完了時に実行するメソッド END
 *==============================================*/

/*==============================================
 * 勤務一覧初期表示に関するスクリプト START
 *==============================================*/

/**
 * 勤務一覧初期化
 */
function initKinmuTable(){

	// jQueryプラグイン「DataTables」を適用
	var oTable = $('#kinmuTable').dataTable({
		'bPaginate' : false,		// ページングOFF
		'bLengthChange' : false,	// カラム幅手動調整OFF
		'bFilter' : false,			// フィルター検索機能OFF
		'bSort' : false,			// ソート機能OFF
		'bInfo' : false,			// テーブル情報(テーブル下に表示される表示行数)OFF
		'bAutoWidth' : false		// 列幅自動調節機能OFF (fnAdjustColumnSizing関数に任せるので)
	});

	// 行ヘッダーを固定するjQueryプラグイン「FixedHeader」を適用
	new FixedHeader(oTable);

	// 偶数行にのみCSS用のクラスを適用（背景色を変える為）
	// $('table.kinmuTableStyle tbody tr:even').addClass('evenRow');

	// 勤務リストの行にツールチップ(trタグのtitleに指定された文字を表示する)を適用する
	$('tr.kinmuDataRow').tooltip({
		// ツールチップ表示位置 対象要素(行)の右端中央に対して、ツールチップの左端中央を合わせる
		position : {
			my : 'left center',
			at : 'right+10 center'
		},
		// 表示エフェクト
		show : {
			effect : 'slide',
			duration : 200
		},
		// 非表示エフェクト:オフ
		hide : false
	});

	/* 予定・実績編集用イベントハンドラ追加
	 * 行ダブルクリックで編集フィールド表示 */
	$('tr.kinmuDataRow').live(
		'dblclick',
		function() {
			// 行を取得する
			if (oTable.fnIsOpen(this)) {
				/* 編集行が開いていた場合、閉じる */
				closeEditRow(oTable, this);
			} else {
				// 編集行を開く
				openEditRow(oTable, this);
			}
		}
	);
	
	/* コピー機能用イベントハンドラ追加
	 * ラジオボタンがチェックされた行のチェックボックスは無効 */
	$('#kinmuTable tbody input[name="updateSource"]').change(
		function() {

			// 親要素(行)取得
			var nTr = $(this).parents('tr')[0];

			// 行の子要素であるチェックボックスを取得
			var chkBox = $('input[name="updateTarget"]', nTr);

			if ($(this).is(':checked')) {
				chkBox.removeAttr('checked');
				$('input[name="updateTarget"]').removeAttr('disabled');
				chkBox.attr('disabled', true);
			}
		}
	);
	
	/* 予定コピー機能用イベントハンドラ追加
	 * ラジオボタンがチェックされた行のチェックボックスは無効 */
	$('#kinmuTable tbody input[name="yoteiupdateSource"]').change(
		function() {

			// 親要素(行)取得
			var nTr = $(this).parents('tr')[0];

			// 行の子要素であるチェックボックスを取得
			var chkBox = $('input[name="yoteiupdateTarget"]', nTr);

			if ($(this).is(':checked')) {
				chkBox.removeAttr('checked');
				$('input[name="yoteiupdateTarget"]').removeAttr('disabled');
				chkBox.attr('disabled', true);
			}
		}
	);
	
	// 登録時にエラーがあった場合、編集していたデータを表示する
	openEditErrorRow(oTable);
}

/**
 * 勤務更新エラー時用の勤務編集行オープン処理
 * @param oTable	勤務一覧テーブル
 */
function openEditErrorRow(oTable) {
	var dayEditError = '';
	
	if($('table#errorYoteiPostData').size() == 1){
		dayEditError = $('table#errorYoteiPostData').find('td.colDay').text();
	}else if($('table#errorJissekiPostData').size() == 1){
		dayEditError = $('table#errorJissekiPostData').find('td.colDay').text();
	}
	
	if(dayEditError != '') {
		var tdDay = $('#kinmuTable').find('td.colDay');
		for (var i=0; i<tdDay.size(); i++) {
			if($(tdDay[i]).text() == dayEditError){
				var kinmuDataRow = $(tdDay[i]).parent('tr.kinmuDataRow');
				openEditRow(oTable, kinmuDataRow[0]);
				break;
			}
		}
	}
}


/*==============================================
 * 勤務一覧初期表示に関するスクリプト END
 *==============================================*/

/*==============================================
 * 表示年月・社員の機能に関するスクリプト START
 *==============================================*/

/**
 * 年月選択ボタン(前月・翌月)の活性状態設定
 * class「invalid」が設定されていたら非活性にする
 * ※class「invalid」は現在年月・集約年月を考慮してJSPで設定される。
 */
function initTargetYMButton(){
	// 前月ボタン
	if ($('a#prevMonth').size() > 0) {
		// アイコンを設定
		var btn = $('a#prevMonth');
		btn.button({
			icons : {
				primary : 'ui-icon-triangle-1-w'
			},
			text : false
		});

		// class「invalid」が設定されていたら非活性にする
		if (btn.hasClass('invalid')) {
			btn.button('disable');
		}
	}

	// 翌月ボタン
	if ($('a#nextMonth').size() > 0) {
		// アイコンを設定
		var btn = $('a#nextMonth');
		btn.button({
			icons : {
				primary : 'ui-icon-triangle-1-e'
			},
			text : false
		});

		// class「invalid」が設定されていたら非活性にする
		if (btn.hasClass('invalid')) {
			btn.button('disable');
		}
	}
}


/**
 * 表示対象社員・年月　要求
 * 社員コードはドロップダウンリストのValueから取得し、
 * 対象年月は引数から取得する。
 * @param targetYM 対象年月
 */
function submitTargetShainYM(targetYM) {
	openProcessingDialog();
	
	// フォームの生成
	var form = document.createElement('form');
	document.body.appendChild(form);

	// 対象年月の設定
	var input = document.createElement('input');
	input.setAttribute('type', 'hidden');
	input.setAttribute('name', 'targetYM');
	input.setAttribute('value', targetYM);
	form.appendChild(input);

	// 対象社員コードの設定
	// (ドロップダウンリストがある場合)
	if ($('select[name=shainDropDownList]').size() > 0) {
		var targetShainCd = $('select[name=shainDropDownList] option:selected').val();
		var input2 = document.createElement('input');
		input2.setAttribute('type', 'hidden');
		input2.setAttribute('name', 'targetShainCd');
		input2.setAttribute('value', targetShainCd);
		form.appendChild(input2);
	}

	form.setAttribute('action', './KinmuListServlet');
	form.setAttribute('method', 'post');
	form.submit();
}

/*==============================================
 * 表示年月・社員の機能に関するスクリプト END
 *==============================================*/


/*==============================================
 * PDF機能に関するスクリプト START
 *==============================================*/

/**
 * PDF出力【勤務予定出力】
 * @param targetShainCd		社員コード
 * @param targetYM			対象年月
 */
function kinmuYoteiPdf(targetShainCd, targetYM) {
	openProcessingDialog();
	goToPdfServlet(targetShainCd, targetYM, './KinmuYoteiPDFServlet');
	closeProcessingDialogAfterComplete();
}

/**
 * PDF出力【勤務実績出力】
 * @param targetShainCd		社員コード
 * @param targetYM			対象年月
 */
function kinmuJissekiPdf(targetShainCd, targetYM) {
	openProcessingDialog();
	goToPdfServlet(targetShainCd, targetYM, './KinmuJissekiPDFServlet');
	closeProcessingDialogAfterComplete();
}

/**
 * PDF出力【作業日誌出力】
 * @param targetShainCd		社員コード
 * @param targetYM			対象年月
 */
function sagyoNisshiPdf(targetShainCd, targetYM) {
	openProcessingDialog();
	goToPdfServlet(targetShainCd, targetYM, './SagyoNisshiPDFServlet');
	closeProcessingDialogAfterComplete();
}

/**
 * PDF出力用サーブレットの呼び出し
 * @param targetShainCd		社員コード
 * @param targetYM			対象年月
 * @param servletName		呼び出すサーブレットのURL
 */
function goToPdfServlet(targetShainCd, targetYM, servletName){
	var form = document.createElement('form');
	document.body.appendChild(form);

	var input1 = document.createElement('input');
	input1.setAttribute('type', 'hidden');
	input1.setAttribute('name', 'targetYM');
	input1.setAttribute('value', targetYM);
	
	var input2 = document.createElement('input');
	input2.setAttribute('type', 'hidden');
	input2.setAttribute('name', 'targetShainCd');
	input2.setAttribute('value', targetShainCd);
	
	form.appendChild(input1);
	form.appendChild(input2);
	
	form.setAttribute('action', servletName);
	form.setAttribute('method', 'get');				//servlet側がgetのみ許可なので。。。
	form.submit();
}

/*==============================================
 * PDF機能に関するスクリプト END
 *==============================================*/

/*==============================================
 * コピー・クリア機能に関するスクリプト START
 *==============================================*/

/**
 * コピー元日付ラジオボタン選択時イベント
 * @param obj	コピー元日付ラジオボタン
 */
function updateSourceRdo_Change(obj) {
	updateSourceDate = obj.value;
}

/**
 * 勤務一覧の作業時間・コピー・クリアのカラム表示を切り替える
 */
function changeEditMode(){
	// 編集モード(作業時間表示)
	if($('#editMode').is(':checked')){
		$('.colGroupYoteiCopy').hide();
		$('.colGroupWorkTime').show();
		$('.colGroupCopy').hide();
		$('.colGroupClear').hide();
		
		// 実行ボタンを非活性にする
		$('#execCopyOrClear').attr("disabled", "disabled");
		$('#execCopyOrClear').removeClass("ui-state-highlight");
	}
	// 予定コピーモード
	else if($('#yoteicopyMode').is(':checked')){
		$('.colGroupYoteiCopy').show();
		$('.colGroupWorkTime').hide();
		$('.colGroupCopy').hide();
		$('.colGroupClear').hide();
		
		// 実行ボタンを活性にする
		$('#execCopyOrClear').removeAttr("disabled");
		$('#execCopyOrClear').addClass("ui-state-highlight");
	}
	// 実績コピーモード
	else if($('#copyMode').is(':checked')){
		$('.colGroupYoteiCopy').hide();
		$('.colGroupWorkTime').hide();
		$('.colGroupCopy').show();
		$('.colGroupClear').hide();
		
		// 実行ボタンを活性にする
		$('#execCopyOrClear').removeAttr("disabled");
		$('#execCopyOrClear').addClass("ui-state-highlight");
	}
	// クリアモード
	else if($('#clearMode').is(':checked')){
		$('.colGroupYoteiCopy').hide();
		$('.colGroupWorkTime').hide();
		$('.colGroupCopy').hide();
		$('.colGroupClear').show();
		
		// 実行ボタンを活性にする
		$('#execCopyOrClear').removeAttr("disabled");
		$('#execCopyOrClear').addClass("ui-state-highlight");
	}
	
	// 表の幅を再設定
	var oTable = $('#kinmuTable').dataTable();
	oTable.fnAdjustColumnSizing();
}

/**
 * コピーまたはクリアの実行ボタン押下時に呼び出されるメソッド
 * コピー・クリア処理サーブレットを呼び出す。
 * @param shain			社員コード
 * @param targetMonth	対象年月
 */
function execCopyOrClear(shain, targetMonth){
	if($('#copyMode').is(':checked')){
		execCopy(shain, targetMonth);
	}
	else if($('#yoteicopyMode').is(':checked')){
		execYoteiCopy(shain, targetMonth);
	}
	else if($('#clearMode').is(':checked')){
		execClear(shain, targetMonth);
	}
}


/**
 * コピー実行
 * @param shain			社員コード
 * @param targetMonth	対象年月
 */
function execCopy(shain, targetMonth) {
	if (updateSourceDate == "") {
		alert("コピー元日付が選択されていません。コピー元日付を選択してください。");
		return;
	}

	var copyTargetDays = getTargetDays("updateTarget");
	if (copyTargetDays == "") {
		alert("コピー先日付が選択されていません。1件以上選択してください。");
		return;
	}
	
	if(!window.confirm(updateSourceDate + '日の勤務実績を選択した日付へコピーします。よろしいですか？')){
		return;
	}
	
	// コピーメソッドの呼び出し
	openProcessingDialog();
	submitCopy(shain, targetMonth, updateSourceDate, copyTargetDays);
}

/**
 * 予定コピー実行
 * @param shain			社員コード
 * @param targetMonth	対象年月
 */
function execYoteiCopy(shain, targetMonth) {
	if (updateSourceDate == "") {
		alert("コピー元日付が選択されていません。コピー元日付を選択してください。");
		return;
	}

	var yoteicopyTargetDays = getTargetDays("yoteiupdateTarget");
	if (yoteicopyTargetDays == "") {
		alert("コピー先日付が選択されていません。1件以上選択してください。");
		return;
	}
	
	if(!window.confirm(updateSourceDate + '日の勤務予定を選択した日付へコピーします。よろしいですか？')){
		return;
	}
	
	// コピーメソッドの呼び出し
	openProcessingDialog();
	submitYoteiCopy(shain, targetMonth, updateSourceDate, yoteicopyTargetDays);
}

/**
 * コピー処理サーブレット呼び出し
 * @param copyShainCd		社員コード
 * @param copySourceMonth	対象年月
 * @param updateSourceDate	コピー元日付
 * @param copyTargetDays	コピー先日付
 */
function submitCopy(copyShainCd, copySourceMonth, updateSourceDate, copyTargetDays) {
	var form = document.createElement('form');
	document.body.appendChild(form);

	// 社員コード、対象年月、コピー元日付、コピー先日付をコロン(:)区切りで文字列結合する。
	// コピー元日付はさらにカンマ（,）区切りになっています。
	// （例）社員コード=1234567、対象年月=2012年7月、コピー元日付=2日、コピー先日付=4、10、15日の場合
	// ⇒1234567:201207:2:4,10,15
	var copySourceData = copyShainCd + ":" + copySourceMonth + ":"
			+ updateSourceDate + ":" + copyTargetDays;

	// サーブレット送信データセット
	var input = document.createElement('input');
	input.setAttribute('type', 'hidden');
	input.setAttribute('name', 'copySourceData');
	input.setAttribute('value', copySourceData);

	form.appendChild(input);
	form.setAttribute('action', './KinmuCopyServlet');
	form.setAttribute('method', 'post');
	form.submit();
}

/**
 * 予定コピー処理サーブレット呼び出し
 * @param copyShainCd		社員コード
 * @param copySourceMonth	対象年月
 * @param updateSourceDate	コピー元日付
 * @param copyTargetDays	コピー先日付
 */
function submitYoteiCopy(copyShainCd, copySourceMonth, updateSourceDate, copyTargetDays) {
	var form = document.createElement('form');
	document.body.appendChild(form);

	// 社員コード、対象年月、コピー元日付、コピー先日付をコロン(:)区切りで文字列結合する。
	// コピー元日付はさらにカンマ（,）区切りになっています。
	// （例）社員コード=1234567、対象年月=2012年7月、コピー元日付=2日、コピー先日付=4、10、15日の場合
	// ⇒1234567:201207:2:4,10,15
	var copySourceData = copyShainCd + ":" + copySourceMonth + ":"
			+ updateSourceDate + ":" + copyTargetDays;

	// サーブレット送信データセット
	var input = document.createElement('input');
	input.setAttribute('type', 'hidden');
	input.setAttribute('name', 'copySourceData');
	input.setAttribute('value', copySourceData);

	form.appendChild(input);
	form.setAttribute('action', './KinmuYoteiCopyServlet');
	form.setAttribute('method', 'post');
	form.submit();
}

/**
 * クリア実行
 * @param shain			社員コード
 * @param targetMonth	対象年月
 */
function execClear(shain, targetMonth) {

	var clearTargetDays = getTargetDays("clearTarget");
	if (clearTargetDays == "") {
		alert("クリア対象の日付が選択されていません。1件以上選択してください。");
		return;
	}
	
	if(!window.confirm('選択した日付の勤務実績をクリアします。よろしいですか？')){
		return;
	}
	// クリアメソッドの呼び出し
	openProcessingDialog();
	submitClear(shain, targetMonth, clearTargetDays);
}

/**
 * クリア処理サーブレット呼び出し
 * @param clearShainCd		社員コード
 * @param clearSourceMonth	対象年月
 * @param clearTargetDays	クリア日付
 */
function submitClear(clearShainCd, clearSourceMonth, clearTargetDays) {

	var form = document.createElement('form');
	document.body.appendChild(form);


	// 社員コード、対象年月、クリア日付をコロン(:)区切りで文字列結合する。
	// クリア日付はさらにカンマ（,）区切りになっています。
	// （例）社員コード=1234567、対象年月=2012年7月、クリア日付=4、10、15日の場合
	// ⇒1234567:201207:4,10,15
	var clearSourceData = clearShainCd + ":" + clearSourceMonth + ":" + clearTargetDays;

	// サーブレット送信データセット
	var input = document.createElement('input');
	input.setAttribute('type', 'hidden');
	input.setAttribute('name', 'clearSourceData');
	input.setAttribute('value', clearSourceData);

	form.appendChild(input);
	form.setAttribute('action', './KinmuClearServlet');
	form.setAttribute('method', 'post');
	form.submit();

}

/**
 * コピー・クリア処理の対象日付を取得する
 * @param targetContrl	コピーまたはクリアのチェックボックス
 * @returns {String}
 */
function getTargetDays(targetContrl) {
	// チェックボックスをすべて取得
	var checkBox = document.getElementsByName(targetContrl);
	
	// チェックONの日付取得用
	var targetDays = "";

	// チェックONの日付のみ取り出す
	for ( var i = 0; i < checkBox.length; i++) {
		if (checkBox[i].checked) {
			// 変数targetDaysが空でない時、区切りのカンマを入れる
			if (targetDays != "") {
				targetDays = targetDays + ",";
			}
			targetDays = targetDays + checkBox[i].value;
		}
	}
	return targetDays;
}
/*==============================================
 * コピー・クリア機能に関するスクリプト END
 *==============================================*/

/*==============================================
 * 予定・実績登録の機能に関するスクリプト START
 *==============================================*/

/**
 * 勤務編集行を開く(厳密にいうと生成する)
 * @param oTable		勤務一覧テーブル
 * @param kinmuDataRow	勤務一覧の編集対象行
 */
function openEditRow(oTable, kinmuDataRow){
	// すでに開いている編集行を閉じる
	// ※うまく動かないのでコメントアウト・・・
	// closeAnotherEditRow(oTable);
	
	// 隠しフィールドの入力フォーマット配下を取得、複製
	var sOut = $('div#editRowTemplate').clone().html();
	
	// 編集行生成
	// css(div.editFieldクラス) の関係でこの段階では非表示
	var openedRow = oTable.fnOpen(kinmuDataRow, sOut, 'editTD');

	// 編集対象行と編集フィールドの見た目を設定
	$(kinmuDataRow).addClass('editTargetRow');
	$(openedRow).addClass('editFieldRow');
	
	// 編集フィールドに値を設定
	setEditRowInitValue(kinmuDataRow, openedRow);
	
	// 予定・実績の更新ボタンにクリックイベント付与
	var day = $('td.colDay', kinmuDataRow).text();
	var yoteiUpdateForm = $('form[name=yoteiUpdateForm]', openedRow);
	$(yoteiUpdateForm).attr("name", "yoteiUpdateForm_" + day);	// formのname属性を変更
	$(yoteiUpdateForm).find("input[name=yoteiUpdate]").click(function(){
        yoteiUpdateSubmit(day);
    });
	var jissekiUpdateForm = $('form[name=jissekiUpdateForm]', openedRow);
	$(jissekiUpdateForm).attr("name", "jissekiUpdateForm_" + day);	// formのname属性を変更
	$(jissekiUpdateForm).find("input[name=jissekiUpdate]").click(function(){
        jissekiUpdateSubmit(day);
    });
	
	/* 勤務予定コードイベントハンドラ追加
	 * ラジオボタンがチェックされた行のチェックボックスは無効 */
	$('input[name=yoteiMinashiCd]').change(function() {
				setEditRowYoteiMinashiCd(kinmuDataRow, openedRow);
				setEditRowYoteiValue(kinmuDataRow, openedRow, KINMU_EVENT_PTN_MINASHI_CD);
	});
	
	// ちょっとトリッキーなやり方で行をなめらかに表示
	// (td要素に対してアニメーションをかけてもうまくいかないので、tdの内部に配置したDivに対して行う)
	$('div.editField', openedRow).hide();			// まずDivを非表示にしておく
	$('div.editField', openedRow).slideDown(500);	// 表示する
	
	oTable.fnAdjustColumnSizing();	// 表の幅を再設定
}

/**
 * 勤務編集行を閉じる(厳密にいうと破棄する)
 * @param oTable		勤務一覧テーブル
 * @param kinmuDataRow	勤務一覧の編集対象行
 */
function closeEditRow(oTable, kinmuDataRow){

	// 自分の要素のすぐ次にある要素(編集用行)を取得する。
	var openedRow = $(kinmuDataRow).next('tr');

	// アニメーションでクローズ
	$('div.editField', openedRow).slideUp(500);

	// 以後の処理は時間(アニメーション分)を待ってから
	setTimeout(function() {
		$('div.editField', openedRow).hide(); 

		// 見た目を解除
		$(kinmuDataRow).removeClass('editTargetRow');
		$(openedRow).removeClass('editFieldRow');
	
		// 編集行自体を破棄
		oTable.fnClose(kinmuDataRow);
	
		oTable.fnAdjustColumnSizing();	// 表の幅を再設定
	}, 500);

}

/**
 * ※未使用メソッド
 * すでに開かれている編集行があった場合、閉じる(破棄する)
 * @param oTable	勤務一覧テーブル
 */
function closeAnotherEditRow(oTable) {
	if($('tr.editTargetRow').size() > 0){
		closeEditRow(oTable, 'tr.editTargetRow');
	}
}

/**
 * 勤務編集欄初期設定
 * @param kinmuDataRow	勤務一覧の編集対象行
 * @param openedRow		勤務編集用行
 */
function setEditRowInitValue(kinmuDataRow, openedRow){
	// 日付
	var day = $('td.colDay', kinmuDataRow).text();
	
	// 隠しフィールド
	$(openedRow).find('input[name=day]').val(day);
	
	// 値の設定
	setEditRowYoteiValue(kinmuDataRow, openedRow, KINMU_EVENT_PTN_INIT);
	setEditRowYoteiMinashiCd(kinmuDataRow, openedRow);
	setEditRowJissekiNinshoCd(kinmuDataRow, openedRow);
	setEditRowJissekiNinshoMsg(kinmuDataRow, openedRow);
	setEditRowJissekiValue(kinmuDataRow, openedRow);
	
	
	// バリデーション(時間計算含む)実行
	checkYoteiTime(kinmuDataRow, openedRow);
	checkJissekiTime(kinmuDataRow, openedRow);
	checkJissekiKiji(kinmuDataRow, openedRow);
		
	/* 実績の勤務認証DDLにイベントハンドラ追加 */
	$(openedRow).find('select[name=jissekiNinshoCd]').change(
		function(){
			setEditRowJissekiNinshoMsg(kinmuDataRow, openedRow);
			setEditRowJissekiValue(kinmuDataRow, openedRow);
		}
	);
	
	// 【予定】時間計算のイベント登録
	// class「checkYoteiTime」が設定されているテキストボックス
	// フォーカスが外れた場合(onblur)に動く
	var inputTimeArray = $(openedRow).find('input.checkYoteiTime[type="text"]');
	for(var i=0; i<inputTimeArray.size(); i++){
		$(inputTimeArray[i]).blur(
			function(){
				checkYoteiTime(kinmuDataRow, openedRow);
			}
		);
	}
	
	// 【予定】時間計算のイベント登録
	// class「checkYoteiTime」が設定されているチェックボックス
	$(openedRow).find('input.checkYoteiTime[type="checkbox"]').click(
		function(){
			checkYoteiTime(kinmuDataRow, openedRow);
		}
	);
	
	// 【実績】時間計算のイベント登録
	// class「checkJissekiTime」が設定されているテキストボックス
	// フォーカスが外れた場合(onblur)に動く
	var inputTimeArray = $(openedRow).find('input.checkJissekiTime[type="text"]');
	for(var i=0; i<inputTimeArray.size(); i++){
		$(inputTimeArray[i]).blur(
			function(){
				checkJissekiTime(kinmuDataRow, openedRow);
			}
		);
	}
	
	// 【実績】時間計算のイベント登録
	// class「checkJissekiTime」が設定されているチェックボックス
	$(openedRow).find('input.checkJissekiTime[type="checkbox"]').click(
		function(){
			checkJissekiTime(kinmuDataRow, openedRow);
		}
	);
	
	
	// イベントハンドラ追加
	// 実績:作業日誌　未入力時間加算
	for (var i=0; i<10; i++){
		$(openedRow).find('button.addSagyoTimeRemain_' + i).click(
			function(){
				var sagyoNisshiRow = $(this).parents('tr')[0];
				var rowCount = $('td.rowCount', sagyoNisshiRow).text();
				
				addSagyoTimeRemain(kinmuDataRow, openedRow, rowCount);
			}
		);
	}
	
	// イベントハンドラ追加
	// 記事欄文字数チェック
	$(openedRow).find('input.checkJissekiKiji').bind(
		"change keyup",
		function(){
			checkJissekiKiji(kinmuDataRow, openedRow);
		}
	);
}

/**
 * 勤務編集行の予定欄に値を設定する。
 * @param kinmuDataRow	勤務一覧の編集対象行
 * @param openedRow		勤務編集用行
 * @param calledEvent	呼出したイベント
 */
function setEditRowYoteiValue(kinmuDataRow, openedRow, calledEvent){
	// 予定編集フィールドがある場合、値を設定
	// ※他者勤務データ編集時は予定編集フィールドが存在しない
	if($(openedRow).find('input[name=yoteiStartHour]').size() > 0){
		// 日付
		var day = $('td.colDay', kinmuDataRow).text();
		
		// 予定
		var yoteiNinshoCd;
		var yoteiKakuninFlg;
		
		var tempYoteiStart;
		var tempYoteiEnd;
		
		var yoteiStartHour = '';
		var yoteiStartMin = '';
		var yoteiEndHour = '';
		var yoteiEndMin = '';
		var yoteiEndNextDayFlg = '';
		var yoteiMinashiCd = '';
		
		
		// 編集エラーデータ
		var dayEditError = '';
		var errorYoteiPostData;
		
		// 個人マスタ(勤務実績)を隠しフィールドから取得(１つ)
		var defaultPersonalValue = $('table#defaultPersonalValue').find('tr');
		
		// 編集対象日付の勤務データ詳細を隠しフィールドから取得(１つ)
		var storedKinmuValues = $('table#storedKinmuValues').find('tr[title=' + day + ']');
		
		yoteiKakuninFlg = $('td.colYoteiKakuninFlg', storedKinmuValues).text();
		yoteiNinshoCd = $('td.colYoteiNinshoCd', kinmuDataRow).text();
		
		// 勤務認証マスタ
		var kinmuNinshoMstSelected;
		var yoteiCalcPtn;
		
		// 勤務認証コードから勤務認証マスタのレコードを取得
		kinmuNinshoMstSelected = $('table#kinmuNinshoMst').find('tr[title=' + yoteiNinshoCd + ']');
		yoteiCalcPtn = $('td.colCalcPtn', kinmuNinshoMstSelected).text();
		
		/*===============
		 * 値の設定
		 *===============*/
		switch(calledEvent){
		
			//初期表示時
			case KINMU_EVENT_PTN_INIT:
				if ($('table#errorYoteiPostData').size() == 1){
					dayEditError = $('table#errorYoteiPostData').find('td.colDay').text();
				}

				if (day == dayEditError){
					errorYoteiPostData = $('table#errorYoteiPostData').find('tr');

					yoteiStartHour = $('td.colKinmuStartHour', errorYoteiPostData).text();
					yoteiStartMin = $('td.colKinmuStartMinute', errorYoteiPostData).text();
					yoteiEndHour = $('td.colKinmuEndHour', errorYoteiPostData).text();
					yoteiEndMin = $('td.colKinmuEndMinute', errorYoteiPostData).text();
					yoteiEndNextDayFlg = $('td.colKinmuEndNextDayFlg', errorYoteiPostData).text();
					yoteiMinashiCd = $('td.colKinmuMinashiCd', errorYoteiPostData).text(); 

				} else if (yoteiKakuninFlg == KAKUNIN_FLG_REGISTERED) {
					// 予定が登録されていた場合、登録済みの値を設定する
					tempYoteiStart = $('td.colYoteiStart', kinmuDataRow).text().split(":");
					tempYoteiEnd = $('td.colYoteiEnd', kinmuDataRow).text().split(":");

					if(tempYoteiStart.length == 2){
						yoteiStartHour = tempYoteiStart[0];
						yoteiStartMin = tempYoteiStart[1];
					}
					if(tempYoteiEnd.length == 2){
						yoteiEndHour = tempYoteiEnd[0];
						yoteiEndMin = tempYoteiEnd[1];
					}

					yoteiEndNextDayFlg = $('td.colYoteiEndNextDayFlg', storedKinmuValues).text();
					yoteiMinashiCd = $('td.colYoteiMinashiCd', storedKinmuValues).text();

				} else {
					switch (yoteiCalcPtn){
					case KINMU_CALC_PTN_SHUKKIN:
						// 【現行】勤務認証が「出勤」で予定が登録されていない場合、個人設定の値を設定する
						// 【次期】計算・表示パターンに変更
						yoteiStartHour = $('td.colKinmuStartHour', defaultPersonalValue).text();
						yoteiStartMin = $('td.colKinmuStartMinute', defaultPersonalValue).text();
						yoteiEndHour = $('td.colKinmuEndHour', defaultPersonalValue).text();
						yoteiEndMin = $('td.colKinmuEndMinute', defaultPersonalValue).text();
						yoteiEndNextDayFlg = $('td.colKinmuEndNextDayFlg', defaultPersonalValue).text();
						break;
					}
				}
				break;
				
			//勤務予定コード変更時
			case KINMU_EVENT_PTN_MINASHI_CD:
				yoteiMinashiCd = $('input[name=yoteiMinashiCd]:checked').val();
				switch(yoteiMinashiCd){
					//出勤は既に入っている値を設定
					case KINMU_MINASHI_CD_SHUKKIN:
						yoteiStartHour = $('td.colKinmuStartHour').text();
						yoteiStartMin = $('td.colKinmuStartMinute').text();
						yoteiEndHour = $('td.colKinmuEndHour').text();
						yoteiEndMin = $('td.colKinmuEndMinute').text();
						yoteiEndNextDayFlg = $('td.colKinmuEndNextDayFlg').text();					
						break;
					//年休は''を設定(宣言時のまま)
					case KINMU_MINASHI_CD_NENKYU:
						break;
					//AM半休は14:00-17:40
					case KINMU_MINASHI_CD_AM_KYU:
						yoteiStartHour = '14';
						yoteiStartMin = '00';
						yoteiEndHour = '17';
						yoteiEndMin = '40';
						break;
					//PM半休は9:00-13:40
					case KINMU_MINASHI_CD_PM_KYU:
						yoteiStartHour = '9';
						yoteiStartMin = '00';
						yoteiEndHour = '13';
						yoteiEndMin = '40';
						break;
				}
				break;
		}
		
		/*=========================
		 * コントロールへの値の設定
		 *=========================*/
		$(openedRow).find('input[name=yoteiStartHour]').val(yoteiStartHour);
		$(openedRow).find('input[name=yoteiStartMinute]').val(yoteiStartMin);
		$(openedRow).find('input[name=yoteiEndHour]').val(yoteiEndHour);
		$(openedRow).find('input[name=yoteiEndMinute]').val(yoteiEndMin);
		
		if(yoteiEndNextDayFlg == KINMU_END_FLG_NEXT_DAY){
			$(openedRow).find('input[name=yoteiEndNextDayFlg]').attr("checked", true);
		}else{
			$(openedRow).find('input[name=yoteiEndNextDayFlg]').attr("checked", false);
		}
		
		switch(yoteiMinashiCd){
			// 勤務予定コードのラジオボタンにチェックを入れる
			// 指定がなければ「出勤」がチェック済になる
			case KINMU_MINASHI_CD_SHUKKIN:
			case '':
				$(openedRow).find('input[name=yoteiMinashiCd]#shukkinStatus').attr("checked", true);
				break;
			case KINMU_MINASHI_CD_NENKYU:
				$(openedRow).find('input[name=yoteiMinashiCd]#nenkyuStatus').attr("checked", true);
				break;
			case KINMU_MINASHI_CD_AM_KYU:
				$(openedRow).find('input[name=yoteiMinashiCd]#am_kyuStatus').attr("checked", true);
				break;
			case KINMU_MINASHI_CD_PM_KYU:
				$(openedRow).find('input[name=yoteiMinashiCd]#pm_kyuStatus').attr("checked", true);
				break;
		}
	}
}
/**
 * 勤務予定コードの初期表示時または変更時、予定項目の入力活性状態を制御する
 * @param kinmuDataRow	勤務一覧の編集対象行
 * @param openedRow		勤務編集用行
 * 
 */
function setEditRowYoteiMinashiCd(kinmuDataRow, openedRow){

	var yoteiControlDisableFlg;
	var yoteiMinashiCd = $('input[name=yoteiMinashiCd]:checked').val();
	
	//活性・非活性
	if(yoteiMinashiCd == KINMU_MINASHI_CD_NENKYU){
		yoteiControlDisableFlg = true;
	}else{
		yoteiControlDisableFlg = false;
	}
	setYoteiControlActiveStatus(openedRow, yoteiControlDisableFlg);	
}



/**
 * 勤務編集行の実績欄の勤務認証コードを設定する。
 * @param kinmuDataRow	勤務一覧の編集対象行
 * @param openedRow		勤務編集用行
 */
function setEditRowJissekiNinshoCd(kinmuDataRow, openedRow){
	var day;
	var dayEditError = '';
	var errorJissekiPostData;
	var storedKinmuValues;
	var jissekiNinshoCd;
	
	// 日付
	day = $('td.colDay', kinmuDataRow).text();
	
	// 登録エラーデータ確認
	if ($('table#errorJissekiPostData').size() == 1){
		dayEditError = $('table#errorJissekiPostData').find('td.colDay').text();
	}

	// 初期選択値を判断
	if (day == dayEditError){
		// 登録エラーで戻ってきた場合
		errorJissekiPostData = $('table#errorJissekiPostData').find('tr');
		jissekiNinshoCd = $('td.colNinshoCd', errorJissekiPostData).text();
	} else {
		// 登録エラー以外の場合
		// 編集対象日付の勤務データ詳細を隠しフィールドから取得(１つ)
		storedKinmuValues = $('table#storedKinmuValues').find('tr[title=' + day + ']');
		jissekiNinshoCd = $('td.colJissekiNinshoCd', storedKinmuValues).text();
	}
	
	$(openedRow).find('select[name=jissekiNinshoCd]').val(jissekiNinshoCd);
}

/**
 * 勤務編集行の実績欄の値(勤務認証コード以外)を設定する。
 * @param kinmuDataRow	勤務一覧の編集対象行
 * @param openedRow		勤務編集用行
 */
function setEditRowJissekiValue(kinmuDataRow, openedRow){
	// 日付
	var day = $('td.colDay', kinmuDataRow).text();
	
	var jissekiNinshoCdSelected;
	var jissekiNinshoCdRegistered;
	var jissekiNinshoCdError = '';
	
	var jissekiKakuninFlg;
	
	var tempJissekiStart;
	var tempJissekiEnd;
	
	var jissekiStartHour = '';
	var jissekiStartMin = '';
	var jissekiEndHour = '';
	var jissekiEndMin = '';
	var jissekiEndNextDayFlg = '';
	var jissekiKiji = '';
	
	// 実績:休憩時間
	var restStartHour = new Array(3);
	var restStartMin = new Array(3);
	var restEndHour = new Array(3);
	var restEndMin = new Array(3);
	for (var i=0; i<3; i++){
		restStartHour[i] = '';
		restStartMin[i] = '';
		restEndHour[i] = '';
		restEndMin[i] = '';
	}
	
	// 実績:作業日誌(配列)
	var projCd = new Array(10);
	var sagyoCd = new Array(10);
	var workTimeHour = new Array(10);
	var workTimeMinute = new Array(10);
	for (var i=0; i<10; i++){
		projCd[i] = '';
		sagyoCd[i] = '';
		workTimeHour[i] = '';
		workTimeMinute[i] = '';
	}
	
	// 個人マスタ(勤務実績)を隠しフィールドから取得(１つ)
	var defaultPersonalValue = $('table#defaultPersonalValue').find('tr');
	// 編集対象日付の勤務データ詳細を隠しフィールドから取得(１つ)
	var storedKinmuValues = $('table#storedKinmuValues').find('tr[title=' + day + ']');
	// 編集対象日付の作業日誌データを隠しフィールドから取得（複数）
	var nisshiData = $('table#storedNisshiValues').find('tr[title=' + day + ']');
	
	// 勤務認証マスタ
	var kinmuNinshoMstSelected;
	var jissekiCalcPtn;
	
	// 編集エラーデータ
	var dayEditError = '';
	var errorJissekiPostData;
	
	/*===============
	 * 勤務実績認証コードはドロップダウンから取得
	 *===============*/
	jissekiNinshoCdRegistered = $('td.colJissekiNinshoCd', storedKinmuValues).text();
	jissekiNinshoCdSelected = $(openedRow).find('select[name=jissekiNinshoCd] option:selected').val();
	
	jissekiKakuninFlg = $('td.colJissekiKakuninFlg', storedKinmuValues).text();
	
	
	if ($('table#errorJissekiPostData').size() == 1){
		dayEditError = $('table#errorJissekiPostData').find('td.colDay').text();
		errorJissekiPostData = $('table#errorJissekiPostData').find('tr');
		jissekiNinshoCdError = $('td.colNinshoCd', errorJissekiPostData).text();
	}
	
	/*===============
	 * 実績 値の設定
	 *===============*/
	
	if (day == dayEditError && 
		jissekiNinshoCdError == jissekiNinshoCdSelected){
		// 登録エラーで戻ってきた場合 かつ 認証コードが「選択中＝エラー時」の場合
		errorJissekiPostData = $('table#errorJissekiPostData').find('tr');
		
		/*=========================
		 * 勤務時間・記事
		 *=========================*/
		jissekiStartHour = $('td.colKinmuStartHour', errorJissekiPostData).text();
		jissekiStartMin = $('td.colKinmuStartMinute', errorJissekiPostData).text();
		jissekiEndHour = $('td.colKinmuEndHour', errorJissekiPostData).text();
		jissekiEndMin = $('td.colKinmuEndMinute', errorJissekiPostData).text();
		jissekiEndNextDayFlg = $('td.colKinmuEndNextDayFlg', errorJissekiPostData).text();
		
		jissekiKiji = replaceSpace($('td.colJissekiKiji', errorJissekiPostData).text());
		
		/*=========================
		 * 休憩時間
		 *=========================*/
		for (var i=0; i<3; i++){
			var suffix = i + 1;
			restStartHour[i] = $('td.colJissekiRestStartHour' + suffix, errorJissekiPostData).text();
			restStartMin[i] = $('td.colJissekiRestStartMin' + suffix, errorJissekiPostData).text();
			restEndHour[i] = $('td.colJissekiRestEndHour' + suffix, errorJissekiPostData).text();
			restEndMin[i] = $('td.colJissekiRestEndMin' + suffix, errorJissekiPostData).text();
		}
		
		/*=========================
		 * 作業日誌
		 *=========================*/
		for (var i=0; i<10; i++){
			var suffix = i;
			projCd[i] = $('td.colNisshiProjCd' + suffix, errorJissekiPostData).text();
			sagyoCd[i] = $('td.colNisshiSagyoCd' + suffix, errorJissekiPostData).text();
			workTimeHour[i] = $('td.colNisshiSagyoHM_Hour' + suffix, errorJissekiPostData).text();
			workTimeMinute[i] = $('td.colNisshiSagyoHM_Minute' + suffix, errorJissekiPostData).text();
		}
		
	} else if (jissekiKakuninFlg == KAKUNIN_FLG_REGISTERED &&
		jissekiNinshoCdRegistered == jissekiNinshoCdSelected){
		/* 実績が登録されていた場合 かつ 認証コードが「選択中＝登録済」の場合
		 * 登録済みの値を設定する
		 */
		
		/*=========================
		 * 勤務時間・記事
		 *=========================*/
		tempJissekiStart = $('td.colJissekiStart', kinmuDataRow).text().split(":");
		tempJissekiEnd = $('td.colJissekiEnd', kinmuDataRow).text().split(":");
		
		if(tempJissekiStart.length == 2) {
			jissekiStartHour = tempJissekiStart[0];
			jissekiStartMin = tempJissekiStart[1];
		}
		if(tempJissekiEnd.length == 2) {
			jissekiEndHour = tempJissekiEnd[0];
			jissekiEndMin = tempJissekiEnd[1];
		}

		jissekiEndNextDayFlg = $('td.colJissekiEndNextDayFlg', storedKinmuValues).text();
		jissekiKiji = replaceSpace($('td.colJissekiKiji', kinmuDataRow).text());
		
		/*=========================
		 * 休憩時間
		 *=========================*/
		for (var i=0; i<3; i++){
			var suffix = i + 1;
			restStartHour[i] = $('td.colJissekiRestStartHour' + suffix, storedKinmuValues).text();
			restStartMin[i] = $('td.colJissekiRestStartMin' + suffix, storedKinmuValues).text();
			restEndHour[i] = $('td.colJissekiRestEndHour' + suffix, storedKinmuValues).text();
			restEndMin[i] = $('td.colJissekiRestEndMin' + suffix, storedKinmuValues).text();
		}
		
		/*=========================
		 * 作業日誌
		 *=========================*/
		for (var i=0; i<nisshiData.size(); i++){
			projCd[i] = $('td.colNisshiProjCd', nisshiData[i]).text();
			sagyoCd[i] = $('td.colNisshiSagyoCd', nisshiData[i]).text();
			workTimeHour[i] = $('td.colNisshiSagyoHM_Hour', nisshiData[i]).text();
			workTimeMinute[i] = $('td.colNisshiSagyoHM_Minute', nisshiData[i]).text();
		}
	} else {
		// 実績が登録されていない場合、勤務認証コードや計算・表示パターン毎に所定の初期値を設定する
		
		// 選択中に認証コードから勤務認証マスタのレコードを取得
		kinmuNinshoMstSelected = $('table#kinmuNinshoMst').find('tr[title=' + jissekiNinshoCdSelected + ']');
		jissekiCalcPtn = $('td.colCalcPtn', kinmuNinshoMstSelected).text();
		
		switch (jissekiCalcPtn){
			case KINMU_CALC_PTN_SHUKKIN:
				// 計算・表示パターンが「出勤」：個人設定の値を設定する
				jissekiStartHour = $('td.colKinmuStartHour', defaultPersonalValue).text();
				jissekiStartMin = $('td.colKinmuStartMinute', defaultPersonalValue).text();
				jissekiEndHour = $('td.colKinmuEndHour', defaultPersonalValue).text();
				jissekiEndMin = $('td.colKinmuEndMinute', defaultPersonalValue).text();
				jissekiEndNextDayFlg = $('td.colKinmuEndYokuFlg', defaultPersonalValue).text();
				
				for (var i=0; i<3; i++){
					var suffix = i + 1;
					restStartHour[i] = $('td.colJissekiRestStartHour' + suffix, defaultPersonalValue).text();
					restStartMin[i] = $('td.colJissekiRestStartMin' + suffix, defaultPersonalValue).text();
					restEndHour[i] = $('td.colJissekiRestEndHour' + suffix, defaultPersonalValue).text();
					restEndMin[i] = $('td.colJissekiRestEndMin' + suffix, defaultPersonalValue).text();
				}
				
				projCd[0] = $('td.colNisshiProjCd', defaultPersonalValue).text();
				sagyoCd[0] = $('td.colNisshiSagyoCd', defaultPersonalValue).text();
				workTimeHour[0] = $('td.colNisshiSagyoHM_Hour', defaultPersonalValue).text();
				workTimeMinute[0] = $('td.colNisshiSagyoHM_Minute', defaultPersonalValue).text();
				
				break;
			case KINMU_CALC_PTN_KENSHU:
				// 計算・表示パターンが「研修」：個人設定の値を設定する
				jissekiStartHour = '9';
				jissekiStartMin = '20';
				jissekiEndHour = '18';
				jissekiEndMin = '00';
				jissekiEndNextDayFlg = KINMU_END_FLG_TODAY;
				
				restStartHour[0] = '12';
				restStartMin[0] = '30';
				restEndHour[0] = '13';
				restEndMin[0] = '30';

				// 果たして、研修でPJコード・作業コードに個人入力設定のものを使うことはあるのだろうか。
				projCd[0] = $('td.colNisshiProjCd', defaultPersonalValue).text();
				sagyoCd[0] = $('td.colNisshiSagyoCd', defaultPersonalValue).text();
				
				jissekiKiji = '研修';
				break;
			case KINMU_CALC_PTN_SHUTCHO:
				// 出張
				jissekiStartHour = '9';
				jissekiStartMin = '00';
				jissekiEndHour = '17';
				jissekiEndMin = '40';
				jissekiEndNextDayFlg = KINMU_END_FLG_TODAY;
				
				restStartHour[0] = '12';
				restStartMin[0] = '00';
				restEndHour[0] = '13';
				restEndMin[0] = '00';

				projCd[0] = $('td.colNisshiProjCd', defaultPersonalValue).text();
				sagyoCd[0] = $('td.colNisshiSagyoCd', defaultPersonalValue).text();
				break;
			case KINMU_CALC_PTN_AM_HALF:
				// AM半休
				jissekiStartHour = '14';
				jissekiStartMin = '00';
				jissekiEndHour = '17';
				jissekiEndMin = '40';
				
				projCd[0] = $('td.colNisshiProjCd', defaultPersonalValue).text();
				sagyoCd[0] = $('td.colNisshiSagyoCd', defaultPersonalValue).text();
				break;
			case KINMU_CALC_PTN_PM_HALF:
				// PM半休
				jissekiStartHour = '9';
				jissekiStartMin = '00';
				jissekiEndHour = '13';
				jissekiEndMin = '40';
				
				restStartHour[0] = '12';
				restStartMin[0] = '00';
				restEndHour[0] = '13';
				restEndMin[0] = '00';
				
				projCd[0] = $('td.colNisshiProjCd', defaultPersonalValue).text();
				sagyoCd[0] = $('td.colNisshiSagyoCd', defaultPersonalValue).text();
				break;
			default:
				break;
		}
	}
	
	/*=========================
	 * コントロールへの値の設定
	 *=========================*/
	$(openedRow).find('input[name=jissekiStartHour]').val(ZeroPadding2IfNumeric(jissekiStartHour));
	$(openedRow).find('input[name=jissekiStartMinute]').val(ZeroPadding2IfNumeric(jissekiStartMin));
	$(openedRow).find('input[name=jissekiEndHour]').val(ZeroPadding2IfNumeric(jissekiEndHour));
	$(openedRow).find('input[name=jissekiEndMinute]').val(ZeroPadding2IfNumeric(jissekiEndMin));
	
	if(jissekiEndNextDayFlg == KINMU_END_FLG_NEXT_DAY){
		$(openedRow).find('input[name=jissekiEndNextDayFlg]').attr("checked", true);
	}else{
		$(openedRow).find('input[name=jissekiEndNextDayFlg]').attr("checked", false);
	}
	
	$(openedRow).find('input[name=jissekiKiji]').val(jissekiKiji);
	
	// 実績:休憩時間
	
	for (var i=0; i<3; i++){
		var suffix = i + 1;
		$(openedRow).find('input[name=jissekiRestStartHour_' + suffix + ']').val(ZeroPadding2IfNumeric(restStartHour[i]));
		$(openedRow).find('input[name=jissekiRestStartMinute_' + suffix + ']').val(ZeroPadding2IfNumeric(restStartMin[i]));
		$(openedRow).find('input[name=jissekiRestEndHour_' + suffix + ']').val(ZeroPadding2IfNumeric(restEndHour[i]));
		$(openedRow).find('input[name=jissekiRestEndMinute_' + suffix + ']').val(ZeroPadding2IfNumeric(restEndMin[i]));
	}
	
	// 実績:作業日誌データ
	for (var i=0; i<10; i++){
		$(openedRow).find('select[name=jissekiProjCd_' + i + ']').val(projCd[i]);
		$(openedRow).find('select[name=jissekiWorkCd_' + i + ']').val(sagyoCd[i]);
		$(openedRow).find('input[name=jissekiWorkHour_' + i + ']').val(ZeroPadding2IfNumeric(workTimeHour[i]));
		$(openedRow).find('input[name=jissekiWorkMinute_' + i + ']').val(ZeroPadding2IfNumeric(workTimeMinute[i]));
		
		if(projCd[i] != '' && sagyoCd[i] != '' && workTimeHour[i] == '' && workTimeMinute[i] == ''){
			addSagyoTimeRemain(kinmuDataRow, openedRow, i);
		}
	}
	
	/* =======================
	 * バリデーション
	 * ======================= */
	checkJissekiTime(kinmuDataRow, openedRow);
	checkJissekiKiji(kinmuDataRow, openedRow);
}


/**
 * 勤務認証コード毎のメッセージ表示と、入力活性状態を制御する
 * @param kinmuDataRow	勤務一覧の編集対象行
 * @param openedRow		勤務編集用行
 */
function setEditRowJissekiNinshoMsg(kinmuDataRow, openedRow){
	// メッセージ表示
	var jissekiNinshoMsg = '';
	
	var jissekiNinshoCdSelected;
	// 勤務認証マスタ
	var kinmuNinshoMstSelected;
	var jissekiCalcPtn;

	var jissekiControlDisableFlg = false;	// 入力欄非活性フラグ
	var onlyKijiFlg = false;				// 記事欄のみ入力可能フラグ
	
	/*===============
	 * 勤務実績認証コードはドロップダウンから取得
	 *===============*/
	jissekiNinshoCdSelected = $(openedRow).find('select[name=jissekiNinshoCd] option:selected').val();
	
	// 選択中に認証コードから勤務認証マスタのレコードを取得
	kinmuNinshoMstSelected = $('table#kinmuNinshoMst').find('tr[title=' + jissekiNinshoCdSelected + ']');
	jissekiCalcPtn = $('td.colCalcPtn', kinmuNinshoMstSelected).text();
	
	switch (jissekiCalcPtn){
		case KINMU_CALC_PTN_AM_HALF:
			//14：00以前始業で入力してください。
			jissekiNinshoMsg = '14:00以前始業で入力してください。';
			break;
		case KINMU_CALC_PTN_PM_HALF:
			//13：40以後終業で入力してください。
			jissekiNinshoMsg = '13:40以後終業で入力してください。';
			break;
		case KINMU_CALC_PTN_NENKYU:	// 年休類（時間・記事欄入力不可）
		case KINMU_CALC_PTN_MUKYU:	// 無給（時間入力不可）
			// ここだけ現行にはないメッセージ(親切心から実装・・・)
			jissekiNinshoMsg = '入力できる項目はありません。このまま登録してください。';
			jissekiControlDisableFlg = true;
			break;
		case KINMU_CALC_PTN_YUKYU_KIJI:
		case KINMU_CALC_PTN_MUKYU_KIJI:
			jissekiNinshoMsg = '記事欄のみ入力できます。';
			jissekiControlDisableFlg = true;
			onlyKijiFlg = true;
			break;
		case KINMU_CALC_PTN_YUKYU_MINASHI_CALC:
			if (jissekiNinshoCdSelected == KINMU_NINSHO_SHOGAI) {
				jissekiNinshoMsg = '勤務時間が発生した場合、その時間を入力してください。';
			} else {
				jissekiNinshoMsg = '09:00～17:40はみなし時間に計上されます。それ以外で勤務時間が発生した場合、その時間を入力してください。';
			}
			break;
		default:
			break;
	}
	
	$(openedRow).find('span.jissekiNinshoMsg').text(jissekiNinshoMsg);
	
	/* =======================
	 * 入力可能制御
	 * ======================= */
	setJissekiControlActiveStatus(openedRow, jissekiControlDisableFlg, onlyKijiFlg);
}
/**
 * 引数に合わせて勤務予定入力用コントロールの活性状態を設定する。
 * @param openedRow		勤務編集用行
 * @param status	非活性にするかのフラグ
 */
function setYoteiControlActiveStatus(openedRow, disableFlg){
	
	// class「checkYoteiTime」が設定されているテキストボックス
	var inputTimeArray = $(openedRow).find('input.checkYoteiTime[type="text"]');
	for(var i=0; i<inputTimeArray.size(); i++){
		$(inputTimeArray[i]).attr("disabled", disableFlg);
	}
	// class「checkYoteiTime」が設定されているチェックボックス
	$(openedRow).find('input.checkYoteiTime[type="checkbox"]').attr('disabled', disableFlg);
}

/**
 * 引数に合わせて勤務実績入力用コントロールの活性状態を設定する。
 * @param openedRow		勤務編集用行
 * @param disableFlg	非活性にするかのフラグ
 * @param onlyKijiFlg	記事のみ入力可とするかのフラグ(trueの場合、記事のみ入力可能)
 */
function setJissekiControlActiveStatus(openedRow, disableFlg, onlyKijiFlg){
	// class「checkJissekiTime」が設定されているテキストボックス
	var inputTimeArray = $(openedRow).find('input.checkJissekiTime[type="text"]');
	for(var i=0; i<inputTimeArray.size(); i++){
		$(inputTimeArray[i]).attr("disabled", disableFlg);
	}
	// class「checkJissekiTime」が設定されているチェックボックス
	$(openedRow).find('input.checkJissekiTime[type="checkbox"]').attr('disabled', disableFlg);
	
	// 作業日誌
	for (var i=0; i<10; i++){
		$(openedRow).find('select[name=jissekiProjCd_' + i + ']').attr('disabled', disableFlg);
		$(openedRow).find('select[name=jissekiWorkCd_' + i + ']').attr('disabled', disableFlg);
		$(openedRow).find('button.addSagyoTimeRemain_' + i).attr('disabled', disableFlg);
	}
	
	// 記事欄
	if (disableFlg == true && onlyKijiFlg == false){
		$(openedRow).find('input.kiji').attr('disabled', true);
	}else{
		$(openedRow).find('input.kiji').attr('disabled', false);
	}
}

/**
 * 【予定】入力テキストボックスから時分を取得し、作業時間を算出、表示する
 * @param kinmuDataRow	勤務一覧の編集対象行
 * @param openedRow		勤務編集用行
 */
function checkYoteiTime(kinmuDataRow, openedRow){
	var kousokuTimeAsMin = 0;
	var kousokuTimeMsg = '';
	
	kousokuTimeAsMin = calcYoteiKousokuTimeAsMin(openedRow);		// 拘束時間(エラーの場合、負の無限大)
	
	kousokuTimeMsg = convertMinToHM(kousokuTimeAsMin);
	if (kousokuTimeAsMin < 0) {
		setValidaterMsg($(openedRow).find('span.yoteiKousokuTimeMsg'), kousokuTimeMsg, true);	// エラー
	} else {
		setValidaterMsg($(openedRow).find('span.yoteiKousokuTimeMsg'), kousokuTimeMsg, false);
	}
}

/**
 * 【実績】入力テキストボックスから時分を取得し、作業時間・未入力時間等を算出、表示する
 * @param kinmuDataRow	勤務一覧の編集対象行
 * @param openedRow		勤務編集用行
 */
function checkJissekiTime(kinmuDataRow, openedRow){
	var kousokuTimeAsMin = 0;
	var restTimeTotalAsMin = 0;
	var sagyoTimeAsMin = 0;
	var nisshiSagyoTimeTotalAsMin = 0;
	var nisshiSagyoTimeRemainAsMin = 0;
	
	var sagyoTimeForDisplay = '';
	var nisshiSagyoTimeTotalForDisplay = '';
	var nisshiSagyoTimeRemainForDisplay = '';
	
	var kousokuTimeMsg = '';
	var restTimeTotalMsg = '';
	
	/*=========================
	 * コントロールから値を取得しつつ計算
	 *=========================*/
	kousokuTimeAsMin = calcJissekiKousokuTimeAsMin(openedRow);		// 拘束時間(エラーの場合、負の無限大)
	restTimeTotalAsMin = calcRestTimeTotalAsMin(openedRow);			// 休憩時間合計(エラーの場合、負の無限大)
	
	
	// 作業時間 (拘束時間 - 休憩時間合計)
	if (kousokuTimeAsMin >= 0 && restTimeTotalAsMin >= 0) {
		sagyoTimeAsMin = kousokuTimeAsMin - restTimeTotalAsMin;
		sagyoTimeForDisplay = convertMinToHM(sagyoTimeAsMin);
	} else {
		sagyoTimeAsMin = Number.NEGATIVE_INFINITY;
		sagyoTimeForDisplay = '--:--';
	}
	
	
	nisshiSagyoTimeTotalAsMin = calcNisshiSagyoTimeTotalAsMin(openedRow);	// 作業日誌:入力済み作業時間合計
	nisshiSagyoTimeTotalForDisplay = convertMinToHM(nisshiSagyoTimeTotalAsMin);
	
	// 作業日誌:未入力の作業時間 (作業時間 - 入力済み作業時間合計)
	if (sagyoTimeAsMin >= 0 && nisshiSagyoTimeTotalAsMin >= 0) {
		nisshiSagyoTimeRemainAsMin = sagyoTimeAsMin - nisshiSagyoTimeTotalAsMin;
		nisshiSagyoTimeRemainForDisplay = convertMinToHM(nisshiSagyoTimeRemainAsMin);
	} else {
		nisshiSagyoTimeRemainAsMin = Number.NEGATIVE_INFINITY;
		nisshiSagyoTimeRemainForDisplay = '--:--';
	}

	
	
	/* ========================
	 * バリデーター メッセージ
	 * ========================
	 */
	
	// 拘束時間
	kousokuTimeMsg = convertMinToHM(kousokuTimeAsMin);
	if (kousokuTimeAsMin < 0) {
		setValidaterMsg($(openedRow).find('span.jissekiKousokuTimeMsg'), kousokuTimeMsg, true);	// エラー
	} else {
		setValidaterMsg($(openedRow).find('span.jissekiKousokuTimeMsg'), kousokuTimeMsg, false);
	}
	
	// 休憩時間
	restTimeTotalMsg = convertMinToHM(restTimeTotalAsMin);
	if (restTimeTotalAsMin < 0) {
		setValidaterMsg($(openedRow).find('span.jissekiRestTimeTotalMsg'), restTimeTotalMsg, true);	// エラー
	} else {
		setValidaterMsg($(openedRow).find('span.jissekiRestTimeTotalMsg'), restTimeTotalMsg, false);
	}
	
	// 作業時間
	if(sagyoTimeAsMin < 0){
		setValidaterMsg($(openedRow).find('span.sagyoTime'), sagyoTimeForDisplay, true);	// エラー
	} else {
		setValidaterMsg($(openedRow).find('span.sagyoTime'), sagyoTimeForDisplay, false);
	}
	
	//$(openedRow).find('span.sagyoTimeHosoku').text(' [入力済時間]' + nisshiSagyoTimeTotalForDisplay);
	
	// 未入力時間
	if(nisshiSagyoTimeRemainAsMin < 0){
		setValidaterMsg($(openedRow).find('span.nisshiSagyoTimeRemain'), nisshiSagyoTimeRemainForDisplay, true);	// エラー
	} else {
		setValidaterMsg($(openedRow).find('span.nisshiSagyoTimeRemain'), nisshiSagyoTimeRemainForDisplay, false);
	}
	
}

/**
 * 実績記事欄の入力文字数を画面に表示する。
 * @param kinmuDataRow	勤務一覧の編集対象行
 * @param openedRow		勤務編集用行
 */
function checkJissekiKiji(kinmuDataRow, openedRow){
	var jissekiKijiMsg = '';
	var len = $(openedRow).find('input.kiji').val().length;
	
	jissekiKijiMsg = len + '/20';
	if (len < 0) {
		setValidaterMsg($(openedRow).find('span.jissekiKijiMsg'),jissekiKijiMsg, true);	// エラー
	} else {
		setValidaterMsg($(openedRow).find('span.jissekiKijiMsg'),jissekiKijiMsg, false);
	}
}


/**
 * 入力初期値のテキストボックスに設定された値に合わせて、バリデーターメッセージを画面に設定する。
 * 引数のエラーフラグがtrueの場合、文字色を警告色(赤)にする。
 * @param control	メッセージ設定対象コントロール
 * @param msg		表示するメッセージ内容
 * @param errorFlg	警告色にするかのフラグ
 */
function setValidaterMsg(control, msg, errorFlg){
	control.text(msg);
	
	if (errorFlg == true) {
		control.removeClass('validaterInfo');
		control.addClass('validaterError');
	} else {
		control.removeClass('validaterError');
		control.addClass('validaterInfo');
	}
}

/**
 * 【予定】拘束時間(分)を算出する
 * エラーの場合、負の無限大を返却する
 * @param openedRow		勤務編集用行
 */
function calcYoteiKousokuTimeAsMin(openedRow){
	var startHour = $(openedRow).find('input[name=yoteiStartHour]').val();
	var startMin = $(openedRow).find('input[name=yoteiStartMinute]').val();
	var endHour = $(openedRow).find('input[name=yoteiEndHour]').val();
	var endMin = $(openedRow).find('input[name=yoteiEndMinute]').val();
	var endNextDayFlg = '';
	
	if($(openedRow).find('input[name=yoteiEndNextDayFlg]').is(':checked') == true){
		endNextDayFlg = KINMU_END_FLG_NEXT_DAY;
	}else{
		endNextDayFlg = KINMU_END_FLG_TODAY;
	}
	
	return calcKousokuTimeAsMin(startHour, startMin, endHour, endMin, endNextDayFlg);
}

/**
 * 【実績】拘束時間(分)を算出する
 * エラーの場合、負の無限大を返却する
 * @param openedRow		勤務編集用行
 */
function calcJissekiKousokuTimeAsMin(openedRow){
	var startHour = $(openedRow).find('input[name=jissekiStartHour]').val();
	var startMin = $(openedRow).find('input[name=jissekiStartMinute]').val();
	var endHour = $(openedRow).find('input[name=jissekiEndHour]').val();
	var endMin = $(openedRow).find('input[name=jissekiEndMinute]').val();
	var endNextDayFlg = '';
	
	if($(openedRow).find('input[name=jissekiEndNextDayFlg]').is(':checked') == true){
		endNextDayFlg = KINMU_END_FLG_NEXT_DAY;
	}else{
		endNextDayFlg = KINMU_END_FLG_TODAY;
	}
	
	return calcKousokuTimeAsMin(startHour, startMin, endHour, endMin, endNextDayFlg);
}

/**
 * 拘束時間(分)を算出する
 * エラーの場合、負の無限大を返却する
 * @param openedRow		勤務編集用行
 */
function calcKousokuTimeAsMin(startHour, startMin, endHour, endMin, endNextDayFlg){
	
	if (startHour == '' &&
		startMin == '' &&
		endHour == '' &&
		endMin == '') {
		return 0;
	} else if (
		$.isNumeric(startHour) == false ||
		$.isNumeric(startMin) == false ||
		$.isNumeric(endHour) == false ||
		$.isNumeric(endMin) == false) {
		return Number.NEGATIVE_INFINITY;
	}
	
	var startTimeAsMin = parseInt(startHour) * 60 + parseInt(startMin);
	var endTimeAsMin = parseInt(endHour) * 60 + parseInt(endMin);
	
	// 勤務時間の場合、翌日フラグが設定されていれば終了時間に24時間プラス
	if (endNextDayFlg == KINMU_END_FLG_NEXT_DAY){
		endTimeAsMin += (24 * 60);
	}
	
	return endTimeAsMin - startTimeAsMin;
}

/**
 * 休憩時間(分)の合計を算出する
 * エラーの場合、負の無限大を返却する
 * @param openedRow		勤務編集用行
 * @returns {Number}
 */
function calcRestTimeTotalAsMin(openedRow){
	var restTimeTotalAsMin = 0;
	
	// 実績:休憩時間
	for (var i=0; i<3; i++){
		
		// 実績:休憩時間【計算】
		var restTimeAsMin = calcRestTimeAsMin(openedRow, i);
		
		if (restTimeAsMin < 0) {
			return Number.NEGATIVE_INFINITY;
		}else{
			restTimeTotalAsMin += restTimeAsMin;
		}
	}
	
	return restTimeTotalAsMin;
}


/**
 * 1行毎の休憩時間(分)を算出する
 * エラーの場合、負の無限大を返却する
 * @param openedRow	勤務編集用行
 * @param i			休憩時間の行インデックス
 * @returns
 */
function calcRestTimeAsMin(openedRow, i){
	var suffix = i + 1;
	
	var restStartHour = $(openedRow).find('input[name=jissekiRestStartHour_' + suffix + ']').val();
	var restStartMin = $(openedRow).find('input[name=jissekiRestStartMinute_' + suffix + ']').val();
	var restEndHour = $(openedRow).find('input[name=jissekiRestEndHour_' + suffix + ']').val();
	var restEndMin = $(openedRow).find('input[name=jissekiRestEndMinute_' + suffix + ']').val();
	
	var jissekiEndNextDayFlg;
	if($(openedRow).find('input[name=jissekiEndNextDayFlg]').is(':checked') == true){
		jissekiEndNextDayFlg = KINMU_END_FLG_NEXT_DAY;
	}else{
		jissekiEndNextDayFlg = KINMU_END_FLG_TODAY;
	}
	
	if (restStartHour == '' &&
		restStartMin == '' &&
		restEndHour == '' &&
		restEndMin == '') {
		return 0;
	} else if (
		$.isNumeric(restStartHour) == false ||
		$.isNumeric(restStartMin) == false ||
		$.isNumeric(restEndHour) == false ||
		$.isNumeric(restEndMin) == false) {
		return Number.NEGATIVE_INFINITY;
	}
	
	var startTimeAsMin = parseInt(restStartHour) * 60 + parseInt(restStartMin);
	var endTimeAsMin = parseInt(restEndHour) * 60 + parseInt(restEndMin);
	
	// 休憩時間の場合、翌日フラグが設定されていて、かつ開始終了の差がマイナスなら終了時間に24時間プラス(日跨り休憩)
	if (jissekiEndNextDayFlg == KINMU_END_FLG_NEXT_DAY && startTimeAsMin > endTimeAsMin){
		endTimeAsMin += 24 * 60;
	}
	
	return endTimeAsMin - startTimeAsMin;
}


/**
 * 作業日誌の作業時間合計を計算する
 * エラーの場合、負の無限大を返却する
 * @param openedRow		勤務編集用行
 * @returns {Number}
 */
function calcNisshiSagyoTimeTotalAsMin(openedRow){
	var nisshiSagyoTimeTotalAsMin = 0;
	
	// 実績:作業日誌データ
	for (var i=0; i<10; i++){
		var nisshiSagyoTimeHour = $(openedRow).find('input[name=jissekiWorkHour_' + i + ']').val();
		var nisshiSagyoTimeMinute = $(openedRow).find('input[name=jissekiWorkMinute_' + i + ']').val();
		
		var nisshiSagyoTimeAsMin = calcMin(nisshiSagyoTimeHour, nisshiSagyoTimeMinute);
		
		if (nisshiSagyoTimeAsMin < 0) {
			return Number.NEGATIVE_INFINITY;	// エラー
		}else{
			nisshiSagyoTimeTotalAsMin += nisshiSagyoTimeAsMin;
		}
	}
	
	return nisshiSagyoTimeTotalAsMin;
}

/**
 * 引数の時・分を、分単位の数値にする。
 * 引数が数値でない場合、負の無限大を返す。
 * @param hour
 * @param min
 * @returns
 */
function calcMin(hour, min){
	if (hour == '' &&
		min == '') {
		return 0;
	} else if (
		$.isNumeric(hour) == false ||
		$.isNumeric(min) == false) {
		return Number.NEGATIVE_INFINITY;	// エラー
	} else {
		return parseInt(hour) * 60 + parseInt(min);
	}
}

/**
 * 分を「99:99」形式の文字列に変換します。
 * 負の値の場合は「-99:99」形式となります。
 * 引数が数値でない場合、または負の無限大の場合、「--:--」とします
 * @param min
 * @returns {String}
 */
function convertMinToHM(min){
	if($.isNumeric(min) == false || min == Number.NEGATIVE_INFINITY){
		return '--:--';
	} else if(min < 0){
		var tempMin = parseInt(min) * -1;
		return '-' + ZeroPadding2(parseInt(tempMin / 60)) + ':' + ZeroPadding2(tempMin % 60);
	} else {
		var tempMin = parseInt(min);
		return ZeroPadding2(parseInt(tempMin / 60)) + ':' + ZeroPadding2(tempMin % 60);
	}
	
}

/**
 * 「99:99」または「-99:99」形式の文字列を、数値型の分に変換する。
 * @param hm
 * @returns {Number}
 */
function convertHMtoMin(hm) {
	var hmArray = hm.split(":");
	var hour = 0;
	var minute = 0;
	
	if (hmArray.length != 2) {
		return 0;
	}
	
	if ($.isNumeric(hmArray[0]) == false ||
		$.isNumeric(hmArray[1]) == false){
		return 0;
	}
	
	hour = parseInt(hmArray[0]);
	minute = parseInt(hmArray[1]);
	
	// 負数判定 
	// 「第一引数<0」だと、「-0:40」の場合に計算結果が不正になるので文字列判定とする。
	if (hmArray[0].charAt(0) == '-') {
		minute *= -1;
	}

	return (hour * 60) + minute;
}

/**
 * 「残り全部」ボタンで実行する処理。未入力作業時間を加算する。
 * @param kinmuDataRow	勤務一覧の編集対象行
 * @param openedRow		勤務編集用行
 * @param i				ボタンが押された行インデックス
 */
function addSagyoTimeRemain(kinmuDataRow, openedRow, i){
	checkJissekiTime(kinmuDataRow, openedRow);
	
	var returnAsMin;
	
	var intHour;
	var intMinute;
	
	var strHour;
	var strMinute;
	
	var nisshiSagyoTimeRemainAsMin;
	var nisshiSagyoTimeRemainForDisplay;
	
	nisshiSagyoTimeRemainForDisplay = $(openedRow).find('span.nisshiSagyoTimeRemain').text();
	nisshiSagyoTimeRemainAsMin = convertHMtoMin(nisshiSagyoTimeRemainForDisplay);
	
	// 未入力時間が0の場合は処理しない
	if(nisshiSagyoTimeRemainAsMin != 0){
		strHour = $(openedRow).find('input[name=jissekiWorkHour_' + i + ']').val();
		strMinute = $(openedRow).find('input[name=jissekiWorkMinute_' + i + ']').val();
		
		if (strHour == ''){
			strHour = 0;
		}
		if (strMinute == ''){
			strMinute = 0;
		}
		
		if($.isNumeric(strHour) == true && $.isNumeric(strMinute) == true) {
			returnAsMin = (parseInt(strHour) * 60) + parseInt(strMinute) + nisshiSagyoTimeRemainAsMin;
			
			if (returnAsMin >= 0) {
				intHour = parseInt(returnAsMin / 60);
				intMinute = parseInt(returnAsMin % 60);
			}else{
				intHour = parseInt(0);
				intMinute = parseInt(0);
			}
			
			$(openedRow).find('input[name=jissekiWorkHour_' + i + ']').val(ZeroPadding2IfNumeric(intHour));
			$(openedRow).find('input[name=jissekiWorkMinute_' + i + ']').val(ZeroPadding2IfNumeric(intMinute));
		}
		
		checkJissekiTime(kinmuDataRow, openedRow);
	}
}

/**
 * 半角スペース（&nbsp;）が文字化けしないように、半角スペース（ ）に置き換える。
 * @param text
 */
function replaceSpace(text) {
	return text.replace(/\s/g, " ");
}

/**
 * 勤務予定更新サーブレット呼び出し
 * @param day
 */
function yoteiUpdateSubmit(day){
	openProcessingDialog();
	var formObj = $('form[name=yoteiUpdateForm_' + day + ']');
	setJissekiControlActiveStatus(formObj, false, false);
    formObj.submit();
}

/**
 * 勤務実績更新サーブレット呼び出し
 * @param day
 */
function jissekiUpdateSubmit(day){
	openProcessingDialog();
	var formObj = $('form[name=jissekiUpdateForm_' + day + ']');
	setJissekiControlActiveStatus(formObj, false, false);
    formObj.submit();
}


/*==============================================
 * 予定・実績登録の機能に関するスクリプト END
 *==============================================*/

//画面遷移スクリプト
function jumpUrl(url){
 openProcessingDialog();
 location.href = url;
}
