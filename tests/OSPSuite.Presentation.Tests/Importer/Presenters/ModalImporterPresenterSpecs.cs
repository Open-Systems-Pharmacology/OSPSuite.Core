using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public abstract class concern_for_ModalImporterPresenter : ContextSpecification<IModalImporterPresenter>
   {
      protected IModalImporterView _view;
      protected IImporterPresenter _presenter;
      protected IDialogCreator _dialogCreator;

      protected override void Context()
      {
         _view = A.Fake<IModalImporterView>();
         _presenter = A.Fake<IImporterPresenter>();
         _dialogCreator = A.Fake<IDialogCreator>();
         sut = new ModalImporterPresenter(_view, _presenter, _dialogCreator);
      }
   }

   public class When_starting_the_modal_presenter_and_the_user_cancels_the_action : concern_for_ModalImporterPresenter
   {
      private IReadOnlyList<DataRepository> _result;

      protected override void Because()
      {
         (_result, _) = sut.ImportDataSets(null, null, null, "some file");
      }

      [Observation]
      public void should_return_an_empty_list_of_imported_results()
      {
         _result.ShouldNotBeNull();
         _result.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_starting_the_modal_presenter_and_the_user_imports : concern_for_ModalImporterPresenter
   {
      private IReadOnlyList<DataRepository> _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _presenter.SetSourceFile(A<string>.Ignored)).Returns(true);
      }

      protected override void Because()
      {
         (_result, _) = sut.ImportDataSets(null, null, null, "some file");
         _presenter.OnTriggerImport += Raise.With(null, new ImportTriggeredEventArgs());
      }

      [Observation]
      public void should_not_prompt_user_for_close()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(Captions.ReallyCancel, ViewResult.Yes)).MustNotHaveHappened();
      }
   }
}