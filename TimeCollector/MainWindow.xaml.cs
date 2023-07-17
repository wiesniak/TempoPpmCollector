using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

using CsvHelper;

using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

using TimeCollector.Models;

namespace TimeCollector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GroupingRule[] groupingRules;

        public MainWindow(GroupingRule[] groupingRules)
        {
            InitializeComponent();
            this.groupingRules = groupingRules;
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            WeeksView.Children.Clear();

            TempoRow[] rows;
            using (var reader = new StreamReader(SourcePathTextBox.Text))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TempoRowMap>();
                var records = csv.GetRecords<TempoRow>();
                rows = records.ToArray();
            }

            if (!rows.Any())
            {
                return;
            }

            var year = rows.First().WorkDate!.Value.Year;
            var month = rows.First().WorkDate!.Value.Month;

            var weeks = rows.GroupBy(r => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(r.WorkDate!.Value, CalendarWeekRule.FirstDay, DayOfWeek.Monday));

            foreach (var week in weeks)
            {
                WeeksView.Children.Add(new WeekView { GroupingRules = groupingRules, Days = week.ToArray() });
                
            }
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                SourcePathTextBox.Text = openFileDialog.FileName;
        }
    }
}
