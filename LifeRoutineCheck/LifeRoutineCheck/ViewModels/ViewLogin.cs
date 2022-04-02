using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.ViewModels
{
    public class ViewLogin
    {
        [Key]
        [DisplayName("ユーザーID")]
        public int Id { get; set; }

        [DisplayName("ユーザー名")]
        [StringLength(50)]
        [Required(ErrorMessage = "ユーザー名は必須入力です。")]
        public string Person_Nm { get; set; }

        [DisplayName("パスワード")]
        [StringLength(20)]
        [Required(ErrorMessage = "パスワードは必須入力です。")]
        public string Password { get; set; }

        [DisplayName("メールアドレス")]
        public string MailAdress { get; set; }

    }
}
