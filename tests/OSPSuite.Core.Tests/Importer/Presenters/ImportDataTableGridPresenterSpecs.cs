using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Services.Importer;
using OSPSuite.Presentation.Views.Importer;
using Constants = OSPSuite.Core.Domain.Constants;

namespace OSPSuite.Importer.Presenters
{
   public abstract class concern_for_ImportDataTableGridPresenter : ContextSpecification<ImportDataTableGridPresenter>
   {
      protected IImportDataTableGridView _importDataTableGridView;
      protected ImportDataTable _table;
      private IImporterTask _importerTask;

      protected override void Context()
      {
         _importerTask = new ImporterTask(new ColumnCaptionHelper(), new LowerLimitOfQuantificationTask());
         _table = new ImportDataTable();
         _importDataTableGridView = A.Fake<IImportDataTableGridView>();
         sut = new ImportDataTableGridPresenter(_importDataTableGridView, _importerTask);
      }
   }

   public class When_checking_row_color_that_does_not_have_a_lower_limit_of_quantification : concern_for_ImportDataTableGridPresenter
   {
      protected override void Context()
      {
         base.Context();
         _table.Columns.Add(new ImportDataColumn());
         var dataColumn = new ImportDataColumn();
         _table.Columns.Add(dataColumn);

         _table.LoadDataRow(new object[] { 1, 1 }, fAcceptChanges: true);
         sut.Edit(_table);
      }

      [Observation]
      public void the_return_value_should_indicate_that_the_row_is_not_below()
      {
         sut.GetBackgroundColorForRow(0).ShouldBeEqualTo(Colors.DefaultRowColor);
      }
   }

   public class When_checking_row_color_where_a_lower_limit_is_specified_but_the_data_is_not_a_number : concern_for_ImportDataTableGridPresenter
   {
      protected override void Context()
      {
         base.Context();
         _table.Columns.Add(new ImportDataColumn());
         var dataColumn = new ImportDataColumn();
         _table.Columns.Add(dataColumn);

         dataColumn.ExtendedProperties.Add(Constants.LLOQ, 10);
         _table.LoadDataRow(new object[] { 1, "q" }, fAcceptChanges: true);

         sut.Edit(_table);
      }

      [Observation]
      public void the_return_value_should_indicate_that_the_row_is_not_below()
      {
         sut.GetBackgroundColorForRow(0).ShouldBeEqualTo(Colors.DefaultRowColor);
      }
   }

   public class When_checking_row_color_that_is_not_below_the_lower_limit_of_quantification : concern_for_ImportDataTableGridPresenter
   {

      protected override void Context()
      {
         base.Context();
         _table.Columns.Add(new ImportDataColumn());
         var dataColumn = new ImportDataColumn();
         _table.Columns.Add(dataColumn);

         dataColumn.ExtendedProperties.Add(Constants.LLOQ, 10);
         _table.LoadDataRow(new object[] { 1, 10 }, fAcceptChanges: true);

         sut.Edit(_table);
      }

      [Observation]
      public void the_return_value_should_indicate_that_the_row_is_not_below()
      {
         sut.GetBackgroundColorForRow(0).ShouldBeEqualTo(Colors.DefaultRowColor);
      }
   }

   public class When_checking_row_color_that_is_below_the_lower_limit_of_quantification : concern_for_ImportDataTableGridPresenter
   {

      protected override void Context()
      {
         base.Context();
         _table.Columns.Add(new ImportDataColumn());
         var dataColumn = new ImportDataColumn();
         _table.Columns.Add(dataColumn);

         dataColumn.ExtendedProperties.Add(Constants.LLOQ, 10);
         _table.LoadDataRow(new object[] {1, 1}, fAcceptChanges:true);

         sut.Edit(_table);
      }

      [Observation]
      public void the_return_value_should_indicate_that_the_row_is_below()
      {
         sut.GetBackgroundColorForRow(0).ShouldBeEqualTo(Colors.BelowLLOQ);
      }
   }
}
