using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public abstract class concern_for_ModalImporterPresenter : ContextSpecification<IModalImporterPresenter>
   {
      protected IModalImporterView _view;
      protected IImporterPresenter _presenter;

      protected override void Context()
      {
         _view = A.Fake<IModalImporterView>();
         _presenter = A.Fake<IImporterPresenter>();
         sut = new ModalImporterPresenter(_view);
      }
   }

   public class When_starting_the_modal_presenter_and_the_user_cancels_the_action : concern_for_ModalImporterPresenter
   {
      private IReadOnlyList<DataRepository> _result;

      protected override void Because()
      {
         (_result, _) = sut.ImportDataSets(_presenter);
      }

      [Observation]
      public void should_return_an_empty_list_of_imported_results()
      {
         _result.ShouldNotBeNull();
         _result.Count.ShouldBeEqualTo(0);
      }
   }
}