using LifeRoutineCheck.Areas.Routine.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Areas.Task.ViewModels
{
    public class ViewTaskAchievement
    {
        [DisplayName("ルーティーンID")]
        public int Routine_Id { get; set; }

        [DisplayName("タスクID")]
        public int Task_Id { get; set; }

        [DisplayName("タスク名")]
        public string Task_Name{ get; set; }

        [DisplayName("回数")]
        public int Task_Count { get; set; }

        [DisplayName("単位")]
        public string Unit{ get; set; }

        [DisplayName("達成フラグ")]
        public bool Achivement_Flg{ get; set; }

    }
}
