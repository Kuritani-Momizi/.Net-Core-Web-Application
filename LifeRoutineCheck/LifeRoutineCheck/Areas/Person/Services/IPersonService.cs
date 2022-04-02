using LifeRoutineCheck.Areas.Person.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Areas.Person.Services
{
    public interface IPersonService
    {
        //=====================================
        //　ユーザー　新規登録画面
        //=====================================

        /// <summary>
        /// ユーザー新規登録画面 - 登録処理
        /// </summary>
        /// <param name="viewModel"></param>
        int InsUser(ViewPersonEntry viewModel);

        /// <summary>
        ///  登録画面　- 既存登録ユーザーチェック
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="ModelState"></param>
        /// <returns>存在フラグ　true:未登録 false:登録済（重複してる）</returns>
        bool RegisteredUserCheck(ViewPersonEntry viewModel, ModelStateDictionary ModelState);

        //=====================================
        //　ユーザー　パスワード再設定画面
        //=====================================

        /// <summary>
        /// パスワード再設定画面 - パスワード再発行処理
        /// </summary>
        /// <param name="viewModel"></param>
        void UpdPassword(ViewPersonPassword viewModel);

        /// <summary>
        /// ユーザー新規登録画面　- 既存登録ユーザーチェック
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="ModelState"></param>
        /// <returns></returns>
        bool UpdateUserCheck(ViewPersonPassword viewModel, ModelStateDictionary ModelState);

        //=====================================
        //　ユーザー　編集画面
        //=====================================

        /// <summary>
        /// ユーザー編集画面 - 初期表示
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        ViewPersonUpdate GetUserInfo(int personId);

        /// <summary>
        /// ユーザー編集画面 - 削除処理
        /// </summary>
        /// <param name="viewModel"></param>
        void DelUser(ViewPersonUpdate viewModel);

        //=====================================
        //　ユーザー名　編集画面
        //=====================================

        /// <summary>
        /// ユーザー名編集画面 - 更新処理
        /// </summary>
        /// <param name="viewModel"></param>
        void UpdUserName(ViewPersonNameUpdate viewModel);

        //=====================================
        //　ユーザーメールアドレス　編集画面
        //=====================================

        /// <summary>
        /// ユーザーメールアドレス編集画面 - 更新処理
        /// </summary>
        /// <param name="viewModel"></param>
        void UpdUserMailAddress(ViewPersonMailAddressUpdate viewModel);

        //=====================================
        //　ユーザーアイコン　編集画面
        //=====================================

        /// <summary>
        /// ユーザーアイコン - 更新処理
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="fileNm"></param>
        void UpdUserIcon(ViewPersonIconUpdate viewModel, string fileNm);
    }
}
