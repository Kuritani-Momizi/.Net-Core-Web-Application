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
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using LifeRoutineCheck.ViewModels;
using System.Text;

namespace LifeRoutineCheck.Services.Imprements
{
    public class LoginService : ILoginService
    {
        #region 初期設定・定数
        private readonly ApplicationDbContext _context;
        public LoginService(ApplicationDbContext context)
        {
            _context = context;
        }
        public NLog.ILogger logger = NLog.LogManager.GetLogger("logger");

        //ユーザー権限区分
        const string IPPAN = "10";
        const string KANRISYA = "20";
        #endregion

        //=====================================
        //　ログイン
        //=====================================

        #region ログイン画面 - ログイン処理
        /// <summary>
        /// ログイン画面 - ログイン処理
        /// </summary>
        /// <param name="viewModel">入力データ</param>
        /// <param name="ModelState"></param>
        /// <returns></returns>
        public bool LoginCheck(ViewLogin viewModel, ModelStateDictionary ModelState)
        {
            //入力されたユーザー名が存在しない場合
            if (!_context.Persons.Any(x => x.Person_Nm == viewModel.Person_Nm))
            {
                ModelState.AddModelError("Person_Nm", "入力されたユーザー名が存在しません。");

                return true;
            }

            var hashedPassword = HashPassword(viewModel.Password);

            //パスワードが一致しない場合
            if (!_context.Persons.Any(x => (x.Person_Nm == viewModel.Person_Nm && x.Password == hashedPassword)))
            {
                ModelState.AddModelError("Password", "入力されたパスワードが正しくありません。");

                return true;
            }

            return false;
        }
        #endregion

        #region ログイン画面 - パスワードのハッシュ化
        /// <summary>
        /// ログイン画面 - パスワードのハッシュ化
        /// </summary>
        /// <param name="password">入力パスワード</param>
        /// <returns>ハッシュ化された入力パスワード</returns>
        public string HashPassword(string password)
        {
            byte[] ar1 = Encoding.UTF8.GetBytes(password);
            byte[] ar2 = new SHA256CryptoServiceProvider().ComputeHash(ar1);

            // バイト配列 → 16進数文字列
            var hashedPassword = new StringBuilder();
            foreach (byte b1 in ar2)
            {
                hashedPassword.Append(b1.ToString("x2"));
            }

            return hashedPassword.ToString();
        }
        #endregion

        #region ログイン画面 - ユーザーIDの取得
        /// <summary>
        /// ログイン画面 - ユーザーIDの取得
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>ログインユーザーID</returns>
        public int GetPersonId(ViewLogin viewModel)
        {
            var person_Id = _context.Persons.FirstOrDefault(x => x.Person_Nm == viewModel.Person_Nm).Id;

            return person_Id;
        }
        #endregion

    }
}
