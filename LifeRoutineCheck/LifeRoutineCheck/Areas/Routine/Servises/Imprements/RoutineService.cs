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
using LifeRoutineCheck.Areas.Routine.ViewModels;

namespace LifeRoutineCheck.Areas.Routine.Services.Imprements
{
    public class RoutineService : IRoutineService
    {
        #region 定数
        private readonly ApplicationDbContext _context;
        public RoutineService(ApplicationDbContext context)
        {
            _context = context;
        }
        public NLog.ILogger logger = NLog.LogManager.GetLogger("logger");

        //ユーザー権限区分
        const string IPPAN = "10";
        const string KANRISYA = "20";

        private const string S3_BUCKETNM = "liferoutinecheck/RoutineIcon";
        #endregion

        //=====================================
        //　ルーティーン　新規登録画面
        //=====================================

        #region 新規登録画面 - 新規登録処理
        /// <summary>
        /// 新規登録画面 - 新規登録処理
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="person_Id"></param>
        /// <param name="fileNm"></param>
        public void InsRoutine(ViewRoutineEntry viewModel, int person_Id, string fileNm)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    var dt = DateTime.Now;
                    int maxSortOrder;

                    var sortOrderInfo = _context.Routines.Where(x => x.Person_Id == person_Id);

                    if(sortOrderInfo == null || !sortOrderInfo.Any())
                    {
                        maxSortOrder = 1;
                    }
                    else
                    {
                        maxSortOrder = sortOrderInfo.Max(x => x.SortOrder) + 1;
                    }

                    var insRoutine = new Models.Routine()
                    {
                        Person_Id = person_Id,
                        Group_Id = 0,
                        Routine_Nm = viewModel.Routtine_Nm,
                        RoutineKbn = "00",
                        RoutineIconImg = fileNm,
                        SortOrder = maxSortOrder,
                        EntryDate = dt,
                        UpdDate = dt,
                        UpdPerson_Id = person_Id
                    };

                    _context.Routines.Add(insRoutine);

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
        //　ルーティーン　更新画面
        //=====================================

        #region 更新画面 - 初期表示
        /// <summary>
        /// 更新画面 - 初期表示
        /// </summary>
        /// <param name="person_Id">ユーザーID</param>
        /// <param name="routine_Id">更新用ルーティーンID</param>
        /// <returns>更新用ルーティーン情報</returns>
        public ViewRoutineUpdate GetRoutineInfo(int person_Id, int routine_Id)
        {
            var updTarget = _context.Routines.FirstOrDefault(x => x.Person_Id == person_Id && x.Id == routine_Id);

            var updRoutine = new ViewRoutineUpdate()
            {
                Person_Id = person_Id,
                Routine_Id = routine_Id,
                IconImage_Nm = updTarget.RoutineIconImg,
                Routtine_Nm = updTarget.Routine_Nm,
                IconImage_URL = Commons.CommonUtil.GetS3AccessUrl(updTarget.RoutineIconImg, S3_BUCKETNM)
            };

            return updRoutine;
        }
        #endregion

        #region 更新画面 - 更新処理
        /// <summary>
        /// 更新画面 - 更新処理
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="person_Id"></param>
        /// <param name="routine_Id"></param>
        /// <param name="fileNm"></param>
        public void UpdRoutine(ViewRoutineUpdate viewModel, int person_Id, int routine_Id, string fileNm)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    var dt = DateTime.Now;

                    var updRoutine = _context.Routines.FirstOrDefault(x => x.Person_Id == person_Id && x.Id == routine_Id);

                    updRoutine.Routine_Nm = viewModel.Routtine_Nm;
                    updRoutine.RoutineIconImg = fileNm;
                    updRoutine.UpdDate = dt;
                    updRoutine.UpdPerson_Id = person_Id;

                    _context.Entry(updRoutine).State = EntityState.Modified;

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

    }
}
