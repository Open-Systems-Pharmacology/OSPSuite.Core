using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using static OSPSuite.Assets.Captions.ParameterIdentification;

namespace OSPSuite.Core.Services
{
   public interface IResidualsVsTimeChartService
   {
      /// <summary>
      ///    This will be the name of the curve added to the chart representing the Zero Curve
      /// </summary>
      string Zero { get; }

      /// <summary>
      ///    Adds the zero marker curve to the <paramref name="chart" /> where the residuals equal zero. The zero line
      ///    will begin at <paramref name="minObservedDataTime" /> and end at <paramref name="maxObservedDataTime" />
      /// </summary>
      DataRepository AddZeroMarkerCurveToChart(AnalysisChartWithLocalRepositories chart, float minObservedDataTime, float maxObservedDataTime);

      /// <summary>
      ///    Creates the DataRepository for the given residuals <paramref name="outputResidual" /> with given
      ///    <paramref name="id" /> and <paramref name="repositoryName" />
      /// </summary>
      DataRepository CreateScatterDataRepository(string id, string repositoryName, OutputResiduals outputResidual);

      void ConfigureChartAxis(AnalysisChartWithLocalRepositories chart);

      /// <summary>
      ///    Get or creates the DataRepository for the given residuals <paramref name="outputResidual" /> for the given chart.
      ///    If it already exists, update the values with the output residuals values (update of chart)
      /// </summary>
      DataRepository GetOrCreateScatterDataRepositoryInChart(AnalysisChartWithLocalRepositories chart, OutputResiduals outputResidual, int? runIndex = null);
   }

   public class ResidualsVsTimeChartService : IResidualsVsTimeChartService
   {
      private readonly IDimensionFactory _dimensionFactory;

      public ResidualsVsTimeChartService(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public string Zero => ResidualsVsTimeChart.ZERO;

      public DataRepository AddZeroMarkerCurveToChart(AnalysisChartWithLocalRepositories chart, float minObservedDataTime, float maxObservedDataTime)
      {
         var markerRepository = createMarkerRepository(minObservedDataTime, maxObservedDataTime, chart);
         AddCurvesFor(markerRepository, (column, curve) => { curve.UpdateMarkerCurve(Zero); }, chart);

         return markerRepository;
      }

      private DataRepository createMarkerRepository(float minObservedDataTime, float maxObservedDataTime, AnalysisChartWithLocalRepositories chart)
      {
         var id = $"{chart.Id}-{Zero}";
         var dataRepository = createEmptyRepository(id, Zero, Zero);
         
         var times = new List<float>{minObservedDataTime};
         var values = new List<float> { 0f};
         //min and max can be equal if the observed data is a single point
         if (!ValueComparer.AreValuesEqual(minObservedDataTime, maxObservedDataTime))
         {
            times.Add(maxObservedDataTime);
            values.Add(0f);
         }

         dataRepository.BaseGrid.Values = times;
         dataRepository.FirstDataColumn().Values = values;
         return dataRepository;
      }

      private DataRepository createEmptyRepository(string id, string name, string valueName)
      {
         var dataRepository = new DataRepository(id) {Name = name};
         var baseGrid = new BaseGrid($"{id}-Time", "Time", _dimensionFactory.Dimension(Constants.Dimension.TIME));
         var values = new DataColumn($"{id}-{valueName}", valueName, _dimensionFactory.NoDimension, baseGrid)
            {DataInfo = {Origin = ColumnOrigins.CalculationAuxiliary}};
         dataRepository.Add(values);
         return dataRepository;
      }

      public void AddCurvesFor(DataRepository dataRepository, Action<DataColumn, Curve> action, CurveChart chart)
      {
         chart.AddCurvesFor(dataRepository.AllButBaseGrid(), x => x.Name, _dimensionFactory, action);
      }

      public DataRepository CreateScatterDataRepository(string id, string repositoryName, OutputResiduals outputResidual)
      {
         var dataRepository = createEmptyRepository(id, repositoryName, "Values");
         var scatterColumn = dataRepository.FirstDataColumn();
         var outputPath = new List<string>(outputResidual.FullOutputPath.ToPathArray());
         //need to create a unique path containing the observed data name. Since we want to keep the entity name and simulation name, we have to insert before the name
         if (outputPath.Count > 0)
            outputPath.Insert(outputPath.Count - 1, outputResidual.ObservedDataName);

         scatterColumn.QuantityInfo.Path = outputPath;
         return dataRepository;
      }

      public void ConfigureChartAxis(AnalysisChartWithLocalRepositories chart)
      {
         chart.YAxis.Caption = Residuals;
         chart.YAxis.Scaling = Scalings.Linear;
         chart.YAxis.DefaultLineStyle = LineStyles.Dash;
      }

      public DataRepository GetOrCreateScatterDataRepositoryInChart(AnalysisChartWithLocalRepositories chart, OutputResiduals outputResidual, int? runIndex = null)
      {
         var repositoryName = runIndex.HasValue ? SimulationResultsForRun(runIndex.Value) : "Simulation Results";
         var idSuffix = runIndex.HasValue ? $"-{runIndex}" : "";
         var residuals = outputResidual.Residuals;
         var id = $"{chart.Id}-{outputResidual.FullOutputPath}-{outputResidual.ObservedData.Id}{idSuffix}";

         var dataRepository = chart.DataRepositories.FindById(id);
         if (dataRepository == null)
         {
            dataRepository = CreateScatterDataRepository(id, repositoryName, outputResidual);
            chart.AddRepository(dataRepository);
         }

         dataRepository.BaseGrid.Values = residuals.Select(x => x.Time).ToFloatArray();
         dataRepository.FirstDataColumn().Values = residuals.Select(x => x.Value).ToFloatArray();

         return dataRepository;
      }
   }
}