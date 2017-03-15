namespace OSPSuite.Presentation.Views
{
   /// <summary>
   /// Represents the Shell of the application. Only one instance at all time
   /// </summary>
   public interface IShell : IView
   {
      /// <summary>
      /// Returns the active view or <c>null</c> if no view was selected
      /// </summary>
      IMdiChildView ActiveView { get; }

      /// <summary>
      /// Sets the cursor to hour glass if <paramref name="hourGlassVisible"> is set to true</paramref>. If <paramref name="forceCursorChange"/> is true,
      /// the cursor is set explicitely
      /// </summary>
      void InWaitCursor(bool hourGlassVisible, bool forceCursorChange);

      /// <summary>
      /// Initializes the view. To be called during startup
      /// </summary>
      void Initialize();

      /// <summary>
      /// Shows the help (Typically called after F1)
      /// </summary>
      void ShowHelp();

      /// <summary>
      /// Display a notification
      /// </summary>
      /// <param name="caption">Caption</param>
      /// <param name="notification">Text being displayed</param>
      /// <param name="url">Full URL of the webpage being displayed when the user clicks on the notification</param>
      void DisplayNotification(string caption, string notification, string url);
   }
}