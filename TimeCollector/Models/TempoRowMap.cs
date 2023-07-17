using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CsvHelper.Configuration;

namespace TimeCollector.Models
{
    internal class TempoRowMap : ClassMap<TempoRow>
    {
        public TempoRowMap()
        {
            Map(x => x.IssueKey).Name("Issue Key");
            Map(x => x.IssueSummary).Name("Issue summary");
            Map(x => x.WorkDate).Name("Work date");
            Map(x => x.Hours).Name("Hours");
        }
    }
}
