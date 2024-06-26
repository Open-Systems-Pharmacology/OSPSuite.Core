using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Charts
{
   /// <summary>
   ///    Displays a simple 2d curve charts that cannot be edited by the user
   /// </summary>
   public interface ISimpleChartPresenter : IPresenter<ISimpleChartView>
   {
      /// <summary>
      ///    creates a CurveChart for the table formula given as parameter and display it in the view
      /// </summary>
      CurveChart Plot(TableFormula tableFormula);

      /// <summary>
      ///    creates a CurveChart for a <paramref name="dataRepository" /> using the default y scaling <paramref name="scale" />
      /// </summary>
      CurveChart Plot(DataRepository dataRepository, Scalings scale);

      /// <summary>
      ///    returns a CurveChart for <paramref name="dataRepository" />
      /// </summary>
      CurveChart Plot(DataRepository dataRepository);

      /// <summary>
      ///    creates a CurveChart for a number of data repositories
      /// </summary>
      CurveChart PlotObservedData(IEnumerable<DataRepository> observedData);

      /// <summary>
      ///    Special plots for observed data: only columns with origin <see cref="ColumnOrigins.Observation" /> will be added to
      ///    the plot
      /// </summary>
      CurveChart PlotObservedData(DataRepository dataRepository);

      CurveChart Chart { get; }

      /// <summary>
      ///    Sets the scaling of the chart to logarithmic or linear
      /// </summary>
      void SetChartScale(Scalings scale);

      /// <summary>
      ///    Enables the selection of log/lin Y axis by a control. Default is <c>false</c>
      /// </summary>
      bool LogLinSelectionEnabled { get; set; }

      /// <summary>
      ///    Action run when a series point is hot tracked in the chart
      /// </summary>
      Action<int> HotTracked { set; }

      /// <summary>
      ///    Refresh the display after external changes were made to the chart
      /// </summary>
      void Refresh();

      /// <summary>
      ///    Hook that can be executed on the dataColumn to modify them, reorder them etc..
      /// </summary>
      Func<IEnumerable<DataColumn>, IEnumerable<DataColumn>> PreExportHook { get; set; }

      void Clear();
   }

   public class SimpleChartPresenter : AbstractCommandCollectorPresenter<ISimpleChartView, ISimpleChartPresenter>, ISimpleChartPresenter
   {
      private readonly IChartDisplayPresenter _chartDisplayPresenter;
      private readonly IPresentationUserSettings _presentationUserSettings;
      private readonly IDimensionFactory _dimensionFactory;
      public CurveChart Chart { get; protected set; }
      private readonly IChartFactory _chartFactory;

      public SimpleChartPresenter(
         ISimpleChartView view,
         IChartDisplayPresenter chartDisplayPresenter,
         IChartFactory chartFactory,
         IPresentationUserSettings presentationUserSettings,
         IDimensionFactory dimensionFactory)
         : base(view)
      {
         _chartDisplayPresenter = chartDisplayPresenter;
         _chartFactory = chartFactory;
         _presentationUserSettings = presentationUserSettings;
         _dimensionFactory = dimensionFactory;
         _view.AddView(_chartDisplayPresenter.View);
         _chartDisplayPresenter.DisableCurveAndAxisEdits();
         LogLinSelectionEnabled = false;
         AddSubPresenters(_chartDisplayPresenter);
      }

      public Func<IEnumerable<DataColumn>, IEnumerable<DataColumn>> PreExportHook
      {
         get => _chartDisplayPresenter.PreExportHook;
         set => _chartDisplayPresenter.PreExportHook = value;
      }

      public void Clear() => _chartDisplayPresenter.Clear();

      public Action<int> HotTracked
      {
         set => _chartDisplayPresenter.HotTracked = value;
      }

      public void Refresh() => _chartDisplayPresenter.Refresh();

      public void SetChartScale(Scalings scale)
      {
         var yAxis = getYAxis(Chart);
         yAxis.Scaling = scale;
         Refresh();
      }

      public bool LogLinSelectionEnabled
      {
         get => _view.LogLinSelectionEnabled;
         set => _view.LogLinSelectionEnabled = value;
      }

      public CurveChart Plot(TableFormula tableFormula)
      {
         Chart = _chartFactory.CreateChartFor(tableFormula);
         var xAxis = getXAxis(Chart);
         if (xAxis != null)
            xAxis.Caption = tableFormula.XName;

         var yAxis = getYAxis(Chart);
         if (yAxis != null)
            yAxis.Caption = tableFormula.Name;
         bindToChart();
         return Chart;
      }

      private void bindToChart()
      {
         Chart.ChartSettings.LegendPosition = LegendPositions.None;
         _chartDisplayPresenter.Edit(Chart);
      }

      public CurveChart Plot(DataRepository dataRepository, Scalings scale)
      {
         Chart = _chartFactory.CreateChartFor(dataRepository, scale);
         setChartScalingForObservedData(new[] { dataRepository });
         bindToChart();
         return Chart;
      }

      public CurveChart Plot(DataRepository dataRepository) => Plot(dataRepository, _presentationUserSettings.DefaultChartYScaling);

      public CurveChart PlotObservedData(IEnumerable<DataRepository> observedData)
      {
         var observedDataList = observedData.ToList();
         Chart = _chartFactory.Create<CurveChart>();
         setChartScalingForObservedData(observedDataList);
         observedDataList.Each(addCurvesToChart);
         bindToChart();
         setScaleInView(Chart);
         return Chart;
      }

      private void setChartScalingForObservedData(IReadOnlyList<DataRepository> observedDataList)
      {
         if (!shouldUseLinearScaling(observedDataList))
            return;

         Chart.DefaultYAxisScaling = Scalings.Linear;
      }

      private bool shouldUseLinearScaling(IReadOnlyList<DataRepository> observedData) => observedData.Any(dataRepositoryHasFraction);

      private bool dataRepositoryHasFraction(DataRepository dataRepository) => dataRepository.AllButBaseGrid().Any(x => x.IsFraction());

      public CurveChart PlotObservedData(DataRepository observedData) => PlotObservedData(new[] { observedData }).WithName(observedData.Name);

      private void setScaleInView(CurveChart chart)
      {
         var yAxis = getYAxis(chart);
         if (yAxis == null) return;

         _view.SetChartScale(yAxis.Scaling);
      }

      private Axis getXAxis(CurveChart chart) => chart.XAxis;

      private Axis getYAxis(CurveChart chart) => chart.YAxis;

      private void setAxesCaptions(DataRepository observedData)
      {
         Chart.XAxis.Caption = observedData.BaseGrid.Name;
         Chart.YAxis.Caption = observedData.ObservationColumns().FirstOrDefault()?.Name;
      }

      private void addCurvesToChart(DataRepository observedData)
      {
         var allObservations = observedData.ObservationColumns().ToList();
         allObservations.Each(c =>
         {
            var curve = Chart.CreateCurve(c.BaseGrid, c, observedData.Name, _dimensionFactory);

            Chart.UpdateCurveColorAndStyle(curve, c, allObservations);
            Chart.AddCurve(curve);
         });

         setAxesCaptions(observedData);
      }
   }
}