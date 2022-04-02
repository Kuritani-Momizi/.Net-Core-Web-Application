using LifeRoutineCheck.Areas.Index.ViewModels;
using LifeRoutineCheck.Areas.Routine.ViewModels;
using LifeRoutineCheck.Areas.Task.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace LifeRoutineCheck.Areas.Task.Services
{
    public interface ITaskService
    {
        //=====================================
        //　タスク登録・更新画面
        //=====================================

        /// <summary>
        /// タスク登録・更新画面 - タスク取得処理
        /// </summary>
        /// <param name="routineId"></param>
        /// <returns></returns>
        List<ViewTaskUpdate> GetTaskUpdateInfo(int routineId);

        /// <summary>
        /// タスク登録・更新画面 - タスク登録・更新処理
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="routineId"></param>
        /// <returns></returns>
        List<ViewTaskUpdate> UpsertTask(ViewTaskIndex viewModel, int routineId);

        //=====================================
        //　タスク達成確認画面
        //=====================================

        /// <summary>
        /// タスク達成確認画面 - タスク取得処理
        /// </summary>
        /// <param name="routineId"></param>
        /// <returns></returns>
        List<ViewTaskAchievement> GetTaskInfo(int routineId);

        /// <summary>
        /// タスク達成確認画面 - 達成フラグ更新処理
        /// </summary>
        /// <param name="achivementFlg"></param>
        /// <param name="taskId"></param>
        void UpdAchivementFlg(bool achivementFlg, int taskId);

        //=====================================
        //　共通処理
        //=====================================

        /// <summary>
        /// タスク存在チェック
        /// </summary>
        /// <param name="routine_Id"></param>
        /// <returns></returns>
        bool TaskExistCheck(int routine_Id);

        /// <summary>
        /// タスク回数用セレクトリスト　取得処理
        /// </summary>
        /// <returns></returns>
        List<SelectListItem> GetTaskUnitList();

    }
}
