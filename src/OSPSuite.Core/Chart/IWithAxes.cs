using System.Collections.Generic;

namespace OSPSuite.Core.Chart
{
   public interface IWithAxes
   {
      IReadOnlyCollection<Axis> Axes { get; }
      void AddAxis(Axis axis);
      void RemoveAxis(Axis axis);
      Axis AddNewAxisFor(AxisTypes axisType);
      Axis AxisBy(AxisTypes axisTypes);
      bool HasAxis(AxisTypes axisTypes);
   }
}