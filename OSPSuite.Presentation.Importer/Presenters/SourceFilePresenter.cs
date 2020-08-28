using OSPSuite.Presentation.Importer.Services;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class SourceFilePresenter : AbstractPresenter<ISourceFileControl, ISourceFilePresenter>, ISourceFilePresenter
   {
      private readonly IImporter _importer;

      public SourceFilePresenter(IImporter importer, ISourceFileControl view) : base(view)
      {
         _importer = importer;
      }

      public void OpenFileDialog()
      {
         OnSourceFileChanged?.Invoke(this, new SourceFileChangedEventArgs() { FileName = _importer.LoadFile()});
      }

      public event SourceFileChangedHandler OnSourceFileChanged;
      public void SetFilePath(string filePath)
      {
         View.SetFilePath(filePath);
      }
   }
}