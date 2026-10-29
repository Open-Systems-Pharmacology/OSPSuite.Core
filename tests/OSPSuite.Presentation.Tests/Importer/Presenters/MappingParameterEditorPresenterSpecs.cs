using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public abstract class concern_for_MappingParameterEditorPresenter : ContextSpecification<MappingParameterEditorPresenter>
   {
      protected const string MEASUREMENT_UNIT = "mg/l";
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

      protected void setErrorType(string errorType)
      {
         A.CallTo(() => _errorEditorPresenter.SelectedText).Returns(errorType);
         sut.SetErrorTypeOptions(new[] { Constants.STD_DEV_ARITHMETIC, Constants.STD_DEV_GEOMETRIC }, errorType,
            type => type == Constants.STD_DEV_ARITHMETIC ? MEASUREMENT_UNIT : null);
      }

      protected override void Because()
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

      [Observation]
      public void should_keep_the_selected_unit()
      {
         A.CallTo(() => _unitsEditorPresenter.SelectUnit(A<string>._)).MustNotHaveHappened();
      }
   }
}
