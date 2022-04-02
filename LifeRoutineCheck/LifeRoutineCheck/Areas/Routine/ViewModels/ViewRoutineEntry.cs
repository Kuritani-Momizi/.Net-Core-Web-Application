using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Areas.Routine.ViewModels
{
    public class ViewRoutineEntry
    {
        [DisplayName("ユーザーID")]
        public int Person_Id { get; set; }

        [DisplayName("ルーティーンID")]
        public int Routine_Id { get; set; }

        [DisplayName("グループID")]
        public int Group_Id { get; set; }

        [DisplayName("ルーティーン名")]
        [Required(ErrorMessage = "{0}は必須項目です。")]
        [StringLength(20, ErrorMessage = "{0}は20文字以内で入力してください。")]

        public string Routtine_Nm { get; set; }

        [DisplayName("ルーティーン区分")]
        public string Routtine_Kbn { get; set; }

        [DisplayName("アイコン画像")]
        [Required(ErrorMessage = "{0}は必須項目です。")]
        public IFormFile IconImage { get; set; }

        [DisplayName("アイコン画像名")]
        public string IconImage_Nm { get; set; }

        [DisplayName("アイコン画像URL")]
        public string IconImage_URL { get; set; }

        [DisplayName("並び順")]
        public int SortOrder { get; set; }

    }
}
