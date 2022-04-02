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

namespace LifeRoutineCheck.Areas.Index.Services.Imprements
{
    public class IndexService : IIndexService
    {
        #region 定数
        private readonly ApplicationDbContext _context;
        public IndexService(ApplicationDbContext context)
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
        //　一覧画面
        //=====================================

        #region 一覧画面 - ユーザー情報取得処理
        /// <summary>
        /// 一覧画面 - ユーザー情報取得処理
        /// </summary>
        /// <param name="viewModel"></param>
        public ViewIndex GetPersonData(int person_Id)
        {
            var person_Info = _context.Persons.FirstOrDefault(x => x.Id == person_Id);

            var viewModel = new ViewIndex()
            {
                Person_Id = person_Info.Id,
                Person_Nm = person_Info.Person_Nm
            };

            return viewModel;
        }
        #endregion

        #region 一覧画面 - ルーティーン情報取得処理
        /// <summary>
        /// 一覧画面 - ルーティーン情報取得処理
        /// </summary>
        /// <param name="person_Id"></param>
        public List<ViewRoutineList> GetRoutineData(int person_Id)
        {
            var routineList = new List<ViewRoutineList>();
            var routine_Info = _context.Routines.Where(x => x.Person_Id == person_Id);

            foreach(var item in routine_Info)
            {
                var data = new ViewRoutineList()
                {
                    Routtine_Id = item.Id,
                    Group_Id = item.Group_Id,
                    Routtine_Nm = item.Routine_Nm,
                    IconImage_Nm = item.RoutineIconImg,
                    IconImage_URL = Commons.CommonUtil.GetS3AccessUrl(item.RoutineIconImg, S3_BUCKETNM),
                    SortOrder = item.SortOrder
                };

                routineList.Add(data);
            }

            return routineList;
        }
        #endregion

        #region 一覧画面 - 並び順更新処理（ajax）
        /// <summary>
        /// 一覧画面 - 並び順更新処理（ajax）
        /// </summary>
        /// <param name="routineList">並び順更新後のルーティーンIDリスト</param>
        /// <param name="person_Id">ログイン者のユーザーID</param>
        public void UpdSortOrder(List<string> routineList, int person_Id)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    for (var i = 0; i < routineList.Count(); i++)
                    {
                        var a = routineList[i];

                        var updTarget = _context.Routines.FirstOrDefault(x => x.Person_Id == person_Id && x.Id == int.Parse(routineList[i]));

                        updTarget.SortOrder = i + 1;

                        _context.Entry(updTarget).State = EntityState.Modified;
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
    }
}
