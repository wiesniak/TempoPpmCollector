using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for WeekView.xaml
    /// </summary>
    public partial class WeekView : UserControl
    {
        public WeekView()
        {
            InitializeComponent();
        }
        public TempoRow[]? Days { get; set; }
        public GroupingRule[] GroupingRules { get; set; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Days is null || Days.Length == 0)
            {
                return;
            }

            var firstDay = Days.First();
            var firstDayDate = firstDay.WorkDate.Value.Date;
            var year = firstDay.WorkDate.Value.Year;
            var weekNumber = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(firstDay.WorkDate.Value, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            var firstDayOfWeek = firstDayDate.DayOfWeek == DayOfWeek.Monday
                ? firstDayDate
                : firstDayDate.AddDays(-1 * (((int)firstDayDate.DayOfWeek - 1 + 7) % 7));
            var lastDayOfWeek = firstDayOfWeek.AddDays(6);


            Description.Text = $"Week {weekNumber}/{year} - {firstDayOfWeek:d} - {lastDayOfWeek:d}";

            foreach (var day in Days.GroupBy(d => d.WorkDate))
            {
                var view = GetDayView(day.Key!.Value);
                view.GroupingRules = GroupingRules;
                view.Day = day.ToArray();
            }
        }

        private DayView GetDayView(DateTime date)
        {
            var enumerator = DayContainer.Children.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is DayView dayView && dayView.Name.Equals(date.DayOfWeek.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return dayView;
                }
            }
            return null;
        }
    }
}
