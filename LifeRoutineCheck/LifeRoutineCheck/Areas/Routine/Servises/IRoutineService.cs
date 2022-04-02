using LifeRoutineCheck.Areas.Person.ViewModels;
using LifeRoutineCheck.Areas.Routine.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Areas.Routine.Services
{
    public interface IRoutineService
    {
        //=====================================
        //　ルーティーン　新規登録画面
        //=====================================

        /// <summary>
        /// ルーティーン新規登録画面 - 新規登録処理
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="person_Id"></param>
        /// <param name="fileNm"></param>
        void InsRoutine(ViewRoutineEntry viewModel, int person_Id, string fileNm);

        //=====================================
        //　ルーティーン　更新画面
        //=====================================

        /// <summary>
        /// 更新画面 - 初期表示
        /// </summary>
        /// <param name="person_Id"></param>
        /// <param name="routine_Id"></param>
        /// <returns></returns>
        ViewRoutineUpdate GetRoutineInfo(int person_Id, int routine_Id);

        /// <summary>
        /// 更新画面 - 更新処理
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="person_Id"></param>
        /// <param name="routine_Id"></param>
        /// <param name="fileNm"></param>
        void UpdRoutine(ViewRoutineUpdate viewModel, int person_Id, int routine_Id, string fileNm);
    }
}
