using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Models
{
    public class SelectList
    {
        [Key]
        [DisplayName("セレクトリストID")]
        public int Id { get; set; }

        [DisplayName("セレクトリスト区分")]
        public string SelectList_Kbn { get; set; }

        [DisplayName("テキスト")]
        public string Text { get; set; }

        [DisplayName("コード")]
        public string Code { get; set; }

        [DisplayName("登録日")]
        public DateTime EntryDate { get; set; }

        [DisplayName("更新日")]
        public DateTime UpdDate { get; set; }

    }
}
