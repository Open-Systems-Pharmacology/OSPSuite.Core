using System.Data;
using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Importer.Presenters
{
   public abstract class concern_for_ExcelPreviewPresenter : ContextSpecification<ExcelPreviewPresenter>
   {
      protected IExcelPreviewView _view;
      protected override void Context()
      {
         base.Context();
         _view = A.Fake<IExcelPreviewView>();
         sut = new ExcelPreviewPresenter(_view);
      }
   }

   public class when_selecting_range : concern_for_ExcelPreviewPresenter
   {
      private DataTable _dataTable;
      protected override void Because()
      {
         _dataTable = new DataTable();
         sut.SelectRange(_dataTable);
      }

      [Observation]
      public void view_must_display()
      {
         A.CallTo(() => _view.Display()).MustHaveHappened();
      }

      [Observation]
      public void data_table_must_be_bound()
      {
         A.CallTo(() => _view.BindTo(_dataTable)).MustHaveHappened();
      }
   }
}
