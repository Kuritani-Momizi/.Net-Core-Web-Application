using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Models
{
    public class Task
    {
        [Key]
        [DisplayName("タスクID")]
        public int Id { get; set; }

        [DisplayName("ルーティーンID")]
        public int RoutineId { get; set; }

        [DisplayName("タスク名")]
        public string Task_Nm { get; set; }

        [DisplayName("回数")]
        public int Task_Count { get; set; }

        [DisplayName("単位")]
        public string Task_Unit { get; set; }

        [DisplayName("登録日")]
        public DateTime EntryDate { get; set; }

        [DisplayName("更新日")]
        public DateTime UpdDate { get; set; }

    }
}
