//達成・解除ボタンクラス付与
$(".taskrow").each(function (i, item) {

    var checked = $(this).find('input[name=achivementcheckbox]').prop("checked");

    if (checked) {
        //解除クラスの追加
        $(this).find(".achivement-text").addClass("lift-btn");
        //$(this).find(".achivement").addClass("lift-btn");
        $(this).find(".achivement-text").text("解除")

    }
    else {
        //達成クラスの追加
        $(this).find(".achivement-text").addClass("achivement-btn");
        //$(this).find(".achivement").addClass("achivement-btn");
        $(this).find(".achivement-text").text("達成")

    }
});


//達成・解除ボタン押下時
$(document).on("click", ".achivementflg", function (e) {

    var achivementFlg = $(this).closest(".taskrow").find('input[name=achivementcheckbox]').prop("checked");
    var taskId = $(this).closest(".taskrow").find(".taskid").val();

    UpdAchivement(achivementFlg, taskId);

    //クリックされたチェックボックスの値を変更する
    var achivementFlg = $(this).closest(".taskrow").find('input[name=achivementcheckbox]').prop("checked");


    alert(achivementFlg);

    if (achivementFlg == "true") {
        //達成クラスの追加
        $(this).closest(".taskrow").find(".achivement-text").addClass("achivement-btn");
        $(this).closest(".taskrow").find(".achivement-text").text("達成");

        $(this).closest(".taskrow").find(".achivement-text").removeClass("lift-btn");

        $(this).closest(".taskrow").find('input[name=achivementcheckbox]').prop("checked", "false");
    }
    else {
        //解除クラスの追加
        $(this).closest(".taskrow").find(".achivement-text").addClass("lift-btn");
        $(this).closest(".taskrow").find(".achivement-text").text("解除");

        $(this).closest(".taskrow").find(".achivement-text").removeClass("achivement-btn");

        $(this).closest(".taskrow").find('input[name=achivementcheckbox]').prop("checked", "true");
    }

});

function UpdAchivement(achivementFlg, taskId) {

    var halfUrl = $(location).attr('href').split('Task/Task/');
    var url = halfUrl[0] + 'Task/Task/UpdAchivement';

    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'html',
        data: {
            achivementFlg: achivementFlg,
            taskId: taskId
        },
    })
        .done(function (response) {

        });
}
