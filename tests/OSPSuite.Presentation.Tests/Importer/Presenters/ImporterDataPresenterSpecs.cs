using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public abstract class concern_for_ImporterDataPresenter : ContextSpecification<ImporterDataPresenter>
   {
      protected IImporterDataView _view;
      protected IImporter _importer;
      protected IDataSourceFile _dataSourceFile;
      protected ColumnInfoCache _columnInfos;
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;
      protected DataSheet _dataSheet;
      protected DataSheet _dataSheet2;
      protected DataSheet _dataSheet3;
      protected DataSheetCollection _sheetsCollection;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _dataSourceFile = A.Fake<IDataSourceFile>();
         _importer = A.Fake<IImporter>();
         _view = A.Fake<IImporterDataView>();
         _dataSheet = new DataSheet { SheetName = "sheet1" };
         _dataSheet.AddColumn("test_column", 0);
         _dataSheet.AddRow(new List<string>() { "1" });
         _dataSheet2 = new DataSheet { SheetName = "sheet2" };
         _dataSheet2.AddColumn("test_column", 0);
         _dataSheet2.AddRow(new List<string>() { "1" });
         _dataSheet3 = new DataSheet { SheetName = "sheet3" };
         _dataSheet3.AddColumn("test_column", 0);
         _dataSheet3.AddRow(new List<string>() { "1" });
         _sheetsCollection = new DataSheetCollection();
         _sheetsCollection.AddSheet(_dataSheet);
         _sheetsCollection.AddSheet(_dataSheet2);
         _sheetsCollection.AddSheet(_dataSheet3);
         A.CallTo(() => _importer.LoadFile(A<ColumnInfoCache>._, A<string>._, A<IReadOnlyList<MetaDataCategory>>._, CancellationToken.None)).Returns(_dataSourceFile);
         A.CallTo(() => _view.GetActiveFilterCriteria()).Returns("active_filter_criteria");
         A.CallTo(() => _dataSourceFile.DataSheets).Returns(_sheetsCollection);
      }

      protected override void Context()
      {
         base.Context();

         sut = new ImporterDataPresenter(_view, _importer);

         _columnInfos = new ColumnInfoCache
         {
            new ColumnInfo() { Name = "Time", IsMandatory = true, BaseGridName = "Time" },
            new ColumnInfo() { Name = "Concentration", IsMandatory = true, BaseGridName = "Time" },
            new ColumnInfo() { Name = "Error", IsMandatory = false, RelatedColumnOf = "Concentration", BaseGridName = "Time" }
         };
         _metaDataCategories = new List<MetaDataCategory>()
         {
            new MetaDataCategory() { Name = "Time", IsMandatory = true, },
            new MetaDataCategory() { Name = "Concentration", IsMandatory = true },
            new MetaDataCategory() { DisplayName = "Error", IsMandatory = false }
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
         sut.ImportedSheets.GetDataSheetNames().ShouldNotContain("sheet1");
         sut.ImportedSheets.GetDataSheetNames().ShouldContain("sheet2");
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
         sut.ImportedSheets.Count().ShouldBeEqualTo(0);
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
         sut.ImportedSheets.GetDataSheetNames().ShouldContain("sheet1");
         sut.ImportedSheets.GetDataSheetNames().ShouldNotContain("sheet2");
         sut.ImportedSheets.GetDataSheetNames().ShouldNotContain("sheet3");
      }
   }

   public class When_loading_for_confirmation : concern_for_ImporterDataPresenter
   {
      protected List<string> sheets;

      protected override void Context()
      {
         base.Context();
         sut.SetDataSource("test_file");
         sut.OnImportSheets += (o, a) => sheets = (List<string>)a.SheetNames;
      }

      protected override void Because()
      {
         sut.ImportDataForConfirmation();
      }

      [Observation]
      public void result_should_be_sheet_names()
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

   public class When_calculating_format_based_on_invalid_sheet : concern_for_ImporterDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.SetDataSource("test_file");
         A.CallTo(() => _importer.CalculateFormat(A<IDataSourceFile>.Ignored, A<ColumnInfoCache>.Ignored, A<IReadOnlyList<MetaDataCategory>>.Ignored,
               A<string>.Ignored))
            .Returns(new List<IDataFormat>());
      }

      [Observation]
      public void throws_exception()
      {
         The.Action(() => sut.GetFormatBasedOnCurrentSheet()).ShouldThrowAn<UnsupportedFormatException>();
      }
   }

   public class When_resetting_format_based_on_current_sheet : concern_for_ImporterDataPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dataSourceFile.FormatCalculatedFrom).Returns("baseSheet");
         sut.SetDataSource("test_file");
         A.CallTo(() => _importer.CalculateFormat(A<IDataSourceFile>.Ignored, A<ColumnInfoCache>.Ignored, A<IReadOnlyList<MetaDataCategory>>.Ignored,
               A<string>.Ignored))
            .Returns(new List<IDataFormat> { A.Fake<IDataFormat>() });
      }

      protected override void Because()
      {
         sut.GetFormatBasedOnCurrentSheet();
      }

      [Observation]
      public void tab_marks_are_cleared()
      {
         A.CallTo(() => _view.SetTabMarks(A<Cache<string, TabMarkInfo>>.Ignored)).MustHaveHappened();
      }
   }
}