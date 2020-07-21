using System;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Importer.Presenters;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class ColumnMappingControl : BaseUserControl, IColumnMappingControl
   {
      public ColumnMappingControl()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IColumnMappingPresenter presenter)
      {
         //throw new NotImplementedException();
      }
   }
}
