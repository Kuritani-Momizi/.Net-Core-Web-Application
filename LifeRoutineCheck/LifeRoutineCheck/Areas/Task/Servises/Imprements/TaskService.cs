using LifeRoutineCheck.Areas.Person.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LifeRoutineCheck.Areas.Index.ViewModels;
using LifeRoutineCheck.Areas.Routine.ViewModels;
using LifeRoutineCheck.Areas.Task.ViewModels;
using LifeRoutineCheck.Models;

namespace LifeRoutineCheck.Areas.Task.Services.Imprements
{
    public class TaskService : ITaskService
    {
        #region 定数
        private readonly ApplicationDbContext _context;
        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }
        public NLog.ILogger logger = NLog.LogManager.GetLogger("logger");

        //ユーザー権限区分
        const string IPPAN = "10";
        const string KANRISYA = "20";

        //セレクトリスト区分
        const string TASK_KAISUU = "01";

        private const string S3_BUCKETNM = "liferoutinecheck/RoutineIcon";

        #endregion

        //=====================================
        //　タスク　登録・更新画面
        //=====================================

        #region タスク登録・更新画面 - タスク取得処理
        /// <summary>
        /// タスク登録・更新画面 - タスク取得処理
        /// </summary>
        /// <param name="routineId"></param>
        /// <returns></returns>
        public List<ViewTaskUpdate> GetTaskUpdateInfo(int routineId)
        {
            var taskList = new List<ViewTaskUpdate>();

            var taskInfoes = _context.Tasks.Where(x => x.RoutineId == routineId);

            if (taskInfoes != null && taskInfoes.Any())
            {
                foreach (var item in taskInfoes)
                {
                    var data = new ViewTaskUpdate();

                    data.Routine_Id = routineId;
                    data.Task_Id = item.Id;
                    data.Task_Name = item.Task_Nm;
                    data.Task_Count = item.Task_Count;
                    data.Task_Unit = item.Task_Unit;

                    taskList.Add(data);
                }
            }

            return taskList;
        }
        #endregion

