using System.Collections.Generic;

namespace OSPSuite.Core.Services
{
   public enum ViewResult
   {
      OK,
      Cancel,
      Yes,
      No
   }

   public interface IDialogCreator
   {
      void MessageBoxError(string message);

      ViewResult MessageBoxYesNoCancel(string message);

      ViewResult MessageBoxYesNoCancel(string message, ViewResult defaultButton);

      /// <summary>
      ///    Customize a yes no cancel message box by allowing to change caption for yes, no, and cancel
      /// </summary>
      /// <remarks>
      ///    Leaving caption empty will use the default for the button
      /// </remarks>
      ViewResult MessageBoxYesNoCancel(string message, string yes, string no, string cancel);

      ViewResult MessageBoxYesNoCancel(string message, string yes, string no, string cancel, ViewResult defaultButton);

      ViewResult MessageBoxYesNo(string message);

      ViewResult MessageBoxYesNo(string message, ViewResult defaultButton);

      /// <summary>
      ///    Customize a yes no cancel message box by allowing to change caption for yes, no
      /// </summary>
      /// <remarks>
      ///    Leaving caption empty will use the default for the button
      /// </remarks>
      ViewResult MessageBoxYesNo(string message, string yes, string no);

      ViewResult MessageBoxYesNo(string message, string yes, string no, ViewResult defaultButton);

      void MessageBoxInfo(string message);

      /// <summary>
      ///    Prompt a dialog asking the user to select a file to open for the given <paramref name="filter" />
      /// </summary>
      /// <param name="title">Title of dialog</param>
      /// <param name="filter">Filter used to retrieve the file to open</param>
      /// <param name="directoryKey">
      ///    Directory key used to retrieve the default directory if <paramref name="defaultDirectory" />
      ///    is not specified. Selcted path will be saved in this key
      /// </param>
      /// <param name="defaultFileName">Optional: default file name</param>
      /// <param name="defaultDirectory">Optional: default directory, This will override the directory key value</param>
      string AskForFileToOpen(string title, string filter, string directoryKey, string defaultFileName = null, string defaultDirectory = null);

      /// <summary>
      ///    Prompt a dialog asking the user to select a file to save for the given <paramref name="filter" />
      /// </summary>
      /// <param name="title">Title of dialog</param>
      /// <param name="filter">Filter used to retrieve the file to save </param>
      /// <param name="directoryKey">
      ///    Directory key used to retrieve the default directory if <paramref name="defaultDirectory" />
      ///    is not specified. Selcted path will be saved in this key
      /// </param>
      /// <param name="defaultFileName">Optional: default file name</param>
      /// <param name="defaultDirectory">Optional: default directory, This will override the directory key value</param>
      string AskForFileToSave(string title, string filter, string directoryKey, string defaultFileName = null, string defaultDirectory = null);


      /// <summary>
      ///    Prompt a dialog asking the user to select a folder. The title is used in the description field.
      ///    The default starting folder will be mapped using the directory key
      /// </summary>
      /// <returns>the selected folder if the user confirmed the action or string.empty on cancel</returns>
      string AskForFolder(string title, string directoryKey, string defaultDirectory = null);

      string AskForInput(string caption, string text, string defaultValue = null, IEnumerable<string> forbiddenValues=null, IEnumerable<string> predefinedValues=null);
   }
}
