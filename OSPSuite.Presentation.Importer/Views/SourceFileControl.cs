using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.UI.Controls;

namespace OSPSuite.Presentation.Importer.Views
{
   public partial class SourceFileControl : BaseUserControl, ISourceFileControl
   {
      private ISourceFilePresenter _presenter;
      public SourceFileControl()
      {
         InitializeComponent();
      }

      public void AttachPresenter(ISourceFilePresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
