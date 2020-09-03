using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Core.Services;


namespace OSPSuite.Presentation.Importer.Presenters
{
   public class SourceFilePresenter : AbstractPresenter<ISourceFileControl, ISourceFilePresenter>, ISourceFilePresenter
   {
      private readonly IDialogCreator _dialogCreator;
      
      public string Title  { get; set;} 
      public string Filter { get; set; }
      public string DirectoryKey { get; set; }


      public SourceFilePresenter(IDialogCreator dialogCreator, ISourceFileControl view) : base(view)
      {
         _dialogCreator = dialogCreator;
         Title = string.Empty;
         Filter = string.Empty;
         DirectoryKey = string.Empty;
      }

      public void OpenFileDialog( string initFileName)
      {
         //consider whether this should be part of the code
         //var initDirectoryName = Path.GetDirectoryName(initFileName);
         OnSourceFileChanged.Invoke(this, new SourceFileChangedEventArgs() { FileName = _dialogCreator.AskForFileToOpen(Title, Filter, DirectoryKey, initFileName) });
      }

      public event SourceFileChangedHandler OnSourceFileChanged =  delegate {};
      public void SetFilePath(string filePath)
      {
         View.SetFilePath(filePath);
      }

   }
}