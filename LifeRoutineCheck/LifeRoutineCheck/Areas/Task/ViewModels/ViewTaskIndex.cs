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
    public class ViewTaskIndex
    {
        [DisplayName("ユーザーID")]
        public int Person_Id { get; set; }

        [DisplayName("ルーティーンID")]
        public int Routine_Id { get; set; }

        [DisplayName("タスクリスト")]
        public List<ViewTaskAchievement> TaskList { get; set; }

        [DisplayName("タスク更新用リスト")]
        public List<ViewTaskUpdate> TaskUpdateList { get; set; }
    }
}
