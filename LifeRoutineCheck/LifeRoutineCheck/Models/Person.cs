using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Models
{
    public class Person
    {
        [Key]
        [DisplayName("ユーザーID")]
        public int Id { get; set; }

        [DisplayName("ユーザー名")]
        [StringLength(50)]
        public string Person_Nm { get; set; }

        [DisplayName("パスワード")]
        [StringLength(20)]
        public string Password { get; set; }

        [DisplayName("メールアドレス")]
        [StringLength(256)]
        public string MailAdress { get; set; }

        [DisplayName("権限区分")]
        [StringLength(20)]
        public string AuthorityKbn { get; set; }

        [DisplayName("登録日")]
        public DateTime EntryDate { get; set; }

        [DisplayName("更新日")]
        public DateTime UpdDate { get; set; }

    }
}