        #region タスク登録・更新画面 - タスク登録・更新処理
        /// <summary>
        /// タスク登録・更新画面 - タスク登録・更新処理
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="routineId"></param>
        /// <returns></returns>
        public List<ViewTaskUpdate> UpsertTask(ViewTaskIndex viewModel, int routineId)
        {
            var dt = DateTime.Now;

            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    var targetTaskList = _context.Tasks.Where(x => x.RoutineId == routineId);

                    if (viewModel.TaskUpdateList != null && viewModel.TaskUpdateList.Any())
                    {
                        //登録処理
                        //新規登録用タスクの取得
                        var registerTaskList = viewModel.TaskUpdateList.Where(x => x.Task_Id == 0);
                        TaskRegister(registerTaskList, viewModel, dt);

                        //更新用タスクの取得
                        var updateTaskList = viewModel.TaskUpdateList.Where(x => x.Task_Id != 0);
                        TaskUpdate(updateTaskList, viewModel, dt);

                        //削除用タスクの取得
                        TaskDelete(targetTaskList, viewModel, dt);
                    }
                    else if (targetTaskList.Any())
                    {
                        //全消去
                        TaskCompleteDelete(viewModel.Routine_Id);
                    }

                    _context.SaveChanges();
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex);
                    tr.Rollback();
                    throw;
                }
            }

            //タスク登録・更新後のリストの取得
            var taskList = GetTaskUpdateInfo(routineId);

            return taskList;
        }
        #endregion

        #region タスク登録・編集画面 - 登録実行処理
        /// <summary>
        /// タスク登録・編集画面 - 登録実行処理
        /// </summary>
        /// <param name="registerTaskList">登録用ViewModel</param>
        /// <param name="viewModel"></param>
        /// <param name="dt"></param>
        public void TaskRegister(IEnumerable<ViewTaskUpdate> registerTaskList, ViewTaskIndex viewModel, DateTime dt)
        {
            if (registerTaskList.Any())
            {
                foreach (var item in registerTaskList)
                {
                    var registerTask = ConvertTaskUpsert(item, viewModel.Routine_Id, dt);

                    _context.Tasks.Add(registerTask);
                }
            }
        }
        #endregion

        #region タスク登録・編集画面 - 更新実行処理
        /// <summary>
        /// タスク登録・編集画面 - 更新実行処理
        /// </summary>
        /// <param name="updateTaskList">更新用ViewModel</param>
        /// <param name="viewModel"></param>
        /// <param name="dt"></param>
        public void TaskUpdate(IEnumerable<ViewTaskUpdate> updateTaskList, ViewTaskIndex viewModel, DateTime dt)
        {
            if (updateTaskList.Any())
            {
                foreach (var item in updateTaskList)
                {
                    var updTarget = _context.Tasks.FirstOrDefault(x => x.Id == item.Task_Id);

                    updTarget.Task_Nm = item.Task_Name;
                    updTarget.Task_Unit = item.Task_Unit;
                    updTarget.Task_Count = item.Task_Count;
                    updTarget.UpdDate = dt;

                    _context.Tasks.Update(updTarget);
                }
            }
        }
        #endregion

        #region タスク登録・編集画面 - 削除実行処理
        /// <summary>
        /// タスク登録・編集画面 - 削除実行処理
        /// </summary>
        /// <param name="targetTaskList">登録済みタスク</param>
        /// <param name="viewModel"></param>
        /// <param name="dt"></param>
        public void TaskDelete(IQueryable<Models.Task> targetTaskList, ViewTaskIndex viewModel, DateTime dt)
        {
            if (targetTaskList.Any())
            {
                var delTaskList = targetTaskList.ToList().Where(x => !viewModel.TaskUpdateList.Any(y => y.Task_Id == x.Id));

                if (delTaskList.Any())
                {
                    _context.Tasks.RemoveRange(delTaskList);
                }
            }
        }
        #endregion

        #region タスク登録・編集画面 - 完全削除実行処理
        /// <summary>
        /// タスク登録・編集画面 - 完全削除実行処理
        /// </summary>
        /// <param name="routineId"></param>
        public void TaskCompleteDelete(int routineId)
        {
            var delAllTaskList = _context.Tasks.Where(x => x.RoutineId == routineId).ToList();

            _context.Tasks.RemoveRange(delAllTaskList);
        }
        #endregion

        #region タスク登録・編集画面 - コンバート処理(ViewModel→Model)
        /// <summary>
        /// タスク登録・編集画面 - コンバート処理(ViewModel→Model)
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="routineId"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public Models.Task ConvertTaskUpsert(ViewTaskUpdate viewModel, int routineId, DateTime dt)
        {
            var model = new Models.Task()
            {
                RoutineId = routineId,
                Task_Nm = viewModel.Task_Name,
                Task_Unit = viewModel.Task_Unit,
                Task_Count = viewModel.Task_Count,
                EntryDate = dt,
                UpdDate = dt
            };

            return model;
        }
        #endregion

        //=====================================
        //　タスク　達成確認画面
        //=====================================

        #region タスク達成確認画面 - タスク取得処理
        /// <summary>
        /// タスク達成確認画面 - タスク取得処理
        /// </summary>
        /// <param name="routineId"></param>
        /// <returns></returns>
        public List<ViewTaskAchievement> GetTaskInfo(int routineId)
        {
            var taskInfoes = _context.Tasks.Where(x => x.RoutineId == routineId);

            var selectListData = _context.SelectLists.Where(x => x.SelectList_Kbn == TASK_KAISUU).ToList();

            var taskList = new List<ViewTaskAchievement>();

            var dt = DateTime.Now;

            foreach (var item in taskInfoes.ToList())
            {
                var task = new ViewTaskAchievement();

                task.Task_Id = item.Id;
                task.Task_Name = item.Task_Nm;
                task.Task_Count = item.Task_Count;
                task.Unit = selectListData.FirstOrDefault(x=>x.Code == item.Task_Unit).Text;
                task.Achivement_Flg = GetTaskAchivementStatus(item.Id, dt);

                taskList.Add(task);
            }

            return taskList;
        }
        #endregion

        #region タスク達成確認画面 - 達成フラグ更新処理
        /// <summary>
        /// タスク達成確認画面 - 達成フラグ更新処理
        /// </summary>
        /// <param name="achivementFlg">解除:true 達成:falseでフラグが引き渡される</param>
        /// <param name="taskId"></param>
        public void UpdAchivementFlg(bool achivementFlg, int taskId)
        {
            var dt = DateTime.Now;

            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {

                    //解除処理
                    if (achivementFlg)
                    {
                        var delTarget = _context.TaskCompletes.FirstOrDefault(x => x.Task_Id == taskId && x.UpdDate == dt.Date);

                        _context.TaskCompletes.Remove(delTarget);
                    }
                    //達成処理
                    else
                    {
                        var updTarget = _context.TaskCompletes.FirstOrDefault(x => x.Task_Id == taskId);

                        //タスク登録後初めてタスク達成した場合、または日付変更後はじめてタスク達成した場合
                        if (updTarget == null || updTarget.UpdDate.Date != dt.Date)
                        {
                            var registerData = new TaskComplete();

                            registerData.Task_Id = taskId;
                            registerData.TaskComplete_Flg = true;
                            registerData.UpdDate = dt.Date;

                            _context.TaskCompletes.Add(registerData);
                        }
                        else
                        {
                            updTarget.TaskComplete_Flg = achivementFlg ? false : true;
                            updTarget.UpdDate = dt.Date;

                            _context.Entry(updTarget).State = EntityState.Modified;
                        }
                    }

                    _context.SaveChanges();
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex);
                    tr.Rollback();
                    throw;
                }
            }
        }
        #endregion

        #region タスク達成確認画面 - 達成状況取得処理
        /// <summary>
        /// タスク達成確認画面 - 達成状況取得処理
        /// </summary>
        /// <param name="taskId">タスクID</param>
        /// <param name="dt">現在日付</param>
        /// <returns>タスク達成フラグ true:達成済み false:未達成</returns>
        public bool GetTaskAchivementStatus(int taskId, DateTime dt)
        {
            var achivementDate = _context.TaskCompletes.FirstOrDefault(x => x.Task_Id == taskId);

            if(achivementDate == null || achivementDate.UpdDate.Date != dt.Date)
            {
                return false;
            }

            return true;
        }
        #endregion

        //=====================================
        //　共通処理
        //=====================================

        #region タスク存在チェック
        /// <summary>
        /// タスク存在チェック
        /// </summary>
        /// <param name="routine_Id">ルーティーンID</param>
        /// <returns>true:タスクが存在する　false:タスクが存在しない</returns>
        public bool TaskExistCheck(int routine_Id)
        {
            if (_context.Tasks.Any(x => x.RoutineId == routine_Id))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region タスク回数用セレクトリスト　取得処理
        /// <summary>
        /// タスク回数用セレクトリスト　取得処理
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetTaskUnitList()
        {
            var selectListData = _context.SelectLists.Where(x => x.SelectList_Kbn == TASK_KAISUU).ToList();

            var selectList = new List<SelectListItem>();

            foreach(var item in selectListData)
            {
                var data = new SelectListItem { Value = item.Code, Text = item.Text };

                selectList.Add(data);
            }

            return selectList;
        }
        #endregion

    }
}
