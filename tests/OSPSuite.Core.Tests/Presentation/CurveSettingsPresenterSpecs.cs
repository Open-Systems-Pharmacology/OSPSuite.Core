using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using OSPSuite.Core;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation
{
   internal abstract class concern_for_CurveSettingsPresenter : ContextSpecification<CurveSettingsPresenter>
   {
      protected IDimensionFactory _dimensionFactory;
      private ICurveSettingsView _view;
      protected CurveChart _chart;

      protected override void Context()
      {
         _view = A.Fake<ICurveSettingsView>();
         _dimensionFactory = DimensionFactoryForSpecs.Factory;
         _chart = new CurveChart().WithAxes();
         
         sut = new CurveSettingsPresenter(_view, _dimensionFactory);
         sut.Edit(_chart);

         var dataColumn = new DataColumn
         {
            Id = "id",
            BaseGrid = new BaseGrid("time", DimensionFactoryForSpecs.TimeDimension),
            Dimension = DimensionFactoryForSpecs.ConcentrationDimension
         };

         dataColumn.DataInfo.Origin = ColumnOrigins.Observation;

         _chart.Axes.Each(axis => axis.DefaultLineStyle = LineStyles.None);
      }
   }

}
