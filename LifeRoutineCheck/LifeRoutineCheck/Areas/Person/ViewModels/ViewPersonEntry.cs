using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Areas.Person.ViewModels
{
    public class ViewPersonEntry
    {
        [Required(ErrorMessage = "{0}は必須項目です。")]
        [DisplayName("ユーザー名")]
        [StringLength(50, ErrorMessage = "{0}は50文字以内で入力してください。")]
        public string Person_Nm { get; set; }

        [Required(ErrorMessage = "{0}は必須項目です。")]
        [DisplayName("パスワード")]
        [StringLength(20, ErrorMessage = "{0}は20文字以内で入力してください。")]
        public string Password { get; set; }

        [Remote("VerifyEmail", "Person")]
        [Required(ErrorMessage = "{0}は必須項目です。")]
        [DisplayName("メールアドレス")]
        [EmailAddress]
        [StringLength(256, ErrorMessage = "{0}は256文字以内で入力してください。")]
        public string MailAdress { get; set; }

    }
}
