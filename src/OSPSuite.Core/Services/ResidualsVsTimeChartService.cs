using System;
using OSPSuite.Core.Domain;
using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

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
      /// will begin at <paramref name="minObservedDataTime" /> and end at <paramref name="maxObservedDataTime" />
      /// </summary>
      DataRepository AddZeroMarkerCurveToChart(AnalysisChartWithLocalRepositories chart, float minObservedDataTime, float maxObservedDataTime);

      /// <summary>
      ///    Creates the DataRepository for the given residuals <paramref name="outputResidual" /> with given
      /// <paramref name="id" /> and <paramref name="repositoryName" /> 
      /// </summary>
      DataRepository CreateScatterDataRepository(string id, string repositoryName, OutputResiduals outputResidual);
   }

   public class ResidualsVsTimeChartService : IResidualsVsTimeChartService
   {
      private const string ZERO = "Zero";
      private string _markerCurveId = string.Empty;
      private readonly IDimensionFactory _dimensionFactory;

      public ResidualsVsTimeChartService(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }


      public string Zero => ZERO;
      public DataRepository AddZeroMarkerCurveToChart(AnalysisChartWithLocalRepositories chart, float minObservedDataTime, float maxObservedDataTime)
      {
         var markerRepository = createMarkerRepository(minObservedDataTime, maxObservedDataTime, chart);
         AddCurvesFor(markerRepository,(column, curve) =>
         {
            curve.UpdateMarkerCurve(ZERO);
            _markerCurveId = curve.Id;
         }, chart);

         return markerRepository;
      }
      private DataRepository createMarkerRepository(float minObservedDataTime, float maxObservedDataTime, AnalysisChartWithLocalRepositories chart)
      {
         var id = $"{chart.Id}-{ZERO}";
         var dataRepository = createEmptyRepository(id, ZERO, ZERO);
         dataRepository.BaseGrid.Values = new[] { minObservedDataTime, maxObservedDataTime};
         dataRepository.FirstDataColumn().Values = new[] { 0f, 0f };
         return dataRepository;
      }

      private DataRepository createEmptyRepository(string id, string name, string valueName)
      {
         var dataRepository = new DataRepository(id) { Name = name };
         var baseGrid = new BaseGrid($"{id}-Time", "Time", _dimensionFactory.Dimension(Constants.Dimension.TIME));
         var values = new DataColumn($"{id}-{valueName}", valueName, _dimensionFactory.NoDimension, baseGrid)
            { DataInfo = { Origin = ColumnOrigins.CalculationAuxiliary } };
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
   }
}