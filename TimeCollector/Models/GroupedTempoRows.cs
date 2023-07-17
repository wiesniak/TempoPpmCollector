using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCollector.Models
{
    public class GroupedTempoRows
    {
        public GroupingRule? GroupingRule { get; set; }
        public ICollection<TempoRow> Rows { get; set; }
    }
}
