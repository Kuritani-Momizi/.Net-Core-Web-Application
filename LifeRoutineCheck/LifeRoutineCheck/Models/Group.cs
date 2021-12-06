using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Models
{
    public class Group
    {
        [Key]
        [DisplayName("グループID")]
        public int Id { get; set; }

        [DisplayName("グループ名")]
        [StringLength(50)]
        public string Group_Nm { get; set; }

        [DisplayName("作成ユーザーID")]
        public int CreateUserId { get; set; }

        [DisplayName("登録日")]
        public DateTime EntryDate { get; set; }

        [DisplayName("更新日")]
        public DateTime UpdDate { get; set; }

    }
}
