using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Chart.Mappers
{
   public struct TicksConfig
   {
      public int MinorTicks;
      public float GridSpacing;
      public bool AutoScale;
   }

   public class UnitToMinorIntervalMapper
   {
      private const float OPTIMAL_PIXEL_PER_INTERVAL = 30;
      private readonly int[] _hoursOrMinutesIntervals = {2, 4, 6, 12};
      private readonly int[] _dayIntervals = { 2, 6, 12, 24 };
      private readonly int[] _weekIntervals = { 7 };
      private readonly int[] _yearIntervals = { 12 };

      private IEnumerable<int> getPossibleIntervals(Unit unit)
      {
         if (unit == null)
            return new List<int> { 1 };

         if (unit.IsMinutes() || unit.IsHours())
            return _hoursOrMinutesIntervals;

         if (unit.IsDays())
            return _dayIntervals;

         if (unit.IsWeeks())
            return _weekIntervals;

         if (unit.IsYears())
            return _yearIntervals;

         return new List<int> {1};
      }

      public TicksConfig MapFrom(Unit unit, float axisWidthInUnit, int axisWidthInPixel)
      {
         // calculate rating for one interval only (no minor tickmarks)
         var optimalNumberOfIntervals = 1;
         var runningOptimalRating = calculateRating(axisWidthInUnit, axisWidthInPixel, optimalNumberOfIntervals);

         foreach (var possibleInterval in getPossibleIntervals(unit))
         {
            var rating = calculateRating(axisWidthInUnit, axisWidthInPixel, possibleInterval);

            if (rating < runningOptimalRating)
            {
               optimalNumberOfIntervals = possibleInterval;
               runningOptimalRating = rating;
            }
         }

         TicksConfig ticksConfig;
         switch (optimalNumberOfIntervals)
         {
            case 24:
               ticksConfig = new TicksConfig{MinorTicks = 5, GridSpacing = 0.25F};
               break;
            case 12:
               ticksConfig =  new TicksConfig{MinorTicks = 2, GridSpacing = 0.25F};
               break;
            case 6:
               ticksConfig =  new TicksConfig{MinorTicks = 2, GridSpacing = 0.5F};
                break;
            case 4:
               ticksConfig =  new TicksConfig{MinorTicks = 1, GridSpacing = 0.5F};
               break;
            default:
               ticksConfig =  new TicksConfig{MinorTicks = optimalNumberOfIntervals-1, GridSpacing = 1F};
               break;
         }

         if (ticksConfig.MinorTicks < 1 || axisWidthInUnit / ticksConfig.GridSpacing < 1.5) //1.5 is a heuristic found good value to balance between appropriate scale ticks and enough labels
            ticksConfig.AutoScale = true; //switch to auto scaling, because not enough labels are displayed
         else
            ticksConfig.AutoScale = false;

         return ticksConfig;

      }

      private double calculateRating(float axisWidthInUnit, int axisWidthInPixel, int possibleInterval)
      {
         var pixelsPerInterval = axisWidthInPixel / axisWidthInUnit / possibleInterval;
         var rating = ratingOfPixelsPerInterval(pixelsPerInterval);
         return rating;
      }

      private static bool ratingIsBetterThanRunningOptimal(double rating, double optimalRating)
      {
         return rating < optimalRating;
      }

      private double ratingOfPixelsPerInterval(float pixelsPerInterval)
      {
         // return best = lowest value for pixelsPerInterval == OPTIMAL_PIXEL_PER_INTERVAL and higher values for smaller and larger values
         return Math.Abs(Math.Log10(pixelsPerInterval / OPTIMAL_PIXEL_PER_INTERVAL));
      }

      public bool HasPreferredMinorIntervalsFor(Unit unit)
      {
         return unit.IsWeeks() || unit.IsHours() || unit.IsDays() || unit.IsYears() || unit.IsMinutes();
      }
   }
}