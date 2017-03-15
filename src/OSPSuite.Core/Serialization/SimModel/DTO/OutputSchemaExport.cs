using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.SimModel.DTO
{
   public class OutputSchemaExport
   {
      public OutputSchemaExport()
      {
         OutputIntervals = new List<OutputIntervalExport>();
         OutputTimes = new List<double>();
      }

      public IList<OutputIntervalExport> OutputIntervals { get; set; }
      public IList<double> OutputTimes { get; set; }
   }

   public class OutputIntervalExport
   {
      public double StartTime { get; set; }
      public double EndTime { get; set; }
      public double Resolution { get; set; }

      public int NumberOfTimePoints
      {
         get
         {
            if (StartTime == EndTime)
               return Constants.MIN_NUMBER_OF_POINTS_PER_INTERVAL; //just a single point

            return Math.Min(
               Math.Max(Constants.MIN_NUMBER_OF_POINTS_PER_INTERVAL, (int) Math.Round(Resolution * (EndTime - StartTime)) + 1),
               Constants.MAX_NUMBER_OF_POINTS_PER_INTERVAL);
         }
      }
   }
}