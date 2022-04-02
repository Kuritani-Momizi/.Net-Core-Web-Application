using LifeRoutineCheck.Areas.Person.Services;
using LifeRoutineCheck.Areas.Person.ViewModels;
using LifeRoutineCheck.Commons;
using LifeRoutineCheck.Commons.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Areas.Person.Controllers
{
    [Area("Person")]
    public class PersonController : Controller
    {
        #region 定数
        private IPersonService _service;
        [Obsolete]
        private IHostingEnvironment _hostingEnvironment;

        //AWSのS3のバケット名
        //バケット内の特定のフォルダーを選択する場合はこのように記載する
        private const string S3_BUCKETNM = "liferoutinecheck/UserIcon";

        [Obsolete]
        public PersonController(IPersonService services, IHostingEnvironment environment)
        {
            _service = services;
            _hostingEnvironment = environment;
        }
        #endregion

        //=====================================
        //　ユーザー　新規登録画面
        //=====================================

        #region 新規登録画面 - 初期表示
        /// <summary>
        /// 新規登録画面 - 初期表示
        /// </summary>
        /// <returns></returns>
        public IActionResult EntryIndex()
        {
            return View("~/Areas/Person/Views/Register/Register.cshtml", new ViewPersonEntry());
        }
        #endregion

        #region 新規登録画面 - 登録ボタン押下
        /// <summary>
        /// 新規登録画面 - 登録ボタン押下
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public IActionResult InsUser(ViewPersonEntry viewModel)
        {
            //エラーチェック
            if (ModelState.IsValid && !_service.RegisteredUserCheck(viewModel, ModelState))
            {
                //ユーザー登録処理
                var person_Id = _service.InsUser(viewModel);

                //一覧画面へ遷移
                return RedirectToAction("Index", "Index", new { area = "Index", person_Id = person_Id });
            }

            return View("~/Areas/Person/Views/Register/Register.cshtml", viewModel);
        }
        #endregion

        //=====================================
        //　パスワード再発行画面
        //=====================================

        #region パスワード再設定画面 - 初期表示
        /// <summary>
        /// パスワード再設定画面 - 初期表示
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdatePasswordIndex()
        {
            return View("~/Areas/Person/Views/Update/UpdatePassword.cshtml", new ViewPersonPassword());
        }
        #endregion

        #region パスワード再発行画面 - パスワード再発行
        /// <summary>
        /// パスワード再発行画面 - パスワード再発行
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public IActionResult UpdPassword(ViewPersonPassword viewModel)
        {
            //エラーチェック
            if (ModelState.IsValid && _service.UpdateUserCheck(viewModel,ModelState))
            {
                //パスワード更新処理
                _service.UpdPassword(viewModel);

                //完了画面へ遷移
                ViewBag.CompleteMessage = "Password";
                return View("~/Areas/Person/Views/Update/Complete.cshtml");
            }

            return View("~/Areas/Person/Views/Update/UpdatePassword.cshtml", viewModel);
        }
        #endregion

        //=====================================
        //　ユーザー　編集画面
        //=====================================

        #region 編集画面 - 初期表示
        /// <summary>
        /// 編集画面 - 初期表示
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public IActionResult UpdateUserIndexPage(int personId)
        {
            //ユーザー情報取得
            var viewModel = _service.GetUserInfo(personId);

            return View("~/Areas/Person/Views/Update/Update.cshtml", viewModel);
        }
        #endregion

        #region 編集画面 - 初期表示(サイドメニューから遷移)
        /// <summary>
        /// 編集画面 - 初期表示
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public IActionResult UpdateUserIndex()
        {
            var personId = HttpContext.Session.Get<int>("PersonId");

            //ユーザー情報取得
            var viewModel = _service.GetUserInfo(personId);

            return View("~/Areas/Person/Views/Update/Update.cshtml", viewModel);
        }
        #endregion

        #region 編集画面 - 削除処理
        /// <summary>
        /// 編集画面 - 削除処理
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public IActionResult DeleteUser(ViewPersonUpdate viewModel)
        {
            //削除処理
            _service.DelUser(viewModel);

            ViewBag.CompleteMessage = "Delete";
            return View("~/Areas/Person/Views/Update/Complete.cshtml", viewModel);
        }
        #endregion
        //=====================================
        //　ユーザー名　編集画面
        //=====================================

        #region ユーザー名編集画面　- 初期表示
        /// <summary>
        /// ユーザー名編集画面　- 初期表示
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public IActionResult UpdateUserNameIndex(int personId)
        {
            //ユーザー情報取得
            var user_Info = _service.GetUserInfo(personId);

            var viewModel = new ViewPersonNameUpdate()
            {
                Person_Id = personId,
                Person_Nm = user_Info.Person_Nm
            };

            return View("~/Areas/Person/Views/Update/UpdateUserName.cshtml", viewModel);
        }
        #endregion

        #region ユーザー名編集画面 - 更新ボタン押下
        /// <summary>
        /// ユーザー名編集画面 - 更新ボタン押下
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public IActionResult UpdateUserName(ViewPersonNameUpdate viewModel)
        {
            var errorCheckViewModel = new ViewPersonEntry()
            {
                Person_Nm = viewModel.Person_Nm
            };

            //エラーチェック
            if (ModelState.IsValid && !_service.RegisteredUserCheck(errorCheckViewModel, ModelState))
            {
                //ユーザー名更新処理
                _service.UpdUserName(viewModel);

                ViewBag.CompleteMessage = "UserName";
                return View("~/Areas/Person/Views/Update/Complete.cshtml", viewModel);
            }

            return View("~/Areas/Person/Views/Update/UpdateUserName.cshtml", viewModel);
        }
        #endregion

        //=====================================
        //　メールアドレス　編集画面
        //=====================================

        #region メールアドレス編集画面　- 初期表示
        /// <summary>
        /// メールアドレス編集画面　- 初期表示
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public IActionResult UpdateUserMailAddressIndex(int personId)
        {
            //ユーザー情報取得
            var user_Info = _service.GetUserInfo(personId);

            var viewModel = new ViewPersonMailAddressUpdate()
            {
                Person_Id = personId,
                MailAdress = user_Info.MailAdress
            };

            return View("~/Areas/Person/Views/Update/UpdateUserMailAddress.cshtml", viewModel);
        }
        #endregion

        #region メールアドレス編集画面 - 更新ボタン押下
        /// <summary>
        /// メールアドレス編集画面 - 更新ボタン押下
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public IActionResult UpdateUserMailAddress(ViewPersonMailAddressUpdate viewModel)
        {
            var errorCheckViewModel = new ViewPersonEntry()
            {
                MailAdress = viewModel.MailAdress
            };

            //エラーチェック
            if (ModelState.IsValid && !_service.RegisteredUserCheck(errorCheckViewModel, ModelState))
            {
                //ユーザー名更新処理
                _service.UpdUserMailAddress(viewModel);

                ViewBag.CompleteMessage = "MailAddress";
                return View("~/Areas/Person/Views/Update/Complete.cshtml", viewModel);
            }

            return View("~/Areas/Person/Views/Update/UpdateUserMailAddress.cshtml", viewModel);
        }
        #endregion

        //=====================================
        //　ユーザーアイコン　編集画面
        //=====================================

        #region ユーザーアイコン編集画面　- 初期表示
        /// <summary>
        /// ユーザーアイコン編集画面　- 初期表示
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public IActionResult UpdateUserIconIndex(int personId)
        {
            //ユーザー情報取得
            var user_Info = _service.GetUserInfo(personId);

            var viewModel = new ViewPersonIconUpdate()
            {
                Person_Id = personId,
                IconImage_URL = user_Info.IconImage_URL
            };

            return View("~/Areas/Person/Views/Update/UpdateUserIcon.cshtml", viewModel);
        }
        #endregion

        #region ユーザーアイコン編集画面 - 更新ボタン押下
        /// <summary>
        /// ユーザーアイコン編集画面 - 更新ボタン押下
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Obsolete]
        public async Task<IActionResult> UpdateUserIcon(ViewPersonIconUpdate viewModel)
        {
            if (ModelState.IsValid)
            {
                var filePath = string.Empty;
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

                //拡張子の取得
                var extent = Path.GetExtension(viewModel.IconImage.FileName);

                if (viewModel.IconImage.Length > 0)
                {
                    filePath = Path.Combine(uploads, viewModel.IconImage.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.IconImage.CopyToAsync(fileStream);
                    }
                }

                var fileNm = CommonUtil.S3FileUpload(filePath, S3_BUCKETNM, extent);

                fileNm.Wait();

                //ルーティーン登録処理
                _service.UpdUserIcon(viewModel, fileNm.Result);

                ViewBag.CompleteMessage = "UserIcon";
                return View("~/Areas/Person/Views/Update/Complete.cshtml", viewModel);
            }

            return View("~/Areas/Person/Views/Update/UpdateUserIcon.cshtml", viewModel);
        }
        #endregion
    }
}
