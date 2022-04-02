using LifeRoutineCheck.Areas.Person.Services;
using LifeRoutineCheck.Commons.Extensions;
using LifeRoutineCheck.Models;
using LifeRoutineCheck.Services;
using LifeRoutineCheck.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Controllers
{
    public class LoginController : Controller
    {
        #region 定数
        private ILoginService _service;

        public LoginController(ILoginService services)
        {
            _service = services;
        }
        #endregion

        #region ログイン画面 - 初期表示
        /// <summary>
        /// ログイン画面 - 初期表示
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View("Index", new ViewLogin());
        }
        #endregion

        #region ログイン画面 - ログイン処理
        /// <summary>
        /// ログイン画面 - ログイン処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns>タスク画面へ遷移</returns>
        public IActionResult Login(ViewLogin viewModel)
        {
            //ログイン処理
            if (!ModelState.IsValid || _service.LoginCheck(viewModel, ModelState))
            {
                //エラー　画面に返す
                return View("Index", viewModel);
            }

            //ユーザーIDの取得
            var person_Id = _service.GetPersonId(viewModel);

            HttpContext.Session.Set<int>("PersonId", person_Id);

            //一覧画面へ遷移
            return RedirectToAction("Index", "Index", new { area = "Index", person_Id = person_Id });
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
