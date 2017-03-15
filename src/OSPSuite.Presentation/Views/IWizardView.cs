namespace OSPSuite.Presentation.Views
{
   public interface IWizardView : IContainerView, IModalView
   {
      /// <summary>
      /// Specifies wether the buttons next and ok are visible.
      /// At least of the button should be visisble. This will not be checked
      /// and is left to the presenter
      /// </summary>
      /// <param name="nextVisible">Next button visible?</param>
      /// <param name="okVisible">OK button visible?</param>
      void SetButtonsVisible(bool nextVisible, bool okVisible);

      /// <summary>
      /// Set the previous button enabled or disabled
      /// </summary>
      bool PreviousEnabled { get; set; }

      /// <summary>
      /// Set the previous button enabled or disabled
      /// </summary>
      bool NextEnabled { get; set; }

      /// <summary>
      /// Initialize the wizard
      /// </summary>
      void InitializeWizard();

      /// <summary>
      /// return the index of the selected page in the wizard
      /// </summary>
      int SelectedPageIndex { get; }

      /// <summary>
      /// Set the cancel button enabled or disabled
      /// </summary>
      bool CancelEnabled { get; set; }
   }
}