using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCollector.Models
{
    public class GroupingRule
    {
        public string Name { get; set; }
        public RuleType Type { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }
    }
}
