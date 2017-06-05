using System.Collections.Generic;

namespace OSPSuite.Core.Chart
{
   public interface IWithAxes
   {
      IReadOnlyCollection<Axis> Axes { get; }
      void AddAxis(Axis axis);
      void RemoveAxis(Axis axis);
      Axis AddNewAxis();
      Axis AxisBy(AxisTypes axisTypes);
   }
}