using System;
using OSPSuite.Assets;

namespace OSPSuite.Presentation.Views
{
   public interface IExceptionView
   {
      /// <summary>
      /// Sets the overall description of the view (typically done once)
      /// </summary>
      string Description { set; }

      /// <summary>
      /// Initialize the view with caption and icon. The assembly info contains the text that
      /// will be written in the email subject
      /// </summary>
      void Initialize(string caption, ApplicationIcon icon,string emailSubject,string supportEmail);

      /// <summary>
      /// Initialize the view with caption and icon and also sets the default display for the groups.The assembly info contains the text that
      /// will be written in the email subject
      /// </summary>
      void Initialize(string caption, ApplicationIcon icon, string emailSubject, string supportEmail, string exceptionDisplay, string fullStackTraceDisplay);

      /// <summary>
      /// set the text being displayed in the exception message field
      /// </summary>
      string ExceptionMessage { set; }

      /// <summary>
      /// set the text being displayed in the exception stack trace field
      /// </summary>
      string FullStackTrace { set; }

      /// <summary>
      /// Displays the view, set the owner form and also sets exception and stack trace bypassing the value sets by the user
      /// </summary>
      void Display(Exception e);

      /// <summary>
      /// Displays the view for the given owner
      /// </summary>
      void Display();

      /// <summary>
      /// Sets the view or control owing the exception view
      /// </summary>
      object MainView { set; }
   }
}