using System;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface ISourceFilePresenter : IPresenter<ISourceFileControl>
   {
      void OpenFileDialog(string initFileName);

      event EventHandler<SourceFileChangedEventArgs> OnSourceFileChanged;
      void SetFilePath(string filePath);
      string Title { get; set; }
      string Filter { get; set; }
      string DirectoryKey { get; set; }
   }

   public class SourceFileChangedEventArgs : EventArgs
   {
      /// <summary>
      ///    FileName describing what is missed.
      /// </summary>
      public string FileName { get; set; }
   }
}