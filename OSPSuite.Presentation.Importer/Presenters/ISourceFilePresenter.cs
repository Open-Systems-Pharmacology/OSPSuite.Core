using System;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
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