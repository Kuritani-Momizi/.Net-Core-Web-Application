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
using System.Text;

namespace LifeRoutineCheck.Areas.Person.Services.Imprements
{
    public class PersonService : IPersonService
    {
        #region 定数
        private readonly ApplicationDbContext _context;
        public PersonService(ApplicationDbContext context)
        {
            _context = context;
        }
        public NLog.ILogger logger = NLog.LogManager.GetLogger("logger");

        //ユーザー権限区分
        const string IPPAN = "10";
        const string KANRISYA = "20";

        private const string S3_BUCKETNM = "liferoutinecheck/UserIcon";

        #endregion

        //=====================================
        //　ユーザー　新規登録画面
        //=====================================

        #region ユーザー新規登録画面 - 登録処理
        /// <summary>
        /// ユーザー新規登録画面 - 登録処理
        /// </summary>
        /// <param name="viewModel"></param>
        public int InsUser(ViewPersonEntry viewModel)
        {
            using(var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    var dt = DateTime.Now;

                    var insPerson = new Models.Person()
                    {
                        Person_Nm = viewModel.Person_Nm,
                        AuthorityKbn = IPPAN,
                        Password = HashPassword(viewModel.Password),
                        MailAdress = viewModel.MailAdress,
                        EntryDate = dt,
                        UpdDate = dt,
                        PersonIconImg = null
                    };

                    _context.Persons.Add(insPerson);

                    _context.SaveChanges();
                    tr.Commit();

                    return insPerson.Id;
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

        #region ユーザー新規登録画面　- 既存登録ユーザーチェック
        /// <summary>
        /// ユーザー新規登録画面　- 既存登録ユーザーチェック
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>存在フラグ　true:登録済（重複してる） false:未登録</returns>
        public bool RegisteredUserCheck(ViewPersonEntry viewModel, ModelStateDictionary ModelState)
        {
            try
            {
                //入力されたメールアドレスがすでに登録されていた場合
                if (_context.Persons.Any(x => x.MailAdress == viewModel.MailAdress))
                {
                    ModelState.AddModelError("MailAdress", "既に登録されているメールアドレスです。");

                    return true;
                }

                //入力されたユーザー名がすでに登録されていた場合
                if (_context.Persons.Any(x => x.Person_Nm == viewModel.Person_Nm))
                {
                    ModelState.AddModelError("Person_Nm", "既に登録されているユーザー名です。");

                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
                throw;
            }

            return false;
        }
        #endregion

        //=====================================
        //　パスワード再設定画面
        //=====================================

        #region パスワード再設定画面 - パスワード再発行処理
        /// <summary>
        /// パスワード再設定画面 - パスワード再発行処理
        /// </summary>
        /// <param name="viewModel"></param>
        public void UpdPassword(ViewPersonPassword viewModel)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    //更新対象の取得
                    var updTargetr = _context.Persons.FirstOrDefault(x => x.Person_Nm == viewModel.Person_Nm);

                    updTargetr.Password = HashPassword(viewModel.Password);

                    _context.Entry(updTargetr).State = EntityState.Modified;
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

        #region パスワード再設定画面 - 入力値チェック
        /// <summary>
        /// パスワード再設定画面 - 入力値チェック
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="ModelState"></param>
        /// <returns></returns>
        public bool UpdateUserCheck(ViewPersonPassword viewModel, ModelStateDictionary ModelState)
        {
            //ユーザーの存在チェック
            if (!_context.Persons.Any(x => x.Person_Nm == viewModel.Person_Nm))
            {
                ModelState.AddModelError("Person_Nm", "入力されたユーザー名が存在しません。");

                return false;
            }

            var hashedPassword = HashPassword(viewModel.Password);

            //パスワードに変更がない場合
            if (_context.Persons.Any(x => (x.Person_Nm == viewModel.Person_Nm && x.Password == hashedPassword)))
            {
                ModelState.AddModelError("Password", "新しいパスワードを入力してください。");

                return false;
            }

            return true;
        }
        #endregion

        //=====================================
        //　ユーザー　編集画面
        //=====================================

        #region ユーザー編集画面 - 初期表示
        /// <summary>
        /// ユーザー編集画面 - 初期表示
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public ViewPersonUpdate GetUserInfo(int personId)
        {
            var userInfo = _context.Persons.FirstOrDefault(x => x.Id == personId);

            var viewModel = new ViewPersonUpdate()
            {
                Person_Id = userInfo.Id,
                MailAdress = userInfo.MailAdress,
                Person_Nm = userInfo.Person_Nm,
                IconImage_Nm = userInfo.PersonIconImg,
                IconImage_URL = String.IsNullOrEmpty(userInfo.PersonIconImg) ? "" : Commons.CommonUtil.GetS3AccessUrl(userInfo.PersonIconImg, S3_BUCKETNM)
            };

            return viewModel;
        }
        #endregion

        #region ユーザー編集画面 - 削除処理
        /// <summary>
        /// ユーザー編集画面 - 削除処理
        /// </summary>
        /// <param name="viewModel"></param>
        public void DelUser(ViewPersonUpdate viewModel)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    //削除対象の取得
                    //ユーザーテーブル
                    var delUserTarget = _context.Persons.FirstOrDefault(x => x.Id == viewModel.Person_Id);
                    _context.Persons.Remove(delUserTarget);

                    //ルーティーンテーブル
                    var delRoutineTarget = _context.Routines.Where(x => x.Id == viewModel.Person_Id);
                    _context.Routines.RemoveRange(delRoutineTarget);

                    //タスクテーブル
                    var delTaskTarget = _context.Tasks.Where(x => x.Id == viewModel.Person_Id);
                    _context.Tasks.RemoveRange(delTaskTarget);

                    //グループメンバー
                    var delGroupMemberTarget = _context.GroupMembers.Where(x => x.Id == viewModel.Person_Id);
                    _context.GroupMembers.RemoveRange(delGroupMemberTarget);

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

        //=====================================
        //　ユーザー名　編集画面
        //=====================================

        #region ユーザー名編集画面 - 更新処理
        /// <summary>
        /// ユーザー名編集画面 - 更新処理
        /// </summary>
        /// <param name="viewModel"></param>
        public void UpdUserName(ViewPersonNameUpdate viewModel)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    //更新対象の取得
                    var updTargetr = _context.Persons.FirstOrDefault(x => x.Id == viewModel.Person_Id);

                    updTargetr.Person_Nm = viewModel.Person_Nm;

                    _context.Entry(updTargetr).State = EntityState.Modified;
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

        //=====================================
        //　ユーザーメールアドレス　編集画面
        //=====================================

        #region ユーザーメールアドレス編集画面 - 更新処理
        /// <summary>
        /// ユーザーメールアドレス編集画面 - 更新処理
        /// </summary>
        /// <param name="viewModel"></param>
        public void UpdUserMailAddress(ViewPersonMailAddressUpdate viewModel)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    //更新対象の取得
                    var updTargetr = _context.Persons.FirstOrDefault(x => x.Id == viewModel.Person_Id);

                    updTargetr.MailAdress = viewModel.MailAdress;

                    _context.Entry(updTargetr).State = EntityState.Modified;
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

        //=====================================
        //　ユーザーアイコン　編集画面
        //=====================================

        #region ユーザーアイコン - 更新処理
        /// <summary>
        /// ユーザーアイコン - 更新処理
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="fileNm"></param>
        public void UpdUserIcon(ViewPersonIconUpdate viewModel, string fileNm)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    var dt = DateTime.Now;

                    var updPerson = _context.Persons.FirstOrDefault(x => x.Id == viewModel.Person_Id);

                    updPerson.PersonIconImg = fileNm;
                    updPerson.UpdDate = dt;

                    _context.Entry(updPerson).State = EntityState.Modified;

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

        //=====================================
        //　共通処理
        //=====================================

        #region パスワードのハッシュ化
        /// <summary>
        /// パスワードのハッシュ化
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
    }
}
