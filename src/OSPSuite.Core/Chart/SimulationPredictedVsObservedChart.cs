using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Chart
{
   public abstract class SimulationAnalysisChart : ChartWithObservedData, ISimulationAnalysis
   {
      public IAnalysable Analysable { get; set; }
   }

   public abstract class SimulationAnalysisChartWithLocalRepositories : SimulationAnalysisChart
   {
      private readonly List<DataRepository> _dataRepositories;
      public virtual IReadOnlyList<DataRepository> DataRepositories => _dataRepositories;

      protected SimulationAnalysisChartWithLocalRepositories()
      {
         _dataRepositories = new List<DataRepository>();
      }

      public virtual void AddRepository(DataRepository dataRepository)
      {
         _dataRepositories.Add(dataRepository);
      }

      public virtual void ClearDataRepositories()
      {
         _dataRepositories.Clear();
      }

      public virtual void AddRepositories(IEnumerable<DataRepository> dataRepositories)
      {
         dataRepositories.Each(AddRepository);
      }
   }
   public class SimulationPredictedVsObservedChart : SimulationAnalysisChart
   {
      public void UpdateAxesVisibility()
      {
         var visibleAxes = Axes.Where(x => x.Dimension != null && x.Dimension.HasSharedUnitNamesWith(XAxis.Dimension)).ToList();
         visibleAxes.Each(axis => axis.Visible = true);
         Axes.Except(visibleAxes).Each(axis => axis.Visible = false);
      }
   }

   public class SimulationResidualVsTimeChart : SimulationAnalysisChartWithLocalRepositories
   {
   }
}