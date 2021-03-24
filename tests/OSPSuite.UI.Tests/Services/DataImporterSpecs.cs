using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.UI.Services
{
   public abstract class concern_for_DataImporter : ContextSpecification<IDataImporter>
   {
      protected IDialogCreator _dialogCreator;
      protected IImporter _importer;
      protected IApplicationController _applicationController;
      protected IDataSetToDataRepositoryMapper _dataRepositoryMapper;
      protected IImporterReloadPresenter _reloadPresenter;
      protected IReadOnlyList<DataRepository> _existingDataSets;
      protected IReadOnlyList<DataRepository> _dataSetsToImport;



      public override void GlobalContext()
      {
         base.GlobalContext();
         _dialogCreator = A.Fake<IDialogCreator>();
         _importer = A.Fake<IImporter>();
         _applicationController = A.Fake<IApplicationController>();
         _dataRepositoryMapper = A.Fake<IDataSetToDataRepositoryMapper>();
         _reloadPresenter = A.Fake<IImporterReloadPresenter>();

         A.CallTo(() => _applicationController.Start<IImporterReloadPresenter>()).Returns(_reloadPresenter);
         A.CallTo(() => _reloadPresenter.Canceled()).Returns(false);

      }

      protected override void Context()
      {
         _existingDataSets = new List<DataRepository>()
         {
            new DataRepository()
            {
               Name = "repo1",
               ExtendedProperties =
               {
                  new ExtendedProperty<string>() {Name = "Sheet", Value = "Sheet1"},
                  new ExtendedProperty<string>() {Name = "Organ", Value = "Organ1"},
                  new ExtendedProperty<string>() {Name = "Patient", Value = "Patient1"},
               }
            },
            new DataRepository()
            {
               Name = "repo2",
               ExtendedProperties =
               {
                  new ExtendedProperty<string>() {Name = "Sheet", Value = "Sheet2"},
                  new ExtendedProperty<string>() {Name = "Organ", Value = "Organ2"},
                  new ExtendedProperty<string>() {Name = "Patient", Value = "Patient2"},
               }
            }
         };
         _dataSetsToImport = new List<DataRepository>()
         {
            new DataRepository()
            {
               Name = "repo1",
               ExtendedProperties =
               {
                  new ExtendedProperty<string>() {Name = "Sheet", Value = "Sheet1"},
                  new ExtendedProperty<string>() {Name = "Organ", Value = "Organ1"},
                  new ExtendedProperty<string>() {Name = "Patient", Value = "Patient1"},
               }
            },
            new DataRepository()
            {
               Name = "repo2",
               ExtendedProperties =
               {
                  new ExtendedProperty<string>() {Name = "Sheet", Value = "Sheet2"},
                  new ExtendedProperty<string>() {Name = "Organ", Value = "Organ2"},
                  new ExtendedProperty<string>() {Name = "Patient", Value = "Patient2"},
               }
            }
         };
         sut = new DataImporter(_dialogCreator, _importer , _applicationController, _dataRepositoryMapper);
      }
   }

   
   public class When_reloading_only_existing_data_sets : concern_for_DataImporter
   {
      private ReloadDataSets _result;

      protected override void Context()
      {
         base.Context();


      }

      protected override void Because()
      {
         _result = sut.CalculateReloadDataSetsFromConfiguration(_dataSetsToImport, _existingDataSets);
      }

      [Test]
      public void should_return_a_both_data_sets_to_overwrite()
      {
         _result.OverwrittenDataSets.Count().ShouldBeEqualTo(2);
         _result.OverwrittenDataSets.Any(x => x.Name == "repo1").ShouldBeTrue();
         _result.OverwrittenDataSets.Any(x => x.Name == "repo2").ShouldBeTrue();
      }

      [Test]
      public void should_return_empty_new_data_sets()
      {
         _result.NewDataSets.Count().ShouldBeEqualTo(0);
      }

      [Test]
      public void should_return_empty_deleted_data_sets()
      {
         _result.DataSetsToBeDeleted.Count().ShouldBeEqualTo(0);
      }
   }

   public class When_reloading_one_new_data_set_one_data_set_to_be_deleted : concern_for_DataImporter
   {
      private ReloadDataSets _result;

      protected override void Context()
      {
         base.Context();
         _dataSetsToImport.FirstOrDefault(x => x.Name == "repo2").ExtendedProperties["Organ"].ValueAsObject = "Organ3";
         _dataSetsToImport.FirstOrDefault(x => x.Name == "repo2").Name = "repo3";

      }

      protected override void Because()
      {
         _result = sut.CalculateReloadDataSetsFromConfiguration(_dataSetsToImport, _existingDataSets);
      }

      [Test]
      public void should_return_one_data_set_to_overwrite()
      {
         _result.OverwrittenDataSets.Count().ShouldBeEqualTo(1);
         _result.OverwrittenDataSets.Any(x => x.Name == "repo1").ShouldBeTrue();
      }

      [Test]
      public void should_return_one_new_data_set()
      {
         _result.NewDataSets.Count().ShouldBeEqualTo(1);
         _result.NewDataSets.Any(x => x.Name == "repo3").ShouldBeTrue();
      }

      [Test]
      public void should_return_one_deleted_data_sets()
      {
         _result.DataSetsToBeDeleted.Count().ShouldBeEqualTo(1);
         _result.DataSetsToBeDeleted.Any(x => x.Name == "repo2").ShouldBeTrue();
      }
   }
}