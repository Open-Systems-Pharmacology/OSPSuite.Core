using System;
using OSPSuite.Utility.Events;

namespace OSPSuite.CLI.Core.MinimalImplementations
{
    public class ProgressUpdater : Utility.Events.ProgressUpdater
    {
        public ProgressUpdater(IEventPublisher eventPublisher) : base(eventPublisher)
        {
        }

        public override void ReportProgress(int iteration, int numberOfIterations, string message)
        {
            var currentPercent = PercentFrom(_currentIteration, numberOfIterations);
            var nextPercent = PercentFrom(iteration, numberOfIterations);
            _currentIteration = iteration;

            if (currentPercent == nextPercent)
                return;

            var inProgress = iteration != numberOfIterations;
            if (inProgress)
                Console.Write(value: $"{currentPercent}..");
            else
                Console.WriteLine(value: $"{currentPercent}");
        }

        public override void ReportStatusMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}