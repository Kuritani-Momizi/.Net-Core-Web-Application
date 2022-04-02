using LifeRoutineCheck.Areas.Index.Services;
using LifeRoutineCheck.Areas.Index.ViewModels;
using LifeRoutineCheck.Areas.Person.ViewModels;
using LifeRoutineCheck.Areas.Routine.ViewModels;
using LifeRoutineCheck.Areas.Task.Services;
using LifeRoutineCheck.Areas.Task.ViewModels;
using LifeRoutineCheck.Commons.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Areas.Task.Controllers
{
    [Area("Task")]
    public class TaskController : Controller
    {
        #region 定数
        private ITaskService _service;

        public TaskController(ITaskService services)
        {
            _service = services;
        }
        #endregion

        //=====================================
        //　タスク　登録・編集画面
        //=====================================

        #region タスク登録・更新画面 - 初期表示
        /// <summary>
        /// タスク登録・更新画面 - 初期表示
        /// </summary>
        /// <returns></returns>
        public IActionResult TaskUpsertIndex(int personId, int routineId)
        {
            //ルーティーンに紐づくタスクの取得
            var viewModel = new ViewTaskIndex()
            {
                Person_Id = personId,
                Routine_Id = routineId
            };

            viewModel.TaskUpdateList = _service.GetTaskUpdateInfo(routineId);

            //セレクトリストの取得
            foreach (var item in viewModel.TaskUpdateList)
            {
                item.Task_UnitList = _service.GetTaskUnitList();
            }

            return View("~/Areas/Task/Views/Update/Update.cshtml", viewModel);
        }
        #endregion

        #region タスク登録・更新画面 - タスク回数用セレクトリスト取得
        /// <summary>
        /// タスク登録・更新画面 - タスク回数用セレクトリスト取得
        /// </summary>
        /// <returns></returns>
        public IActionResult GetSelectListItem()
        {
            //セレクトリスト用データの取得
            var taskUnitList = _service.GetTaskUnitList();

            return Json(taskUnitList);
        }
        #endregion

        #region タスク登録・更新画面 - 確定ボタン押下
        /// <summary>
        /// タスク登録・更新画面 - 確定ボタン押下
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public IActionResult UpsertTask(ViewTaskIndex viewModel)
        {
            //エラーチェック
            if (ModelState.IsValid)
            {
                //タスク登録・編集処理
                viewModel.TaskUpdateList = _service.UpsertTask(viewModel, viewModel.Routine_Id); 
            }

            //セレクトリストの取得
            foreach (var item in viewModel.TaskUpdateList)
            {
                item.Task_UnitList = _service.GetTaskUnitList();
            }

            return View("~/Areas/Task/Views/Update/Update.cshtml", viewModel);
        }
        #endregion

        //=====================================
        //　タスク　達成確認画面
        //=====================================

        #region タスク達成確認画面 - 初期表示
        /// <summary>
        /// タスク達成確認画面 - 初期表示
        /// </summary>
        /// <returns></returns>
        public IActionResult TaskAchievementIndex(int personId, int routineId)
        {
            //ルーティーンに紐づくタスクの取得
            var viewModel = new ViewTaskIndex()
            {
                Person_Id = personId,
                Routine_Id = routineId
            };

            viewModel.TaskList = _service.GetTaskInfo(routineId);

            return View("~/Areas/Task/Views/Achievement/Achievement.cshtml", viewModel);
        }
        #endregion

        #region タスク達成確認画面 - 達成フラグ更新(ajax)
        /// <summary>
        /// タスク達成確認画面 - 達成フラグ更新(ajax)
        /// </summary>
        /// <param name="achivementFlg">解除:true 達成:falseでフラグが引き渡される</param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public bool UpdAchivement(string achivementFlg, string taskId)
        {
            var achivement_Flg = bool.Parse(achivementFlg);
            var task_Id = int.Parse(taskId);

            //達成フラグ更新
            _service.UpdAchivementFlg(achivement_Flg, task_Id);

            var returnAchivement_Flg = achivement_Flg ? false : true;

            return returnAchivement_Flg;
        }
        #endregion

        //=====================================
        //　共通処理
        //=====================================

        #region タスク振り分け用コントローラー
        /// <summary>
        /// タスク振り分け用コントローラー
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="routineId"></param>
        /// <returns></returns>
        public IActionResult TaskViewSort(int personId, int routineId)
        {
            //タスクの存在チェック
            //ルーティーンに紐づくタスクが存在する場合は、タスク達成画面へ遷移する
            if (_service.TaskExistCheck(routineId))
            {
                return RedirectToAction("TaskAchievementIndex", new { person_Id = personId, routineId = routineId });
            }

            //存在しない場合は、タスク登録画面へ遷移する
            return RedirectToAction("TaskUpsertIndex", new { person_Id = personId, routineId = routineId });
        }
        #endregion

    }
}
