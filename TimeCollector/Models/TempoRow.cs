using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCollector.Models
{
    public class TempoRow
    {
        public string? IssueKey { get; set; }
        public string? IssueSummary { get; set; }
        public DateTime? WorkDate { get; set; }
        public decimal Hours { get; set; } = 0;

    }
}
