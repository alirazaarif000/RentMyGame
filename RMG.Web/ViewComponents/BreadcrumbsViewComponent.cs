using Microsoft.AspNetCore.Mvc;
using RMG.DAL.Repository.IRepository;

namespace RMG.Web.ViewComponents
{
    public class BreadcrumbsViewComponent: ViewComponent
    {
        private readonly IBreadcrumbService _breadcrumbService;

        public BreadcrumbsViewComponent(IBreadcrumbService breadcrumbService)
        {
            _breadcrumbService = breadcrumbService;
        }

        public IViewComponentResult Invoke()
        {
            var breadcrumbs = _breadcrumbService.GetBreadcrumbs();
            return View(breadcrumbs);
        }
    }
}
