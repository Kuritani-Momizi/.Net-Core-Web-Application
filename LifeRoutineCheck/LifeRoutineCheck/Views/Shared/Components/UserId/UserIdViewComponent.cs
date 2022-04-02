using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Components.UserId
{
    public class UserIdViewComponent : ViewComponent
    {
        //Invoke()
        // ViewComponent読み込み時に実行される
        public IViewComponentResult Invoke()
        {
            return View(HttpContext.User);
        }
    }
}
