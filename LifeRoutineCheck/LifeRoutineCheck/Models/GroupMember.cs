using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Models
{
    public class GroupMember
    {
        [Key]
        [DisplayName("グループメンバーID")]
        public int Id { get; set; }

        [DisplayName("グループID")]
        public int GroupId { get; set; }

        [DisplayName("ユーザーID")]
        public int PersonId { get; set; }

        [DisplayName("承認フラグ")]
        public bool AcceptFlg { get; set; }

        [DisplayName("登録日")]
        public DateTime EntryDate { get; set; }

        [DisplayName("更新日")]
        public DateTime UpdDate { get; set; }

    }
}
