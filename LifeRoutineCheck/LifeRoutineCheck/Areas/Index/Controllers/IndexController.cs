using LifeRoutineCheck.Areas.Index.Services;
using LifeRoutineCheck.Areas.Index.ViewModels;
using LifeRoutineCheck.Areas.Person.ViewModels;
using LifeRoutineCheck.Areas.Routine.ViewModels;
using LifeRoutineCheck.Commons.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Areas.Index.Controllers
{
    [Area("Index")]
    public class IndexController : Controller
    {
        #region 定数
        private IIndexService _service;

        public IndexController(IIndexService services)
        {
            _service = services;
        }
        #endregion

        //=====================================
        //　一覧
        //=====================================

        #region 一覧画面 - 初期表示
        /// <summary>
        /// 新規登録画面 - 初期表示
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int person_Id)
        {
            //ログインユーザー情報の取得
            var viewModel = _service.GetPersonData(person_Id);

            //ルーティーンの取得
            var routineList = _service.GetRoutineData(person_Id);

            viewModel.RoutineLists = routineList.OrderBy(x => x.SortOrder).ToList();

            return View("~/Areas/Index/Views/Index/Index.cshtml", viewModel);
        }
        #endregion

        #region 一覧画面 - ルーティーン並び順更新
        /// <summary>
        /// 一覧画面 - ルーティーン並び順更新
        /// </summary>
        /// <param name="sortOrderIdList">並び順更新後のルーティーンIDリスト</param>
        /// <param name="personId">ユーザーID</param>
        [HttpPost]
        public bool UpdSortOrder(string[] sortOrderIdList, int personId)
        {
            var result = true;

            try
            {
                //並び順の更新処理
                _service.UpdSortOrder(sortOrderIdList.ToList(), personId);

            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
        #endregion
    }
}
