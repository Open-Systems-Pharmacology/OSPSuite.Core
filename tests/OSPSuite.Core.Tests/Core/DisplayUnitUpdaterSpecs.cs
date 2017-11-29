using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_DisplayUnitUpdater : ContextSpecification<IDisplayUnitUpdater>
   {
      protected IProject _project;
      protected IDisplayUnitRetriever _displayUnitRetriever;

      protected override void Context()
      {
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         sut = new DisplayUnitUpdater(_displayUnitRetriever);
         _project = A.Fake<IProject>();
      }
   }

   public class When_updating_the_display_units_used_in_the_project : concern_for_DisplayUnitUpdater
   {
      private Unit _myDefaultUnit;
      private IDimension _dimension1;
      private Parameter _parameter;
      private IMoleculeStartValue _moleculeStartValue;
      private MoleculeStartValuesBuildingBlock _moleculeStartValueBuildingBlock;
      private Container _topContainer;

      protected override void Context()
      {
         base.Context();
         _dimension1 = new Dimension(new BaseDimensionRepresentation {TimeExponent = 1}, "Time", "min");
         _myDefaultUnit = _dimension1.AddUnit("h", 60, 0);
         _topContainer = new Container();
         _parameter = new Parameter().WithDimension(_dimension1);
         _parameter.DisplayUnit = _dimension1.DefaultUnit;
         _topContainer.Add(_parameter);
         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(_parameter)).Returns(_myDefaultUnit);

         _moleculeStartValueBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _moleculeStartValue = new MoleculeStartValue {Path = new ObjectPath(new[] {"A", "B", "Molecule"}), Dimension = _dimension1};
         _moleculeStartValue.DisplayUnit = _dimension1.DefaultUnit;
         _moleculeStartValueBuildingBlock.Add(_moleculeStartValue);
         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(_moleculeStartValue)).Returns(_myDefaultUnit);
      }

      protected override void Because()
      {
         sut.UpdateDisplayUnitsIn(_topContainer);
         sut.UpdateDisplayUnitsIn(_moleculeStartValueBuildingBlock);
      }

      [Observation]
      public void should_retrieve_all_object_with_display_units_and_update_their_units_according_to_the_user_mapping()
      {
         _parameter.DisplayUnit.ShouldBeEqualTo(_myDefaultUnit);
         _moleculeStartValue.DisplayUnit.ShouldBeEqualTo(_myDefaultUnit);
      }
   }

   public class When_updating_the_display_units_used_in_a_given_data_repository : concern_for_DisplayUnitUpdater
   {
      private DataRepository _dataRepository;
      private IDimension _timeDimension;
      private BaseGrid _time;
      private Unit _hourUnit;
      private Dimension _concentrationDimension;
      private Unit _gramPerLiterUnit;
      private DataColumn _conc;

      protected override void Context()
      {
         base.Context();
         _dataRepository = new DataRepository();
         _timeDimension = new Dimension(new BaseDimensionRepresentation {TimeExponent = 1}, "Time", "min");
         _hourUnit = _timeDimension.AddUnit("h", 60, 0);
         _concentrationDimension = new Dimension(new BaseDimensionRepresentation {MassExponent = 1}, "Conc", "mg/l");
         _gramPerLiterUnit = _concentrationDimension.AddUnit("g/l", 1000, 0);
         _time = new BaseGrid("Time", _timeDimension);
         _conc = new DataColumn("Conc", _concentrationDimension, _time);
         _dataRepository.Add(_conc);
         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(_conc)).Returns(_gramPerLiterUnit);
         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(_time)).Returns(_hourUnit);
      }

      protected override void Because()
      {
         sut.UpdateDisplayUnitsIn(_dataRepository);
      }

      [Observation]
      public void should_update_the_display_unit_of_each_column_defined_in_the_data_repository()
      {
         _conc.DisplayUnit.ShouldBeEqualTo(_gramPerLiterUnit);
         _time.DisplayUnit.ShouldBeEqualTo(_hourUnit);
      }
   }

   public class When_updating_the_display_units_used_in_an_undefined_repository : concern_for_DisplayUnitUpdater
   {
      [Observation]
      public void should_not_crash()
      {
         sut.UpdateDisplayUnitsIn((DataRepository) null);
      }
   }

   public class When_updating_the_display_unit_for_a_parameter_using_a_table_formula : concern_for_DisplayUnitUpdater
   {
      private IParameter _parameter;
      private TableFormula _tableFormula;
      private Unit _xPreferredUnit;
      private Unit _yPreferredUnit;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula
         {
            XDimension = A.Fake<IDimension>(),
            Dimension = A.Fake<IDimension>()
         };

         _xPreferredUnit = new Unit("A", 1, 0);
         _yPreferredUnit = new Unit("B", 1, 0);

         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(_tableFormula.XDimension)).Returns(_xPreferredUnit);
         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(_tableFormula.Dimension)).Returns(_yPreferredUnit);

         _parameter = new Parameter {Formula = _tableFormula};
      }

      protected override void Because()
      {
         sut.UpdateDisplayUnitsIn(_parameter);
      }

      [Observation]
      public void should_also_update_the_display_unit_of_the_table_formula()
      {
         _tableFormula.XDisplayUnit.ShouldBeEqualTo(_xPreferredUnit);
         _tableFormula.YDisplayUnit.ShouldBeEqualTo(_yPreferredUnit);
      }
   }

   public class When_updating_the_display_unit_for_a_curve_chart : concern_for_DisplayUnitUpdater
   {
      private CurveChart _curveChart;
      private Unit _unit1;
      private Axis _axisX;
      private Axis _axisY;

      protected override void Context()
      {
         base.Context();

         _curveChart = new CurveChart().WithAxes();
         _unit1 = new Unit("XX", 1, 0);

         _axisX = _curveChart.AxisBy(AxisTypes.X);
         _axisX.UnitName = "OldX";
         _axisY = _curveChart.AxisBy(AxisTypes.Y);
         _axisY.UnitName = "OldY";

         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(_axisX, _axisX.Unit)).Returns(_unit1);
         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(_axisY, _axisY.Unit)).Returns(null);
      }

      protected override void Because()
      {
         sut.UpdateDisplayUnitsIn(_curveChart);
      }

      [Observation]
      public void should_update_the_unit_name_of_all_axis_for_which_a_display_unit_could_be_found()
      {
         _axisX.UnitName.ShouldBeEqualTo(_unit1.Name);
      }

      [Observation]
      public void should_not_update_the_unit_name_of_all_axis_for_which_a_display_unit_could_not_be_found()
      {
         _axisY.UnitName.ShouldBeEqualTo("OldY");
      }
   }
}