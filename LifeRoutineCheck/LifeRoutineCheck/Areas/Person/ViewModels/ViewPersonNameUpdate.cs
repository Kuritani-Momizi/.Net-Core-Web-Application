using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Areas.Person.ViewModels
{
    public class ViewPersonNameUpdate
    {
        [DisplayName("ユーザーId")]
        public int Person_Id { get; set; }

        [Required(ErrorMessage = "{0}は必須入力です。")]
        [DisplayName("ユーザー名")]
        [StringLength(50, ErrorMessage = "{0}は50文字以内で入力してください。")]
        public string Person_Nm { get; set; }

    }
}
