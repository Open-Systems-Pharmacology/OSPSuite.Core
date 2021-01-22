using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Importer
{
   public partial class SourceFileControl : BaseUserControl, ISourceFileControl
   {
      private ISourceFilePresenter _presenter;

      public SourceFileControl()
      {
         InitializeComponent();
         OpenSourceFileLayoutControlItem.Text = Captions.Importer.File;
         openSourceFileButtonEdit.Click += (sender, args) => OnEvent(() => _presenter.OpenFileDialog(openSourceFileButtonEdit.Text));
      }

      public void AttachPresenter(ISourceFilePresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetFilePath(string filePath)
      {
         openSourceFileButtonEdit.Text = filePath;
      }
   }
}
