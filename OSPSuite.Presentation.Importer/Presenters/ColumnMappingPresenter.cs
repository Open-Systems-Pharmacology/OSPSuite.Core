using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ColumnMappingPresenter : AbstractPresenter<IColumnMappingControl, IColumnMappingPresenter>, IColumnMappingPresenter
   {
      public ColumnMappingPresenter(IColumnMappingControl view) : base(view)
      {

      }
   }
}
