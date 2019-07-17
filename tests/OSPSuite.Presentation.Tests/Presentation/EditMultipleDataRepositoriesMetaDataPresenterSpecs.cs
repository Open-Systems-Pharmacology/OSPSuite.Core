using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ObservedData;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_EditMultipleMetaDataRepositoriesMetaDataPresenter : ContextSpecification<EditMultipleDataRepositoriesMetaDataPresenter>
   {
      protected IEditMultipleDataRepositoriesMetaDataView _view;
      protected IDataRepositoryMetaDataPresenter _subPresenter;
      private IOSPSuiteExecutionContext _executionContext;

      protected override void Context()
      {
         _view = A.Fake<IEditMultipleDataRepositoriesMetaDataView>();
         _subPresenter = A.Fake<IDataRepositoryMetaDataPresenter>();
         _executionContext= A.Fake<IOSPSuiteExecutionContext>();
         sut = new EditMultipleDataRepositoriesMetaDataPresenter(_view, _subPresenter, A.Fake<ICommandTask>(), _executionContext);
      }
   }

   public class When_dialog_cancelled : concern_for_EditMultipleMetaDataRepositoriesMetaDataPresenter
   {
      private ICommand _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         _result = sut.Edit(A.Fake<IEnumerable<DataRepository>>());
      }

      [Observation]
      public void an_emptycommand_is_returned()
      {
         _result.IsEmpty().ShouldBeTrue();
      }
   }

   public class When_initializing_without_command_collector : concern_for_EditMultipleMetaDataRepositoriesMetaDataPresenter
   {
      protected override void Because()
      {
         sut.Initialize();
      }

      [Observation]
      public void a_call_to_subpresenter_initializewith_should_result()
      {
         A.CallTo(() => _subPresenter.InitializeWith(A<ICommandCollector>.Ignored)).MustHaveHappened();
      }
   }

   public class When_initializing_with_command_collector : concern_for_EditMultipleMetaDataRepositoriesMetaDataPresenter
   {
      private ICommandCollector _commandCollector;

      protected override void Because()
      {
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
      }

      [Observation]
      public void a_call_to_subpresenter_initialize_should_result()
      {
         A.CallTo(() => _subPresenter.InitializeWith(sut)).MustHaveHappened();
      }
   }

   public class When_editing_data_repository_metadata : concern_for_EditMultipleMetaDataRepositoriesMetaDataPresenter
   {
      protected override void Because()
      {
         sut.Edit(new List<DataRepository>());
      }

      [Observation]
      public void should_result_in_call_to_set_view()
      {
         A.CallTo(() => _view.SetDataEditor(A<IView>.Ignored)).MustHaveHappened();
      }

      [Observation]
      public void should_result_in_call_to_set_repository_in_subpresenter()
      {
         A.CallTo(() => _subPresenter.EditObservedData(A<IEnumerable<DataRepository>>.Ignored)).MustHaveHappened();
      }
   }
}