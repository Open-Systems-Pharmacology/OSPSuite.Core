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
         openSourceFileButton.Click += (sender, args) => _presenter.OpenFileDialog(sourceFileTextEdit.Text);
      }

      public void AttachPresenter(ISourceFilePresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetFilePath(string filePath)
      {
         sourceFileTextEdit.Text = filePath;
      }
   }
}
