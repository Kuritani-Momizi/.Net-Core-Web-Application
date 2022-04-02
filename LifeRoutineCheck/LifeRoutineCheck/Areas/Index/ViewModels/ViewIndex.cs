using LifeRoutineCheck.Areas.Routine.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Areas.Index.ViewModels
{
    public class ViewIndex
    {
        [DisplayName("ユーザーId")]
        public int Person_Id { get; set; }

        [DisplayName("ユーザー名")]
        public string Person_Nm { get; set; }

        [DisplayName("ルーティーンリスト")]
        public List<ViewRoutineList> RoutineLists { get; set; }  
    }
}
