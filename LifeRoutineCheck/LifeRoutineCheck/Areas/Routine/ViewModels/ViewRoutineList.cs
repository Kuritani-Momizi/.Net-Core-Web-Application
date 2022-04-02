using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Areas.Routine.ViewModels
{
    public class ViewRoutineList
    {
        [DisplayName("ルーティーンID")]
        public int Routtine_Id { get; set; }

        [DisplayName("グループID")]
        public int Group_Id { get; set; }

        [DisplayName("ルーティーン名")]
        public string Routtine_Nm { get; set; }

        [DisplayName("グループ名")]
        public string Group_Nm { get; set; }

        [DisplayName("アイコン画像名")]
        public string IconImage_Nm { get; set; }

        [DisplayName("アイコン画像URL")]
        public string IconImage_URL { get; set; }

        [DisplayName("並び順")]
        public int SortOrder { get; set; }

    }
}
