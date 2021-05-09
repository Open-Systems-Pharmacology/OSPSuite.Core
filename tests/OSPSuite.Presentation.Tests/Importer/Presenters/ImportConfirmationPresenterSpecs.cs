using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Presenters.ObservedData;

namespace OSPSuite.Presentation.Importer.Presenters 
{
   public abstract class ConcernForImportConfirmationPresenter : ContextSpecification<ImportConfirmationPresenter>
   {
      protected IImportConfirmationView _view;
      protected IDataRepositoryChartPresenter _dataRepositoryChartPresenter;
      protected IDataRepositoryDataPresenter _dataRepositoryDataPresenter;
      public override void GlobalContext()
      {
         base.GlobalContext();

         _view = A.Fake<IImportConfirmationView>();
         _dataRepositoryChartPresenter = A.Fake<IDataRepositoryChartPresenter>();
         _dataRepositoryDataPresenter = A.Fake<IDataRepositoryDataPresenter>();
      }

      protected override void Context()
      {
         base.Context();

         sut = new ImportConfirmationPresenter(_view, _dataRepositoryChartPresenter, _dataRepositoryDataPresenter);
      }
   }

   public class When_setting_naming_conventions : ConcernForImportConfirmationPresenter
   {
      [TestCase]
      public void null_naming_conventions_throws_exception()
      {
         Assert.Throws<NullNamingConventionsException>(()=> sut.SetNamingConventions(null, null));
      }

      [TestCase]
      public void empty_naming_conventions_throws_exception()
      {
         Assert.Throws<EmptyNamingConventionsException>(() => sut.SetNamingConventions(new List<string>(), null));
      }
   }
}
