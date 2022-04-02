using Amazon.S3;
using Amazon.S3.Transfer;
using LifeRoutineCheck.Areas.Person.Services;
using LifeRoutineCheck.Areas.Person.ViewModels;
using LifeRoutineCheck.Areas.Routine.Services;
using LifeRoutineCheck.Areas.Routine.ViewModels;
using LifeRoutineCheck.Commons;
using LifeRoutineCheck.Commons.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Areas.Routine.Controllers
{
    [Area("Routine")]
    public class RoutineController : Controller
    {
        #region 定数
        private IRoutineService _service;
        [Obsolete]
        private IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public RoutineController(IRoutineService services, IHostingEnvironment environment)
        {
            _service = services;
            _hostingEnvironment = environment;
        }

        //AWSのS3のバケット名
        //バケット内の特定のフォルダーを選択する場合はこのように記載する
        private const string S3_BUCKETNM = "liferoutinecheck/RoutineIcon";

        #endregion

        //=====================================
        //　ルーティーン　新規登録画面
        //=====================================

        #region 新規登録画面 - 初期表示
        /// <summary>
        /// 新規登録画面 - 初期表示
        /// </summary>
        /// <returns></returns>
        public IActionResult EntryIndex()
        {
            var person_Id = HttpContext.Session.Get<int>("PersonId");

            var viewModel = new ViewRoutineEntry()
            {
                Person_Id = (int)person_Id
            };

            return View("~/Areas/Routine/Views/Register.cshtml", viewModel);
        }
        #endregion

        #region 新規登録画面 - 登録ボタン押下
        /// <summary>
        /// 新規登録画面 - 登録ボタン押下
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Obsolete]
        public async Task<IActionResult> InsEntry(ViewRoutineEntry viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Areas/Routine/Views/Register.cshtml", viewModel);
            }

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
            _service.InsRoutine(viewModel, viewModel.Person_Id, fileNm.Result);

            //一覧画面へ遷移
            return RedirectToAction("Index", "Index", new { area = "Index", person_Id = viewModel.Person_Id });
        }
        #endregion

        //=====================================
        //　ルーティーン　編集画面
        //=====================================

        #region 更新画面 - 初期表示
        /// <summary>
        /// 更新画面 - 初期表示
        /// </summary>
        /// <param name="personId">ユーザーID</param>
        /// <param name="routineId">更新用ルーティーンID</param>
        /// <returns></returns>
        public IActionResult UpdateIndex(int personId, int routineId)
        {
            var viewModel = _service.GetRoutineInfo(personId,routineId);

            return View("~/Areas/Routine/Views/Update.cshtml", viewModel);
        }
        #endregion

        #region 更新画面 - 更新ボタン押下
        /// <summary>
        /// 更新画面 - 更新ボタン押下
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Obsolete]
        public async Task<IActionResult> UpdRoutine(ViewRoutineUpdate viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Areas/Routine/Views/Update.cshtml", viewModel);
            }

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
            _service.UpdRoutine(viewModel, viewModel.Person_Id, viewModel.Routine_Id, fileNm.Result);

            //一覧画面へ遷移
            return RedirectToAction("Index", "Index", new { area = "Index", person_Id = viewModel.Person_Id });
        }
        #endregion

    }
}
