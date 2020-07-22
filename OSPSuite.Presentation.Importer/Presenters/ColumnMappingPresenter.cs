using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ColumnMappingPresenter : AbstractPresenter<IColumnMappingControl, IColumnMappingPresenter>, IColumnMappingPresenter
   {
      public ColumnMappingPresenter(IColumnMappingControl view) : base(view) {}

      public void SetDataFormatParameters(IList<DataFormatParameter> parameters)
      {
         View.SetMappingSource(parameters);
      }
   }
}
