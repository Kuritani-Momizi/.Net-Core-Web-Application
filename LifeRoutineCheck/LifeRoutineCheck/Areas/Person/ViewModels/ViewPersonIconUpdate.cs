using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Areas.Person.ViewModels
{
    public class ViewPersonIconUpdate
    {
        [DisplayName("ユーザーId")]
        public int Person_Id { get; set; }

        [DisplayName("アイコン画像")]
        [Required(ErrorMessage = "{0}は必須項目です。")]
        public IFormFile IconImage { get; set; }

        [DisplayName("アイコン画像名")]
        public string IconImage_Nm { get; set; }

        [DisplayName("アイコン画像URL")]
        public string IconImage_URL { get; set; }

    }
}
