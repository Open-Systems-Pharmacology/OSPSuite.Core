using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class OutputSchema : Container
   {
      private readonly HashSet<double> _timePoints = new HashSet<double>();

      public OutputSchema()
      {
         Name = Constants.OUTPUT_SCHEMA;
      }

      public virtual IEnumerable<OutputInterval> Intervals => GetChildren<OutputInterval>();

      public virtual void AddInterval(OutputInterval outputInterval) => Add(outputInterval);

      public virtual void RemoveInterval(OutputInterval outputInterval) => RemoveChild(outputInterval);

      /// <summary>
      ///    Adds discrete time points to the output schema. Duplicate values will be ignored. This is for internal use only.
      /// </summary>
      public virtual void AddTimePoints(IReadOnlyList<double> timePoints) => timePoints.Each(AddTimePoint);

      /// <summary>
      ///    Adds a single time point to the output schema. Duplicate value will be ignored. This is for internal use only.
      /// </summary>
      public virtual void AddTimePoint(double timePoint) => _timePoints.Add(timePoint);

      /// <summary>
      /// Returns the sorted time points without duplicate
      /// </summary>
      public IReadOnlyList<double> TimePoints => _timePoints.OrderBy(x => x).ToList();
   }
}