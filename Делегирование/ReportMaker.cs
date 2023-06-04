using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Reports
{
    
    public interface IReportFormatter
    {
       
        string MakeCaption(string caption);

        string BeginList();

        string MakeItem(string valueType, string entry);

        string EndList();
    }

    public interface IStatisticsCalculator
    {
        object MakeStatistics(IEnumerable<double> data);
    }

    public class HtmlReportFormatter : IReportFormatter
    {
        public string MakeCaption(string caption)
        {
            return $"<h1>{caption}</h1>";
        }

        public string BeginList()
        {
            return "<ul>";
        }

        public string MakeItem(string valueType, string entry)
        {
            return $"<li><b>{valueType}</b>: {entry}";
        }

        public string EndList()
        {
            return "</ul>";
        }
    }

    public class MarkdownReportFormatter : IReportFormatter
    {
        public string MakeCaption(string caption)
        {
            return $"## {caption}\n\n";
        }

        public string BeginList()
        {
            return "";
        }

        public string MakeItem(string valueType, string entry)
        {
            return $" * **{valueType}**: {entry}\n\n";
        }

        public string EndList()
        {
            return "";
        }
    }

    public class MeanAndStdCalculator : IStatisticsCalculator
    {
        public object MakeStatistics(IEnumerable<double> _data)
        {
            var data = _data.ToList();

            var mean = data.Average();

            var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));

            return new MeanAndStd
            {
                Mean = mean,
                Std = std
            };
        }
    }

    public class MedianCalculator : IStatisticsCalculator
    {
        public object MakeStatistics(IEnumerable<double> data)
        {
            var list = data.OrderBy(z => z).ToList();

            if (list.Count % 2 == 0)
                return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

            return list[list.Count / 2];
        }
    }

    public class ReportMaker
    {
        private readonly IReportFormatter _formatter;
        private readonly IStatisticsCalculator _calculator;

        public ReportMaker(IReportFormatter formatter, IStatisticsCalculator calculator)
        {
            _formatter = formatter;
            _calculator = calculator;
        }

        public string MakeReport(IEnumerable<Measurement> measurements)
        {
            var data = measurements.ToList();
            var result = new StringBuilder();
            result.Append(_formatter.MakeCaption(Caption));
            result.Append(_formatter.BeginList());
            result.Append(_formatter.MakeItem("Temperature", _calculator.MakeStatistics(data.Select(z => z.Temperature)).ToString()));
            result.Append(_formatter.MakeItem("Humidity", _calculator.MakeStatistics(data.Select(z => z.Humidity)).ToString()));
            result.Append(_formatter.EndList());
            return result.ToString();
        }
        public string Caption { get; set; }
    }

    public static class ReportMakerHelper
    {
        public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
        {
            var htmlReportMaker = new ReportMaker(new HtmlReportFormatter(), new MeanAndStdCalculator());
            htmlReportMaker.Caption = "Mean and Std";
            return htmlReportMaker.MakeReport(data);
        }

        public static string MedianMarkdownReport(IEnumerable<Measurement> data)
        {
            var markdownReportMaker = new ReportMaker(new MarkdownReportFormatter(), new MedianCalculator());
            markdownReportMaker.Caption = "Median";
            return markdownReportMaker.MakeReport(data);
        }

        public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> data)
        {
            var markdownReportMaker = new ReportMaker(new MarkdownReportFormatter(), new MeanAndStdCalculator());
            markdownReportMaker.Caption = "Mean and Std";
            return markdownReportMaker.MakeReport(data);
        }

        public static string MedianHtmlReport(IEnumerable<Measurement> data)
        {
            var htmlReportMaker = new ReportMaker(new HtmlReportFormatter(), new MedianCalculator());
            htmlReportMaker.Caption = "Median";
            return htmlReportMaker.MakeReport(data);
        }
    }
}