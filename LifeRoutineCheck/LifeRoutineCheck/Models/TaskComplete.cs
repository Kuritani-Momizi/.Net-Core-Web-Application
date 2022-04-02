using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Models
{
    public class TaskComplete
    {
        [Key]
        [DisplayName("タスク完了ID")]
        public int Id { get; set; }

        [DisplayName("タスクID")]
        public int Task_Id { get; set; }

        [DisplayName("タスク完了フラグ")]
        public bool TaskComplete_Flg { get; set; }

        [DisplayName("更新日")]
        public DateTime UpdDate { get; set; }

    }
}
