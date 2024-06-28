using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.Models.ViewModels
{
    public class Breadcrumb
    {
        public string Title { get; set; }
        public string Url { get; set; }

        public Breadcrumb(string title, string url)
        {
            Title = title;
            Url = url;
        }
    }
}
