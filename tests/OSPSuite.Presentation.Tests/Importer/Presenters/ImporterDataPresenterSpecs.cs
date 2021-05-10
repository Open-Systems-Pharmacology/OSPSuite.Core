using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Presentation.Importer.Presenters 
{
   public abstract class ConcernForImporterDataPresenter : ContextSpecification<ImporterDataPresenter>
   {
      protected IImporterDataView _view;
      protected IImporter _importer;
      protected IDataSourceFile _dataSourceFile;
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _dataSourceFile = A.Fake<IDataSourceFile>();
         _importer = A.Fake<IImporter>();
         _view = A.Fake<IImporterDataView>();
         A.CallTo(() => _importer.LoadFile(A<IReadOnlyList<ColumnInfo>>._, A<string>._, A<IReadOnlyList<MetaDataCategory>>._)).Returns(_dataSourceFile);
      }

      protected override void Because()
      {
         base.Because();
         sut.SetSettings(_metaDataCategories, _columnInfos);
         sut.SetDataSource("test_file");
      }

      protected override void Context()
      {
         base.Context();

         sut = new ImporterDataPresenter(_view, _importer);
         
         _columnInfos = new List<ColumnInfo>()
         {
            new ColumnInfo() { Name = "Time", IsMandatory = true, BaseGridName = "Time" },
            new ColumnInfo() { Name = "Concentration", IsMandatory = true, BaseGridName = "Time" },
            new ColumnInfo() { Name = "Error", IsMandatory = false, RelatedColumnOf = "Concentration", BaseGridName = "Time" }
         };
         _metaDataCategories = new List<MetaDataCategory>()
         {
            new MetaDataCategory()
            {
               Name = "Time",
               IsMandatory = true,
            },
            new MetaDataCategory()
            {
               Name = "Concentration",
               IsMandatory = true
            },
            new MetaDataCategory()
            {
               DisplayName = "Error",
               IsMandatory = false
            }
         };
      }

      //SetDataSource

      //we should definately test loading specific//all sheets when some of the sheets have been loaded already
   }

   public class When_loading_new_file : ConcernForImporterDataPresenter
   {
      [TestCase]
      public void loading_a_new_file()
      {
         A.CallTo(() => _view.AddTabs(A<List<string>>._)).MustHaveHappened();

      }
   }
}
