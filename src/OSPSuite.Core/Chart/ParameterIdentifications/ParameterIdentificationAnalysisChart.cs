using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Chart.ParameterIdentifications
{
   public abstract class ParameterIdentificationAnalysisChart : ChartWithObservedData, ISimulationAnalysis
   {
      public IAnalysable Analysable { get; set; }
   }

   public abstract class ParameterIdentificationAnalysisChartWithLocalRepositories : ParameterIdentificationAnalysisChart
   {
      private readonly List<DataRepository> _dataRepositories;
      public virtual IReadOnlyList<DataRepository> DataRepositories => _dataRepositories;

      protected ParameterIdentificationAnalysisChartWithLocalRepositories()
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

   public class ParameterIdentificationTimeProfileChart : ParameterIdentificationAnalysisChart
   {
   }

   public class ParameterIdentificationTimeProfileConfidenceIntervalChart : ParameterIdentificationAnalysisChartWithLocalRepositories
   {
   }

   public class ParameterIdentificationTimeProfilePredictionIntervalChart : ParameterIdentificationAnalysisChartWithLocalRepositories
   {
   }

   public class ParameterIdentificationTimeProfileVPCIntervalChart : ParameterIdentificationAnalysisChartWithLocalRepositories
   {
   }

   public class ParameterIdentificationResidualVsTimeChart : ParameterIdentificationAnalysisChartWithLocalRepositories
   {
   }

   public class ParameterIdentificationPredictedVsObservedChart : ParameterIdentificationAnalysisChart
   {
      public ParameterIdentificationPredictedVsObservedChart()
      {
         ChartSettings.LegendPosition = LegendPositions.BottomInside;
      }

      /// <summary>
      ///    Modifies the Y axes visibility based on X axis dimension. Y axes which do not share a unit with X axis are not shown
      /// </summary>
      public void UpdateAxesVisibility()
      {
         var visibleAxes = Axes.Where(x => x.Dimension != null && x.Dimension.HasSharedUnitNamesWith(XAxis.Dimension)).ToList();
         visibleAxes.Each(axis => axis.Visible = true);
         Axes.Except(visibleAxes).Each(axis => axis.Visible = false);
      }
   }
}