using RMG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository.IRepository
{
    public interface IBreadcrumbService
    {
        List<Breadcrumb> GetBreadcrumbs();
    }
}
