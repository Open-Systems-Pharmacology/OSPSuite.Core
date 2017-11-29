using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using OSPSuite.Core;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Mappers;
using OSPSuite.Utility;

namespace OSPSuite.UI.Services
{
   public class DialogCreator : IDialogCreator
   {
      private readonly IDialogResultToViewResultMapper _mapper;
      private readonly DirectoryMapSettings _directoryMapSettings;
      private readonly IApplicationConfiguration _applicationConfiguration;
      private readonly ViewResult _defaultButton;

      public DialogCreator(IDialogResultToViewResultMapper mapper, DirectoryMapSettings directoryMapSettings, IApplicationConfiguration applicationConfiguration)
      {
         _mapper = mapper;
         _directoryMapSettings = directoryMapSettings;
         _applicationConfiguration = applicationConfiguration;
         _defaultButton = ViewResult.Yes;
      }

      public void MessageBoxError(string message)
      {
         XtraMessageBox.Show(message, _applicationConfiguration.ProductNameWithTrademark, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      public ViewResult MessageBoxYesNoCancel(string message)
      {
         return MessageBoxYesNoCancel(message, _defaultButton);
      }

      public ViewResult MessageBoxYesNoCancel(string message, ViewResult defaultButton)
      {
         return MessageBoxYesNoCancel(message, string.Empty, string.Empty, string.Empty, defaultButton);
      }

      public ViewResult MessageBoxYesNoCancel(string message, string yes, string no, string cancel)
      {
         return MessageBoxYesNoCancel(message, yes, no, cancel, _defaultButton);
      }

      public ViewResult MessageBoxYesNoCancel(string message, string yes, string no, string cancel, ViewResult defaultButton)
      {
         return showQuestionMessageBox(message, MessageBoxButtons.YesNoCancel, yes, no, cancel, defaultButton);
      }

      public ViewResult MessageBoxYesNo(string message)
      {
         return MessageBoxYesNo(message, _defaultButton);
      }

      public ViewResult MessageBoxYesNo(string message, ViewResult defaultButton)
      {
         return MessageBoxYesNo(message, string.Empty, string.Empty, defaultButton);
      }

      public ViewResult MessageBoxYesNo(string message, string yes, string no)
      {
         return showQuestionMessageBox(message, MessageBoxButtons.YesNo, yes, no, string.Empty, _defaultButton);
      }

      public ViewResult MessageBoxYesNo(string message, string yes, string no, ViewResult defaultButton)
      {
         return showQuestionMessageBox(message, MessageBoxButtons.YesNo, yes, no, string.Empty, defaultButton);
      }

      private ViewResult showQuestionMessageBox(string message, MessageBoxButtons buttons, string yes, string no, string cancel, ViewResult defaultButton)
      {
         var currentLocalizer = Localizer.Active;
         try
         {
            Localizer.Active = new XtraMessageBoxLocalizer(yes, no, cancel);
            return _mapper.MapFrom(XtraMessageBox.Show(message, _applicationConfiguration.ProductNameWithTrademark, buttons, MessageBoxIcon.Question, defaultButtonFrom(defaultButton)));
         }
         finally
         {
            Localizer.Active = currentLocalizer;
         }
      }

      private MessageBoxDefaultButton defaultButtonFrom(ViewResult defaultButton)
      {
         if (defaultButton == ViewResult.Yes)
            return MessageBoxDefaultButton.Button1;

         if (defaultButton == ViewResult.No)
            return MessageBoxDefaultButton.Button2;

         if (defaultButton == ViewResult.Cancel)
            return MessageBoxDefaultButton.Button3;

         return MessageBoxDefaultButton.Button1;
      }

      public void MessageBoxInfo(string message)
      {
         XtraMessageBox.Show(message, _applicationConfiguration.ProductNameWithTrademark, MessageBoxButtons.OK, MessageBoxIcon.Information);
      }

      public string AskForFileToOpen(string title, string filter, string directoryKey, string defaultFileName = null, string defaultDirectory = null)
      {
         var frmDialog = new OpenFileDialog {Multiselect = false};
         return selectionFor(frmDialog, title, filter, directoryKey, defaultFileName, defaultDirectory);
      }

      public string AskForFileToSave(string title, string filter, string directoryKey, string defaultFileName = null, string defaultDirectory = null)
      {
         var frmDialog = new SaveFileDialog {OverwritePrompt = true};
         return selectionFor(frmDialog, title, filter, directoryKey, defaultFileName, defaultDirectory);
      }

      private string selectionFor(FileDialog fileDialog, string title, string filter, string directoryKey, string defaultFileName, string defaultDirectory)
      {
         fileDialog.Title = title;
         fileDialog.Filter = filter;
         fileDialog.FileName = fileNameFrom(defaultFileName);
         fileDialog.InitialDirectory = getInitialDirectory(directoryKey, defaultDirectory);

         if (fileDialog.ShowDialog() != DialogResult.OK)
            return string.Empty;

         _directoryMapSettings.AddUsedDirectory(directoryKey, FileHelper.FolderFromFileFullPath(fileDialog.FileName));

         return fileDialog.FileName;
      }

      private string fileNameFrom(string fileName)
      {
         return FileHelper.RemoveIllegalCharactersFrom(fileName ?? string.Empty);
      }

      private string getInitialDirectory(string directoryKey, string defaultDirectory)
      {
         if (!string.IsNullOrEmpty(defaultDirectory))
            return defaultDirectory;

         var defaultFolder = _directoryMapSettings.UsedDirectories[directoryKey];
         return string.IsNullOrEmpty(defaultFolder?.Path) ? string.Empty : defaultFolder.Path;
      }

      public string AskForFolder(string title, string directoryKey, string defaultDirectory = null)
      {
         using (var folderDialog = new CommonOpenFileDialog {Title = title, InitialDirectory = getInitialDirectory(directoryKey, defaultDirectory)})
         {
            folderDialog.IsFolderPicker = true;
            if (folderDialog.ShowDialog() != CommonFileDialogResult.Ok)
               return string.Empty;

            _directoryMapSettings.AddUsedDirectory(directoryKey.ToString(), folderDialog.FileName);

            return folderDialog.FileName;
         }
      }

      public string AskForInput(string caption, string text, string defaultValue = null, IEnumerable<string> forbiddenValues = null, IEnumerable<string> predefinedValues = null)
      {
         return InputBoxDialog.Show(caption, text, defaultValue, forbiddenValues, predefinedValues);
      }

      private class XtraMessageBoxLocalizer : Localizer
      {
         private readonly string _yes = "&Yes";
         private readonly string _no = "&No";
         private readonly string _cancel = "&Cancel";

         public XtraMessageBoxLocalizer(string yes, string no, string cancel)
         {
            if (!string.IsNullOrEmpty(yes)) _yes = yes;
            if (!string.IsNullOrEmpty(no)) _no = no;
            if (!string.IsNullOrEmpty(cancel)) _cancel = cancel;
         }

         public override string GetLocalizedString(StringId id)
         {
            switch (id)
            {
               case StringId.XtraMessageBoxCancelButtonText:
                  return _cancel;
               case StringId.XtraMessageBoxYesButtonText:
                  return _yes;
               case StringId.XtraMessageBoxNoButtonText:
                  return _no;
            }
            return string.Empty;
         }
      }
   }
}