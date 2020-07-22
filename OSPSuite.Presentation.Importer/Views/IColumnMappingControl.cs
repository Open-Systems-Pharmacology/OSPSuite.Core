using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Views;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Views
{
   public interface IColumnMappingControl : IView<IColumnMappingPresenter>
   {
      void SetMappingSource(IEnumerable<DataFormatParameter> mappings);
   }
}
