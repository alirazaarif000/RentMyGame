using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.Models
{
    public class Result<T>
    {
        public string? Message { get; set; }
        public T? Data { get; set; }
        public bool Status { get; set; }
        public short StatusCode { get; set; }
        public int PageNumber { get; set; }
        public long TotalRecords { get; set; }
        public string? MessageDetails { get; set; }
    }
}
