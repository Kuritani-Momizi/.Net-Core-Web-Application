using LifeRoutineCheck.Areas.Person.ViewModels;
using LifeRoutineCheck.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Services
{
    public interface ILoginService
    {
        /// <summary>
        /// ログイン画面 - ログイン処理
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="ModelState"></param>
        /// <returns></returns>
        bool LoginCheck(ViewLogin viewModel, ModelStateDictionary ModelState);

        /// <summary>
        /// ログイン画面 - ユーザーIDの取得
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>ユーザーID</returns>
        int GetPersonId(ViewLogin viewModel);
    }
}
