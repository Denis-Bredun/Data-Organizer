using Data_Organizer.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Organizer.Services
{
    public class StopwatchService : IStopwatchService
    {
        private readonly Stopwatch _stopwatch;

        public int Hours { get; private set; }
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }
        public string FormattedHours => ConvertToStopwatchFormat(Hours);
        public string FormattedMinutes => ConvertToStopwatchFormat(Minutes);
        public string FormattedSeconds => ConvertToStopwatchFormat(Seconds);

        public StopwatchService()
        {
            _stopwatch = new Stopwatch();
        }

        public void StartStopwatch()
        {
            if (_stopwatch.IsRunning)
                throw new InvalidOperationException("Секундомер вже працює!");

            _stopwatch.Start();
            Task.Run(async () =>
            {
                while (_stopwatch.IsRunning && Application.Current != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        UpdateValues(_stopwatch.Elapsed.Hours,
                                     _stopwatch.Elapsed.Minutes,
                                     _stopwatch.Elapsed.Seconds);
                    });

                    await Task.Delay(1000);
                }
            });
        }

        public void StopStopwatch()
        {
            if (_stopwatch.IsRunning)
                _stopwatch.Stop();
        }

        public void ResetStopwatch()
        {
            UpdateValues(0, 0, 0);
        }

        public void RestartStopwatch()
        {
            StopStopwatch();
            ResetStopwatch();
            StartStopwatch();
        }

        private void UpdateValues(int hours, int minutes, int seconds)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }

        private string ConvertToStopwatchFormat(int value) => value.ToString("D2");
    }
}
