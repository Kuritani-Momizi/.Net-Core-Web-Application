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
    public class ViewPersonMailAddressUpdate
    {
        [DisplayName("ユーザーId")]
        public int Person_Id { get; set; }

        [DisplayName("メールアドレス")]
        [EmailAddress]
        [StringLength(256, ErrorMessage = "{0}は256文字以内で入力してください。")]
        public string MailAdress { get; set; }

    }
}
