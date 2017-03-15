using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ParameterIdentificationResultsPresenter : ContextSpecification<IParameterIdentificationResultsPresenter>
   {
      protected IMultipleParameterIdentificationResultsPresenter _multipleRunResultPresenter;
      protected IParameterIdentificationResultsView _view;
      protected ISingleParameterIdentificationResultsPresenter _singleRunResultPresenter;
      protected ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         _view = A.Fake<IParameterIdentificationResultsView>();
         _multipleRunResultPresenter = A.Fake<IMultipleParameterIdentificationResultsPresenter>();
         _singleRunResultPresenter = A.Fake<ISingleParameterIdentificationResultsPresenter>();
         sut = new ParameterIdentificationResultsPresenter(_view, _singleRunResultPresenter, _multipleRunResultPresenter);

         _parameterIdentification = A.Fake<ParameterIdentification>();
         sut.EditParameterIdentification(_parameterIdentification);
      }
   }

   public class When_notified_that_the_results_of_the_edited_single_parameter_identification_were_updated : concern_for_ParameterIdentificationResultsPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterIdentification.HasResults).Returns(true);
         A.CallTo(() => _parameterIdentification.IsSingleRun).Returns(true);
      }

      protected override void Because()
      {
         sut.Handle(new ParameterIdentificationResultsUpdatedEvent(_parameterIdentification));
      }

      [Observation]
      public void should_display_the_single_result()
      {
         A.CallTo(() => _view.ShowResultsView(_singleRunResultPresenter.BaseView)).MustHaveHappened();
      }
   }

   public class When_notified_that_the_results_of_the_edited_multi_parameter_identification_were_updated : concern_for_ParameterIdentificationResultsPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterIdentification.HasResults).Returns(true);
         A.CallTo(() => _parameterIdentification.IsSingleRun).Returns(false);
      }

      protected override void Because()
      {
         sut.Handle(new ParameterIdentificationResultsUpdatedEvent(_parameterIdentification));
      }

      [Observation]
      public void should_display_the_multi_result()
      {
         A.CallTo(() => _view.ShowResultsView(_multipleRunResultPresenter.BaseView)).MustHaveHappened();
      }
   }

   public class When_notified_that_the_results_of_the_edited_parameter_identification_were_updated_and_there_are_no_results_available : concern_for_ParameterIdentificationResultsPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _parameterIdentification.HasResults).Returns(false);
      }

      protected override void Because()
      {
         sut.Handle(new ParameterIdentificationResultsUpdatedEvent(_parameterIdentification));
      }

      [Observation]
      public void should_display_the_hide_the_result()
      {
         A.CallTo(() => _view.HideResultsView()).MustHaveHappened();
      }
   }
}