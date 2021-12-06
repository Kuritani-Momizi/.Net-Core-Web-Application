using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Models
{
    public class Routine
    {
        [Key]
        [DisplayName("ルーティーンID")]
        public int Id { get; set; }

        [DisplayName("ユーザーID")]
        public int Person_Id { get; set; }

        [DisplayName("グループID")]
        public int Group_Id { get; set; }

        [DisplayName("ルーティーン名")]
        [StringLength(50)]
        public string Routine_Nm { get; set; }

        [DisplayName("ルーティーン区分")]
        public string RoutineKbn { get; set; }

        [DisplayName("ルーティーンアイコン画像")]
        public string RoutineIconImg { get; set; }

        [DisplayName("更新ユーザーID")]
        public int UpdPerson_Id { get; set; }

        [DisplayName("登録日")]
        public DateTime EntryDate { get; set; }

        [DisplayName("更新日")]
        public DateTime UpdDate { get; set; }

    }
}
