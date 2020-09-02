using OSPSuite.Core.Importer;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;
using System.IO;
using OSPSuite.Assets;
using OSPSuite.Core.Services;


namespace OSPSuite.Presentation.Importer.Presenters
{
   public class SourceFilePresenter : AbstractPresenter<ISourceFileControl, ISourceFilePresenter>, ISourceFilePresenter
   {
      private readonly IDialogCreator _dialogCreator;


      public SourceFilePresenter(IDialogCreator dialogCreator, ISourceFileControl view) : base(view)
      {
         _dialogCreator = dialogCreator;
      }

      public void OpenFileDialog( string initFileName)
      {
         //var initDirectoryName = Path.GetDirectoryName(initFileName);

         //the constants here should come as parameters if we are to use this dialog for other things too
         OnSourceFileChanged.Invoke(this, new SourceFileChangedEventArgs() { FileName = _dialogCreator.AskForFileToOpen(Captions.Importer.PleaseSelectDataFile, Captions.Importer.ImportFileFilter, Constants.DirectoryKey.OBSERVED_DATA, initFileName) });
      }

      public event SourceFileChangedHandler OnSourceFileChanged =  delegate {};
      public void SetFilePath(string filePath)
      {
         View.SetFilePath(filePath);
      }

   }
}