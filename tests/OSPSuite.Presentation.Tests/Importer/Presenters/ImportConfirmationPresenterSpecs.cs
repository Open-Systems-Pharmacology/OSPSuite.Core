using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using System.Collections.Generic;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Presenters.ObservedData;

namespace OSPSuite.Presentation.Importer.Presenters 
{
   public abstract class concern_for_ImportConfirmationPresenter : ContextSpecification<ImportPreviewPresenter>
   {
      protected IImportPreviewView _view;
      protected IDataRepositoryChartPresenter _dataRepositoryChartPresenter;
      protected IDataRepositoryDataPresenter _dataRepositoryDataPresenter;
      public override void GlobalContext()
      {
         base.GlobalContext();

         _view = A.Fake<IImportPreviewView>();
         _dataRepositoryChartPresenter = A.Fake<IDataRepositoryChartPresenter>();
         _dataRepositoryDataPresenter = A.Fake<IDataRepositoryDataPresenter>();
      }

      protected override void Context()
      {
         base.Context();

         sut = new ImportPreviewPresenter(_view, _dataRepositoryChartPresenter, _dataRepositoryDataPresenter);
      }
   }

   public class When_setting_naming_conventions : concern_for_ImportConfirmationPresenter
   {
      [Observation]
      public void null_naming_conventions_throws_exception()
      {
         Assert.Throws<NullNamingConventionsException>(()=> sut.SetNamingConventions(null, null));
      }

      [Observation]
      public void empty_naming_conventions_throws_exception()
      {
         Assert.Throws<EmptyNamingConventionsException>(() => sut.SetNamingConventions(new List<string>(), null));
      }
   }

   public class When_setting_error_state : concern_for_ImportConfirmationPresenter
   {
      protected override void Because()
      {
         sut.SetViewingStateToError("test error");
      }
      [Observation]
      public void error_should_be_correctly_set()
      {
         A.CallTo(() => _view.SetErrorMessage("test error")).MustHaveHappened();
      }

      [Observation]
      public void selecting_datasets_should_be_disabled()
      {
         _view.SelectingDataSetsEnabled.ShouldBeFalse();
      }
   }

   public class When_setting_error_free_state : concern_for_ImportConfirmationPresenter
   {
      protected override void Because()
      {
         sut.SetViewingStateToNormal();
      }

      [Observation]
      public void selecting_datasets_should_be_enabled()
      {
         _view.SelectingDataSetsEnabled.ShouldBeTrue();
      }
   }
}
