using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.PKAnalyses;

namespace OSPSuite.Core.Domain.Services
{
   public class PKCalculationOptions
   {
      private readonly List<DosingInterval> _dosingIntervals = new List<DosingInterval>();

      /// <summary>
      ///    Total drug mass per body weight (in µmol/kg BW)
      /// </summary>
      public double? TotalDrugMassPerBodyWeight { get; set; }

      /// <summary>
      ///    Infusion time (in time unit)
      /// </summary>
      public double? InfusionTime { get; set; }

      /// <summary>
      ///    One dosing interval per application, ordered by start time. Applications starting at the same time have the same
      ///    interval
      /// </summary>
      public IReadOnlyList<DosingInterval> DosingIntervals => _dosingIntervals;

      public virtual void AddInterval(DosingInterval dosingInterval) => _dosingIntervals.Add(dosingInterval);

      /// <summary>
      /// Returns the <see cref="DosingInterval"/> at index <paramref name="index"/> or null if the <paramref name="index"/> is out of bound
      /// </summary>
      public DosingInterval DosingIntervalAt(int index)
      {
         if (index < 0 || index >= _dosingIntervals.Count)
            return null;

         return _dosingIntervals[index];
      }

      public bool SingleDosing
      {
         get
         {
            if (!dosingIntervalsWellDefined)
               return true;

            return allDosingIntervalsByStartTime.Count <= 1;
         }
      }

      /// <summary>
      ///    Returns <c>PKParameterMode.Single</c> if the options represents a single dosing application and
      ///    <c>PKParameterMode.Multi</c>
      ///    otherwise
      /// </summary>
      public PKParameterMode PKParameterMode => SingleDosing ? PKParameterMode.Single : PKParameterMode.Multi;

      private bool dosingIntervalsWellDefined => _dosingIntervals.All(x => x.IsValid);

      public DosingInterval FirstInterval => allDosingIntervalsByStartTime.FirstOrDefault();

      public DosingInterval LastMinusOneInterval => allDosingIntervalsByStartTime.ElementAtOrDefault(allDosingIntervalsByStartTime.Count - 2);

      public DosingInterval LastInterval => allDosingIntervalsByStartTime.LastOrDefault();

      private IReadOnlyList<DosingInterval> allDosingIntervalsByStartTime => _dosingIntervals.GroupBy(x => x.StartValue).Select(mergedDosingInterval).ToList();

      private DosingInterval mergedDosingInterval(IGrouping<float?, DosingInterval> dosingIntervals)
      {
         if (dosingIntervals.Count() == 1)
            return dosingIntervals.First();

         var allDrugMassesPerBodyWeight = dosingIntervals.Select(x => x.DrugMassPerBodyWeight).ToList();
         return new DosingInterval
         {
            StartValue = dosingIntervals.Key,
            EndValue = dosingIntervals.First().EndValue,
            DrugMassPerBodyWeight = allDrugMassesPerBodyWeight.Contains(null) ? null : allDrugMassesPerBodyWeight.Sum()
         };
      }
   }
}