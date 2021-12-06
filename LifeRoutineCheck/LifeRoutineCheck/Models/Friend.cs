using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Models
{
    public class Friend
    {
        [Key]
        [DisplayName("フレンドID")]
        public int Id { get; set; }

        [DisplayName("申請者ユーザーID")]
        public int ApplicationPersonId { get; set; }

        [DisplayName("承認ユーザーID")]
        public int ApprovalPersonId { get; set; }

        [DisplayName("承認フラグ")]
        public bool ApprovalFlg { get; set; }

        [DisplayName("登録日")]
        public DateTime EntryDate { get; set; }

        [DisplayName("更新日")]
        public DateTime UpdDate { get; set; }

    }
}
