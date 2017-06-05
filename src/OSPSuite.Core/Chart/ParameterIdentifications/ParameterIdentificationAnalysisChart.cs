using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

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

      //     TODO REVIEW ALL
      public ParameterIdentificationPredictedVsObservedChart()
      {
         ChartSettings.LegendPosition = LegendPositions.BottomInside;

         //TODO 
//         Axes.CollectionChanged += (o, e) => addEventHandlerForXAxis(e);
      }

//      private void addEventHandlerForXAxis(NotifyCollectionChangedEventArgs e)
//      {
//         if (e.NewItems != null && itemsContainsXAxis(e.NewItems))
//         {
//            Axes[AxisTypes.X].PropertyChanged += (o, args) => UpdateAxesVisibility();
//         }
//      }
//
//      private static bool itemsContainsXAxis(IList axes)
//      {
//         return axes.Cast<Axis>().Any(x => x.AxisType == AxisTypes.X);
//      }
//
     public void UpdateAxesVisibility()
      {
         var visibleAxes = Axes.Where(x => x.Dimension != null && x.Dimension.HasSharedUnitNamesWith(AxisBy(AxisTypes.X).Dimension)).ToList();

         visibleAxes.Each(axis => axis.Visible = true);
         Axes.Except(visibleAxes).Each(axis => axis.Visible = false);
      }
   }
}