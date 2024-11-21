using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Receptionist.Filters
{
    public class AuthenticateAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Kiểm tra nếu không có session
            if (HttpContext.Current.Session["EmployeeId"] == null)
            {
                // Chuyển hướng đến localhost:44371
                filterContext.Result = new RedirectResult("https://localhost:44371/");
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Để trống nếu không cần thực hiện gì sau khi action được thực thi
        }
    }
}