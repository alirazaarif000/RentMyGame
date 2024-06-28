using Microsoft.AspNetCore.Mvc;
using RMG.BLL;
using RMG.Models;
using System.Security.Claims;

namespace RMG.Web.Areas.Admin.ViewComponents
{
    public class NotificationViewComponent:ViewComponent
    {
        private readonly NotificationBll _notificationBll; 
        public NotificationViewComponent(NotificationBll notificationBll)
        {
            _notificationBll = notificationBll;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Notification> notifications = _notificationBll.GetAllNotification().Data;
            return View(notifications);
        }
    }
}
