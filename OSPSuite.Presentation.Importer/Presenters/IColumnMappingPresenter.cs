using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IColumnMappingPresenter : IPresenter<IColumnMappingControl>
   {
      void SetDataFormatParameters(IList<DataFormatParameter> parameters);
   }
}
