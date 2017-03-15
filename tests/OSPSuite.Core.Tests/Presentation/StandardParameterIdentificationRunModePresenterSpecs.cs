using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_StandardParameterIdentificationRunModePresenter : ContextSpecification<StandardParameterIdentificationRunModePresenter>
   {
      private IStandardParameterIdentificationRunModeView _view;

      protected override void Context()
      {
         _view = A.Fake<IStandardParameterIdentificationRunModeView>();
         sut = new StandardParameterIdentificationRunModePresenter(_view);
      }
   }

   public class When_asking_for_the_None_options_and_an_option_has_not_been_edited : concern_for_StandardParameterIdentificationRunModePresenter
   {
      private ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification = new ParameterIdentification();
         _parameterIdentification.Configuration.RunMode = new StandardParameterIdentificationRunMode();
      }

      [Observation]
      public void should_return_a_new_instance()
      {
         sut.RunMode.ShouldBeAnInstanceOf<StandardParameterIdentificationRunMode>();
      }

      [Observation]
      public void should_be_able_to_edit_a_none_optimization()
      {
         sut.CanEdit(_parameterIdentification).ShouldBeTrue();
      }
   }

   public class When_editing_new_None_options : concern_for_StandardParameterIdentificationRunModePresenter
   {
      private ParameterIdentificationRunMode _newOptions;
      private ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         base.Context();
         _newOptions = A.Fake<StandardParameterIdentificationRunMode>();
         _parameterIdentification = new ParameterIdentification();
         _parameterIdentification.Configuration.RunMode = _newOptions;
      }

      protected override void Because()
      {
         sut.Edit(_parameterIdentification);
      }

      [Observation]
      public void the_options_should_be_updated_to_use_the_edited()
      {
         sut.RunMode.ShouldBeEqualTo(_newOptions);
      }
   }
}