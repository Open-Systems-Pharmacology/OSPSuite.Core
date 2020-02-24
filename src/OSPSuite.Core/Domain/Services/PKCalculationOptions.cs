using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.PKAnalyses;

namespace OSPSuite.Core.Domain.Services
{
   public class PKCalculationOptions
   {
      private readonly List<DosingInterval> _dosingIntervals = new List<DosingInterval>();

      /// <summary>
      ///    Total dose (in µmol/kg BW)
      /// </summary>
      public double? Dose { get; set; }

      /// <summary>
      ///    Infusion time (in time unit)
      /// </summary>
      public double? InfusionTime { get; set; }

      public IReadOnlyList<DosingInterval> DosingIntervals => _dosingIntervals;

      public virtual void AddInterval(DosingInterval dosingInterval) => _dosingIntervals.Add(dosingInterval);

      public bool SingleDosing
      {
         get
         {
            if (!dosingIntervalsWellDefined)
               return true;

            return _dosingIntervals.Count <= 1;
         }
      }

      /// <summary>
      ///    Returns <c>PKParameterMode.Single</c> if the options represents a single dosing application and
      ///    <c>PKParameterMode.Multi</c>
      ///    otherwise
      /// </summary>
      public PKParameterMode PKParameterMode => SingleDosing ? PKParameterMode.Single : PKParameterMode.Multi;

      private bool dosingIntervalsWellDefined => _dosingIntervals.All(x => x.IsValid);

      public float? FirstDosingStartValue => startValueAt(0);

      public float? FirstDosingEndValue => endValueAt(0);

      public double? FirstDose => doseAt(0);

      public float? LastMinusOneDosingStartValue => startValueAt(_dosingIntervals.Count - 2);

      public double? LastMinusOneDose => doseAt(0);

      public float? LastDosingStartValue => startValueAt(_dosingIntervals.Count - 1);

      public float? LastDosingEndValue => endValueAt(_dosingIntervals.Count - 1);

      public double? LastDose => doseAt(_dosingIntervals.Count - 1);

      private float? startValueAt(int index) => intervalAt(index)?.StartValue;

      private float? endValueAt(int index) => intervalAt(index)?.EndValue;

      private double? doseAt(int index) => intervalAt(index)?.Dose;

      private DosingInterval intervalAt(int index)
      {
         if (index < 0 || index >= _dosingIntervals.Count)
            return null;

         return _dosingIntervals[index];
      }
   }
}