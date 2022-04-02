using LifeRoutineCheck.Areas.Routine.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Areas.Task.ViewModels
{
    public class ViewTaskUpdate
    {
        [DisplayName("ルーティーンID")]
        public int Routine_Id { get; set; }

        [DisplayName("タスクID")]
        public int Task_Id { get; set; }

        [DisplayName("タスク名")]
        [Required(ErrorMessage = "{0}は必須項目です。")]
        public string Task_Name{ get; set; }

        [DisplayName("回数")]
        [Range(1, 10000, ErrorMessage = "回数は 0～10000 以内で入力してください。")]
        public int Task_Count { get; set; }

        [DisplayName("単位")]
        [Required(ErrorMessage = "{0}は必須項目です。")]
        public string Task_Unit { get; set; }

        [DisplayName("単位")]
        public List<SelectListItem> Task_UnitList { get; set; }

    }
}
