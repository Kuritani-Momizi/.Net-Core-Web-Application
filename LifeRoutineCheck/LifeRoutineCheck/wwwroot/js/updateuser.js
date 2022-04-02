$(document).on('click', '.fake-delete-submit-btn', function () {
    if (!confirm('ユーザーを削除します。よろしいですか？')) {

        return false;
    } else {
        $('.delete-submit-btn').trigger("click");
    }
});
