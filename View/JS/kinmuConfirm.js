/**
 * フレックス確認画面用JavaScript
 * @version 2013/09/03 SKK宇戸 新規作成
 */

READYSTATE_COMPLETE = 'complete';		// 全データ読込完了
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
		// 年月選択ボタン活性状態設定
		initTargetYMButton();

		// 処理中ダイアログ初期化
		initProcessingDialog();
	}
);

/*==============================================
 * ページ読み込み完了時に実行するメソッド END
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

	form.setAttribute('action', './KinmuConfirmServlet');
	form.setAttribute('method', 'post');
	form.submit();
}

/*==============================================
 * 表示年月・社員の機能に関するスクリプト END
 *==============================================*/
/*==============================================
 * フレックス確認に関するスクリプト START
 *==============================================*/
function submitKinmuConfirm(targetConfirmYM) {
	openProcessingDialog();
	
	// フォームの生成
	var form = document.createElement('form');
	document.body.appendChild(form);

	// 対象年月の設定
	var input = document.createElement('input');
	input.setAttribute('type', 'hidden');
	input.setAttribute('name', 'targetConfirmYM');
	input.setAttribute('value', targetConfirmYM);
	form.appendChild(input);

	// チェックボックスをすべて取得
	var checkBox = document.getElementsByName("confirmChekBox");
	
	// チェックONの日付取得用
	var targetConfirmShainCd = "";
	
	// チェックONの日付のみ取り出す
	for ( var i = 0; i < checkBox.length; i++) {
		if (checkBox[i].checked) {
			// 変数targetDaysが空でない時、区切りのコロンを入れる
			if (targetConfirmShainCd != "") {
				targetConfirmShainCd = targetConfirmShainCd + ":";
			}
			targetConfirmShainCd = targetConfirmShainCd + checkBox[i].value;
		}
	}
	
	// 入力チェック
	if (targetConfirmShainCd == "") {
		alert("確認対象社員が選択されていません。1件以上選択してください。");
		return;
	}
	
	var input2 = document.createElement('input');
	input2.setAttribute('type', 'hidden');
	input2.setAttribute('name', 'targetConfirmShainCd');
	input2.setAttribute('value', targetConfirmShainCd);
	form.appendChild(input2);

	form.setAttribute('action', './KinmuConfirmServlet');
	form.setAttribute('method', 'post');
	form.submit();
}
/*==============================================
 * フレックス確認に関するスクリプト END
 *==============================================*/

//画面遷移スクリプト
function jumpUrl(url){
 openProcessingDialog();
 location.href = url;
}
