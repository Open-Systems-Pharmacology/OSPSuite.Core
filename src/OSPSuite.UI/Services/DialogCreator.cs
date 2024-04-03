using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using OSPSuite.Core;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Mappers;
using OSPSuite.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Assets;

namespace OSPSuite.UI.Services
{
   public class DialogCreator : IDialogCreator
   {
      private readonly IDialogResultToViewResultMapper _mapper;
      private readonly DirectoryMapSettings _directoryMapSettings;
      private readonly IApplicationConfiguration _applicationConfiguration;

      public DialogCreator(IDialogResultToViewResultMapper mapper, DirectoryMapSettings directoryMapSettings, IApplicationConfiguration applicationConfiguration)
      {
         _mapper = mapper;
         _directoryMapSettings = directoryMapSettings;
         _applicationConfiguration = applicationConfiguration;
      }

      public void MessageBoxError(string message)
      {
         showMessageBox(message, new[] { DialogResult.OK }, getIcon(SystemIcons.Error));
      }

      public void MessageBoxInfo(string message)
      {
         showMessageBox(message, new[] { DialogResult.OK }, getIcon(SystemIcons.Information));
      }

      private void showMessageBox(string message, DialogResult[] buttons, Icon icon)
      {
         XtraMessageBox.Show(createMessageBoxArgs(message, buttons, icon));
      }

      private XtraMessageBoxArgs createMessageBoxArgs(string message, DialogResult[] buttons, Icon icon, int defaultButtonIndex = 0, bool doNotShowAgainCheckBoxVisible = false)
      {
         var args = new XtraMessageBoxArgs
         {
            Caption = _applicationConfiguration.ProductNameWithTrademark,
            Text = message,
            Buttons = buttons,
            Icon = icon,
            DefaultButtonIndex = defaultButtonIndex,
            AllowHtmlText = DefaultBoolean.True,
            DoNotShowAgainCheckBoxVisible = doNotShowAgainCheckBoxVisible,
            DoNotShowAgainCheckBoxText = Captions.DoNotShowThisAgainUntilRestart
         };

         if (containsHyperlink(message))
            args.HyperlinkClick += (o, e) => { System.Diagnostics.Process.Start(e.Link); };

         return args;
      }

      public ViewResult MessageBoxYesNoCancel(string message, ViewResult defaultButton = ViewResult.Yes)
      {
         return MessageBoxYesNoCancel(message, string.Empty, string.Empty, string.Empty, defaultButton);
      }

      public ViewResult MessageBoxYesNoCancel(string message, string yes, string no, string cancel, ViewResult defaultButton = ViewResult.Yes)
      {
         return showQuestionMessageBox(message, new[] {
            DialogResult.Yes, DialogResult.No, DialogResult.Cancel}, yes, no, cancel, defaultButton);
      }

      public ViewResult MessageBoxYesNo(string message, ViewResult defaultButton = ViewResult.Yes)
      {
         return MessageBoxYesNo(message, string.Empty, string.Empty, defaultButton);
      }

      public ViewResult MessageBoxYesNo(string message, string yes, string no, ViewResult defaultButton = ViewResult.Yes)
      {
         return showQuestionMessageBox(message, new[]
         {
            DialogResult.Yes, DialogResult.No
         }, yes, no, string.Empty, defaultButton);
      }

      public ViewResult MessageBoxConfirm(string message, Action doNotShowAgain, ViewResult defaultButton = ViewResult.Yes)
      {
         return showQuestionMessageBox(message, new[]
         {
            DialogResult.Yes, DialogResult.Cancel
         }, string.Empty, string.Empty, string.Empty, defaultButton, doNotShowAgain);
      }

      private ViewResult showQuestionMessageBox(string message, IReadOnlyList<DialogResult> buttons, string yes, string no, string cancel, ViewResult defaultButton, Action doNotShowAgain = null)
      {
         var currentLocalizer = Localizer.Active;
         try
         {
            Localizer.Active = new XtraMessageBoxLocalizer(yes, no, cancel);
            var args = createMessageBoxArgs(message, buttons.ToArray(), getIcon(SystemIcons.Question), defaultButtonFrom(defaultButton), doNotShowAgain != null);
            args.Closed += (sender, e) => onMessageBoxClosed(e, doNotShowAgain);
            DialogResult input = XtraMessageBox.Show(args);
            return _mapper.MapFrom(input);
         }
         finally
         {
            Localizer.Active = currentLocalizer;
         }
      }

      private void onMessageBoxClosed(XtraMessageBoxClosedArgs e, Action doNotShowAgain)
      {
         // e.Visible gets whether the user checked the 'Do not show this message again' checkbox. Checked makes e.Visible = false 
         if (!e.Visible && e.DialogResult == DialogResult.Yes)
         {
            doNotShowAgain?.Invoke();
         }
      }

      private Icon getIcon(Icon icon)
      {
         return DevExpress.Utils.Drawing.Helpers.StockIconHelper.GetWindows8AssociatedIcon(icon);
      }

      private int defaultButtonFrom(ViewResult defaultButton)
      {
         switch (defaultButton)
         {
            case ViewResult.Yes:
               return 0;
            case ViewResult.No:
               return 1;
            case ViewResult.Cancel:
               return 2;
            default:
               return 0;
         }
      }

      private bool containsHyperlink(string message)
      {
         return message.Contains("href");
      }

      public string AskForFileToOpen(string title, string filter, string directoryKey, string defaultFileName = null, string defaultDirectory = null)
      {
         var frmDialog = new OpenFileDialog { Multiselect = false };
         return selectionFor(frmDialog, title, filter, directoryKey, defaultFileName, defaultDirectory);
      }

      public string AskForFileToSave(string title, string filter, string directoryKey, string defaultFileName = null, string defaultDirectory = null)
      {
         var frmDialog = new SaveFileDialog { OverwritePrompt = true };
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
         using (var folderDialog = new CommonOpenFileDialog { Title = title, InitialDirectory = getInitialDirectory(directoryKey, defaultDirectory) })
         {
            folderDialog.IsFolderPicker = true;
            if (folderDialog.ShowDialog() != CommonFileDialogResult.Ok)
               return string.Empty;

            _directoryMapSettings.AddUsedDirectory(directoryKey.ToString(), folderDialog.FileName);

            return folderDialog.FileName;
         }
      }

      public string AskForInput(string caption, string text, string defaultValue = null, IEnumerable<string> forbiddenValues = null, IEnumerable<string> predefinedValues = null, string iconName = null)
      {
         return InputBoxDialog.Show(caption, text, defaultValue, forbiddenValues, predefinedValues, iconName);
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