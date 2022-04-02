using LifeRoutineCheck.Areas.Index.ViewModels;
using LifeRoutineCheck.Areas.Routine.ViewModels;
using System.Collections.Generic;

namespace LifeRoutineCheck.Areas.Index.Services
{
    public interface IIndexService
    {
        //=====================================
        //　一覧画面
        //=====================================

        /// <summary>
        /// 一覧画面 - ユーザー情報取得処理
        /// </summary>
        /// <param name="person_Id"></param>
        /// <returns></returns>
        ViewIndex GetPersonData(int person_Id);

        /// <summary>
        /// 一覧画面 - 並び順更新処理
        /// </summary>
        /// <param name="routineList"></param>
        /// <param name="person_Id"></param>
        void UpdSortOrder(List<string> routineList, int person_Id);

        /// <summary>
        /// 一覧画面 - ルーティーン情報取得処理
        /// </summary>
        /// <param name="person_Id"></param>
        /// <returns></returns>
        List<ViewRoutineList> GetRoutineData(int person_Id);
    }
}
