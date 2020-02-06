/*==============================================
 * 共通ページ用Script
 *==============================================*/

// リンク用メッセージ表示スクリプト
$(function(){
     $(".commonLinkedMessage").click(function(){
         window.location=$(this).find("a").attr("href");
         return false;
    });
});