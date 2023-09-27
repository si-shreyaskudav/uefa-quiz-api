using System;
using Microsoft.AspNetCore.Mvc;

namespace  Gaming.Quiz.Admin.ViewComponents
{
    public class MessageViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(String component, Object message)
        {
            if (!String.IsNullOrEmpty(component))
                return View($"/Views/Partial/Message/{component}.cshtml", message);
            else
                return Content("");
        }
    }
}
