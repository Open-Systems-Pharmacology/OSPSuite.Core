using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Import;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public abstract class concern_for_MappingParameterEditorPresenter : ContextSpecification<MappingParameterEditorPresenter>
   {
      protected const string MEASUREMENT_UNIT = "mg/l";
      protected const string MEASUREMENT_UNIT_COLUMN = "Unit";
      protected IUnitsEditorPresenter _unitsEditorPresenter;
      protected IOptionsEditorPresenter _errorEditorPresenter;
      protected IErrorTypeColumnEditorPresenter _errorTypeColumnEditorPresenter;

      protected override void Context()
      {
         _unitsEditorPresenter = A.Fake<IUnitsEditorPresenter>();
         _errorEditorPresenter = A.Fake<IOptionsEditorPresenter>();
         _errorTypeColumnEditorPresenter = A.Fake<IErrorTypeColumnEditorPresenter>();
         sut = new MappingParameterEditorPresenter(A.Fake<IMappingParameterEditorView>(), _unitsEditorPresenter, A.Fake<ILloqEditorPresenter>(),
            _errorEditorPresenter, _errorTypeColumnEditorPresenter);
      }

      protected void setErrorType(string errorType, UnitDescription measurementUnit = null)
      {
         measurementUnit = measurementUnit ?? new UnitDescription(MEASUREMENT_UNIT);
         A.CallTo(() => _errorEditorPresenter.SelectedText).Returns(errorType);
         sut.SetErrorTypeOptions(new[] { Constants.STD_DEV_ARITHMETIC, Constants.STD_DEV_GEOMETRIC }, errorType,
            type => type == Constants.STD_DEV_ARITHMETIC ? measurementUnit : null);
      }

      protected void turnOnErrorTypeFromColumn()
      {
         A.CallTo(() => _errorTypeColumnEditorPresenter.ErrorTypeFromColumn()).Returns(true);
         _errorTypeColumnEditorPresenter.OnErrorTypeFromColumnChanged += Raise.WithEmpty();
      }
   }

   public class When_importing_the_error_type_from_a_column_with_a_geometric_default : concern_for_MappingParameterEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         setErrorType(Constants.STD_DEV_GEOMETRIC);
      }

      protected override void Because()
      {
         turnOnErrorTypeFromColumn();
      }

      [Observation]
      public void should_select_the_measurement_unit_for_the_arithmetic_data_sets()
      {
         A.CallTo(() => _unitsEditorPresenter.SelectUnit(MEASUREMENT_UNIT)).MustHaveHappened();
      }
   }

   public class When_importing_the_error_type_from_a_column_with_an_arithmetic_default : concern_for_MappingParameterEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         setErrorType(Constants.STD_DEV_ARITHMETIC);
      }

      protected override void Because()
      {
         turnOnErrorTypeFromColumn();
      }

      [Observation]
      public void should_keep_the_selected_unit()
      {
         A.CallTo(() => _unitsEditorPresenter.SelectUnit(A<string>._)).MustNotHaveHappened();
      }
   }

   public class When_importing_the_error_type_from_a_column_with_a_geometric_default_and_the_measurement_unit_from_a_column : concern_for_MappingParameterEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _unitsEditorPresenter.Unit).Returns(new UnitDescription(MEASUREMENT_UNIT, null));
         setErrorType(Constants.STD_DEV_GEOMETRIC, new UnitDescription(MEASUREMENT_UNIT, MEASUREMENT_UNIT_COLUMN));
      }

      protected override void Because()
      {
         turnOnErrorTypeFromColumn();
      }

      [Observation]
      public void should_use_the_unit_column_of_the_measurement()
      {
         A.CallTo(() => _unitsEditorPresenter.SelectColumn(MEASUREMENT_UNIT_COLUMN)).MustHaveHappened();
         A.CallTo(() => _unitsEditorPresenter.SelectUnit(A<string>._)).MustNotHaveHappened();
      }
   }

   public class When_importing_the_error_type_from_a_column_for_an_error_with_a_unit_column : concern_for_MappingParameterEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _unitsEditorPresenter.Unit).Returns(new UnitDescription(MEASUREMENT_UNIT, "Error unit"));
         setErrorType(Constants.STD_DEV_GEOMETRIC, new UnitDescription(MEASUREMENT_UNIT, MEASUREMENT_UNIT_COLUMN));
      }

      protected override void Because()
      {
         turnOnErrorTypeFromColumn();
      }

      [Observation]
      public void should_keep_the_unit_column_of_the_error()
      {
         A.CallTo(() => _unitsEditorPresenter.SelectColumn(A<string>._)).MustNotHaveHappened();
         A.CallTo(() => _unitsEditorPresenter.SelectUnit(A<string>._)).MustNotHaveHappened();
      }
   }

   public class When_setting_the_error_type_column_options_after_editing_a_geometric_error : concern_for_MappingParameterEditorPresenter
   {
      protected override void Context()
      {
         base.Context();
         setErrorType(Constants.STD_DEV_GEOMETRIC);
         //like the view, setting the toggle raises its changed event
         A.CallTo(() => _errorTypeColumnEditorPresenter.SetOptions(A<IEnumerable<string>>._, true, A<string>._))
            .Invokes(turnOnErrorTypeFromColumn);
      }

      protected override void Because()
      {
         sut.SetErrorTypeColumnOptions(new[] { "Error type" }, "Error type", true);
      }

      [Observation]
      public void should_keep_the_unit_loaded_for_the_mapping()
      {
         A.CallTo(() => _unitsEditorPresenter.SelectUnit(A<string>._)).MustNotHaveHappened();
      }
   }
}
