using System;
using OSPSuite.Assets;

namespace OSPSuite.Presentation.Views
{
   public interface IExceptionView
   {
      /// <summary>
      ///    Sets the overall description of the view (typically done once)
      /// </summary>
      string Description { set; }

      /// <summary>
      ///    Initialize the view with caption and icon. The assembly info contains information about the calling assembly 
      /// </summary>
      void Initialize(string caption, ApplicationIcon icon,  string issueTrackerUrl, string productName);

      /// <summary>
      ///    set the text being displayed in the exception message field
      /// </summary>
      string ExceptionMessage { set; }

      /// <summary>
      ///    set the text being displayed in the exception stack trace field
      /// </summary>
      string FullStackTrace { set; }

      /// <summary>
      ///    Displays the view, set the owner form and also sets exception, stack trace and clipboard content
      /// </summary>
      void Display(string message, string stackTrace, string clipboardContent);

      /// <summary>
      ///    Displays the view for the given owner
      /// </summary>
      void Display();

      /// <summary>
      ///    Sets the view or control owing the exception view
      /// </summary>
      object MainView { set; }
   }
}