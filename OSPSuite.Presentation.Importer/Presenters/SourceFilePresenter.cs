using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Services;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class SourceFilePresenter : AbstractPresenter<ISourceFileControl, ISourceFilePresenter>, ISourceFilePresenter
   {
      private readonly IImporter _importer;
      private IReadOnlyList<ColumnInfo> _columnInfos;

      public SourceFilePresenter(IImporter importer, ISourceFileControl view) : base(view)
      {
         _importer = importer;
      }

      public void OpenFileDialog()
      {
         OnSourceFileChanged?.Invoke(this, new SourceFileChangedEventArgs() { FileName = _importer.LoadFile(_columnInfos)});
      }

      public event SourceFileChangedHandler OnSourceFileChanged;
      public void SetFilePath(string filePath)
      {
         View.SetFilePath(filePath);
      }

      public void SetColumnInfos(IReadOnlyList<ColumnInfo> columnInfos)
      {
         _columnInfos = columnInfos;
      }
   }
}