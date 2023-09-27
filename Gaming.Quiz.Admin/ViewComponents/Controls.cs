using System;
using Microsoft.AspNetCore.Mvc;

namespace  Gaming.Quiz.Admin.ViewComponents
{
    public class ControlsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(String component, Object model)
        {
            return View($"/Views/Partial/{component}.cshtml", model);
        }
    }
}
