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
    public class ViewPersonUpdate
    {
        [DisplayName("ユーザーId")]
        public int Person_Id { get; set; }

        [DisplayName("ユーザー名")]
        [StringLength(50, ErrorMessage = "{0}は50文字以内で入力してください。")]
        public string Person_Nm { get; set; }

        [DisplayName("パスワード")]
        [StringLength(20, ErrorMessage = "{0}は20文字以内で入力してください。")]
        public string Password { get; set; }

        [DisplayName("メールアドレス")]
        [EmailAddress]
        [StringLength(256, ErrorMessage = "{0}は256文字以内で入力してください。")]
        public string MailAdress { get; set; }

        [DisplayName("アイコン画像")]
        public IFormFile IconImage { get; set; }

        [DisplayName("アイコン画像名")]
        public string IconImage_Nm { get; set; }

        [DisplayName("アイコン画像URL")]
        public string IconImage_URL { get; set; }


    }
}
