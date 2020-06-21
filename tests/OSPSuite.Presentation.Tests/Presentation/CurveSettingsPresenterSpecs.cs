using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_CurveSettingsPresenter : ContextSpecification<CurveSettingsPresenter>
   {
      protected IDimensionFactory _dimensionFactory;
      protected ICurveSettingsView _view;
      protected CurveChart _chart;
      protected Curve _curve1;
      protected Curve _curve2;
      private DataColumn _datColumn1;
      private DataColumn _datColumn2;
      protected List<CurveDTO> _allCurveDTOs;
      protected CurveDTO _curveDTO1;

      protected override void Context()
      {
         _view = A.Fake<ICurveSettingsView>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _chart = new CurveChart();
         A.CallTo(() => _dimensionFactory.MergedDimensionFor(A<IWithDimension>._)).ReturnsLazily(x => x.GetArgument<IWithDimension>(0).Dimension);

         var dataRepo1 = DomainHelperForSpecs.SimulationDataRepositoryFor("Sim1");
         _datColumn1 = dataRepo1.FirstDataColumn();

         var dataRepo2 = DomainHelperForSpecs.SimulationDataRepositoryFor("Sim2");
         _datColumn2 = dataRepo2.FirstDataColumn();

         _curve1 = new Curve();
         _curve1.SetxData(dataRepo1.BaseGrid, _dimensionFactory);
         _curve1.SetyData(_datColumn1, _dimensionFactory);

         _curve2 = new Curve();
         _curve2.SetxData(dataRepo2.BaseGrid, _dimensionFactory);
         _curve2.SetyData(_datColumn2, _dimensionFactory);

         _chart.AddCurve(_curve1);
         _chart.AddCurve(_curve2);

         sut = new CurveSettingsPresenter(_view, _dimensionFactory);

         A.CallTo(() => _view.BindTo(A<IEnumerable<CurveDTO>>._))
            .Invokes(x =>
            {
               _allCurveDTOs = x.GetArgument<IEnumerable<CurveDTO>>(0).ToList();
               _curveDTO1 = _allCurveDTOs.FirstOrDefault();
            });

         sut.Edit(_chart);
      }
   }

   public class When_the_curve_settings_presenter_is_editing_a_chart : concern_for_CurveSettingsPresenter
   {
      [Observation]
      public void should_edit_the_curves_ddefined_in_the_chart()
      {
         _allCurveDTOs.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_the_curve_settings_presenter_is_notified_that_x_data_where_changed : concern_for_CurveSettingsPresenter
   {
      private DataColumn _newDataColum;
      private bool _curvePropertyChanged;

      protected override void Context()
      {
         base.Context();
         var obsData = DomainHelperForSpecs.ObservedData();
         _newDataColum = obsData.FirstDataColumn();
         sut.CurvePropertyChanged += (o, e) => _curvePropertyChanged = true;
      }

      protected override void Because()
      {
         sut.SetCurveXData(_curveDTO1, _newDataColum);
      }

      [Observation]
      public void should_update_the_x_data_in_the_selected_curve()
      {
         _curve1.xData.ShouldBeEqualTo(_newDataColum);
      }

      [Observation]
      public void should_notify_a_curve_changed_property_event()
      {
         _curvePropertyChanged.ShouldBeTrue();
      }
   }

   public class When_the_curve_settings_presenter_is_notified_that_y_data_where_changed : concern_for_CurveSettingsPresenter
   {
      private DataColumn _newDataColum;
      private bool _curvePropertyChanged;

      protected override void Context()
      {
         base.Context();
         var obsData = DomainHelperForSpecs.ObservedData();
         _newDataColum = obsData.FirstDataColumn();
         sut.CurvePropertyChanged += (o, e) => _curvePropertyChanged = true;
      }

      protected override void Because()
      {
         sut.SetCurveYData(_curveDTO1, _newDataColum);
      }

      [Observation]
      public void should_update_the_x_data_in_the_selected_curve()
      {
         _curve1.yData.ShouldBeEqualTo(_newDataColum);
      }

      [Observation]
      public void should_notify_a_curve_changed_property_event()
      {
         _curvePropertyChanged.ShouldBeTrue();
      }
   }

   public class When_the_curve_settings_presenter_is_notified_that_a_curve_was_removed : concern_for_CurveSettingsPresenter
   {
      private Curve _curveRemoved;

      protected override void Context()
      {
         base.Context();
         sut.RemoveCurve += (o, e) => { _curveRemoved = e.Curve; };
      }

      protected override void Because()
      {
         sut.Remove(_curveDTO1);
      }

      [Observation]
      public void should_notify_a_curve_removed_event()
      {
         _curveRemoved.ShouldBeEqualTo(_curveDTO1.Curve);
      }
   }

   public class When_the_curve_settings_presenter_is_notified_that_curve_should_be_added_for_a_set_of_columns : concern_for_CurveSettingsPresenter
   {
      private IReadOnlyList<DataColumn> _columnsToAdd;
      private IReadOnlyList<DataColumn> _columnsEvent;

      protected override void Context()
      {
         base.Context();
         _columnsToAdd = A.Fake<IReadOnlyList<DataColumn>>();
         sut.AddCurves += (o, e) => _columnsEvent = e.Columns;
      }

      protected override void Because()
      {
         sut.AddCurvesForColumns(_columnsToAdd);
      }

      [Observation]
      public void should_notify_the_column_added_event()
      {
         _columnsEvent.ShouldBeEqualTo(_columnsToAdd);
      }
   }

   public class When_the_curve_settings_presenter_is_retrieving_the_tool_tip_for_a_curve : concern_for_CurveSettingsPresenter
   {
      private readonly string _curveName = "TOTO";

      protected override void Context()
      {
         base.Context();
         sut.CurveNameDefinition = x => _curveName;
      }

      [Observation]
      public void should_use_the_curve_name_defintion_method_to_retrieve_the_name_for_the_y_data()
      {
         sut.ToolTipFor(_curveDTO1).ShouldBeEqualTo(_curveName);
      }
   }

   public class When_the_curve_settings_presenter_is_retrieving_the_tool_tip_for_a_curve_but_the_curve_name_definition_method_was_not_set : concern_for_CurveSettingsPresenter
   {
      [Observation]
      public void should_return_the_column_name()
      {
         sut.ToolTipFor(_curveDTO1).ShouldBeEqualTo(_curve1.yData.Name);
      }
   }

   public class When_the_curve_settings_presenter_is_clearing_its_content : concern_for_CurveSettingsPresenter
   {
      protected override void Because()
      {
         sut.Clear();
      }

      [Observation]
      public void should_clear_the_binding()
      {
         _allCurveDTOs.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_the_curve_settings_presenter_is_refreshing_its_content : concern_for_CurveSettingsPresenter
   {
      private Curve _curve3;

      protected override void Context()
      {
         base.Context();
         _chart.RemoveCurve(_curve2);
         _curve3 = _curve2.Clone().WithName("Curve3");
         _chart.AddCurve(_curve3);
      }

      protected override void Because()
      {
         sut.Refresh();
      }

      [Observation]
      public void should_remove_all_curves_that_were_removed_since_previous_refresh()
      {
         _allCurveDTOs.Find(x => x.Curve == _curve2).ShouldBeNull();
      }

      [Observation]
      public void should_add_all_curves_that_were_added_since_previous_refresh()
      {
         _allCurveDTOs.Find(x => x.Curve == _curve3).ShouldNotBeNull();
      }
   }

   public class When_the_curve_settings_presenter_is_refreshing_and_is_in_latch: concern_for_CurveSettingsPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.IsLatched = true;
      }

      protected override void Because()
      {
         sut.Refresh();
      }

      [Observation]
      public void should_not_update_the_view()
      {
         //once becasue of initial context
         A.CallTo(() => _view.BindTo(A<IEnumerable<CurveDTO>>._)).MustHaveHappenedOnceExactly();
      }

   }

   public class When_the_curve_settings_presenter_is_moving_a_curve_legend_position_after_another_curve : concern_for_CurveSettingsPresenter
   {
      private bool _curvePropertyChanged;

      protected override void Context()
      {
         base.Context();
         sut.CurvePropertyChanged += (o, e) => _curvePropertyChanged = true;
         _curve1.LegendIndex = 2;
         _curve2.LegendIndex = 3;
      }

      protected override void Because()
      {
         sut.MoveCurvesInLegend(_curveDTO1, _allCurveDTOs[1]);
      }

      [Observation]
      public void should_notify_a_curve_property_changed_event()
      {
         _curvePropertyChanged.ShouldBeTrue();
      }

      [Observation]
      public void should_update_the_legend_index_of_the_curve_being_moved()
      {
         _curve1.LegendIndex.ShouldBeEqualTo(3);
         _curve2.LegendIndex.ShouldBeEqualTo(4);
      }
   }

   public class When_the_curve_settings_presenter_is_updating_the_color_of_a_given_curve : concern_for_CurveSettingsPresenter
   {
      private bool _curvePropertyChanged;

      protected override void Context()
      {
         base.Context();
         _curveDTO1.Color = Color.Red;
         sut.CurvePropertyChanged += (o,e) => _curvePropertyChanged = true;
         ;
      }

      protected override void Because()
      {
         sut.UpdateCurveColor(_curveDTO1, Color.Blue);
      }

      [Observation]
      public void should_update_the_color()
      {
         _curve1.Color.ShouldBeEqualTo(Color.Blue);
      }

      [Observation]
      public void should_notify_a_curve_property_changed_event()
      {
         _curvePropertyChanged.ShouldBeTrue();
      }
   }
}