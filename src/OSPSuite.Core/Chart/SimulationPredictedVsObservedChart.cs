using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Chart
{
   public class SimulationPredictedVsObservedChart : ChartWithObservedData, ISimulationAnalysis
   { 
      public IAnalysable Analysable { get; set; }

      public void UpdateAxesVisibility()
      {
         var visibleAxes = Axes.Where(x => x.Dimension != null && x.Dimension.HasSharedUnitNamesWith(XAxis.Dimension)).ToList();
         visibleAxes.Each(axis => axis.Visible = true);
         Axes.Except(visibleAxes).Each(axis => axis.Visible = false);
      }
   }
}