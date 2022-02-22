using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Importer.Presenters 
{
   public abstract class concern_for_ImporterDataPresenter : ContextSpecification<ImporterDataPresenter>
   {
      protected IImporterDataView _view;
      protected IImporter _importer;
      protected IDataSourceFile _dataSourceFile;
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;
      protected Cache<string, DataSheet> _sheetCache;
      protected DataSheet _dataSheet;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _dataSourceFile = A.Fake<IDataSourceFile>();
         _importer = A.Fake<IImporter>();
         _view = A.Fake<IImporterDataView>();
         _dataSheet = new DataSheet();
         _dataSheet.RawData = new UnformattedData();
         _dataSheet.RawData.AddColumn("test_column", 0);
         _dataSheet.RawData.AddRow(new List<string>(){ "1"});
         _sheetCache = new Cache<string, DataSheet> {{"sheet1", _dataSheet}, {"sheet2", _dataSheet}, {"sheet3", _dataSheet}};
         A.CallTo(() => _importer.LoadFile(A<IReadOnlyList<ColumnInfo>>._, A<string>._, A<IReadOnlyList<MetaDataCategory>>._))
            .Returns(_dataSourceFile);
         A.CallTo(() => _view.GetActiveFilterCriteria()).Returns("active_filter_criteria");
         A.CallTo(() => _dataSourceFile.DataSheets).Returns(_sheetCache);
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

         sut.SetSettings(_metaDataCategories, _columnInfos);
      }
}

   public class When_loading_new_file : concern_for_ImporterDataPresenter
   {
      protected override void Because()
      {
         sut.SetDataSource("test_file");
      }

      [Observation]
      public void tabs_must_have_been_added()
      {
         A.CallTo(() => _view.AddTabs(A<List<string>>._)).MustHaveHappened();
      }

      [Observation]
      public void correct_sheets_should_have_been_added()
      {
         var tabNames = new List<string> { "sheet1", "sheet2", "sheet3" };
         sut.GetSheetNames().ShouldBeEqualTo(tabNames);
      }
   }

   public class When_loading_an_empty_file : concern_for_ImporterDataPresenter
   {
      [Observation]
      public void result_should_be_null()
      {
         sut.SetDataSource(null).ShouldBeEqualTo(null);         
         A.CallTo(() => _view.AddTabs(A<List<string>>._)).MustNotHaveHappened();
      }
   }

   public class When_selecting_a_specific_tab : concern_for_ImporterDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.SetDataSource("test_file");
      }

      protected override void Because()
      {
         sut.SelectTab("sheet1");
      }

      [Observation]
      public void view_should_be_notified()
      {
         A.CallTo(() => _view.SetGridSource("sheet1")).MustHaveHappened();
      }

      [Observation]
      public void filter_should_be_applied()
      {
         A.CallTo(() => _view.SetFilter("active_filter_criteria")).MustHaveHappened();
      }
   }

   public class When_selecting_an_already_deleted_tab : concern_for_ImporterDataPresenter
   {
      private bool _selectTabResult;
      protected override void Context()
      {
         base.Context();
         sut.SetDataSource("test_file");
      }

      protected override void Because()
      {
         _selectTabResult = sut.SelectTab("sheet5");
      }

      [Observation]
      public void no_further_action_should_be_taken()
      {
         _selectTabResult.ShouldBeEqualTo(false);
         A.CallTo(() => _view.SetGridSource("sheet5")).MustNotHaveHappened();
      }
   }

   public class When_hidig_an_already_loaded_tab : concern_for_ImporterDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.SetDataSource("test_file");
         sut.ImportDataForConfirmation("sheet1");
         sut.ImportDataForConfirmation("sheet2");
      }

      protected override void Because()
      {
         sut.RemoveTab("sheet1");
      }

      [Observation]
      public void no_further_action_should_be_taken()
      {
         sut.Sheets.Keys.ShouldNotContain("sheet1");
         sut.Sheets.Keys.ShouldContain("sheet2");
      }
   }


   public class When_dropping_loaded_sheets : concern_for_ImporterDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.SetDataSource("test_file");
         sut.ImportDataForConfirmation("sheet1");
         sut.ImportDataForConfirmation("sheet2");
      }

      protected override void Because()
      {
         sut.ResetLoadedSheets();
      }

      [Observation]
      public void sheets_should_have_been_cleared()
      {
         sut.Sheets.Keys.Count().ShouldBeEqualTo(0);
      }

      [Observation]
      public void import_buttons_should_have_been_reset()
      {
         A.CallTo(() => _view.ResetImportButtons()).MustHaveHappened();
      }
   }

   public class When_hiding_all_already_loaded_tabs : concern_for_ImporterDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.SetDataSource("test_file");
         sut.ImportDataForConfirmation("sheet1");
         sut.ImportDataForConfirmation("sheet2");
         sut.ImportDataForConfirmation("sheet3");
      }

      protected override void Because()
      {
         sut.RemoveAllButThisTab("sheet1");
      }

      [Observation]
      public void no_further_action_should_be_taken()
      {
         sut.Sheets.Keys.ShouldContain("sheet1");
         sut.Sheets.Keys.ShouldNotContain("sheet2");
         sut.Sheets.Keys.ShouldNotContain("sheet3");
      }
   }

   public class When_loading_for_confirmation : concern_for_ImporterDataPresenter
   {
      protected List<string> sheets;
      protected override void Context()
      {
         base.Context();
         sut.SetDataSource("test_file");
         sut.OnImportSheets += (o, a) => sheets = a.Sheets.Keys.ToList();
      }

      protected override void Because()
      {
         sut.ImportDataForConfirmation();
      }

      [Observation]
      public void result_should_be_null()
      {
         sheets.ShouldBeEqualTo(new List<string> { "sheet1", "sheet2", "sheet3" });
      }
   }

   public class When_loading_file_with_invalid_sheets : concern_for_ImporterDataPresenter
   {
      protected string _baseSheet = "baseSheet";

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dataSourceFile.FormatCalculatedFrom).Returns(_baseSheet);
      }

      protected override void Because()
      {
         sut.SetDataSource("test_file");
      }

      [Observation]
      public void first_valid_sheet_is_selected()
      {
         A.CallTo(() => _view.SelectTab(_baseSheet)).MustHaveHappened();
      }
   }
}
