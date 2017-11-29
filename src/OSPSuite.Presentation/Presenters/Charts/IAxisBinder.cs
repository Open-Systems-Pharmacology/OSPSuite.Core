using System;
using System.Drawing;
using OSPSuite.Core.Chart;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IAxisBinder : IDisposable
   {
      AxisTypes AxisType { get; }
      Axis Axis { get; }

      /// <summary>
      /// When the <see cref="Axis"/> is not hidden, the Visible property can be used to show or hide the axis.
      /// When the <see cref="Axis"/> is hidden, the Visible property is always false;
      /// </summary>
      bool Visible { get; set; }
      object AxisView { get; }
      void RefreshRange(bool sideMarginsEnabled, Size diagramSize );
      void Refresh();
   }
}