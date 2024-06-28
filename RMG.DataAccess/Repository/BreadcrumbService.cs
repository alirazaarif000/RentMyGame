using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RMG.DAL.Repository.IRepository;
using RMG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository
{
    public class BreadcrumbService : IBreadcrumbService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BreadcrumbService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Breadcrumb> GetBreadcrumbs()
        {
            var breadcrumbs = new List<Breadcrumb>();
            var context = _httpContextAccessor.HttpContext;
            var routeData = context.GetRouteData();
            var segments = context.Request.Path.Value.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            breadcrumbs.Add(new Breadcrumb("Home", "/Admin/Home"));
            var url = "/";
            foreach (var segment in segments)
            {
                if (!string.Equals(segment, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    url += segment + "/";
                    breadcrumbs.Add(new Breadcrumb(segment, url));
                }
                else
                {
                    // Keep "Admin" in the URL but do not add to breadcrumbs
                    url += segment + "/";
                }
            }

            return breadcrumbs;
        }
    }
}
