using System;
using System.IO;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class SourceFilePresenter : AbstractPresenter<ISourceFileControl, ISourceFilePresenter>, ISourceFilePresenter
   {
      private readonly IDialogCreator _dialogCreator;
      public string Title  { get; set;} 
      public string Filter { get; set; }
      public string DirectoryKey { get; set; }
      public void SetSourceFile(string path)
      {
         throw new NotImplementedException();
      }

      public Func<bool> CheckBeforeSelectFile { get; set; } = () => true;

      public SourceFilePresenter(IDialogCreator dialogCreator, ISourceFileControl view) : base(view)
      {
         _dialogCreator = dialogCreator;
         Title = string.Empty;
         Filter = string.Empty;
         DirectoryKey = string.Empty;
      }

      public void OpenFileDialog( string initFileName)
      {
         if (!CheckBeforeSelectFile()) return;

         string initDirectoryName = null;
         if (!initFileName.IsNullOrEmpty()) 
            initDirectoryName = Path.GetDirectoryName(initFileName);
         OnSourceFileChanged.Invoke(this, new SourceFileChangedEventArgs() { FileName = _dialogCreator.AskForFileToOpen(Title, Filter, DirectoryKey, null, initDirectoryName) });
      }

      public event EventHandler<SourceFileChangedEventArgs> OnSourceFileChanged =  delegate {};
      public void SetFilePath(string filePath)
      {
         View.SetFilePath(filePath);
      }

   }
}