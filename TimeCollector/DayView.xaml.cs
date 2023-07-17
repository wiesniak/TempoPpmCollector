using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TimeCollector.Models;

namespace TimeCollector
{
    /// <summary>
    /// Interaction logic for DayView.xaml
    /// </summary>
    public partial class DayView : UserControl
    {
        public DayView()
        {
            InitializeComponent();
        }

        public TempoRow[]? Day { get; set; }
        public GroupingRule[] GroupingRules { get; set; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Day is null)
            {
                return;
            }

            Date.SelectedDate = Day.First().WorkDate;

            var groups = GroupByTheRules(Day, GroupingRules);

            StringBuilder reportBuilder = new StringBuilder();
            foreach ( var group in groups)
            {
                StringBuilder groupBuilder = new StringBuilder();
                decimal hours = 0;

                for (int i = 0; i < group.Rows.Count; i++)
                {
                    var groupRow = group.Rows.ElementAt(i);
                    hours += groupRow.Hours;
                    groupBuilder.Append(groupRow.IssueKey);
                    if (i < group.Rows.Count - 1)
                    {
                        groupBuilder.Append(", ");
                    }
                }

                reportBuilder.AppendLine($"Rule: {group.GroupingRule?.Name ?? "--"}\r\nTime: {hours}\r\nComment:\r\n{groupBuilder}");
                reportBuilder.AppendLine();
            }

            Report.Text = reportBuilder.ToString().TrimEnd();
        }

        private IEnumerable<GroupedTempoRows> GroupByTheRules(TempoRow[] items, GroupingRule[] groupingRules) 
        {
            if (groupingRules is null)
            {
                yield return new GroupedTempoRows { GroupingRule = null, Rows = items };
                yield break;
            }

            var workingSet = items.ToList();

            foreach (var groupingRule in groupingRules)
            {
                var property = typeof(TempoRow).GetProperty(groupingRule.Property);

                List<TempoRow> groupRows = new();
                for (int i = workingSet.Count - 1; i >= 0; i--)
                {
                    var value = property.GetValue(workingSet[i]);
                    switch (groupingRule.Type)
                    {
                        case RuleType.Contains:
                            if (((string)value).Contains(groupingRule.Value, StringComparison.OrdinalIgnoreCase))
                            {
                                groupRows.Add(workingSet[i]);
                                workingSet.RemoveAt(i);
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                if (groupRows.Count > 0)
                {
                    yield return new GroupedTempoRows { GroupingRule = groupingRule, Rows = groupRows };
                    
                }

            }

            if (workingSet.Count > 0)
            {
                yield return new GroupedTempoRows { GroupingRule = null, Rows = workingSet };
            }
        }
    }
}
