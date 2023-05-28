using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Memory.Timers
{
    public class Timer : IDisposable
    {
        private Stopwatch stopwatch = new Stopwatch();
        private List<Timer> childTimers = new List<Timer>();

        public string name { get; }
        public int level { get; }
        public StringWriter writer { get; }

        private Timer(string name, int level = 0, StringWriter writer = null)
        {
            this.name = name;
            this.level = level;
            this.writer = writer;
            stopwatch.Start();
        }

        public static Timer Start(StringWriter writer, string name = "*")
        {
            Timer start = new Timer(name, 0, writer);
            return start;
        }

        public Timer StartChildTimer(string name)
        {
            Timer child = new Timer(name, level + 1);
            childTimers.Add(child);
            return child;
        }

        public void Dispose()
        {
            stopwatch.Stop();
            if (level == 0)
                WriteReport(writer);
        }

        private string FormatReportLine(string timerName, int level, long value)
        {
            string indent = new string(' ', level * 4);
            return $"{indent}{timerName,-20}: {value}\n";
        }

        private void WriteReport(StringWriter stringWriter)
        {
            WriteTimerReport(stringWriter);
            WriteChildTimerReports(stringWriter);
            WriteRestReport(stringWriter);
        }

        private void WriteTimerReport(StringWriter stringWriter)
        {
            string line = FormatReportLine(name, level, stopwatch.ElapsedMilliseconds);
            stringWriter.Write(line);
        }

        private void WriteChildTimerReports(StringWriter stringWriter)
        {
            foreach (Timer child in childTimers)
                child.WriteReport(stringWriter);
        }

        private void WriteRestReport(StringWriter stringWriter)
        {
            if (childTimers.Count == 0)
                return;

            long childTime = childTimers.Sum(c => c.stopwatch.ElapsedMilliseconds);
            long totalTime = stopwatch.ElapsedMilliseconds - childTime;
            string line = FormatReportLine("Rest", level + 1, totalTime);
            stringWriter.Write(line);
        }
    }
}
