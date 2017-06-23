using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ChartFromTemplateService : ContextSpecification<IChartFromTemplateService>
   {
      protected IKeyPathMapper _keyPathMapper;
      protected IDimensionFactory _dimensionFactory;
      protected CurveChart _chart;
      protected List<DataColumn> _dataColumns;
      protected CurveChartTemplate _template;
      protected BaseGrid _baseGrid;
      protected DataColumn _drugColumn;
      protected IDimension _timeDimension;
      protected IDimension _concentrationDimension;
      protected DataColumn _enzymeColumn;
      protected IDialogCreator _dialogCreator;
      private ICloneManager _cloneManager;
      protected string[] _drugTemplatePathArray;
      protected string[] _enzymeTemplatePathArray;
      protected IChartUpdater _chartUpdater;

      protected override void Context()
      {
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _keyPathMapper = new KeyPathMapper(new EntityPathResolverForSpecs(), new ObjectPathFactoryForSpecs());
         _dialogCreator = A.Fake<IDialogCreator>();
         _cloneManager = A.Fake<ICloneManager>();
         _chartUpdater= A.Fake<IChartUpdater>();
         sut = new ChartFromTemplateService(_dimensionFactory, _keyPathMapper, _dialogCreator, _cloneManager, _chartUpdater);
         _chart = new CurveChart();
         _dataColumns = new List<DataColumn>();
         _template = new CurveChartTemplate();
         _timeDimension = A.Fake<IDimension>();
         _concentrationDimension = A.Fake<IDimension>();
         _baseGrid = new BaseGrid("Time", _timeDimension);

         _drugColumn = new DataColumn("Drug", _concentrationDimension, _baseGrid);
         _drugTemplatePathArray = new[] {"Organism", "Liver", "Cells", "Drug", "Concentration"};
         var drugPath = new List<string>(_drugTemplatePathArray);
         drugPath.Insert(0, "Sim");
         _drugColumn.QuantityInfo.Path = drugPath;
         _drugColumn.QuantityInfo.Type = QuantityType.Drug | QuantityType.Observer;
         _drugColumn.DataInfo.Origin = ColumnOrigins.Calculation;


         _enzymeColumn = new DataColumn("Enzyme", _concentrationDimension, _baseGrid);
         _enzymeTemplatePathArray = new[] {"Organism", "Liver", "Cells", "Enzyme", "Concentration"};
         var enzymePath = new List<string>(_enzymeTemplatePathArray);
         enzymePath.Insert(0, "Sim");

         _enzymeColumn.QuantityInfo.Path = enzymePath;
         _enzymeColumn.QuantityInfo.Type = QuantityType.Enzyme;
         _enzymeColumn.DataInfo.Origin = ColumnOrigins.Calculation;

         A.CallTo(() => _dimensionFactory.MergedDimensionFor(A<IWithDimension>._))
            .ReturnsLazily(x => x.GetArgument<IWithDimension>(0).Dimension);

         //always available
         _dataColumns.Add(_baseGrid);
         _dataColumns.Add(_drugColumn);
         _dataColumns.Add(_enzymeColumn);
      }

      protected override void Because()
      {
         sut.InitializeChartFromTemplate(_chart, _dataColumns, _template);
      }

      protected void ValidateCurve(Curve curve, DataColumn xData, DataColumn yData, string name)
      {
         curve.xData.ShouldBeEqualTo(xData);
         curve.yData.ShouldBeEqualTo(yData);
         curve.Name.ShouldBeEqualTo(name);
      }
   }

   public class When_initializing_a_chart_from_a_template_containing_curves_with_base_grid_and_with_the_exact_same_path_as_the_data_columns : concern_for_ChartFromTemplateService
   {
      private bool _propagateChartChangedEvent;

      protected override void Context()
      {
         base.Context();

         _template.Curves.Add(new CurveTemplate
         {
            Name = "Curve1",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = _drugTemplatePathArray.ToPathString()}
         });
         _propagateChartChangedEvent = false;
      }

      protected override void Because()
      {
         sut.InitializeChartFromTemplate(_chart, _dataColumns, _template, propogateChartChangeEvent:_propagateChartChangedEvent);
      }


      [Observation]
      public void should_return_a_chart_with_the_curves_for_the_exact_data()
      {
         _chart.Curves.Count.ShouldBeEqualTo(1);
         var curve = _chart.Curves.ElementAt(0);
         ValidateCurve(curve, _baseGrid, _drugColumn, "Curve1");
      }

      [Observation]
      public void should_not_add_the_curve_with_data_having_another_type()
      {
         _chart.Curves.Any(x => x.yData == _enzymeColumn).ShouldBeFalse();
      }

      [Observation]
      public void should_have_updated_the_chart_using_the_chart_updater()
      {
         A.CallTo(() => _chartUpdater.UpdateTransaction(_chart, _propagateChartChangedEvent)).MustHaveHappened();
      }
   }

   public class When_initializing_a_chart_from_a_template_containing_curves_with_base_grid_and_with_the_exact_same_path_as_the_data_columns_as_well_as_another_curve_template_pattern_matching : concern_for_ChartFromTemplateService
   {
      protected override void Context()
      {
         base.Context();

         _template.Curves.Add(new CurveTemplate
         {
            Name = "PATTERN_MATCH",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = _enzymeTemplatePathArray.ToPathString()}
         });

         _template.Curves.Add(new CurveTemplate
         {
            Name = "EXACT_MATCH",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = _drugTemplatePathArray.ToPathString()}
         });
      }

      [Observation]
      public void should_return_the_exact_match()
      {
         _chart.Curves.Count.ShouldBeEqualTo(1);
         var curve = _chart.Curves.ElementAt(0);
         ValidateCurve(curve, _baseGrid, _drugColumn, "EXACT_MATCH");
      }
   }

   public class When_initializing_a_chart_from_a_template_containing_curves_with_base_grid_and_with_two_template_patterns_matching_the_same_curve : concern_for_ChartFromTemplateService
   {
      protected override void Context()
      {
         base.Context();

         _template.Curves.Add(new CurveTemplate
         {
            Name = "PATTERN_MATCH_1",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = new[] {"Organism", "Liver", "Cells", "D1", "Concentration"}.ToPathString()}
         });

         _template.Curves.Add(new CurveTemplate
         {
            Name = "PATTERN_MATCH_2",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = new[] {"Organism", "Liver", "Cells", "D2", "Concentration"}.ToPathString()}
         });
      }

      [Observation]
      public void should_use_the_first_pattern_matching()
      {
         _chart.Curves.Count.ShouldBeEqualTo(1);
         var curve = _chart.Curves.ElementAt(0);
         ValidateCurve(curve, _baseGrid, _drugColumn, "PATTERN_MATCH_1");
      }
   }

   public class When_initializing_a_chart_from_a_template_containing_curves_with_base_grid_and_with_a_template_patterns_matching_a_curve_for_molecule : concern_for_ChartFromTemplateService
   {
      protected override void Context()
      {
         base.Context();

         _template.Curves.Add(new CurveTemplate
         {
            Name = "PATTERN_NAME_MATCH_D1",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = new[] {"Organism", "Liver", "Cells", "D1", "Concentration"}.ToPathString()}
         });
      }

      [Observation]
      public void should_use_the_pattern_matching_and_rename_the_curve_name_to_use_the_drug_name()
      {
         _chart.Curves.Count.ShouldBeEqualTo(1);
         var curve = _chart.Curves.ElementAt(0);
         ValidateCurve(curve, _baseGrid, _drugColumn, "PATTERN_NAME_MATCH_Drug");
      }
   }

   public class When_initializing_a_chart_from_a_template_containing_curves_with_base_grid_and_with_the_exact_same_path_as_the_data_columns_except_the_simulation_name : concern_for_ChartFromTemplateService
   {
      protected override void Context()
      {
         base.Context();

         _template.Curves.Add(new CurveTemplate
         {
            Name = "Curve1",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = _drugTemplatePathArray.ToPathString()}
         });

         _template.Curves.Add(new CurveTemplate
         {
            Name = "Curve2",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _enzymeColumn.QuantityInfo.Type, Path = _enzymeTemplatePathArray.ToPathString()}
         });
      }

      [Observation]
      public void should_return_a_chart_with_the_curves_for_the_exact_data()
      {
         _chart.Curves.Count.ShouldBeEqualTo(2);
         var curve1 = _chart.Curves.ElementAt(0);
         var curve2 = _chart.Curves.ElementAt(1);
         ValidateCurve(curve1, _baseGrid, _drugColumn, "Curve1");
         ValidateCurve(curve2, _baseGrid, _enzymeColumn, "Curve2");
      }
   }

   public class When_initializing_a_chart_from_a_template_containing_curves_without_base_grid_and_with_the_exact_same_path_as_the_data_columns : concern_for_ChartFromTemplateService
   {
      protected override void Context()
      {
         base.Context();
         _template.Curves.Add(new CurveTemplate
         {
            Name = "Curve1",
            xData = {QuantityType = QuantityType.Enzyme, Path = _enzymeTemplatePathArray.ToPathString()},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = _drugTemplatePathArray.ToPathString()}
         });
      }

      [Observation]
      public void should_return_a_chart_with_the_curves_for_the_exact_data()
      {
         _chart.Curves.Count.ShouldBeEqualTo(1);
         var curve = _chart.Curves.ElementAt(0);
         ValidateCurve(curve, _enzymeColumn, _drugColumn, "Curve1");
      }
   }

   public class When_initializing_a_chart_from_a_template_containing_curves_with_base_grid_and_not_matching_exactly_the_data_columns : concern_for_ChartFromTemplateService
   {
      private DataColumn _anotherColumn;

      protected override void Context()
      {
         base.Context();

         _template.Curves.Add(new CurveTemplate
         {
            Name = "Curve1",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = new[] {"Organism", "Liver", "Cells", "Aspirin", "Concentration"}.ToPathString()}
         });

         _anotherColumn = new DataColumn("Another", _concentrationDimension, _baseGrid);
         _anotherColumn.QuantityInfo.Path = new[] {"Sim", "Organism", "Liver", "Cells", "ANOTHER", "Concentration"};
         _anotherColumn.QuantityInfo.Type = QuantityType.Drug | QuantityType.Observer;
         _anotherColumn.DataInfo.Origin = ColumnOrigins.Calculation;

         _dataColumns.Add(_anotherColumn);
      }

      [Observation]
      public void should_return_a_chart_with_the_curves_for_the_matching_data_with_the_name_matching_the_curve_name_definition_method()
      {
         _chart.Curves.Count.ShouldBeEqualTo(2);
         var curve1 = _chart.Curves.ElementAt(0);
         var curve2 = _chart.Curves.ElementAt(1);
         ValidateCurve(curve1, _baseGrid, _drugColumn, "Drug");
         ValidateCurve(curve2, _baseGrid, _anotherColumn, "Another");
      }
   }

   public class When_initializing_a_chart_from_a_template_containing_curves_with_base_grid_and_not_matching_exactly_the_data_columns_that_could_lead_to_duplicate_columns : concern_for_ChartFromTemplateService
   {
      private DataColumn _anotherColumn;

      protected override void Context()
      {
         base.Context();

         _template.Curves.Add(new CurveTemplate
         {
            Name = "Curve1",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = new[] {"Organism", "Liver", "Cells", "Aspirin", "Concentration"}.ToPathString()}
         });
         _template.Curves.Add(new CurveTemplate
         {
            Name = "Curve2",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = new[] {"Organism", "Liver", "Cells", "Midazolam", "Concentration"}.ToPathString()}
         });

         _anotherColumn = new DataColumn("Another", _concentrationDimension, _baseGrid);
         _anotherColumn.QuantityInfo.Path = new[] {"Sim", "Organism", "Liver", "Cells", "ANOTHER", "Concentration"};
         _anotherColumn.QuantityInfo.Type = QuantityType.Drug | QuantityType.Observer;
         _anotherColumn.DataInfo.Origin = ColumnOrigins.Calculation;

         _dataColumns.Add(_anotherColumn);
      }

      [Observation]
      public void should_return_a_chart_with_the_curves_for_the_matching_data_with_the_name_matching_the_curve_name_definition_method()
      {
         _chart.Curves.Count.ShouldBeEqualTo(2);
         var curve1 = _chart.Curves.ElementAt(0);
         var curve2 = _chart.Curves.ElementAt(1);
         ValidateCurve(curve1, _baseGrid, _drugColumn, "Drug");
         ValidateCurve(curve2, _baseGrid, _anotherColumn, "Another");
      }
   }

   public class When_the_number_of_matching_curves_is_exceeding_the_given_threshold_and_the_user_decides_to_cancel_the_action : concern_for_ChartFromTemplateService
   {
      protected override void Context()
      {
         base.Context();
         _chart.ChartSettings.BackColor = Color.Red;
         _template.ChartSettings.BackColor = Color.Aqua;

         _template.Curves.Add(new CurveTemplate
         {
            Name = "Curve1",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = new[] {"Organism", "Liver", "Cells", "Aspirin", "Concentration"}.ToPathString()}
         });

         var anotherColumn = new DataColumn("Another", _concentrationDimension, _baseGrid);
         anotherColumn.QuantityInfo.Path = new[] {"Sim", "Organism", "Liver", "Cells", "ANOTHER", "Concentration"};
         anotherColumn.QuantityInfo.Type = QuantityType.Drug | QuantityType.Observer;
         anotherColumn.DataInfo.Origin = ColumnOrigins.Calculation;

         _dataColumns.Add(anotherColumn);

         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.No);
      }

      protected override void Because()
      {
         sut.InitializeChartFromTemplate(_chart, _dataColumns, _template, warnIfNumberOfCurvesAboveThreshold: true, warningThreshold:1);
      }

      [Observation]
      public void should_not_update_the_chart()
      {
         _chart.ChartSettings.BackColor.ShouldBeEqualTo(Color.Red);
      }
   }

   public class When_the_number_of_matching_curves_is_exceeding_the_given_threshold_and_the_user_decides_to_continue_the_action : concern_for_ChartFromTemplateService
   {
      protected override void Context()
      {
         base.Context();
         _chart.ChartSettings.BackColor = Color.Red;
         _template.ChartSettings.BackColor = Color.Aqua;
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.Yes);

         _template.Curves.Add(new CurveTemplate
         {
            Name = "Curve1",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = new[] {"Organism", "Liver", "Cells", "Aspirin", "Concentration"}.ToPathString()}
         });

         var anotherColumn = new DataColumn("Another", _concentrationDimension, _baseGrid);
         anotherColumn.QuantityInfo.Path = new[] {"Sim", "Organism", "Liver", "Cells", "ANOTHER", "Concentration"};
         anotherColumn.QuantityInfo.Type = QuantityType.Drug | QuantityType.Observer;
         _dataColumns.Add(anotherColumn);
      }

      protected override void Because()
      {
         sut.InitializeChartFromTemplate(_chart, _dataColumns, _template, warnIfNumberOfCurvesAboveThreshold: true, warningThreshold:1);
      }

      [Observation]
      public void should_update_the_chart()
      {
         _chart.ChartSettings.BackColor.ShouldBeEqualTo(_template.ChartSettings.BackColor);
      }
   }

   public class When_there_are_more_than_one_exact_match_for_a_path_with_a_matching_repository_name : concern_for_ChartFromTemplateService
   {
      private DataRepository _repository;

      protected override void Context()
      {
         base.Context();
         _repository = new DataRepository().WithName("REP1");
         _template.Curves.Add(new CurveTemplate
         {
            Name = "Curve1",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = _drugTemplatePathArray.ToPathString(), RepositoryName = "REP1"}
         });

         _template.Curves.Add(new CurveTemplate
         {
            Name = "Curve2",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = _drugTemplatePathArray.ToPathString(), RepositoryName = "REP2"}
         });

         _repository.Add(_drugColumn);
      }

      [Observation]
      public void the_template_with_the_matching_repository_name_should_be_used()
      {
         _chart.Curves.Count.ShouldBeEqualTo(1);
         var curve1 = _chart.Curves.ElementAt(0);
         curve1.Name.ShouldBeEqualTo("Curve1");
      }
   }

   public class When_there_are_more_than_one_exact_match_for_a_path_with_non_matching_repository_name : concern_for_ChartFromTemplateService
   {
      private DataRepository _repository1;
      private DataRepository _repository2;
      private DataColumn _drugColumn2;

      protected override void Context()
      {
         base.Context();
         _repository1 = new DataRepository().WithName("REP3");
         _repository2 = new DataRepository().WithName("REP4");

         _template.Curves.Add(new CurveTemplate
         {
            Name = "Curve1",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = _drugTemplatePathArray.ToPathString(), RepositoryName = "REP1"}
         });

         _template.Curves.Add(new CurveTemplate
         {
            Name = "Curve2",
            xData = {QuantityType = QuantityType.BaseGrid},
            yData = {QuantityType = _drugColumn.QuantityInfo.Type, Path = _drugTemplatePathArray.ToPathString(), RepositoryName = "REP2"}
         });

         _repository1.Add(_drugColumn);

         _drugColumn2 = new DataColumn("Drug", _concentrationDimension, new BaseGrid("BaseGrid", _timeDimension))
         {
            QuantityInfo = _drugColumn.QuantityInfo,
            DataInfo = _drugColumn.DataInfo
         };

         _repository2.Add(_drugColumn2);
         _dataColumns.Add(_drugColumn2);
      }

      [Observation]
      public void the_template_with_the_matching_repository_name_should_be_used()
      {
         _chart.Curves.Count.ShouldBeEqualTo(2);
         var curve1 = _chart.Curves.ElementAt(0);
         curve1.Name.ShouldBeEqualTo("Curve1");

         var curve2 = _chart.Curves.ElementAt(1);
         curve2.Name.ShouldBeEqualTo("Curve2");
      }
   }
}