//追加ボタン押下時
$(document).on("click", ".add-task", function (e) {

    //ajax用URLの作成
    var halfUrl = $(location).attr('href').split('Task/Task/');
    var url = halfUrl[0] + 'Task/Task/GetSelectListItem';

    //入力欄の追加
    //====================削除ボタンの追加===================
    let tr = $('<tr class="taskrow style="height: 100px;""></tr>');
    let td1 = $('<td class="col-1"></td>');
    let img = $('<img src="/resource/DeleteTask.png" class="delete-icon" />');
    td1.append(img);

    //====================タスク名欄の追加===================
    let td2 = $('<td class="col-4"></td>');
    let input1 = $('<input type="text" data-val="true" data-val-required="タスク名は必須項目です。" id="TaskUpdateList_i__Task_Name" name="TaskUpdateList[i].Task_Name" class="taskNm">');
    td2.append(input1);

    //====================タスク回数欄の追加===================
    let td3 = $('<td class="col-2"></td>');
    let input2 = $('<input type="number" data-val="true" data-val-required="回数は必須項目です。" id="TaskUpdateList_i__Task_Count" name="TaskUpdateList[i].Task_Count" class="taskCount">');
    td3.append(input2);

    //====================タスク単位セレクトリスト欄の追加===================
    let td4 = $('<td class="col-4"></td>');
    let input3 = $('<select class="form-control taskunitlist" data-val="true" data-val-required="単位は必須項目です。" id="TaskUpdateList_i__Task_Unit" name="TaskUpdateList[i].Task_Unit"></select>');
    //ajaxでセレクトリストの中身を取得する
    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'json',
        data: {

        },
    })
        .done(function (response) {

            var selectlistitem = "<option selected disabled>選択</option>";;

            $.each(response, function (i, item) {
                selectlistitem += "<option value='" + item.value + "'>" + item.text + "</option>";
            });

            input3.append(selectlistitem);
        });

    td4.append(input3);

    //====================タスクIDの追加===================
    let td5 = $('<td class="hidden"></td>');
    let input4 = $('<input type="number" data-val="true" id="TaskUpdateList_i__Task_Id" name="TaskUpdateList[i].Task_Id" class="taskId" value="0" >');
    td5.append(input4);

    //テーブルリストに要素を追加する
    tr.append(td1);
    tr.append(td2);
    tr.append(td3);
    tr.append(td4);
    tr.append(td5);

    $('table').append(tr);

    //indexの振りなおし
    ReNumber();
});

//indexの振りなおし
function ReNumber() {
    $(".taskrow").each(function (i) {

        //行数の取得
        $(this).find('.taskNm').attr("name", 'TaskUpdateList[' + i + '].Task_Name');
        $(this).find('.taskCount').attr("name", 'TaskUpdateList[' + i + '].Task_Count');
        $(this).find('.taskId').attr("name", 'TaskUpdateList[' + i + '].Task_Id');
        $(this).find('.taskunitlist').attr("name", 'TaskUpdateList[' + i + '].Task_Unit');

        $(this).find('.taskNm').attr("id", 'TaskUpdateList[' + i + '].Task_Name');
        $(this).find('.taskCount').attr("id", 'TaskUpdateList[' + i + '].Task_Count');
        $(this).find('.taskId').attr("id", 'TaskUpdateList[' + i + '].Task_Id');
        $(this).find('.taskunitlist').attr("id", 'TaskUpdateList[' + i + '].Task_Unit');

    });
}

//追加したセレクトリストにデータを追加する
function GetSelectListItem() {

    var halfUrl = $(location).attr('href').split('Task/Task/');
    var url = halfUrl[0] + 'Task/Task/GetSelectListItem';

   //ajaxで配列を取得する
    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'json',
        data: {
            
        },
    })
        .done(function (response) {

            $.each(response, function (i, item) {
                var selectlistitem = "<option value='" + ingredient.value + "'>" + ingredient.text + "</option>";
            });
        });

}


//削除ボタン押下時
$(document).on("click", ".delete-icon", function (e) {

    $(this).closest('.taskrow').remove();

    //indexの振りなおし
    ReNumber();

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

            alert(response);

        });
}
