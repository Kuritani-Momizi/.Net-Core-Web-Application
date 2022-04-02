//ルーティーン並び替え
$("#sortable").sortable({
    update: function () {
        //$(".sortorder-icon")のIDをeachで回して全部取得する
        //取得したのちシリアライズ化する
        var routineIdList = [];

        $.each($(".sortorder-icon"), function () {
            routineIdList.push($(this).attr('id'));
        });

        UpdSortOrder(routineIdList);
    }
});


//並び順変更をajaxでDBに登録する
// URLの取得
//[ToDo:操谷]デプロイしたらこの書き方のurlだとうまくいかない

function UpdSortOrder(routineIdList) {

    var personId = $(".personId").val();

    $.ajax({
        url: 'Index/UpdSortOrder',
        type: 'POST',
        dataType: 'application/json',
        data: {
            sortOrderIdList: routineIdList,
            personId: personId
        },
    })
    .done(function (response) {
        if (response == false) {
            alert("並び替えに失敗しました。画面を更新してください。");
        }
    });
}

